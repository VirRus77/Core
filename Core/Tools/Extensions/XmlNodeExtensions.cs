using System.Xml;

namespace Core.Tools.Extensions
{
    public static class XmlNodeExtensions
    {
        /// <summary>
        /// Добавить новый <see cref="XmlElement"/>
        /// </summary>
        /// <param name="node">К какой ноде добавить</param>
        /// <param name="name">Имя нового элемента</param>
        /// <returns>Ссылку на созданный элемент</returns>
        public static XmlElement AddElement(this XmlNode node, string name)
        {
            var xmlDocument = node as XmlDocument ?? node.OwnerDocument;
            var newNode = (XmlElement)xmlDocument.CreateNode(XmlNodeType.Element, name, null);
            node.AppendChild(newNode);
            return newNode;
        }

        /// <summary>
        /// Добавить новый <see cref="XmlAttribute"/>
        /// </summary>
        /// <param name="node">К какой ноде добавить</param>
        /// <param name="name">Имя нового аттрибута</param>
        /// <param name="value">Значение нового аттрибута</param>
        /// <returns>Ссылку на созданный аттрибут</returns>
        public static XmlAttribute AddAttribute(this XmlNode node, string name, string value)
        {
            var xmlDocument = node.OwnerDocument;
            var newAttribute = (XmlAttribute)xmlDocument.CreateNode(XmlNodeType.Attribute, name, null);
            newAttribute.Value = value;
            node.Attributes.Append(newAttribute);
            return newAttribute;
        }

        /// <summary>
        /// Добавить новый <see cref="XmlAttribute"/>
        /// </summary>
        /// <param name="node">К какой ноде добавить</param>
        /// <param name="name">Имя нового аттрибута</param>
        /// <param name="value">Значение нового аттрибута</param>
        /// <returns>Ссылку на созданный аттрибут</returns>
        public static XmlAttribute AddAttribute(this XmlNode node, string name, object value)
        {
            var xmlDocument = node.OwnerDocument;
            var newAttribute = (XmlAttribute)xmlDocument.CreateNode(XmlNodeType.Attribute, name, null);
            newAttribute.Value = $"{value}";
            node.Attributes.Append(newAttribute);
            return newAttribute;
        }

        /// <summary>
        /// Получить значение аттрибута
        /// </summary>
        /// <param name="node">У какой ноды взять аттрибут</param>
        /// <param name="attributeName">Имя аттрибута</param>
        /// <param name="defaultValue">Значение по умолчани если не найден аттрибут</param>
        /// <returns>Значение аттрибута если он есть, иначе значение по умолчанию</returns>
        public static string GetAttributeValue(this XmlNode node, string attributeName, string defaultValue = null)
        {
            if (node.Attributes == null)
                return defaultValue;
            var attribute = node.Attributes[attributeName];
            if (attribute == null)
                return defaultValue;
            return attribute.Value;
        }
    }
}
