using Common.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics
{
    /// <summary>
    /// Extension methods related to genetics.
    /// </summary>
    public static class GeneticExtensions
    {
        /// <summary>
        /// Converts genotype to fenotype.
        /// </summary>
        /// <param name="genotype">Source genotype.</param>
        /// <returns>Fenotype.</returns>
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

        /// <summary>
        /// Converts fenotype to genotype.
        /// </summary>
        /// <param name="fenotype">Source fenotype.</param>
        /// <returns>Genotype.</returns>
        public static List<bool> ToGenotype(this List<Direction> fenotype)
        {
            List<bool> result = new List<bool>();

            foreach (var f in fenotype)
            {
                switch (f)
                {
                    case Direction.LEFT:
                        result.Add(false);
                        result.Add(false);
                        break;
                    case Direction.UP:
                        result.Add(true);
                        result.Add(false);
                        break;
                    case Direction.RIGHT:
                        result.Add(true);
                        result.Add(true);
                        break;
                    case Direction.DOWN:
                        result.Add(false);
                        result.Add(true);
                        break;
                }
            }

            return result;
        }
    }
}
