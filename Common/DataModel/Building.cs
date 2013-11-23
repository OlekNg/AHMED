using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Common.DataModel
{
    [Serializable]
    public class Building
    {
        public List<Floor> Floors;
        public List<StairsPair> Stairs;

        public Building()
        {
            Floors = new List<Floor>();
            Stairs = new List<StairsPair>();
        }

        public void Load(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Building));

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                Building b = (Building)serializer.Deserialize(fs);
                Floors = b.Floors;
                Stairs = b.Stairs;
            }
        }

        public void Save(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Building));

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(fs, this);
            }
        }
    }
}
