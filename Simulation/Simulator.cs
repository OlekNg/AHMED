using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structure;

namespace Simulation
{
    public class Simulator
    {
        public BuildingMap BuildingMap { get; set; }

        public PeopleMap PeopleMap { get; set; }

        public uint MaximumTicks { get; set; }

        public PeopleGroup[] Simulate()
        {
            for (uint i = 0; i < MaximumTicks; ++i)
            {
                foreach(PeopleGroup group in PeopleMap.People){
                    if (!group.Processed)
                    {
                        //TODO: magic


                       
                    }
                }
            }

            return null;
        }

        public void MapGenotype(bool[] genotype)
        {
            BuildingMap.MapGenotype(genotype);
        }

        private void Process(PeopleGroup group)
        {
            //TODO: magic^2

            group.Processed = true;

            
        }
    }
}
