namespace ConsoleApp1
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    public class ArrayMinimumFinderWithCountDown
    {
        private readonly ArrayMinimumManager _manager;
        private readonly int _threadCount;

        public ArrayMinimumFinderWithCountDown(ArrayMinimumManager manager, int threadCount)
        {
            _manager = manager;
            _threadCount = threadCount;
        }

        public void FindMinInArr(int[] arr, int start, int end)
        {
            int min = arr[start];
            for (int i = start + 1; i < end; i++)
            {
                if (arr[i] < min)
                {
                    min = arr[i];
                }
            }

            _manager.UpdateMin(min);
        }

        public void StartFindingMin(int[] arr)
        {
            int size = arr.Length;

            var sw = Stopwatch.StartNew();

            CountdownEvent countdown = new CountdownEvent(_threadCount);

            for (int i = 0; i < _threadCount; i++)
            {
                var segment = GetSegment(i, _threadCount, size);
                Task.Run(() => ThreadWorker(arr, segment.Start, segment.End, countdown));
            }

            countdown.Wait();


            sw.Stop();
            Console.WriteLine($"----Search time with Tasks: {sw.ElapsedMilliseconds} ms");
        }

        private void ThreadWorker(int[] arr, int start, int end, CountdownEvent countdown)
        {
            try
            {
                FindMinInArr(arr, start, end);
            }
            finally
            {
                countdown.Signal();
            }
        }

        private static Bound GetSegment(int index, int segmentsCount, int length)
        {
            int segmentSize = length / segmentsCount;
            int start = index * segmentSize;
            int end = (index == segmentsCount - 1) ? length : start + segmentSize;
            return new Bound(start, end);
        }
    }
}
