﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulation;
using Structure;
using Genetics;
using Readers;

namespace TestSimulator
{

    //zebysmy sobie nie wchodzili w droge z testowaniem
    //ja bede sie bawil na tym projekcie
    //potem, oczywiscie, sie go usunie
    //Andrzej

    class Program
    {
        static void Main(string[] args)
        {
            const uint w = 7;
            const uint h = 5;
            const uint capacity = 6;
            const uint doorCapacity = 3;
            const uint standardEff = 4;
            BuildingMap map = new BuildingMap();
            PeopleMap pmap/* = new PeopleMap()*/;
            Simulator sim = new Simulator();
            XMLReader reader = new XMLReader();

            map.Setup(w, h, standardEff);
            //add floor
            for (uint i = 0; i < h; ++i)
                for (uint j = 0; j < w; ++j)
                    map.SetFloor(i, j, capacity);
            //add outer walls
            for (uint i = 0; i < w; ++i)
            {
                map.SetWall(0, i, WallPosition.TOP);
                map.SetWall(h, i, WallPosition.TOP);
            }
            for (uint i = 0; i < h; ++i)
            {
                map.SetWall(i, 0, WallPosition.LEFT);
                map.SetWall(i, w, WallPosition.LEFT);
            }
            //add outer doors
            map.SetDoor(3, 0, 10, WallPosition.LEFT);
            map.SetDoor(2, 7, 5, WallPosition.LEFT);

            //set inner walls
            for (uint i = 0; i < 3; ++i)
                map.SetWall(4, 2 + i, WallPosition.LEFT);
            for(uint i = 0; i < 2; ++i)
            {
                map.SetWall(i, 2, WallPosition.LEFT);
                map.SetWall(i, 4, WallPosition.LEFT);
                map.SetWall(3 + i, 5, WallPosition.LEFT);
                map.SetWall(3, 5 + i, WallPosition.TOP);
            }
            for (uint i = 0; i < w; ++i)
                map.SetWall(2, i, WallPosition.TOP);
            map.SetWall(4, 0, WallPosition.TOP);

            //set inner doors
            map.SetDoor(1, 4, doorCapacity, WallPosition.LEFT);
            map.SetDoor(3, 5, doorCapacity, WallPosition.LEFT);
            for (uint i = 0; i < 2; ++i)
                map.SetDoor(2, 1 + i, doorCapacity, WallPosition.TOP);
            for (uint i = 0; i < 3; ++i)
                map.SetDoor(4, 1 + i, doorCapacity, WallPosition.TOP);


            //show
            Console.WriteLine("Capacity[UP,DOWN,LEFT,RIGHT]");
            for (int i = 0; i < h; ++i)
            {
                for (int j = 0; j < w; ++j)
                {
                    Console.Write(map.Floor[i][j].Capacity);
                    Console.Write("[");
                    foreach (IWallElement e in map.Floor[i][j].Side)
                    {
                        if (e == null)
                        {
                            Console.Write("_");
                        }
                        else if (e.CanPassThrough)
                        {
                            Console.Write(e.Capacity);
                        }
                        else if (!e.CanPassThrough)
                        {
                            Console.Write("W");
                        }
                        else
                        {
                            Console.Write("#");
                        }
                    }
                    Console.Write("] ");
                }
                Console.WriteLine();
            }
            Console.ReadLine();

            //add people group
            /*
            pmap.People.Add(new PeopleGroup(0, 0, 3));
            pmap.People.Add(new PeopleGroup(0, 3, 2));
            pmap.People.Add(new PeopleGroup(0, 6, 6));
            pmap.People.Add(new PeopleGroup(3, 6, 1));
            pmap.People.Add(new PeopleGroup(4, 0, 2));
            pmap.People.Add(new PeopleGroup(4, 2, 1));
            pmap.People.Add(new PeopleGroup(4, 3, 1));
            */
            pmap = reader.ReadPeopleMap("people_map.apm");
            if (pmap == null)
            {
                Console.WriteLine("Problem with reading people map.");
                return;
            }


            sim.SetupSimulator(map, pmap);
            sim.MaximumTicks = 50;


            Chromosome chr = new Chromosome("11010101010101" +
                                            "11010100000000" +
                                            "11011111111111" +
                                            "00000000100000" + 
                                            "11101010101010");

            List<EscapedGroup> escape = sim.Simulate(chr.Fenotype);

            foreach (EscapedGroup e in escape)
            {
                Console.WriteLine("Escaped " + e.Quantity + " people in " + e.Ticks + " ticks");
            }
            
            Console.ReadLine();
        }
    }
}
