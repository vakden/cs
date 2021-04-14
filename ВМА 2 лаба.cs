using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMA_2
{
    class Program
    {
        static Random rnd = new Random();
        static int n = 10;
        static double[,] A = new double[n, n];
        static double[] answer = new double[n];
        static double[] a = new double[n - 1];
        static double[] b = new double[n];
        static double[] c = new double[n - 1];
        static double[] f = new double[n];
        static double[] y = new double[n];
        static int left = -10, right = 10;
        static int numbOfDecimals = 2;
        static double[] Copy(double[] a)
        {
            double[] b;
            b = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
                b[i] = a[i];
            return b;
        }
        static double[,] CreateMatrix(double[] a, double[] b, double[] c)
        {
            double[,] A = new double[n,n];
            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    A[i, j] = 0;
                }
            }
            for (int i = 0; i < n; i++)
            {
                A[i, i] = b[i];
            }
            for (int i = 0; i < n- 1; i++)
            {
                A[i + 1, i] = a[i];
            }
            for (int i = 0; i < n - 1; i++)
            {
                A[i, i+1] = c[i];
            }
            return A;
        }
        static void PrintMatrix(double[,] A)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write("{0:f" + numbOfDecimals + "}", A[i, j]);
                    Console.Write("\t");
                }
                Console.Write("\n");
                Console.Write("\n");
            }
        }
        static void PrintMatrixWithF(double[,] A, double[] f)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write("{0:f" + numbOfDecimals + "}", A[i, j]);
                    Console.Write("\t");
                }
                Console.Write("|");
                Console.Write("\t");
                Console.Write("{0:f" + numbOfDecimals + "}", f[i]);
                Console.Write("\n");
                Console.Write("\n");
            }
        }
        static double[] Generating(double[] x)
        {
            for(int i = 0; i < x.Length; i++)
            {
                x[i] = rnd.Next(left * (int)(Math.Pow(10, numbOfDecimals)), right * (int)(Math.Pow(10, numbOfDecimals))) / Math.Pow(10, numbOfDecimals);
            }
            return x;
        }
        static double[] GeneratingB(double[] a, double[] c, double[] b)
        {
            for (int i = 0; i < b.Length; i++)
            {
                if (i == 0)
                {
                    b[0] = 2 * Math.Abs(c[0]); 
                }
                else if (i == n-1)
                {
                    b[n-1] = 2 * Math.Abs(a[n - 2]);
                }
                else
                {
                    b[i] = Math.Abs(c[i]) + Math.Abs(a[i - 1]);
                }
            }
            return b;
        }
        static double[] GeneratingF(double[] a, double[] b, double[] c, double[] y)
        {
            double[] f = new double[n];
            for (int i = 0; i < f.Length; i++)
            {
                if (i == 0)
                {
                    f[i] = b[i] * y[i] + c[i] * y[i + 1]; 
                }
                else if (i == n-1)
                {
                    f[i] = b[i] * y[i] + a[i - 1] * y[i - 1];
                }
                else
                {
                    f[i] = b[i] * y[i] + a[i - 1] * y[i - 1] + c[i] * y[i + 1];
                }
            }

            return f;
        }
        static double[] solveMatrix(int n, double[] a, double[] b, double[] c, double[] f, double[] answer)
        {
            double m;
            for (int i = 0; i < n - 1; i++)
            {
                m = a[i] / b[i];
                b[i + 1] = b[i + 1] - m * c[i];
                f[i + 1] = f[i + 1] - m * f[i];
            }

            answer[n - 1] = f[n - 1] / b[n - 1];

            for (int i = n - 2; i >= 0; i--)
            {
                answer[i] = (f[i] - c[i] * answer[i + 1]) / b[i];
            }
            return answer;
        }
        static double MaxNormDiscrepancy(double[] a, double[] b, double[] c, double[] y, double[] f)
        {
            double[] desF = new double[n];
            desF = GeneratingF(a, b, c, y);

            double maxND = Math.Abs(desF[0] - f[0]);
            for (int i = 0; i < n; i++)
            {
                if (Math.Abs(desF[i] - f[i]) > maxND)
                    maxND = Math.Abs(desF[i] - f[i]);
            }
            return maxND;

            return maxND;
        }
        public static double MaxNormDiscrepancy2(double[,] A, double[] answer, double[] F)
        {
            int n = 10;
            double[] desF = new double[n];
            for (int i = 0; i < n; i++)
            {
                desF[i] = 0;
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    desF[i] += A[i,j] * answer[j];
                }
                desF[i] = Math.Abs(desF[i] - F[i]);
            }
            double maxND = desF[0];
            for (int i = 0; i < n; i++)
            {
                if (desF[i] > maxND)
                    maxND = desF[i];
            }
            return maxND;
        }
        //Нахождение максимум-нормы погрешности
        public static double MaxNormError(double[] y, double[] answer)
        {
            int n = 10;
            double[] errorX = new double[n];
            for (int i = 0; i < n; i++)
            {
                errorX[i] = 0;
            }
            for (int i = 0; i < n; i++)
            {
                errorX[i] = Math.Abs(y[i] - answer[i]);
            }
            double maxNE = errorX[0];
            for (int i = 0; i < n; i++)
            {
                if (errorX[i] > maxNE)
                    maxNE = errorX[i];
            }
            return maxNE;
        }
        static int Main(string[] args)
        {
            a = Generating(a);
            c = Generating(c);
            b = GeneratingB(a, c, b);
            y = Generating(y);
            f = GeneratingF(a, b, c, y);
            A = CreateMatrix(Copy(a), Copy(b), Copy(c));
            Console.WriteLine("\nMatrix:\n");
            PrintMatrix(A);
            Console.WriteLine("\nVector y:\n");
            for (int i = 0; i < y.Length; i++)
            {
                Console.WriteLine("{0:f"+numbOfDecimals+"}",y[i]);
            }
            Console.WriteLine("\nVector f");
            for (int i = 0; i < f.Length; i++)
            {
                Console.WriteLine("{0:f" + numbOfDecimals + "}", f[i]);
            }
            answer = solveMatrix(n, Copy(a), Copy(b), Copy(c), Copy(f), answer);
            Console.WriteLine("\nMatrix with vector f:\n");
            PrintMatrixWithF(A, f);
            Console.WriteLine("\nVector y and answer:\n");
            for (int i = 0; i < answer.Length; i++)
            {
                Console.Write(y[i]+"\t\t");
                Console.Write("{0:f15}",answer[i]);
                Console.WriteLine("\n");
            }
            Console.WriteLine("\nМакимум-норма невязки: " + MaxNormDiscrepancy(a, b, c , answer, f));
            Console.WriteLine("\nМакимум-нормa погрешности: " + MaxNormError(y, answer));
            Console.ReadLine();
            return 0;
        }
    }
}
