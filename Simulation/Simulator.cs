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

        private List<EvacuationElement> _evacuationGroups = new List<EvacuationElement>();

        private List<EscapedGroup> _escapedGroups = new List<EscapedGroup>();

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
                if (group.Passage.CanPassThrough())
                {
                    uint passageEfficency = group.Passage.Capacity();
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
                    if (nextStep.Passage.CanPassThrough())
                    {
                        //door, uff
                        maximumCapacity = Math.Min(maximumCapacity, nextStep.Passage.Capacity());
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
                    if (nextStep.Passage.CanPassThrough())
                    {
                        //door, uff
                        maximumCapacity = Math.Min(maximumCapacity, nextStep.Passage.Capacity());
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
