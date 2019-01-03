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
    

    public class SwingDetector
    {
        private readonly int _step;
        private List<int> _positions;
        private MovingDir _dir = MovingDir.None;
        public SwingDetector(int step)
        {
            _step = step;
            _positions = new List<int>();
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
            if (!(pos == 0 || pos == _positions.Count - 1) )
            {
                _positions.Clear();
                return;
            }
            var low = FindLow(_positions.ToArray(), _positions.Count);
            if (!(low == 0 || low == _positions.Count - 1))
            {
                _positions.Clear();
                return;
            }
            _dir = MovingDir.None;
        }

        int FindPeakUtil(int[] arr, int low,
            int high, int n)
        {
            // Find index of  
            // middle element 
            int mid = low + (high - low) / 2;

            // Compare middle element with 
            // its neighbours (if neighbours 
            // exist) 
            //max
            if ((mid == 0 || arr[mid - 1] <= arr[mid]) && (mid == n - 1 || arr[mid + 1] <= arr[mid]))
            {
                _dir = MovingDir.Left;
                return mid;
            }
            

            // If middle element is not  
            // peak and its left neighbor 
            // is greater than it,then  
            // left half must have a  
            // peak element 
            else if (mid > 0 &&
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

            // Compare middle element with 
            // its neighbours (if neighbours 
            // exist) 
            //max
            if ((mid == 0 || arr[mid - 1] >= arr[mid]) && (mid == n - 1 || arr[mid + 1] >= arr[mid]))
            {
                _dir = MovingDir.Right;
                return mid;
            }


            // If middle element is not  
            // peak and its left neighbor 
            // is greater than it,then  
            // left half must have a  
            // peak element 
            else if (mid > 0 &&
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