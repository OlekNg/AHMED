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

        private EvacuationElement[][] _evacuationMap;

        private EvacuationGroup[] _groupsToEvacuate;

        public void SetupSimulator()
        {
            if (BuildingMap == null) return;
            if (PeopleMap == null) return;

            _evacuationMap = new EvacuationElement[BuildingMap.Width][];
            for (uint i = 0; i < BuildingMap.Width; ++i)
            {
                _evacuationMap[i] = new EvacuationElement[BuildingMap.Height];
                for (int j = 0; j < BuildingMap.Height; ++j)
                {
                    _evacuationMap[i][j].FloorSquare = BuildingMap.Floor[i][j];
                }
            }
        }

        public PeopleGroup[] Simulate(bool[] genotype)
        {
            foreach (EvacuationElement[] e in _evacuationMap)
            {
                foreach (EvacuationElement element in e)
                {
                    //change nextroom, passage and so on
                }
            }

            for (uint i = 0; i < MaximumTicks; ++i)
            {
                foreach(EvacuationGroup group in _groupsToEvacuate){
                    if (!group.Processed)
                    {
                        //TODO: magic
                        Process(group);

                       
                    }
                }

                //reset Processed
            }

            return null;
        }

        private EvacuationGroup Process(EvacuationGroup group)
        {
            //TODO: magic^2
            EvacuationGroup targetGroup;

            group.Processed = true;

            if (group.Position.NextStep == null)
            {
                //group finally evacuated, yeah
                if (group.Position.Passage.CanPassThrough())
                {
                    uint passageEfficency = group.Position.Passage.Capacity();
                    if (group.Quantity <= passageEfficency)
                    {
                        //whole group escaped
                        return group;
                    }
                    else
                    {
                        //partial group escaped

                    }
                }
                else
                {
                    //there is a wall
                    //error
                }
            }

            return null;

            /*
            if ((targetGroup = group.Position.NextStep.EvacuatingGroup) == null)
            {

            }
            else
            {
                //if(targetGroup
            }
            */
        }
    }
}
