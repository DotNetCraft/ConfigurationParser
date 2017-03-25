using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace DotNetCraft.ConfigurationParser.Mapping.Strategies.Implementation
{
    /// <summary>
    /// The strategy is using for converting node data into the IDictionary<K,V>
    /// </summary>
    public class GenericDictionaryMappingStrategy : IMappingStrategy
    {
        #region Fields...

        /// <summary>
        /// The mapping strategies factory.
        /// </summary>
        private readonly MappingStrategyFactory mappingStrategyFactory;

        #endregion

        #region Constructors...

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mappingStrategyFactory">The mapping strategies factory.</param>
        /// <exception cref="ArgumentNullException"><paramref name="mappingStrategyFactory"/> is <see langword="null"/></exception>
        public GenericDictionaryMappingStrategy(MappingStrategyFactory mappingStrategyFactory)
        {
            if (mappingStrategyFactory == null)
                throw new ArgumentNullException(nameof(mappingStrategyFactory));

            this.mappingStrategyFactory = mappingStrategyFactory;
        }

        #endregion

        #region Implementation of IMappingStrategy

        /// <summary>
        /// Convert xml node into the IDictionary<K, itemType>.
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="collectionType">Collection's type.</param>
        /// <param name="configurationReader">The IConfigurationReader instance.</param>
        /// <returns>The IDictionary<K, itemType>.</returns>
        /// <exception cref="KeyNotFoundException">There is no key in the collection..</exception>
        /// <exception cref="NotSupportedException">Only primitive keys are supported.</exception>
        /// <exception cref="IndexOutOfRangeException">The value section should contain only one inner element.</exception>
        /// <exception cref="TargetInvocationException">The constructor being called throws an exception. </exception>
        /// <exception cref="AmbiguousMatchException">More than one method is found with the specified name. </exception>
        /// <exception cref="TargetException">In the .NET for Windows Store apps or the Portable Class Library, catch <see cref="T:System.Exception" /> instead.The <paramref name="obj" /> parameter is null and the method is not static.-or- The method is not declared or inherited by the class of <paramref name="obj" />. -or-A static constructor is invoked, and <paramref name="obj" /> is neither null nor an instance of the class that declared the constructor.</exception>
        public object Map(XmlNode node, Type collectionType, IConfigurationReader configurationReader)
        {
            //Type collectionType = propertyInfo.PropertyType;
            var dictionary = Activator.CreateInstance(collectionType);
            Type keyType = collectionType.GetGenericArguments()[0];
            Type itemType = collectionType.GetGenericArguments()[1];
            MethodInfo addMethod = collectionType.GetMethod("Add");

            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                XmlNode childNode = node.ChildNodes[i];
                string keyValue = childNode.GetNodeValue("key");
                if (string.IsNullOrEmpty(keyValue))
                {
                    string msg = string.Format("There is no 'key' in the {0}.{1}", node.Name, childNode.Name);
                    throw new KeyNotFoundException(msg);
                }

                object key;
                object item;

                if (keyType.IsPrimitive || keyType == typeof(string) || keyType.IsEnum)
                {
                    IPrimitiveMappingStrategy mappingStrategy = mappingStrategyFactory.CreatePrimitiveStrategy(itemType);
                    key = mappingStrategy.Map(keyValue, keyType);
                }
                else
                {
                    string msg = string.Format("{0} as a key not supported.", keyType);
                    throw new NotSupportedException(msg);
                }

                if (itemType.IsPrimitive || itemType == typeof(string) || itemType.IsEnum)
                {
                    IPrimitiveMappingStrategy mappingStrategy = mappingStrategyFactory.CreatePrimitiveStrategy(itemType);
                    string itemValue = childNode.GetNodeValue("value");
                    item = mappingStrategy.Map(itemValue, itemType);
                }
                else
                {
                    XmlNode innerXml = childNode.GetXmlNode("value");

                    if (innerXml.ChildNodes.Count != 1)
                    {
                        string msg = string.Format("The value section should contain only one inner element in the {0}.{1}", childNode.Name, innerXml.Name);
                        throw new IndexOutOfRangeException(msg);
                    }

                    innerXml = innerXml.ChildNodes[0];
                    IMappingStrategy mappingStrategy = mappingStrategyFactory.CreateComplexStrategy(itemType);
                    item = mappingStrategy.Map(innerXml, itemType, configurationReader);
                }

                addMethod.Invoke(dictionary, new[] { key, item });
            }

            return dictionary;
        }

        #endregion
    }
}
