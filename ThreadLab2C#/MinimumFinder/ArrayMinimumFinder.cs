using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
    public class ArrayMinimumFinder
    {
        private readonly ArrayMinimumManager _manager;
        private readonly int _threadCount;

        private readonly object _countLocker = new object();
        private int _completedThreadCount = 0;

        public ArrayMinimumFinder(ArrayMinimumManager manager, int threadCount)
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
            _completedThreadCount = 0;

            var sw = Stopwatch.StartNew();

            for (int i = 0; i < _threadCount; i++)
            {
                var bound = GetSegment(i, _threadCount, size);
                new Thread(() => ThreadWorker(arr, bound.Start, bound.End)).Start();
            }

            lock (_countLocker)
            {
                while (_completedThreadCount < _threadCount)
                {
                    Monitor.Wait(_countLocker);
                }
            }

            sw.Stop();
            Console.WriteLine($"----Search time with Threads: {sw.ElapsedMilliseconds} ms");
        }

        private void ThreadWorker(int[] arr, int start, int end)
        {
            FindMinInArr(arr, start, end);
            IncThreadCount();
        }

        private void IncThreadCount()
        {
            lock (_countLocker)
            {
                _completedThreadCount++;
                Monitor.Pulse(_countLocker);
            }
        }

        private static Bound GetSegment(int index, int arrCount, int length)
        {
            int segmentSize = length / arrCount;
            int start = index * segmentSize;
            int end = (index == arrCount - 1) ? length : start + segmentSize;

            return new Bound(start, end);
        }
    }
}
