using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Xml;
using Microsoft.Web.XmlTransform;

namespace XMLTransformer.Shared
{
    public class XmlTransformService
    {
        public string Transform(string sourceXml, string transformXml)
        {
            // Remove the BOM (Byte Order Mark) from the source XML if it's there.
            // This will most likely be the case when you try to call this multiple times.
            sourceXml = RemoveBOM3(sourceXml);
            
            using (var document = new XmlTransformableDocument())
            {
                document.PreserveWhitespace = true;
                document.LoadXml(sourceXml);

                using (var transform = new XmlTransformation(transformXml, false, null))
                {
                    if (transform.Apply(document))
                    {
                        var stringBuilder = new StringBuilder();
                        var xmlWriterSettings = new XmlWriterSettings();
                        xmlWriterSettings.Indent = true;
                        xmlWriterSettings.IndentChars = "    ";
                        
                        using (var xmlTextWriter = XmlTextWriter.Create(stringBuilder, xmlWriterSettings))
                        using (var ms = new MemoryStream())
                        {
                            document.Save(ms);
                            ms.Seek(0, SeekOrigin.Begin);
                            ms.Flush();

                            var bytes = ms.ToArray();
                            return System.Text.Encoding.UTF8.GetString(bytes);
                        }
                    }
                    else
                    {
                        throw new Exception("Transformation failed.");
                    }
                }
            }
        }

        private string RemoveBOM(string xml)
        {
            // https://stackoverflow.com/questions/17795167/xml-loaddata-data-at-the-root-level-is-invalid-line-1-position-1
            var preamble = Encoding.UTF8.GetPreamble();
            string byteOrderMarkUtf8 = Encoding.UTF8.GetString(preamble);
            if (xml.StartsWith(byteOrderMarkUtf8))
            {
                xml = xml.Remove(0, byteOrderMarkUtf8.Length);
            }

            return xml;
        }

        private string RemoveBOM2(string xml)
        {
            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/types/how-to-convert-a-byte-array-to-an-int
            var preamble = System.Text.Encoding.UTF8.GetPreamble();
            if (BitConverter.IsLittleEndian)
                Array.Reverse(preamble);
            
            var preambleChar = BitConverter.ToChar(preamble, 0);
            if (preambleChar == xml[0])
            {
                return xml.Remove(0, 1);
            }

            return xml;
        }

        private string RemoveBOM3(string xml)
        {
            int index = xml.IndexOf('<');
            if (index > 0)
            {
                xml = xml.Substring(index, xml.Length - index);
            }

            return xml;
        }
    }
}