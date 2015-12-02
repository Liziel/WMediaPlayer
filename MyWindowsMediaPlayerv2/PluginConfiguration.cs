using System.IO;
using System.Windows.Documents.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace MyWindowsMediaPlayerv2
{
    namespace Configuration
    {
        public class PluginConfiguration
        {
            public string ToolBarPlugin = "DefaultMWMP2toolbar";
            public string MediaViewPlugin = "DefaultMWMP2mediaview";

            public string Serialized => Serialize(this);

            private static string                   Serialize(PluginConfiguration pluginConfiguration)
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer xmlSerializer = new XmlSerializer(typeof (PluginConfiguration));

                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, pluginConfiguration);
                    xmlStream.Position = 0;
                    xmlDocument.Load(xmlStream);
                }
                return xmlDocument.InnerXml;
            }

            private static PluginConfiguration      Deserialize(string xmlString)
            {
                XmlSerializer oXmlSerializer = new XmlSerializer( typeof (PluginConfiguration) );
                return (PluginConfiguration) oXmlSerializer.Deserialize(new StreamReader(xmlString));
            }
        }
    }
}