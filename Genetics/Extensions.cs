using Common.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics
{
    public static class GeneticExtensions
    {
        public static List<Direction> ToFenotype(this List<bool> genotype)
        {
            if (genotype.Count % 2 != 0)
                throw new Exception("Invalid genotype.");

            List<Direction> result = new List<Direction>();

            for (int i = 0; i < genotype.Count; i += 2)
            {
                if (genotype[i] == true)
                {
                    if (genotype[i + 1] == true)
                        result.Add(Direction.RIGHT); // 11
                    else
                        result.Add(Direction.UP); // 10
                }
                else
                {
                    if (genotype[i + 1] == true)
                        result.Add(Direction.DOWN); // 01
                    else
                        result.Add(Direction.LEFT); // 00
                }
            }

            return result;
        }
    }
}
