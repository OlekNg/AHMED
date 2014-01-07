using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structure;
using Simulation.Exceptions;
using Common.DataModel.Enums;

namespace Simulation
{
    /// <summary>
    /// Main class for simulator
    /// </summary>
    public class Simulator
    {
        /// <summary>
        /// Building map
        /// </summary>
        private BuildingMap _buildingMap;

        /// <summary>
        /// People map
        /// </summary>
        private PeopleMap _peopleMap;

        /// <summary>
        /// Maximum ticks for simulation
        /// </summary>
        public int MaximumTicks { get; set; }

        /// <summary>
        /// Evacuation map
        /// </summary>
        private EvacuationMap _evacuationMap = new EvacuationMap();

        /// <summary>
        /// List of evacuation elements which needs evacuate people
        /// </summary>
        private List<EvacuationElement> _evacuationGroups = new List<EvacuationElement>();

        /// <summary>
        /// Groups which have already escaped
        /// </summary>
        private List<EscapedGroup> _escapedGroups = new List<EscapedGroup>();

        /// <summary>
        /// Initialize simulator for given building and people map
        /// </summary>
        /// <param name="bm">Building map</param>
        /// <param name="pm">People map</param>
        public void SetupSimulator(BuildingMap bm, PeopleMap pm)
        {
            if (bm == null) return;
            if (pm == null) return;

            _buildingMap = bm;
            _peopleMap = pm;

            _evacuationMap.InitializeFromBuildingMap(_buildingMap);
        }

        /// <summary>
        /// Simulate given fenotype
        /// </summary>
        /// <param name="fenotype">Fenotype</param>
        /// <returns>List of escaped groups</returns>
        public List<EscapedGroup> Simulate(List<List<Direction>> fenotype)
        {
            _escapedGroups.Clear();

            //reset
            _evacuationGroups.Clear();
            _evacuationMap.ResetPeopleGroups();

            //setup current situation
            foreach (PeopleGroup group in _peopleMap.People)
            {
                _evacuationMap.SetPeopleGroup(group);
                _evacuationGroups.Add(_evacuationMap.Get(group.Floor, group.Row, group.Col));
            }
            _evacuationMap.MapFenotype(fenotype);

            //start simulation
            for (int i = 1; i <= MaximumTicks; ++i)
            {
                //processing tiles
                for (int j = _evacuationGroups.Count - 1; j >= 0; --j)
                    Process(_evacuationGroups[j], i);

                //resetting 
                for (int j = _evacuationGroups.Count - 1; j >= 0; --j)
                {
                    _evacuationGroups[j].ResetProcessing();

                    if (!_evacuationGroups[j].ContainsPeople())
                        _evacuationGroups.RemoveAt(j);
                }
            }

            return _escapedGroups;
        }

        /// <summary>
        /// Process one element of evacuation route
        /// </summary>
        /// <param name="group">Evacuation element to process</param>
        /// <param name="tick">Process given element with this tick</param>
        private void Process(EvacuationElement group, int tick)
        {
            int peopleCount;
            EvacuationElement nextStep = group.NextStep;

            if (group.Processed == true) return;

            group.StartProcessing();

            if (nextStep == null)
            {
                //group finally evacuated, yeah
                peopleCount = Math.Min(group.Passage.Efficiency, group.PeopleQuantity);

                if (peopleCount > 0)
                {
                    _escapedGroups.Add(new EscapedGroup(peopleCount, tick));
                    group.RemovePeople(peopleCount);
                }
                    
                return;
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
  
            peopleCount = Math.Min(Math.Min(peopleCount, group.Passage.Efficiency), group.PeopleQuantity);
            if (peopleCount != 0)
            {
                if (!nextStep.Processed)
                {
                    //new field for processing
                    _evacuationGroups.Add(nextStep);
                }

                nextStep.Processed = true; //prevent from multiple moves
                //move people
                nextStep.AddPeople(peopleCount);
                group.RemovePeople(peopleCount);
            }
        }
    }
}
