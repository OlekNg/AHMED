using Common.DataModel.Enums;
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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Building));

        public List<Floor> Floors;
        public List<StairsPair> Stairs;

        public Building()
        {
            Floors = new List<Floor>();
            Stairs = new List<StairsPair>();
        }

        public void Load(string path)
        {
            log.Debug(String.Format("Loading building from file {0}", path));
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
            log.Debug(String.Format("Saving building to file {0}", path));
            XmlSerializer serializer = new XmlSerializer(typeof(Building));

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(fs, this);
            }
        }
    }
}
