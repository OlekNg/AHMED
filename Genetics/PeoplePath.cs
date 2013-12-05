using BuildingEditor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics
{
    /// <summary>
    /// Handles one people group path. Allows to detect loops,
    /// get lowest building flow value (how close to escape they are),
    /// get number of direction changes...
    /// </summary>
    public class PeoplePath
    {
        private Segment _segment;

        /// <param name="peopleSegment">Segment with people count > 0</param>
        public PeoplePath(Segment peopleSegment)
        {
            _segment = peopleSegment;
        }

        /// <summary>
        /// Number of direction changes along the path.
        /// </summary>
        public int Corners { get; set; }
        public int LowestFlowValue { get; set; }
        public Segment LowestFlowSegment { get; set; }
        public bool LoopDetected { get { return LowestFlowValue > 0; } }

        public void Update()
        {
            ResetProperties();
            ProcessPath();
        }

        private void ProcessPath()
        {
            // Already visited segments
            List<Segment> history = new List<Segment>();

            var segment = _segment;
            var direction = segment.Fenotype;

            // Follow fenotype until null segment or loop detected.
            while (segment != null && !history.Contains(segment))
            {
                if (direction != segment.Fenotype)
                {
                    Corners++;
                    direction = segment.Fenotype;
                }

                if (segment.FlowValue < LowestFlowValue)
                {
                    LowestFlowValue = segment.FlowValue;
                    LowestFlowSegment = segment;
                }

                history.Add(segment);
                segment = segment.GetNextSegment();
            }
        }

        private void ResetProperties()
        {
            LowestFlowValue = _segment.FlowValue;
            LowestFlowSegment = _segment;
            Corners = 0;
        }
    }
}
