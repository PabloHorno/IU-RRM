using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            object a = 1;
            Console.WriteLine(a);
            Console.WriteLine(a.GetType());
            a = new ObjectText();
            Console.WriteLine((a as ObjectText).i);
            Console.WriteLine(a.GetType());
            Console.ReadKey();
        }
        void print(object a)
        {
            Console.WriteLine(a);
        }
    }
    class ObjectText
    {
        public int i = 10;
    }
}
