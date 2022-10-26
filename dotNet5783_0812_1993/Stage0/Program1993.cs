using System;

namespace Stage0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome0812();
            Console.ReadKey();
        }

        static partial void Welcome1993();
        private static void Welcome0812()
        {
            Console.WriteLine("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application");
        }
    }
}