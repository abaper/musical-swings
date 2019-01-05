using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfMusicalSwingPlayer
{
    public enum MovingDir
    {
        Left,
        Right,
        None
    }

    public interface ISoundGenerator
    {
        void PlaySound(int swingId, int value, MovingDir dir);
    }
    public class SoundGenerator: ISoundGenerator
    {
        public void PlaySound(int swingId,int value, MovingDir dir)
        {

        }
    }


    public class SwingDispatch
    {
        private readonly int[] _ids;
        private readonly ISoundGenerator _generator;
        private Dictionary<int,SwingDetector> _swings=new Dictionary<int, SwingDetector>();
        private const int Step = 5;
        
        public SwingDispatch(int[] ids, ISoundGenerator generator)
        {
            _ids = ids;
            _generator = generator;
            foreach (var id in ids)
            {
                _swings[id]=new SwingDetector(id, Step);
            }
        }

        public void AddPositions(string positions)
        {
            var posArray = positions.Split(',');
            if (posArray.Length != _ids.Length * 2)
            {
                throw new InvalidOperationException($"{positions} is invalid. You can only send array of {_ids.Length * 2} integers");
            }

            for (int i = 0; i < 12; i+=2)
            {
                var swingId = int.Parse(posArray[i]);
                var position = int.Parse(posArray[i+1]);
                var swing = _swings[swingId];
                swing.AddPosition(position);
                if (swing.Dir != MovingDir.None)
                {
                    _generator.PlaySound(swingId,swing.Value,swing.Dir);
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
            var pos = FindPeak(_positions.ToArray(), _positions.Count);
            if (!(pos == 0 || pos == _positions.Count - 1) && !IsFlat(_positions))
            {
                _positions.Clear();
                return;
            }
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