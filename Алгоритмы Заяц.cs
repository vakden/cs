using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Заяц
{
    class Program
    {


        static public int var = 0;

        static void fib(int num)
        {
            int temp1 = 1;
            int temp2 = 1;
            if (num == 1)
            {
                var = 1;
                return;
            }

            for (int i = 2; i <= num; i++)
            {
                var = (temp1 + temp2) % 1000000007;
                temp1 = temp2 % 1000000007;
                temp2 = var % 1000000007;
            }
        }
        static void Main(string[] args)
        {
            StreamReader input = new StreamReader(new FileStream("input.txt", FileMode.Open));
            int num = int.Parse(input.ReadLine());
            input.Close();
            fib(num);
            StreamWriter output = new StreamWriter("output.txt");
            output.WriteLine(var % 1000000007);
            output.Close();
        }
    }
}
