using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulation;
using Structure;

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
            TestStairsRouting();
        }

        static void TestStairsRouting()
        {
            BuildingMap map = new BuildingMap();
            Stairs s = new Stairs(3, 2);
            StairsEntry[] se = new StairsEntry[2];
            WallElementPosition[] wep = new WallElementPosition[2];
            Simulator sim = new Simulator();
            List<List<Direction>> fenotype = new List<List<Direction>>();
            PeopleMap pm = new PeopleMap();

            pm.People.Add(new PeopleGroup(1, 0, 0, 3));

            fenotype.Add(new List<Direction>());
            fenotype.Add(new List<Direction>());

            fenotype[0].Add(Direction.RIGHT);
            fenotype[0].Add(Direction.RIGHT);
            fenotype[0].Add(Direction.RIGHT);

            fenotype[1].Add(Direction.RIGHT);
            fenotype[1].Add(Direction.RIGHT);
            fenotype[1].Add(Direction.RIGHT);

            

            Floor[] f = new Floor[2];
            f[0] = new Floor();
            f[1] = new Floor();

            f[0].Setup(3, 1, 3);
            f[1].Setup(3, 1, 3);

            for (uint i = 0; i < 3; ++i)
            {
                f[0].SetFloor(0, i, 3);
                f[1].SetFloor(0, i, 3);
                f[0].SetWall(0, i, WallPlace.TOP);
                f[0].SetWall(1, i, WallPlace.TOP);
                f[1].SetWall(0, i, WallPlace.TOP);
                f[1].SetWall(1, i, WallPlace.TOP);
            }
            f[0].SetDoor(0, 3, 3, WallPlace.LEFT);
            f[1].SetWall(0, 0, WallPlace.LEFT);

            map.AddFloor(f[0]);
            map.AddFloor(f[1]);

            wep[0] = new WallElementPosition(0, 0, 0, WallPlace.LEFT);
            wep[1] = new WallElementPosition(1, 0, 3, WallPlace.LEFT);
            se[0] = new StairsEntry(2, wep[0]);
            se[1] = new StairsEntry(2, wep[1]);
            s.SetEntries(se[0], se[1]);
            
            map.AddStairs(s);

            sim.SetupSimulator(map, pm);
            sim.MaximumTicks = 50;
            List<EscapedGroup> eg = sim.Simulate(fenotype);

            Console.WriteLine(eg.ToArray().ToString());
            Console.ReadLine();
        }

        /*
        static void OldTest()
        {
            const uint w = 7;
            const uint h = 5;
            const uint capacity = 6;
            const uint doorCapacity = 3;
            const uint standardEff = 4;
            BuildingMap map/* = new BuildingMap()*/
            //PeopleMap pmap/* = new PeopleMap()*/;
           // Simulator sim = new Simulator();
            //XMLReader reader = new XMLReader();

            /*
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
            */

            /*map = reader.ReadBuildingMap("building_map.abm");

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
                            Console.Write(e.Efficiency);
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
            Console.ReadLine();*/

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
            /*pmap = reader.ReadPeopleMap("people_map.apm");
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

            Console.ReadLine();*/
        //}
    }
}
