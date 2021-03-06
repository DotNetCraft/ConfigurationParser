﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace DotNetCraft.ConfigurationParser.Mapping.Strategies.Implementation
{
    class ArrayMappingStrategy : IMappingStrategy
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
        public ArrayMappingStrategy(IMappingStrategyFactory mappingStrategyFactory)
        {
            this.mappingStrategyFactory = mappingStrategyFactory;
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
            Type itemType = collectionType.GetElementType();
            var d1 = typeof(List<>);
            Type[] typeArgs = { itemType };
            var makeme = d1.MakeGenericType(typeArgs);
            var list = Activator.CreateInstance(makeme);
            MethodInfo addMethod = makeme.GetMethod("Add");
            
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                XmlNode childNode = node.ChildNodes[i];

                if (itemType.IsPrimitive || itemType == typeof(string) || itemType.IsEnum)
                {
                    IPrimitiveMappingStrategy mappingStrategy = mappingStrategyFactory.CreatePrimitiveStrategy(itemType);
                    var item = mappingStrategy.Map(childNode.InnerText, itemType);
                    addMethod.Invoke(list, new[] { item });
                }
                else
                {
                    IMappingStrategy mappingStrategy = mappingStrategyFactory.CreateComplexStrategy(itemType);
                    var item = mappingStrategy.Map(childNode, itemType, configurationReader);
                    addMethod.Invoke(list, new[] { item });
                }
            }

            MethodInfo toArrayMethod = makeme.GetMethod("ToArray");
            return toArrayMethod.Invoke(list, null);
        }

        #endregion
    }
}
