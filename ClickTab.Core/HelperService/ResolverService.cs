using Autofac;
using ClickTab.Core.Attributes;
using EQP.EFRepository.Core.Interface;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClickTab.Core.HelperService
{
    /// <summary>
    /// Servizio che utilizza il container di autofac (popolato nello startup)
    /// </summary>
    public class EQPResolverService
    {
        private ILifetimeScope _root;
        public EQPResolverService(ILifetimeScope autofacRoot)
        {
            _root = autofacRoot;
        }

        /// <summary>
        /// Dato il nome del tipo di una classe restituisce le istanze dei resolvers definiti tramite il decoratore
        /// EntityResolverAttribute.
        /// Se all'interno del decoratore viene definito un tipo di resolver che non implementa IResolverService o IResolverEntity allora viene ignorato.
        /// Le istanze dei resolver vengono istanziate usando il container di autofac, quindi il resolver deve essere inserito anche nel catalogo di autofac.
        /// </summary>
        /// <param name="EntityTypeName"></param>
        /// <returns></returns>
        public List<IResolverService> GetEntityResolvers(string EntityTypeName)
        {
            List<IResolverService> resolvers = new List<IResolverService>();

            Type EntityType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(t => t.GetTypes()).FirstOrDefault(t => !t.IsSubclassOf(typeof(Migration)) && t.Name.ToLower() == EntityTypeName.ToLower());

            var attributes = EntityType.GetCustomAttributes(typeof(EntityResolverAttribute), false);
            if (attributes == null || attributes.Length == 0)
                return resolvers;

            EntityResolverAttribute resolverAttribute = attributes[0] as EntityResolverAttribute;
            if (resolverAttribute.Resolvers == null || !resolverAttribute.Resolvers.Any())
                return resolvers;

            foreach (Type resolverType in resolverAttribute.Resolvers)
            {
                object resolvedService = _root.Resolve(resolverType);
                if (typeof(IResolverService).IsAssignableFrom(resolvedService.GetType()))
                    resolvers.Add(resolvedService as IResolverService);
            }

            return resolvers;
        }
    }
}
