using BuildingEditor.Logic;
using Genetics.Generic;
using Genetics.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics.Operators
{
    public class FloorByFloorCrossover : ICrossoverOperator<List<bool>>
    {
        protected ICrossoverOperator<List<bool>> _multi;
        protected List<int> _genotypeSlicePoints;
        protected List<int> _cumulativePeopleCount;

        public FloorByFloorCrossover(Building building)
        {
            _multi = new MultiPointCrossover(5);
            
            _genotypeSlicePoints = new List<int>();
            int sum = 0;
            foreach(var f in building.Floors)
            {
                sum += (f.GetFloorCount() * 2);
                _genotypeSlicePoints.Add(sum);
            }

            _cumulativePeopleCount = new List<int>();
            sum = 0;
            foreach (var f in building.Floors)
            {
                sum += f.GetPeopleCount();
                _cumulativePeopleCount.Add(sum);
            }
        }

        public Tuple<List<bool>, List<bool>> Crossover(Chromosome<List<bool>> c1, Chromosome<List<bool>> c2)
        {
            var genotype1 = c1.Genotype;
            var genotype2 = c2.Genotype;

            var parts1 = SliceGenotype(genotype1);
            var parts2 = SliceGenotype(genotype2);

            // Find part that should be crossedover
            for (var i = 0; i < parts1.Count; i++)
            {
                double value = Math.Min(c1.Value, c2.Value) + 0.01;

                if (value < _cumulativePeopleCount[i])
                {
                    var result = _multi.Crossover(new BinaryChromosome(parts1[i]), new BinaryChromosome(parts1[i]));
                    parts1[i] = result.Item1;
                    parts2[i] = result.Item2;
                    break;
                }
            }

            return new Tuple<List<bool>, List<bool>>(JoinGenotype(parts1), JoinGenotype(parts2));
        }

        protected List<bool> JoinGenotype(List<List<bool>> parts)
        {
            List<bool> result = new List<bool>();

            foreach (var part in parts)
                result.AddRange(part);

            return result;
        }

        protected List<List<bool>> SliceGenotype(List<bool> genotype)
        {
            List<List<bool>> result = new List<List<bool>>();

            int lastPoint = 0;
            foreach (int point in _genotypeSlicePoints)
            {
                List<bool> part = new List<bool>(genotype.GetRange(lastPoint, point - lastPoint));
                lastPoint = point;

                result.Add(part);
            }

            return result;
        }

        
    }
}
