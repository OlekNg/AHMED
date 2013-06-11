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
        private string _fileName;

        public PeopleMap ReadPeopleMap(string filepath)
        {
            PeopleMap result = new PeopleMap();
            XDocument doc;

            _fileName = filepath;
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
    }
}
