﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Xml;
using DotNetCraft.ConfigurationParser.Attributes;
using DotNetCraft.ConfigurationParser.Mapping;
using DotNetCraft.ConfigurationParser.Mapping.Strategies;

namespace DotNetCraft.ConfigurationParser
{
    public class ConfigurationReader: DynamicObject, IConfigurationReader
    {
        #region Fields...

        /// <summary>
        /// The root node.
        /// </summary>
        private readonly XmlNode rootNode;

        /// <summary>
        /// THe IMappingStrategyFactory instance.
        /// </summary>
        private readonly IMappingStrategyFactory mappingStrategyFactory;

        #endregion

        #region Constructors...

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rootNode">The root xml node.</param>
        /// <param name="mappingStrategyFactory">THe IMappingStrategyFactory instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="rootNode"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentNullException"><paramref name="mappingStrategyFactory"/> is <see langword="null"/></exception>
        public ConfigurationReader(XmlNode rootNode, IMappingStrategyFactory mappingStrategyFactory)
        {
            if (rootNode == null)
                throw new ArgumentNullException(nameof(rootNode));
            if (mappingStrategyFactory == null)
                throw new ArgumentNullException(nameof(mappingStrategyFactory));

            this.rootNode = rootNode;
            this.mappingStrategyFactory = mappingStrategyFactory;
        }
        #endregion

        /// <summary>
        /// Provides implementation for type conversion operations. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations that convert an object from one type to another.
        /// </summary>
        /// <returns>true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)</returns>
        /// <param name="binder">Provides information about the conversion operation. The binder.Type property provides the type to which the object must be converted. For example, for the statement (String)sampleObject in C# (CType(sampleObject, Type) in Visual Basic), where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Type returns the <see cref="T:System.String" /> type. The binder.Explicit property provides information about the kind of conversion that occurs. It returns true for explicit conversion and false for implicit conversion.</param>
        /// <param name="result">The result of the type conversion operation.</param>
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = ReadObject(binder.ReturnType);
            return true;
        }

        #region Implementation of IConfigurationReader

        /// <summary>
        /// Read object.
        /// </summary>
        /// <param name="type">The object's type.</param>
        /// <returns>The object.</returns>
        public object ReadObject(Type type)
        {
            object obj = ReadObject(type, rootNode);
            return obj;
        }

        /// <summary>
        /// Read object from the node.
        /// </summary>
        /// <param name="type">The object's type.</param>
        /// <param name="xmlNode">The node.</param>
        /// <returns>The object.</returns>
        public object ReadObject(Type type, XmlNode xmlNode)
        {
            //if (type.Name.ToLower() != xmlNode.Name.ToLower())
            //    throw new InvalidCastException("Cannot convert " + xmlNode.Name + " into the " + type.Name);

            object obj = Activator.CreateInstance(type);

            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            Dictionary<string, PropertyInfo> attributes = new Dictionary<string, PropertyInfo>();
            Dictionary<string, ICustomMappingStrategy> customStrategies = new Dictionary<string, ICustomMappingStrategy>();
            List<string> ignoreList = new List<string>();
            
            IEnumerable<PropertyInfo> props = type.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PropertyMappingAttribute)));
            foreach (PropertyInfo propertyInfo in props)
            {
                PropertyMappingAttribute attr = (PropertyMappingAttribute) Attribute.GetCustomAttribute(propertyInfo, typeof(PropertyMappingAttribute));
                attributes.Add(attr.Name.ToLower(), propertyInfo);
            }

            props = type.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(CustomStrategyAttribute)));
            foreach (PropertyInfo propertyInfo in props)
            {
                CustomStrategyAttribute attr = (CustomStrategyAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(CustomStrategyAttribute));
                ICustomMappingStrategy strategy = (ICustomMappingStrategy) Activator.CreateInstance(attr.StrategyType);
                customStrategies.Add(propertyInfo.Name.ToLower(), strategy);
            }

            props = type.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(IgnorePropertyAttribute)));
            foreach (PropertyInfo propertyInfo in props)
            {
                ignoreList.Add(propertyInfo.Name.ToLower());
            }

            if (xmlNode.Attributes != null)
            {
                for (int i = 0; i < xmlNode.Attributes.Count; i++)
                {
                    XmlAttribute xmlAttribute = xmlNode.Attributes[i];
                    string attributeName = xmlAttribute.Name.ToLower();
                    string attributeValue = xmlAttribute.Value;

                    if (ignoreList.Contains(attributeName))
                        continue;

                    PropertyInfo propertyInfo;
                    if (attributes.TryGetValue(attributeName, out propertyInfo) == false)
                        propertyInfo = propertyInfos.Single(x => x.Name.ToLower() == attributeName);

                    object value;
                    if (customStrategies.ContainsKey(propertyInfo.Name.ToLower()))
                    {
                        ICustomMappingStrategy customMappingStrategy = customStrategies[propertyInfo.Name.ToLower()];
                        value = customMappingStrategy.Map(attributeValue, propertyInfo.PropertyType);
                    }
                    else
                    {
                        IPrimitiveMappingStrategy mappingStrategy = mappingStrategyFactory.CreatePrimitiveStrategy(propertyInfo.PropertyType);
                        value = mappingStrategy.Map(attributeValue, propertyInfo.PropertyType);
                    }
                    propertyInfo.SetValue(obj, value, null);
                }
            }

            if (xmlNode.HasChildNodes)
            {
                for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
                {
                    XmlNode child = xmlNode.ChildNodes[i];                   
                    if (child.NodeType != XmlNodeType.Element)
                        continue;

                    string childName = child.Name.ToLower();
                    if (ignoreList.Contains(childName))
                        continue;

                    PropertyInfo propertyInfo;                    
                    if (attributes.TryGetValue(childName, out propertyInfo) == false)
                        propertyInfo = propertyInfos.Single(x => x.Name.ToLower() == childName);

                    if (customStrategies.ContainsKey(propertyInfo.Name.ToLower()))
                    {
                        ICustomMappingStrategy customMappingStrategy = customStrategies[propertyInfo.Name.ToLower()];
                        var value = customMappingStrategy.Map(child, propertyInfo.PropertyType);
                        propertyInfo.SetValue(obj, value, null);
                        continue;
                    }

                    Type propertyType = propertyInfo.PropertyType;
                    if (propertyType.IsPrimitive || propertyType == typeof(string) || propertyType.IsEnum)
                    {
                        IPrimitiveMappingStrategy mappingStrategy = mappingStrategyFactory.CreatePrimitiveStrategy(propertyInfo.PropertyType);
                        var value = mappingStrategy.Map(child.InnerText, propertyInfo.PropertyType);
                        propertyInfo.SetValue(obj, value, null);
                    }
                    else
                    {                       
                        IMappingStrategy mappingStrategy = mappingStrategyFactory.CreateComplexStrategy(propertyInfo.PropertyType);                      
                        var value = mappingStrategy.Map(child, propertyInfo.PropertyType, this);
                        propertyInfo.SetValue(obj, value, null);
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// Read object.
        /// </summary>
        /// <typeparam name="TObject">The object's type.</typeparam>
        /// <returns>The object.</returns>
        public TObject ReadObject<TObject>()
        {
            Type type = typeof(TObject);
            return (TObject) ReadObject(type);
        }

        #endregion
    }
}
