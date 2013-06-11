using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Structure;

namespace Readers
{
    public class XMLReader
    {
        public PeopleMap ReadPeopleMap(string filepath)
        {
            PeopleMap result = new PeopleMap();
            XDocument doc;

            try
            {
                doc = XDocument.Load(filepath);
                var peopleGroups = doc.Descendants("group");
                foreach (var group in peopleGroups)
                {
                    result.People.Add(new PeopleGroup(uint.Parse(group.Attribute("row").Value),
                                                      uint.Parse(group.Attribute("col").Value),
                                                      uint.Parse(group.Attribute("quantity").Value)));
                }
            }
            catch (Exception)
            {
                return null;
            }


            return result;
        }

        public BuildingMap ReadBuildingMap(string filepath)
        {
            BuildingMap result = new BuildingMap();
            XDocument doc;

            try
            {
                doc = XDocument.Load(filepath);
                /*var peopleGroups = doc.Descendants("group");
                foreach (var group in peopleGroups)
                {
                    result.People.Add(new PeopleGroup(uint.Parse(group.Attribute("row").Value),
                                                      uint.Parse(group.Attribute("col").Value),
                                                      uint.Parse(group.Attribute("quantity").Value)));
                }*/
                result.Setup(uint.Parse(doc.Root.Attribute("w").Value),
                             uint.Parse(doc.Root.Attribute("h").Value),
                             uint.Parse(doc.Root.Attribute("std_eff").Value));
                var floors = doc.Descendants("floors").Descendants("square");
                foreach (var f in floors)
                {
                    result.SetFloor(uint.Parse(f.Attribute("row").Value),
                                    uint.Parse(f.Attribute("col").Value),
                                    uint.Parse(f.Attribute("capacity").Value));
                }
                var walls = doc.Descendants("walls").Descendants("column");
                foreach (var w in walls)
                {
                    result.SetWall(uint.Parse(w.Attribute("row").Value),
                                   uint.Parse(w.Attribute("col").Value),
                                   WallPosition.LEFT);
                }
                walls = doc.Descendants("walls").Descendants("rows");
                foreach (var w in walls)
                {
                    result.SetWall(uint.Parse(w.Attribute("row").Value),
                                   uint.Parse(w.Attribute("col").Value),
                                   WallPosition.TOP);
                }
                var doors = doc.Descendants("doors").Descendants("column");
                foreach (var d in doors)
                {
                    result.SetDoor(uint.Parse(d.Attribute("row").Value),
                                   uint.Parse(d.Attribute("col").Value),
                                   uint.Parse(d.Attribute("efficiency").Value),
                                   WallPosition.LEFT);
                }
                doors = doc.Descendants("doors").Descendants("row");
                foreach (var d in doors)
                {
                    result.SetDoor(uint.Parse(d.Attribute("row").Value),
                                   uint.Parse(d.Attribute("col").Value),
                                   uint.Parse(d.Attribute("efficiency").Value),
                                   WallPosition.TOP);
                }
            }
            catch (Exception)
            {
                return null;
            }


            return result;
        }
    }
}
