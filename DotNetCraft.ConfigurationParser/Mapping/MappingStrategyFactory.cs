using System;
using System.Collections.Generic;
using DotNetCraft.ConfigurationParser.Exceptions;
using DotNetCraft.ConfigurationParser.Mapping.Strategies;
using DotNetCraft.ConfigurationParser.Mapping.Strategies.Implementation;

namespace DotNetCraft.ConfigurationParser.Mapping
{
    /// <summary>
    /// Mapping strategies factory.
    /// </summary>
    public class MappingStrategyFactory : IMappingStrategyFactory
    {
        #region Fields...

        /// <summary>
        /// Mapping strategies.
        /// </summary>
        private readonly Dictionary<Type, IMappingStrategy> mappingStrategies;

        #endregion        

        #region Constructors...

        /// <summary>
        /// Constructor.
        /// </summary>
        public MappingStrategyFactory()
        {
            mappingStrategies = new Dictionary<Type, IMappingStrategy>();

            Register(typeof(Array), new ArrayMappingStrategy(this));
            Register(typeof(List<>), new GenericCollectionMappingStrategy(this));
            Register(typeof(IList<>), new GenericCollectionMappingStrategy(this));
            Register(typeof(ICollection<>), new GenericCollectionMappingStrategy(this));
            Register(typeof(HashSet<>), new GenericCollectionMappingStrategy(this));

            Register(typeof(Dictionary<,>), new GenericDictionaryMappingStrategy(this));
            Register(typeof(SortedList<,>), new GenericDictionaryMappingStrategy(this));
            Register(typeof(IDictionary<,>), new GenericDictionaryMappingStrategy(this));
        }

        #endregion

        #region Implementation of IMappingStrategyFactory

        /// <summary>
        /// Create a primitive mapping strategy by object's type.
        /// </summary>
        /// <param name="itemType">The object's type.</param>
        /// <returns>The IPrimitiveMappingStrategy instance.</returns>
        public IPrimitiveMappingStrategy CreatePrimitiveStrategy(Type itemType)
        {
            //string msg = string.Format("There is no mapping strategy for the field {0} ({1})...", propertyInfo.Name, propertyInfo.PropertyType);
            //throw new MappingStrategyNotFoundException(msg);

            IPrimitiveMappingStrategy strategy = new PrimitiveMappingStrategy();
            return strategy;
        }

        /// <summary>
        /// Create a complex mapping strategy by object's type.
        /// </summary>
        /// <param name="itemType">The object's type.</param>
        /// <returns>The IMappingStrategy instance.</returns>
        public IMappingStrategy CreateComplexStrategy(Type itemType)
        {
            IMappingStrategy mappingStrategy;
            Type key = itemType;

            if (itemType.IsGenericType)
                key = itemType.GetGenericTypeDefinition();
            else if (itemType.IsArray)
                key = typeof(Array);

            if (mappingStrategies.TryGetValue(key, out mappingStrategy))
                return mappingStrategy;

            //Just a class.
            return new ObjectMappingStrategy();
        }

        /// <summary>
        /// Register mapping strategy using object's type.
        /// </summary>
        /// <param name="itemType">The object's type.</param>
        /// <param name="mappingStrategy">The <see cref="IMappingStrategy"/> instance.</param>
        public void Register(Type itemType, IMappingStrategy mappingStrategy)
        {
            if (mappingStrategies.ContainsKey(itemType))
            {
                string msg = string.Format("The strategy for the {0} has been already existed.", itemType);
                throw new MappingStrategyFactoryException(msg);
            }

            mappingStrategies.Add(itemType, mappingStrategy);
        }

        #endregion
    }
}
