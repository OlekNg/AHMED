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
            const uint w = 7;
            const uint h = 5;
            const uint capacity = 6;
            const int doorCapacity = 3;
            BuildingMap map = new BuildingMap();
            PeopleMap pmap = new PeopleMap();


            map.SetSize(w, h);
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
                map.SetWall(i, 5, WallPosition.LEFT);
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



        }
    }
}
