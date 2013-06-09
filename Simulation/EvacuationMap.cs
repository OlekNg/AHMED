using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structure;

namespace Simulation
{
    public class EvacuationMap
    {
        private uint _width;

        private uint _height;

        private EvacuationElement[][] _map;

        private bool CheckRanges(uint row, uint col)
        {
            return !(row >= _height || col >= _width);
        }

        public EvacuationElement Get(uint row, uint col)
        {
            if (CheckRanges(row, col)) return _map[row][col];
            return null;
        }

        public void InitializeFromBuildingMap(BuildingMap bm)
        {
            _width = bm.Width;
            _height = bm.Height;

            _map = new EvacuationElement[bm.Height][];
            for (uint i = 0; i < bm.Height; ++i)
            {
                _map[i] = new EvacuationElement[bm.Width];
                for (int j = 0; j < bm.Width; ++j)
                {
                    _map[i][j] = new EvacuationElement(bm.Floor[i][j]);
                }
            }
        }

        public void SetPeopleGroup(PeopleGroup group)
        {
            _map[group.Row][group.Col].PeopleQuantity = group.Quantity;
        }

        public void ResetPeopleGroups()
        {
            foreach (EvacuationElement[] e in _map)
                foreach (EvacuationElement element in e)
                {
                    element.PeopleQuantity = 0;
                    element.Processed = false;
                }
        }

        public void MapFenotype(List<Direction> fenotype)
        {
            var fenotypeEnumerator = fenotype.GetEnumerator();
            for (uint i = 0; i < _height; ++i)
            {
                for (uint j = 0; j < _width; ++j)
                {
                    EvacuationElement element = Get(i, j);

                    if (element == null) continue;

                    if (!fenotypeEnumerator.MoveNext())
                    {
                        //TODO: error, not enough genes
                    }

                    Direction direction = fenotypeEnumerator.Current;

                    element.Passage = element.FloorSquare.GetSide(direction);

                    switch (direction)
                    {
                        case Direction.DOWN:
                            element.NextStep = Get(i + 1, j);
                            break;
                        case Direction.UP:
                            element.NextStep = Get(i - 1, j);
                            break;
                        case Direction.LEFT:
                            element.NextStep = Get(i, j - 1);
                            break;
                        case Direction.RIGHT:
                            element.NextStep = Get(i, j + 1);
                            break;
                    }
                }
            }
        }
    }
}
