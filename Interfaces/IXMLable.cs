using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace big
{

    public interface IXMLable
    {
        // public XElement printinXML();

        public string getType();

    }


    public static class XMLInterface
    {
        
        public static XElement printinXML()
        {
            return new XElement("XMLInterface");
        }
        private static string path = Environment.CurrentDirectory + "/XML/";

        public static void SaveXML()
        {
            XDocument doc = new XDocument();
            doc.Add(printinXML());
            doc.Save(path);
        }

        public static void LoadXML()
        {
            XDocument doc = XDocument.Load(path);
            XElement root = doc.Root;
            // var xml = root.Elements("XMLInterface").Select(x => new XMLInterface
            // {
            //     // Name = x.Element("Name").Value,
            //     // Age = int.Parse(x.Element("Age").Value),
            //
        }

        public static void DumpAllIntoXML()
        {
            // foreach (var item in )
            // {
            //     
            // }
        }
        

        public static void AddObjectToXML(IXMLable objectToAdd)
        {
            XmlSerializer seralizer;
            switch (objectToAdd.getType())
            {
                case "Team":
                    seralizer = new XmlSerializer(typeof(Team));
                    path = path + "Teams.xml";
                    break;
                case "User":
                    seralizer = new XmlSerializer(typeof(User));
                    path = path + "Users.xml";
                    break;
                case "UserMMR":
                    seralizer = new XmlSerializer(typeof(UserMMR));
                    path = path + "UserMMR.xml";
                    break;
                case "TeamMMR":
                    seralizer = new XmlSerializer(typeof(TeamMMR));
                    path = path + "TeamMMR.xml";
                    break;
                case "UserGames":
                    seralizer = new XmlSerializer(typeof(UserGames));
                    path = path + "UserGames.xml";
                    break;
                default:
                    seralizer = new XmlSerializer(typeof(IXMLable));
                    path = path + "IXMLable.xml";
                    break;
            }
            
            using (var writer = new StreamWriter(path))
            {
                seralizer.Serialize(writer, objectToAdd);
            }
        }

        private static void PushToXML()
        {
            
        }

        public static void AddObjectToXML(List<IXMLable> objectsToAdd)
        {
            
            XmlSerializer seralizer;
            switch (objectsToAdd[0].getType())
            {
                case "Team":
                    seralizer = new XmlSerializer(typeof(Team));
                    path = path + "Teams.xml";
                    break;
                case "User":
                    seralizer = new XmlSerializer(typeof(User));
                    path = path + "Users.xml";
                    break;
                case "UserMMR":
                    seralizer = new XmlSerializer(typeof(UserMMR));
                    path = path + "UserMMR.xml";
                    break;
                case "TeamMMR":
                    seralizer = new XmlSerializer(typeof(TeamMMR));
                    path = path + "TeamMMR.xml";
                    break;
                case "UserGames":
                    seralizer = new XmlSerializer(typeof(UserGames));
                    path = path + "UserGames.xml";
                    break;
                default:
                    seralizer = new XmlSerializer(typeof(IXMLable));
                    path = path + "IXMLable.xml";
                    break;
            }
            
            using (var writer = new StreamWriter(path))
            {                
                seralizer.Serialize(writer, objectsToAdd);
            }

            


        }
    }
}

