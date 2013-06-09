using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structure;
using Genetics;


namespace Simulation
{
    public class Simulator
    {
        private BuildingMap _buildingMap;

        private PeopleMap _peopleMap;

        public uint MaximumTicks { get; set; }

        private EvacuationMap _evacuationMap = new EvacuationMap();

        private List<EvacuationElement> _evacuationGroups = new List<EvacuationElement>();

        private List<EscapedGroup> _escapedGroups = new List<EscapedGroup>();

        public void SetupSimulator(BuildingMap bm, PeopleMap pm)
        {
            if (bm == null) return;
            if (pm == null) return;

            _buildingMap = bm;
            _peopleMap = pm;

            _evacuationMap.InitializeFromBuildingMap(_buildingMap);
        }

        public List<EscapedGroup> Simulate(Chromosome genotype)
        {
            //setup current situation
            _evacuationGroups.Clear();
            _evacuationMap.ResetPeopleGroups();
            foreach (PeopleGroup group in _peopleMap.People)
            {
                _evacuationMap.SetPeopleGroup(group);
                _evacuationGroups.Add(_evacuationMap.Get(group.Row, group.Col));
            }
            _evacuationMap.MapGenotype(genotype);

            //start simulation
            for (uint i = 1; i <= MaximumTicks; ++i)
            {
                for (int j = _evacuationGroups.Count - 1; j >= 0; --j)
                    Process(_evacuationGroups[j], i);

                for (int j = _evacuationGroups.Count - 1; j >= 0; --j)
                {
                    if (_evacuationGroups[j].PeopleQuantity == 0)
                        _evacuationGroups.RemoveAt(j);
                    else 
                        _evacuationGroups[j].Processed = false;
                }

            }

            return _escapedGroups;
        }

        private void Process(EvacuationElement group, uint tick)
        {
            uint peopleCount;
            EvacuationElement nextStep = group.NextStep;

            if (group.Processed == true) return;

            group.Processed = true;
            group.Ticks = tick;

            if (nextStep == null)
            {
                //group finally evacuated, yeah
                if (group.Passage.CanPassThrough)
                {
                    peopleCount = Math.Min(group.Passage.Capacity, group.PeopleQuantity);

                    _escapedGroups.Add(new EscapedGroup(peopleCount, tick));
                    group.PeopleQuantity -= peopleCount;
                    
                    return;
                }
                else
                {
                    //there is a wall
                    //error
                }
            }

            //there is more steps to do in evacuation route
            if(nextStep.PeopleQuantity != 0)
                Process(nextStep, tick);

            peopleCount = nextStep.PeopleQuantityLeft;
            if (peopleCount == 0)
            {
                //there is no room for anybody
                return;
            }
            if (group.Passage != null)
            {
                if (group.Passage.CanPassThrough)
                {
                    peopleCount = Math.Min(peopleCount, group.Passage.Capacity);
                }
                else
                {
                    //ooops, something went wrong, wall found
                }
            }

            peopleCount = Math.Min(peopleCount, group.PeopleQuantity);
            nextStep.PeopleQuantity += peopleCount;
            //TODO: maybe there is no need for this
            nextStep.Processed = true;
            group.PeopleQuantity -= peopleCount;
            _evacuationGroups.Add(nextStep);
        }
    }
}
