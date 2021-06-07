using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace API.Seguimiento.Extensiones
{
    public class XsltExtension
    {
        private readonly string _basePathCatalogos;
        public XsltExtension(string basePathCatalogos)
        {
            _basePathCatalogos = basePathCatalogos;
        }

        public bool Matches(string pattern, string input)
        {
            return Regex.IsMatch(input, pattern);
        }

        public bool ExistsItemCatalogo(string catalogo, string xPath)
        {
            var xmlPath = _basePathCatalogos + catalogo;

            XPathDocument myXPathDocument = new XPathDocument(xmlPath);
            var navigator = myXPathDocument.CreateNavigator();
            var nodes = navigator.Select(xPath);

            return nodes.Count > 0;
        }
    }
}
