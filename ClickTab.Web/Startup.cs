using Autofac;
using ClickTab.Core.Attributes;
using ClickTab.Core.HelperService;
using ClickTab.Core.IoC;
using ClickTab.Core.Middleware;
using ClickTab.Core.Services;
using ClickTab.Core.Tenant;
using ClickTab.Web.Mappings;
using ClickTab.Web.MiddleWares;
using ClickTab.Web.NotificationCenter.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;

namespace ClickTab.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.Name = "EQP.Session";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllers(config =>
            {
                config.Filters.Add(new ExceptionHandlerFilterMiddleware());
            })
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClickTabClient/dist";
            });
            services.AddCors();
            services.AddOptions();

            services.AddSignalR();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ITenantConnectionService, TenantConnectionService>();

        }

        /// <summary>
        /// Configura il container per la DependencyInjection tramite Autofac
        /// </summary>
        /// <param name="containerBuilder"></param>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            //Recupera connection string e provider dall'appsettings.json e istanzia ;il container autofac definito nel progetto ClickTab.Core
            string connectionString = this.Configuration["Configuration:ConnectionString"];
            DbProvider provider = (DbProvider)(int.Parse(this.Configuration["Configuration:Provider"]));

            containerBuilder.RegisterModule(new ApplicationContainerService());

            //Registra nell'IoC anche il servizio per l'automapping (non � stato usato il middleware di asp.net core
            //perch� nel mapping service abbiamo la necessit� di usare la dependency injection per gestire le traduzioni dei dati,
            //pertanto abbiamo creato un servizio di mappatura che, aggiunto nel catalogo del container builder, permette di usare
            //la dependency injection)
            containerBuilder.RegisterType<AutoMappingService>().SingleInstance();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ConfigurationService configService, JwtService jwtService, DatabaseService dbService)
        {
            //Legge l'appsettings.json e recupera i parametri di configurazione dell'ambiente
            configService.ConnectionString = this.Configuration["Configuration:ConnectionString"];
            configService.DbContextProvider = (DbProvider)(int.Parse(this.Configuration["Configuration:Provider"]));
            configService.BaseDomainClient = this.Configuration["Configuration:BaseDomainClient"];
            configService.BaseDomainServer = this.Configuration["Configuration:BaseDomainServer"];
            configService.BaseDomainMobileAppANDROID = this.Configuration["Configuration:BaseDomainMobileAppANDROID"];
            configService.BaseDomainMobileAppIOS = this.Configuration["Configuration:BaseDomainMobileAppIOS"];
            configService.AdditionalCorsOrigins = this.Configuration["Configuration:AdditionalCorsOrigins"].Split(',').Where(o => !String.IsNullOrEmpty(o)).ToArray();
            configService.FileStorageMode = (FileStorageMode)(int.Parse(this.Configuration["Configuration:FileStorageMode"]));
            configService.SyncDefaultData = bool.Parse(this.Configuration["Configuration:SyncDefaultData"].ToString());
            configService.SyncVersionData = this.Configuration["Configuration:SyncVersionData"];
            configService.AppName = this.Configuration["Configuration:AppName"];
            configService.IsDevelopment = env.IsDevelopment();

            #region Configurazione EMAIL

            configService.EmailEnableSSL = this.Configuration["Configuration:EmailEnableSSL"] != null ? bool.Parse(this.Configuration["Configuration:EmailEnableSSL"]) : false;
            configService.EmailFrom = this.Configuration["Configuration:EmailFrom"];
            configService.EmailPassword = this.Configuration["Configuration:EmailPassword"];
            configService.EmailPortNumber = int.Parse(this.Configuration["Configuration:EmailPortNumber"]);
            configService.EmailSMTPClient = this.Configuration["Configuration:EmailSMTPClient"];

            #endregion

            //Recupera la secret key per la gestione dei token con jwt
            jwtService.SecretKey = this.Configuration["Configuration:JwtSecretKey"];

            //Prova commento ale

            //Sincronizza il database in caso di migration pendenti non ancora eseguite
            //dbService.ApplyMigrations(configService.DbContextProvider, configService.ConnectionString, configService.SyncDefaultData);
            var tenantConnectionService = app.ApplicationServices.GetRequiredService<ITenantConnectionService>();

            foreach (var tenant in ((TenantConnectionService)tenantConnectionService).GetAllTenantIds())
            {
                var connectionString = tenantConnectionService.GetConnectionString(tenant);
                var provider = tenantConnectionService.GetDbProvider(tenant);

                dbService.ApplyMigrations(provider, connectionString, configService.SyncDefaultData);
                dbService.AutoUpdate(provider, connectionString);
            }

            app.UseStaticFiles();
            if (!env.IsDevelopment())
                app.UseSpaStaticFiles();

            //Creo un array con i domini da abilitare nel CORS perch� non posso passare come parametri
            //nel WithOrigins delle stringhe e un array di stringhe insieme.
            string[] corsOrigins = new string[] { configService.BaseDomainClient, configService.BaseDomainMobileAppIOS, configService.BaseDomainMobileAppANDROID };
            corsOrigins = corsOrigins.Concat(configService.AdditionalCorsOrigins).ToArray();
            // Shows UseCors with CorsPolicyBuilder.
            app.UseCors(builder =>
            {
                builder
                    .WithOrigins(corsOrigins)
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            app.UseMiddleware<TenantMiddleware>();

            app.UseRouting();

            //Aggiunge il middleware per la gestione delle autenticazioni delle chiamate
            app.UseSession();
            app.UseEQPAuthentication();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapHub<NotificationHub>("/NotificationHub").WithMetadata(new WebSocketAttribute());
            });

        }
    }
}
