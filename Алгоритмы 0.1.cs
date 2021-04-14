using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace A_0._3
{
    
    class Program
    {
        static public string check(StreamReader input)
        {
            int number = int.Parse(input.ReadLine());
            long[] nodes = new long[number];
            int[] father = new int[number - 1];
            char[] son = new char[number - 1];
            nodes[0] = long.Parse(input.ReadLine());
            for (int i = 0; i < number - 1; i++)
            {
                string newline = input.ReadLine();
                string[] words = newline.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                nodes[i+1] = long.Parse(words[0]);
                father[i] = int.Parse(words[1]);
                son[i] = char.Parse(words[2]);
            }

            int[] right = new int[number];
            int[] left = new int[number];
            for (int i = 0; i < number; i++)
            {
                right[i] = 0;
                left[i] = 0;
            }

            for (int i = 0; i < number - 1; i++)
            {
                if (nodes[i + 1] > nodes[father[i] - 1])
                {
                    if (son[i] == 'L')
                    {
                        return "NO";
                    }
                    else
                    {
                        if (number < 1000)
                        {
                            long check = nodes[i + 1];
                            long checkFather = nodes[father[i] - 1];
                            for (int j = 0; j < father[i] - 1; j++)
                            {
                                if (check.CompareTo(nodes[j]) == checkFather.CompareTo(nodes[j]))
                                {
                                    continue;
                                }
                                else
                                {
                                    return "NO";
                                }
                            }
                        }

                        right[father[i] - 1]++;
                        continue;
                    }
                }
                else if (nodes[i + 1] < nodes[father[i] - 1])
                {
                    if (son[i] == 'R')
                    {
                        return "NO";
                    }
                    else
                    {
                        if (number < 1000)
                        {
                            long check = nodes[i + 1];
                            long checkFather = nodes[father[i] - 1];
                            for (int j = 0; j < father[i] - 1; j++)
                            {
                                if (check.CompareTo(nodes[j]) == checkFather.CompareTo(nodes[j]))
                                    continue;
                                else
                                {
                                    return "NO";
                                }
                            }
                        }

                        left[father[i] - 1]++;
                        continue;
                    }
                }
                else if (nodes[i + 1] == nodes[father[i] - 1])
                {
                    if (son[i] == 'R')
                    {
                        right[father[i] - 1]++;
                        continue;
                    }
                    else
                    {
                        return "NO";
                    }
                }
            }

           for (int i = 0; i < number; i++)
           {
               if (left[i] > 1 || right[i] > 1)
               {
                   return "NO";
               }
           }
            return "YES";
        }
        static void Main(string[] args)
        {
            StreamReader input = new StreamReader(new FileStream("bst.in", FileMode.OpenOrCreate));
            string answer = check(input);
            input.Close();
            StreamWriter output = new StreamWriter("bst.out");
            output.WriteLine(answer);
            output.Close();
        }
    }
}
