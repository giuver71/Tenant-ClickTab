using System;
using System.Collections.Generic;
using System.Text;

namespace ClickTab.Core.Attributes
{
    /// <summary>
    /// Attributo da usare per definire tutti i resolver associati a una specifica entità (vedi Employee come esempio)
    /// </summary>
    public class EntityResolverAttribute : Attribute
    {
        private Type[] _resolvers;
        public Type[] Resolvers { get { return _resolvers; } }

        public EntityResolverAttribute(Type[] ResolversTypes)
        {
            _resolvers = ResolversTypes;
        }
    }
}
