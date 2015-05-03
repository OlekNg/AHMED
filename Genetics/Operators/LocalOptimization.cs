using BuildingEditor.ViewModel;
using Common.DataModel.Enums;
using Genetics.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics.Operators
{
    /// <summary>
    /// Tries to change single bits of chromosome to improve solution.
    /// </summary>
    public class LocalOptimization : ITransformer<List<bool>>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(LocalOptimization));

        private Random _randomizer = new Random();
        private Building _building;
        private List<PeoplePath> _paths;
        private IEvaluator<List<bool>> _evaluator;
        private int maxIterations = 10;
        
        public LocalOptimization(Building building, IEvaluator<List<bool>> evaluator)
        {
            _building = new Building(building);
            _evaluator = evaluator;
            _paths = _building.GetPeoplePaths();
        }

        #region ITransformer<List<bool>> Members
        public List<bool> Transform(Chromosome<List<bool>> c)
        {
            _building.SetFenotype(c.Genotype.ToFenotype());
            _paths.ForEach(x => x.Update());

            int iteration = 0;

            // Analyze only paths, that have fenotype segments.
            var paths = _paths.Where(x => x.FenotypeSegments.Count > 0).ToList();
            if (paths.Count == 0)
                return c.Genotype;

            double currentChromosomeValue = _evaluator.Eval(c.Genotype);

            while (iteration < maxIterations)
            {
                var path = paths[_randomizer.Next(paths.Count)];
                var segment = path.FenotypeSegments[_randomizer.Next(path.FenotypeSegments.Count)];
                var currentDirection = segment.Fenotype;

                var availableDirections = segment.GetAvailableDirections(currentDirection);
                if (availableDirections.Count > 0)
                {
                    segment.Fenotype = availableDirections[_randomizer.Next(availableDirections.Count)];

                    var newChromosomeValue = _evaluator.Eval(_building.GetFenotype().ToGenotype());
                    if (newChromosomeValue > currentChromosomeValue)
                    {
                        currentChromosomeValue = newChromosomeValue;
                        path.Update(); // Update path because fenotype has changed.
                    }
                    else
                    {
                        // Restore previous fenotype.
                        segment.Fenotype = currentDirection;
                    }
                }

                iteration++;
            }

            return _building.GetFenotype().ToGenotype();
        }

        #endregion
    }
}
