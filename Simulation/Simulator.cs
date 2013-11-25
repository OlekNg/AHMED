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
        public uint MaximumTicks { get; set; }

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

            //setup current situation
            _evacuationGroups.Clear();
            _evacuationMap.ResetPeopleGroups();
            foreach (PeopleGroup group in _peopleMap.People)
            {
                _evacuationMap.SetPeopleGroup(group);
                _evacuationGroups.Add(_evacuationMap.Get(group.Floor, group.Row, group.Col));
            }
            _evacuationMap.MapFenotype(fenotype);

            //start simulation
            for (uint i = 1; i <= MaximumTicks; ++i)
            {
                for (int j = _evacuationGroups.Count - 1; j >= 0; --j)
                    Process(_evacuationGroups[j], i);

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
        /// <param name="tick">Process goven element with this tick</param>
        private void Process(EvacuationElement group, uint tick)
        {
            uint peopleCount;
            EvacuationElement nextStep = group.NextStep;

            if (group.Processed == true) return;

            group.StartProcessing();
            group.Ticks = tick;

            if (nextStep == null)
            {
                //group finally evacuated, yeah
                if (group.Passage.CanPassThrough)
                {
                    peopleCount = Math.Min(group.Passage.Efficiency, group.PeopleQuantity);

                    if (peopleCount > 0)
                    {
                        _escapedGroups.Add(new EscapedGroup(peopleCount, tick));
                        group.RemovePeople(peopleCount);
                    }
                    
                    return;
                }
                else
                {
                    //there is a wall
                    throw new UnexpectedWallException();
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
            if (group.Passage.CanPassThrough)
            {
                peopleCount = Math.Min(peopleCount, group.Passage.Efficiency);
            }
            else
            {
                //ooops, something went wrong, wall found
                throw new UnexpectedWallException();
            }

            peopleCount = Math.Min(peopleCount, group.PeopleQuantity);
            if (peopleCount != 0)
            {
                if(!nextStep.Processed)
                    _evacuationGroups.Add(nextStep);

                nextStep.AddPeople(peopleCount);
                //TODO: maybe there is no need for this
                //nextStep.Processed = true;
                group.RemovePeople(peopleCount);
            }
        }
    }
}
