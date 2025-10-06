using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading;

namespace ClickTab.Core.HelperService.NotificationCenter
{
    public class NotificationHubService
    {
        private HubConnection _hubConnection;
        private ConfigurationService _configService;

        public NotificationHubService(ConfigurationService configService)
        {
            _configService = configService;
        }

        /// <summary>
        /// Apre una nuova connessione al NotificationHub se _hubConnection è null o ha stato diverso da Connected.
        /// </summary>
        private void InitConnection()
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl($"{_configService.BaseDomainServer}/NotificationHub")
                    .Build();
                _hubConnection.StartAsync().Wait();
            }
        }

        /// <summary>
        /// Se _hubConnection esiste e ha stato diverso da Disconnected chiude la connessione al NotificationHub.
        /// Metodo chiamato quando viene distrutta l'istanza di questo servizio da Autofac.
        /// </summary>
        public async void CloseConnection()
        {
            if (_hubConnection != null && _hubConnection.State != HubConnectionState.Disconnected)
                await _hubConnection.DisposeAsync();
        }

        /// <summary>
        /// Invoca il metodo NotificationDataUpdate nel NotificationHub (o all'hub a cui si è connessi)
        /// passando i dati rivevuti.
        /// NOTA: i parametri ricevuti devono coincidere con quelli del metodo invocato altrimenti la notifica non viene inviata.
        /// </summary>
        /// <param name="receiverIDs">Elenco contenente gli ID degli utenti che devono ricevere la notifica</param>
        /// <param name="notificationTitle">Testo del titolo della notifica da inviare ai client</param>
        /// <returns></returns>
        public void NotificationDataUpdate(List<int> receiverIDs, string notificationTitle)
        {
            InitConnection();
            _hubConnection.InvokeAsync("NotificationDataUpdate", receiverIDs, notificationTitle).Wait();
        }

        /// <summary>
        /// Metodo generico per inviare una notifica su un Thread parallelo.
        /// Nel nuovo thread viene aperta una nuova connessione al socket in modo da essere totalmente indipendente 
        /// dal thread principale (se quest'ultimo finisce prima che la notifica viene inviata la connessione al socket
        /// viene chiusa). Una volta scodata la notifica la connessione aperta viene chiusa.
        /// </summary>
        /// <remarks>
        /// NOTA IMPORTANTE: i parametri passati in questo metodo devono coincidere in ordine e tipo con quelli che 
        /// si aspetta metodo invocato. Se non coincidono il metodo non viene invocato e la push non arriva ai client connessi.
        /// </remarks>
        /// <param name="methodName">Nome del metodo dell'Hub da invocare</param>
        /// <param name="paramenters">Parametri da passare al metodo dell'Hub invocato</param>
        public void SendNotificationOnParallelThread(string methodName, params object[] paramenters)
        {
            object notificationThreadParameters = new
            {
                configService = _configService,
                methodName = methodName,
                notificationParams = paramenters,
            };

            Thread notificationThread = new Thread(async (object parameters) =>
            {
                ConfigurationService ConfigService = parameters.GetType().GetProperty("configService").GetValue(parameters) as ConfigurationService;
                string MethodName = parameters.GetType().GetProperty("methodName").GetValue(parameters) as string;
                object[] NotificationParams = parameters.GetType().GetProperty("notificationParams").GetValue(parameters) as object[];

                HubConnection HubConnection = new HubConnectionBuilder()
                    .WithUrl($"{ConfigService.BaseDomainServer}/NotificationHub")
                    .Build();

                await HubConnection.StartAsync();

                if (HubConnection.State == HubConnectionState.Connected)
                {
                    await HubConnection.InvokeCoreAsync(MethodName, NotificationParams);
                    await HubConnection.DisposeAsync();
                }
            });

            notificationThread.Start(notificationThreadParameters);
        }
    }
}
