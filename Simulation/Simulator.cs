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
        private BuildingMap _buildingMap;

        private PeopleMap _peopleMap;

        public uint MaximumTicks { get; set; }

        private EvacuationElement[][] _evacuationMap;

        private List<EvacuationElement> _evacuationGroups = new List<EvacuationElement>();

        private List<EscapedGroup> _escapedGroups = new List<EscapedGroup>();

        public void SetupSimulator(BuildingMap bm, PeopleMap pm)
        {
            if (bm == null) return;
            if (pm == null) return;

            _buildingMap = bm;
            _peopleMap = pm;

            _evacuationMap = new EvacuationElement[_buildingMap.Width][];
            for (uint i = 0; i < _buildingMap.Width; ++i)
            {
                _evacuationMap[i] = new EvacuationElement[_buildingMap.Height];
                for (int j = 0; j < _buildingMap.Height; ++j)
                {
                    _evacuationMap[i][j].FloorSquare = _buildingMap.Floor[i][j];
                }
            }
        }

        public PeopleGroup[] Simulate(bool[] genotype)
        {
            //setup current situation
            foreach (EvacuationElement[] e in _evacuationMap)
            {
                foreach (EvacuationElement element in e)
                {
                    //change nextroom, passage and so on
                }
            }

            for (uint i = 0; i < MaximumTicks; ++i)
            {
                for (int j = _evacuationGroups.Count - 1; j >= 0; --j)
                {
                    Process(_evacuationGroups[j], i);
                }

                for (int j = _evacuationGroups.Count - 1; j >= 0; --j)
                {
                    if (_evacuationGroups[j].PeopleQuantity == 0)
                    {
                        _evacuationGroups.RemoveAt(j);
                    }
                    else
                    {
                        _evacuationGroups[j].Processed = false;
                    }
                }

            }

            return null;
        }

        private void Process(EvacuationElement group, uint tick)
        {
            uint targetGroupQuantity;
            EvacuationElement nextStep = group.NextStep;

            if (group.Processed == true) return;

            group.Processed = true;
            group.Ticks = tick;

            if (nextStep == null)
            {
                //group finally evacuated, yeah
                if (group.Passage.CanPassThrough)
                {
                    uint passageEfficency = group.Passage.Capacity;
                    if (group.PeopleQuantity <= passageEfficency)
                    {
                        //whole group escaped
                        group.PeopleQuantity = 0;
                        _escapedGroups.Add(new EscapedGroup(group));
                        return;
                    }
                    else
                    {
                        //partial group escaped
                        group.PeopleQuantity -= passageEfficency;
                        _escapedGroups.Add(new EscapedGroup(group));
                        return;
                    }
                }
                else
                {
                    //there is a wall
                    //error
                }
            }

            //there is more steps to do in evacuation route
            targetGroupQuantity = nextStep.PeopleQuantity;

            if (targetGroupQuantity == 0)
            {
                //there is no other group on the next step
                uint maximumCapacity = nextStep.FloorSquare.Capacity;
                if (nextStep.Passage != null)
                {
                    //there is wall or door
                    if (nextStep.Passage.CanPassThrough)
                    {
                        //door, uff
                        maximumCapacity = Math.Min(maximumCapacity, nextStep.Passage.Capacity);
                        if (group.PeopleQuantity <= maximumCapacity)
                        {
                            //move whole group
                            nextStep.PeopleQuantity = group.PeopleQuantity;
                            nextStep.Processed = true;
                            group.PeopleQuantity = 0;
                            _evacuationGroups.Add(nextStep);
                        }
                        else
                        {
                            //move part of group
                            nextStep.PeopleQuantity = maximumCapacity;
                            nextStep.Processed = true;
                            group.PeopleQuantity -= maximumCapacity;
                            _evacuationGroups.Add(nextStep);
                        }
                    }
                    else
                    {
                        //wall, wtf?!
                        //error
                    }

                }
                else
                {
                    //there is no wall or door
                    if (group.PeopleQuantity <= maximumCapacity)
                    {
                        //move whole group
                        nextStep.PeopleQuantity = group.PeopleQuantity;
                        nextStep.Processed = true;
                        group.PeopleQuantity = 0;
                        _evacuationGroups.Add(nextStep);
                    }
                    else
                    {
                        //move part of group
                        nextStep.PeopleQuantity = maximumCapacity;
                        nextStep.Processed = true;
                        group.PeopleQuantity -= maximumCapacity;
                        _evacuationGroups.Add(nextStep);
                    }
                }
            }
            else
            {
                //there is somebody
                Process(nextStep, tick);
                uint maximumCapacity = nextStep.PeopleQuantityLeft;
                if (maximumCapacity == 0)
                {
                    //there is no room for anybody
                    return;
                }

                if (nextStep.Passage != null)
                {
                    //there is wall or door
                    if (nextStep.Passage.CanPassThrough)
                    {
                        //door, uff
                        maximumCapacity = Math.Min(maximumCapacity, nextStep.Passage.Capacity);
                        if (group.PeopleQuantity <= maximumCapacity)
                        {
                            //move whole group
                            nextStep.PeopleQuantity = group.PeopleQuantity;
                            nextStep.Processed = true;
                            group.PeopleQuantity = 0;
                            _evacuationGroups.Add(nextStep);
                        }
                        else
                        {
                            //move part of group
                            nextStep.PeopleQuantity = maximumCapacity;
                            nextStep.Processed = true;
                            group.PeopleQuantity -= maximumCapacity;
                            _evacuationGroups.Add(nextStep);
                        }
                    }
                    else
                    {
                        //wall, wtf?!
                        //error
                    }

                }
                else
                {
                    //there is no wall or door
                    if (group.PeopleQuantity <= maximumCapacity)
                    {
                        //move whole group
                        nextStep.PeopleQuantity = group.PeopleQuantity;
                        nextStep.Processed = true;
                        group.PeopleQuantity = 0;
                        _evacuationGroups.Add(nextStep);
                    }
                    else
                    {
                        //move part of group
                        nextStep.PeopleQuantity = maximumCapacity;
                        nextStep.Processed = true;
                        group.PeopleQuantity -= maximumCapacity;
                        _evacuationGroups.Add(nextStep);
                    }
                }
            }

        }
    }
}
