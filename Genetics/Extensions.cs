using Common.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics
{
    public static class GeneticExtensions
    {
        public static List<Side> ToFenotype(this List<bool> genotype)
        {
            if (genotype.Count % 2 != 0)
                throw new Exception("Invalid genotype.");

            List<Side> result = new List<Side>();

            for (int i = 0; i < genotype.Count; i += 2)
            {
                if (genotype[i] == true)
                {
                    if (genotype[i + 1] == true)
                        result.Add(Side.RIGHT); // 11
                    else
                        result.Add(Side.TOP); // 10
                }
                else
                {
                    if (genotype[i + 1] == true)
                        result.Add(Side.BOTTOM); // 01
                    else
                        result.Add(Side.LEFT); // 00
                }
            }

            return result;
        }
    }
}
