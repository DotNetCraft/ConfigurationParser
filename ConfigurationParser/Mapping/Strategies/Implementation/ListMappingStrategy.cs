using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace ConfigurationParser.Mapping.Strategies.Implementation
{
    /// <summary>
    /// The strategy is using for converting node data into the List<T>
    /// </summary>
    public class ListMappingStrategy : IMappingStrategy
    {
        #region Fields...

        /// <summary>
        /// The mapping strategies factory.
        /// </summary>
        private readonly IMappingStrategyFactory mappingStrategyFactory;

        #endregion

        #region Constructors...

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mappingStrategyFactory">The mapping strategies factory.</param>
        /// <exception cref="ArgumentNullException"><paramref name="mappingStrategyFactory"/> is <see langword="null"/></exception>
        public ListMappingStrategy(IMappingStrategyFactory mappingStrategyFactory)
        {
            if (mappingStrategyFactory == null)
                throw new ArgumentNullException(nameof(mappingStrategyFactory));

            this.mappingStrategyFactory = mappingStrategyFactory;
        }

        #endregion

        #region Implementation of IMappingStrategy

        /// <summary>
        /// Convert xml node into the List<itemType>.
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="itemType">Object's type.</param>
        /// <param name="configurationReader">The IConfigurationReader instance.</param>
        /// <returns>The List<itemType>.</returns>
        public object Map(XmlNode node, Type itemType, IConfigurationReader configurationReader)
        {
            Type elementType = itemType.GetElementType();
            Type listGenericType = typeof(List<>);
            Type listType = listGenericType.MakeGenericType(elementType);

            IMappingStrategy mappingStrategy = mappingStrategyFactory.CreateComplexStrategy(listType);
            var list = mappingStrategy.Map(node, listType, configurationReader);

            MethodInfo toArrayMethod = listType.GetMethod("ToArray");
            var result = toArrayMethod.Invoke(list, null);
            return result;
        }

        #endregion
    }
}
