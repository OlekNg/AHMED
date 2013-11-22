using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BuildingEditor.DataModel
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

        public Building(BuildingEditor.Logic.Building building)
            : this()
        {
            for(int i = 0; i < building.Floors.Count; i++)
                Floors.Add(new Floor(building.Floors[i]));

            for (int i = 0; i < building.Stairs.Count; i++)
                Stairs.Add(new StairsPair(building.Stairs[i]));
        }

        public BuildingEditor.Logic.Building ToViewModel()
        {
            return new BuildingEditor.Logic.Building(this);
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
