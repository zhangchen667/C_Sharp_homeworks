using System;

namespace findsu
{
    class Program
    {
        public static bool isSu(int n)
        {
            for (int i = 2; i < n / 2; i++)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("素数查找");
            Console.WriteLine("请输入一个上限和一个下限：");
            int n1 = int.Parse(Console.ReadLine());
            int n2 = int.Parse(Console.ReadLine());
            int count = 0;
            for(int i = n1; i < n2; i++)
            {
                if (count == 10)
                {
                    count = 0;
                    Console.WriteLine();
                }
                if (isSu(i))
                {
                    count++;
                    Console.Write(i);
                    Console.Write(" ");
                }
            }
            
        }
    }
}
