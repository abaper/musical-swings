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
        private readonly int _octave;
        private readonly float _basePitch;
        private float _theValue;

        public NoteMappnig(SwingEvent @event, List<int> ranges, int octave,float basePitch)
        {
            _ranges = ranges;
            _octave = octave;
            _basePitch = basePitch;
            _event = @event;
            _theValue = Math.Abs((_event.Pitch-basePitch));
        }

        public bool IsPlayable
        {
            get { return true; }
        }

        Pitch[] _notes = new Pitch[]{Pitch.C0,Pitch.D0, Pitch.E0 , Pitch.F0 };

        public Pitch GetNote()
        {
            for (int i = 0; i < _ranges.Count; i++)
            {
                var rangeVal = _ranges[i];
                if (_theValue < rangeVal)
                    return _notes[i]+(_octave*12);
            }
            return Pitch.F0+(_octave * 12);
        }
    }
}