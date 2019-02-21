using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Midi;
/// <summary>
/// http://bluelemonlabs.blogspot.com/2013/08/arduino-imu-pitch-roll-from-adxl345.html
/// </summary>
namespace WpfMusicalSwingPlayer
{
    public enum MovingDir
    {
        Left,
        Right,
        None
    }


    public interface INoteMapper
    {
        void Map(int swingId, int value, MovingDir dir);
    }

    public class NoteMapper: INoteMapper
    {
        private readonly int _zero;
        private readonly IPlayingDevice _playingDevice;
        private readonly int[] _ranges = new[] {330,400,550,600};

        readonly Dictionary<int,Midi.Pitch> _notes = new Dictionary<int, Pitch>()
        {
            {85, Pitch.C0},
            {70, Pitch.D0},
            {50, Pitch.E0},
            {30, Pitch.F0}
        };
        public NoteMapper(int zero, IPlayingDevice playingDevice)
        {
            _zero = zero;
            _playingDevice = playingDevice;
        }
        public void Map(int swingId,int value, MovingDir dir)
        {
            if (value > _notes.Keys.First())
            {
                return;
            }
            var last = 0;
            foreach (var range in _notes.Keys)
            {
                if (value >= last && value <= range)
                {
                    _playingDevice.PlayNote(_notes[range]);
                }
                last = range;
            }
        }
    }

    public class SwingDispatch
    {
        private readonly int[] _ids;
        private readonly INoteMapper _generator;
        private readonly Dictionary<int,SwingDetector> _swingDetectors=new Dictionary<int, SwingDetector>();
        private const int Step = 5;
        private const int Sensors = 1;
        public SwingDispatch(int[] ids, INoteMapper generator)
        {
            _ids = ids;
            _generator = generator;
            foreach (var id in ids)
            {
                _swingDetectors[id]=new SwingDetector(id, Step);
            }
        }

        public void AddPositions(string positions)
        {
            var posArray = positions.Split(',');
            if (posArray.Length != _ids.Length * 2)
            {
                return;
                //ReturnValueNameAttribute;
                //throw new InvalidOperationException($"{positions} is invalid. You can only send array of {_ids.Length * 2} integers");
            }

            for (int i = 0; i < 12; i+=2)
            {
                var swingId = int.Parse(posArray[i]);
                var position = int.Parse(posArray[i+1]);
                var swing = _swingDetectors[swingId];
                swing.AddPosition(position);
                if (swing.Dir != MovingDir.None)
                {
                    _generator.Map(swingId,swing.Value,swing.Dir);
                }
            }
        }
    }
        


    public class SwingDetector
    {
        private readonly int _id;
        private readonly int _step;
        private List<int> _positions;
        private MovingDir _dir = MovingDir.None;
        private int _value;

        public SwingDetector(int id,int step)
        {
            _id = id;
            _step = step;
            _positions = new List<int>();
        }

        public int Value
        {
            get { return _value; }
        }
        public MovingDir Dir
        {
            get { return _dir; }
        }

        public void AddPosition(int pos)
        {
            _positions.Add(pos);
            Detect();
        }

        public void AddPosition(int[] pos)
        {
            _positions.AddRange(pos);
            Detect();
        }

        private void Detect()
        {
            if (_positions.Count < _step)
            {
                _dir = MovingDir.None;
                return;
            }
          /*  var pos = FindPeak(_positions.ToArray(), _positions.Count);
            if (!(pos == 0 || pos == _positions.Count - 1) && !IsFlat(_positions))
            {
                _positions.Clear();
                return;
            }*/
            var low = FindLow(_positions.ToArray(), _positions.Count );
            if (!(low == 0 || low == _positions.Count - 1) && !IsFlat(_positions))
            {
                _positions.Clear();
                return;
            }

            _value = 0;
            _dir = MovingDir.None;
        }

        private bool IsFlat(List<int> positions)
        {
            return positions.All(c => c == positions.First());
        }

        int FindPeakUtil(int[] arr, int low,
            int high, int n)
        {
            // Find index of  
            // middle element 
            int mid = low + (high - low) / 2;
            if (mid == arr.Length - 1)
                return 0;
            // Compare middle element with 
            // its neighbours (if neighbours 
            // exist) 
            //max
            if ((mid == 0 || arr[mid - 1] < arr[mid]) && (mid == n - 1 || arr[mid + 1] < arr[mid]))
            {
                _dir = MovingDir.Left;
                _value = arr[mid];
                return mid;
            }
            

            // If middle element is not  
            // peak and its left neighbor 
            // is greater than it,then  
            // left half must have a  
            // peak element 
            else if (mid > 0 && mid < arr.Length &&
                     arr[mid - 1] > arr[mid])
                return FindPeakUtil(arr, low,
                    (mid - 1), n);

            // If middle element is not  
            // peak and its right neighbor 
            // is greater than it, then  
            // right half must have a peak 
            // element 
            else return FindPeakUtil(arr, (mid + 1),
                high, n);
        }


        int FindLowUtil(int[] arr, int low,
            int high, int n)
        {
            // Find index of  
            // middle element 
            int mid = low + (high - low) / 2;
            if (mid == arr.Length - 1)
                return 0;
            
            // Compare middle element with 
            // its neighbours (if neighbours 
            // exist) 
            //max
            if ((mid == 0 || arr[mid - 1] > arr[mid]) && (mid == n - 1 || arr[mid + 1] > arr[mid]))
            {
                _dir = MovingDir.Right;
                _value = arr[mid];
                return mid;
            }


            // If middle element is not  
            // peak and its left neighbor 
            // is greater than it,then  
            // left half must have a  
            // peak element 
            else if (mid > arr.Length &&
                     arr[mid - 1] < arr[mid])
                return FindLowUtil(arr, low,
                    (mid - 1), n);

            // If middle element is not  
            // peak and its right neighbor 
            // is greater than it, then  
            // right half must have a peak 
            // element 
            else return FindLowUtil(arr, (mid + 1),
                high, n);
        }

        // A wrapper over recursive  
        // function findPeakUtil() 
        int FindPeak(int[] arr,
            int n)
        {
            return FindPeakUtil(arr, 0,
                n - 1, n);
        }

        int FindLow(int[] arr,
            int n)
        {
            return FindLowUtil(arr, 0,
                n - 1, n);
        }
    }
}