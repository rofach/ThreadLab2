using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class ArrayGenerator
    {
        private readonly int _size;
        private bool _isGenerated = false;
        private int[] _generatedArray;

        public int[] GeneratedArray
        { 
            get
            {
                if (!_isGenerated)
                {
                    _generatedArray = Generate();
                    _isGenerated = true;
                }

                return _generatedArray;
            }
        }

        public ArrayGenerator(int size)
        {
            _size = size;
            _generatedArray = Generate();
        }

        public int[] Generate()
        {
            Console.WriteLine("Generating array...");
            int[] arr = new int[_size];
            for (int i = 0; i < _size; i++)
            {
                arr[i] = i % 20000;
            }

            arr[arr.Length - 1] = -1;
            _isGenerated = true;
            return arr;
        }
    }
}
