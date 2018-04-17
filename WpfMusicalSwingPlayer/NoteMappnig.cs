using System;
using System.Collections.Generic;
using System.Linq;
using Midi;

namespace WpfMusicalSwingPlayer
{
    public class NoteMappnig
    {
        private readonly SwingEvent _event;
        List<int> _ranges = new List<int>();
        private float _theValue;

        public NoteMappnig(SwingEvent @event, List<int> ranges)
        {
            _ranges = ranges;
            _event = @event;
            _theValue = Math.Abs(_event.Pitch);
        }

        public bool IsPlayable
        {
            get { return true; }
        }

        Pitch[] _notes = new Pitch[]{Pitch.C4,Pitch.D4, Pitch.E4 , Pitch.F4 };

        public Pitch GetNote()
        {
            for (int i = 0; i < _ranges.Count; i++)
            {
                var rangeVal = _ranges[i];
                if (_theValue < rangeVal)
                    return _notes[i];
            }
            return Pitch.F4;
        }
    }
}