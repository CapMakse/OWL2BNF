using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace OWL2BNF
{
    class Grammar
    {
        public List<ClassObject> classes { get; set; } = new List<ClassObject>();
        public Grammar(List<ClassObject> classes)
        {
            this.classes = classes;
        }
        public void WriteToFile(string path)
        {
            using (StreamWriter str = new StreamWriter(path, false))
            {
                str.Write(this.ToString());
            }
        }
        public override string ToString()
        {
            string ret = "Thing : Thing '{' ";
            if (classes.Count != 0)
            {
                ret += "(";
                ret += classes[0].className;
                for (int i = 1; i < classes.Count; i++)
                {
                    ret += "|";
                    ret += classes[i].className;
                }
                ret += ")";
            }
            ret += " '}'";
            foreach (var cls in classes)
            {
                ret += "\n";
                ret += cls.ToString();
            }
            return ret;
        }
    }
    class ClassObject
    {
        public string className { get; set; }
        public List<PropertyObject> properties { get; set; } = new List<PropertyObject>();

        public ClassObject(string name)
        {
            className = name;
        }
        public override string ToString()
        {
            string ret = className + " : " + className + " '{' ";
            if (properties.Count != 0)
            {
                ret += "[";
                ret += className + "_" + properties[0].propertyName + "_" + properties[0].className;
                for (int i = 1; i < properties.Count; i++)
                {
                    ret += ", ";
                    ret += className + "_" + properties[i].propertyName + "_" + properties[i].className;
                }
                ret += "]";
            }
            ret += " '};'";
            foreach (var prop in properties)
            {
                ret += "\n";
                string pr = className + "_" + prop.propertyName + "_" + prop.className;
                ret += pr + " : '{' '" + prop.propertyName + "' '" + prop.className + "' '" + prop.className + "ID '}';";
            }
            return ret;
        }
    }
    class PropertyObject
    {
        public string propertyName { get; set; }
        public string className { get; set; }
        public PropertyObject(string propertyName, string className)
        {
            this.propertyName = propertyName;
            this.className = className;
        }
    }
}
