using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structure;

namespace Readers
{
    public class XMLReader
    {
        private string _fileName;

        public PeopleMap ReadPeopleMap(string filepath)
        {
            PeopleMap result = new PeopleMap();

            _fileName = filepath;


            return result;
        }
    }
}
