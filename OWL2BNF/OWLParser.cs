using System;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace OWL2BNF
{
    class OWLParser
    { 
        public static List<ClassObject> GetClasses(string file)
        {
            XDocument xdoc = XDocument.Load(file);
            var ont = xdoc.Root;
            string nameSpace = "{" + ont.Name.Namespace.ToString() + "}";
            var pref = ont.Elements(nameSpace + "Prefix").ToList();
            List<string> prefixes = new List<string>();
            foreach (var prefix in pref)
            {
                prefixes.Add(prefix.Attribute("name").Value + ":");
            }

            Dictionary<string, ClassObject> classes = new Dictionary<string, ClassObject>();
            List<string> properties = new List<string>();

            string GetAttribute(XAttribute attribute)
            {
                string name = attribute.Name.LocalName;
                if (name == "IRI") return attribute.Value.TrimStart('#');
                else if (name == "abbreviatedIRI") 
                {
                    foreach (var prefix in prefixes)
                    {
                        string val = attribute.Value;
                        if (val.StartsWith(prefix))
                        {
                            val = val.Replace(prefix, "");
                            return val;
                        }
                    }
                }
                throw new Exception();
            }

            var declaration = ont.Elements(nameSpace + "Declaration").ToList();
            foreach (var decl in declaration)
            {
                if (decl.Element(nameSpace + "Class") != null) 
                {
                    string name = GetAttribute(decl.Element(nameSpace + "Class").FirstAttribute);
                    classes.Add(name, new ClassObject(name)); 
                }
                else if (decl.Element(nameSpace + "ObjectProperty") != null) { properties.Add(GetAttribute(decl.Element(nameSpace + "ObjectProperty").FirstAttribute)); }
            }

            var propperties = ont.Elements(nameSpace + "SubClassOf").ToList();
            foreach (var prop in propperties)
            {
                string className = GetAttribute(prop.Element(nameSpace + "Class").FirstAttribute);
                string propertyName = GetAttribute(prop.Element(nameSpace + "ObjectAllValuesFrom").Element(nameSpace + "ObjectProperty").FirstAttribute);
                string propertyClassName = GetAttribute(prop.Element(nameSpace + "ObjectAllValuesFrom").Element(nameSpace + "Class").FirstAttribute);
                if (properties.Contains(propertyName)) classes[className].properties.Add(new PropertyObject(propertyName, propertyClassName));
            }
            return classes.Values.ToList<ClassObject>();
        }
    }
}
