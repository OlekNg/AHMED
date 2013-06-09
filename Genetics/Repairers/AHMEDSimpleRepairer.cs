using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structure;

namespace Genetics.Repairers
{
    public class AHMEDSimpleRepairer : IRepairer
    {
        private BuildingMap _bmap;

        private int _expectedChromosomeLength;

        private Random _randomizer = new Random();

        public AHMEDSimpleRepairer(BuildingMap bmap)
        {
            _bmap = bmap;
            _expectedChromosomeLength = (int)_bmap.Height * (int)_bmap.Width * 2;
        }

        public void RepairAndReplace(Chromosome c)
        {
            if (c.Length != _expectedChromosomeLength)
                throw new RepairerException("Chromosome does not match the building map. Different length.");

            List<Direction> fenotype = c.Fenotype;

            // Check every tile. If sign points at wall then
            // change to another direction.
            for (int i = 0; i < _bmap.Height; i++)
            {
                for (int j = 0; j < _bmap.Width; j++)
                {
                    int fenotypeIndex = i * (int)_bmap.Width + j;

                    IWallElement element;
                    element = _bmap.Floor[i][j].GetSide(fenotype[fenotypeIndex]);

                    if(element.CanPassThrough)
                        continue;

                    // People cannot pass (wall)
                    // Find available direction.
                    List<Direction> availableDirections = new List<Direction>();

                    foreach (Direction d in Enum.GetValues(typeof(Direction)))
                    {
                        element = _bmap.Floor[i][j].GetSide(d);
                        if (element.CanPassThrough)
                            availableDirections.Add(d);
                    }

                    if(availableDirections.Count == 0)
                        throw new RepairerException("Cannot fix chromosome. There is a tile with no available direction.");

//                     Console.WriteLine("Row: {0} Col: {1} Fenotypeindex: {2}", i, j, fenotypeIndex);
//                     Console.WriteLine("Incorrect direction: {0}", fenotype[fenotypeIndex]);
//                     Console.Write("Available directions: ");
//                     foreach (Direction d in availableDirections)
//                         Console.Write(d.ToString() + " ");
//                     Console.WriteLine();
                    

                    // Set new direction choosing one of available randomly.
                    c.SetFenotype(fenotypeIndex, availableDirections[_randomizer.Next(0, availableDirections.Count)]);
/*                    Console.WriteLine("Changed to: {0}", c.Fenotype[fenotypeIndex]);*/
                }
            }
        }

        public Chromosome RepairAndCreate(Chromosome c)
        {
            throw new NotImplementedException();
        }
    }
}
