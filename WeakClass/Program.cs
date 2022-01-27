using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeakClass
{
    class Program
    {
        static void Main(string[] args)
        {
            //Person person = new Person();
            new WeakClass().DoSomething();

            Console.ReadKey();
        }
    }

    class WeakClass
    {
        public string name = "Undefined";   // имя
        public int age;                     // возраст

        public WeakClass()
        {
            Console.WriteLine("Ctor");
        }

        public void DoSomething()
        {
            Console.WriteLine("DoSomething");
        }

        public void Print()
        {
            Console.WriteLine($"Имя: {name}  Возраст: {age}");
        }
    }
}
