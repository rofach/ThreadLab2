using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class ArrayMinimumManager
    {
        private int _min = int.MaxValue;
        private readonly object _lock = new object();

        public void UpdateMin(int newMin)
        {
            lock (_lock)
            {
                if (newMin < _min)
                {
                    _min = newMin;
                }
            }
        }

        public int GetMin()
        {
            lock (_lock)
            {
                return _min;
            }
        }
    }
}
