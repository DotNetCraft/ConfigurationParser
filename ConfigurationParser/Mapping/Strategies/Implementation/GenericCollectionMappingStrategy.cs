using System;
using System.Reflection;
using System.Xml;

namespace ConfigurationParser.Mapping.Strategies.Implementation
{
    /// <summary>
    /// The strategy is using for converting node data into the collection
    /// </summary>
    public class GenericCollectionMappingStrategy : IMappingStrategy
    {
        #region Fields...

        /// <summary>
        /// The mapping strategies factory.
        /// </summary>
        private readonly MappingStrategyFactory _mappingStrategyFactory;

        #endregion

        #region Constructors...

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mappingStrategyFactory">The mapping strategies factory.</param>
        public GenericCollectionMappingStrategy(MappingStrategyFactory mappingStrategyFactory)
        {
            _mappingStrategyFactory = mappingStrategyFactory;
        }

        #endregion

        #region Implementation of IMappingStrategy

        /// <summary>
        /// Convert xml node into the collection.
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="collectionType">collection's type.</param>
        /// <param name="configurationReader">The IConfigurationReader instance.</param>
        /// <returns>The collection.</returns>
        public object Map(XmlNode node, Type collectionType, IConfigurationReader configurationReader)
        {
            var list = Activator.CreateInstance(collectionType);
            Type itemType = collectionType.GetGenericArguments()[0];
            MethodInfo addMethod = collectionType.GetMethod("Add");

            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                XmlNode childNode = node.ChildNodes[i];

                if (itemType.IsPrimitive || itemType == typeof(string) || itemType.IsEnum)
                {
                    IPrimitiveMappingStrategy mappingStrategy = _mappingStrategyFactory.CreatePrimitiveStrategy(itemType);
                    var item = mappingStrategy.Map(childNode.InnerText, itemType);
                    addMethod.Invoke(list, new[] { item });
                }
                else
                {
                    IMappingStrategy mappingStrategy = _mappingStrategyFactory.CreateComplexStrategy(itemType);
                    var item = mappingStrategy.Map(childNode, itemType, configurationReader);
                    addMethod.Invoke(list, new[] { item });
                }
            }

            return list;
        }

        #endregion
    }
}
