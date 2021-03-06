﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingEditor.ViewModel
{
    /// <summary>
    /// Handles one people group path. Allows to detect loops,
    /// get lowest building flow value (how close to escape they are),
    /// get number of direction changes...
    /// </summary>
    public class PeoplePath
    {
        private Segment _segment;
        private List<Segment> _pathSegments;
        private List<Segment> _pathFenotypeSegments;

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
        public int TotalDangerValue { get; set; }
        public Segment LowestFlowSegment { get; set; }
        public bool LoopDetected { get { return LowestFlowValue > 0; } }

        public int PeopleCount { get { return _segment.PeopleCount; } }
        public bool SuccessfulEscape { get; private set; }

        public List<Segment> Segments
        {
            get
            {
                if (_pathSegments == null)
                    Update();
                return _pathSegments;
            }
        }

        /// <summary>
        /// Segments in path, that are part of fenotype.
        /// </summary>
        public List<Segment> FenotypeSegments
        {
            get
            {
                if (_pathFenotypeSegments == null)
                    _pathFenotypeSegments = Segments.Where(x => x.FenotypeIndex.HasValue).ToList();
                return _pathFenotypeSegments;
            }
        }

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
                TotalDangerValue += segment.DangerValue;

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

            if (segment == null && LowestFlowValue == 0)
                SuccessfulEscape = true;

            _pathSegments = history;
        }

        private void ResetProperties()
        {
            LowestFlowValue = _segment.FlowValue;
            LowestFlowSegment = _segment;
            Corners = 0;
            SuccessfulEscape = false;
            TotalDangerValue = 0;
        }
    }
}
