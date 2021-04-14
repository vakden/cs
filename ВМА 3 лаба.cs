using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMA_3
{
    class Program
    {
        static Random rnd = new Random();
        static int n = 10;
        static double[,] A = new double[n, n];
        static double[] answer = new double[n];
        static double[] f = new double[n];
        static double[] y = new double[n];
        static int left = -10, right = 10;
        static int numbOfDecimals = 2;
        static double eps = 0.00001;
        public static double[,] Mandarinka = {{0.2, 0.5, 0.8, 1, 1.3, 1.5, 1.8},
                                       {0, 0, 0, 0, 0, 0, 0}};
        public static int t = 0;


        static void PrintVector(double[] vec)
        {
            for (int i = 0; i < n; i++)
                Console.Write(vec[i] + "\n\n");
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
        static double[,] Generating(double[,] x)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    x[i, j] = rnd.Next(left * (int)(Math.Pow(10, numbOfDecimals)), right * (int)(Math.Pow(10, numbOfDecimals))) / Math.Pow(10, numbOfDecimals);
                }
            }
            return x;
        }
        static double[] GeneratingY(double[] x)
        {
            for (int j = 0; j < n; j++)
            {
                x[j] = rnd.Next(left * (int)(Math.Pow(10, numbOfDecimals)), right * (int)(Math.Pow(10, numbOfDecimals))) / Math.Pow(10, numbOfDecimals);
            }
            return x;
        }
        static double[] GeneratingF(double[] x, double[,] A, double[] Y)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    x[i] += Y[j] * A[i, j];
                }
            }
            return x;
        }
        public static double MaxNormDiscrepancy(double[,] A, double[] answer, double[] F)
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
                    desF[i] += A[i, j] * answer[j];
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
        static double[] copy(double[] a, double[] b)
        {
            for (int i = 0; i < n; i++)
            {
                a[i] = b[i];
            }
            return a;
        }
        static double[] DefaultSolution(double[] answer, double[,] A, double[] f, double eps)
        {
            double[] generation = new double[n];
            double[] generationNext = new double[n];
            double a;
            for (int i = 0; i < n; i++)
            {
                generation[i] = f[i] / A[i, i];
            }
            int numberOfIteration = 0;
            do
            {
                generation = copy(generation, generationNext);
                for (int i = 0; i < n; i++)
                {
                    generationNext[i] = f[i] / A[i, i];
                    for (int j = 0; j < n; j++)
                    {
                        if(i != j)
                        generationNext[i] += (A[i, j] / ((-1) * A[i, i])) * generation[j];
                        a = generationNext[i];
                    }
                    
                }
                numberOfIteration++;
            } while (Math.Abs(generation[0] - generationNext[0]) > eps);
            answer = generation;
            return answer;
        }
        
        public static double[] SimpleIterationsMethod(double[,] A, double[] F)
        {
            double[,] _A_ = new double[n,n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    _A_[i,j] = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    for (int k = 0; k < n; k++)
                        _A_[i,j] += A[k,i] * A[k,j];

            double[] _F_ = new double[n];
            for (int j = 0; j < n; j++)
                _F_[j] = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    _F_[i] += A[j,i] * F[j];

            double nm = mNorm(_A_);

            double[,] E = new double[n,n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    E[i,j] = 0;
                E[i,i] = 1;
            }
            double[] g = new double[n];
            double[,] B = new double[n,n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    B[i,j] = -1 * _A_[i,j] / nm;
                    B[i,j] += E[i,j];
                }
            }
            for (int i = 0; i < n; i++)
                g[i] = _F_[i] / nm;
            double[] x0 = new double[n];
            for (int i = 0; i < n; i++)
                x0[i] = g[i];
            double[] x1 = new double[n];
            for (int i = 0; i < n; i++)
            {
                double temp = 0;
                for (int j = 0; j < n; j++)
                {
                    temp += B[i,j] * x0[j];
                }
                x1[i] = temp + g[i];
            }

            while (Math.Abs(vNorm(x0, x1)) > eps)
            {
                for (int i = 0; i < n; i++)
                    x0[i] = x1[i];
                for (int i = 0; i < n; i++)
                {
                    double temp = 0;
                    for (int j = 0; j < n; j++)
                    {
                        temp += B[i,j] * x0[j];
                    }
                    x1[i] = temp + g[i];
                }
            }
            return x1;
        }
        public static double vNorm(double[] a, double[] b)
        {
            double max = 0;
            for (int i = 0; i < n; i++)
            {
                if (Math.Abs(a[i] - b[i]) > max)
                    max = Math.Abs(a[i] - b[i]);
            }
            return max;
        }
        public static int RelaxMethodK(double w, double[,] A, double[] F)
        {
            double[,] _A_ = new double[n,n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    _A_[i, j] = 0;
                }
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    for (int o = 0; o < n; o++)
                    {
                        _A_[i, j] += A[o, i] * A[o, j];
                    }
                }
            }

            double[] _F_ = new double[n];
            for (int j = 0; j < n; j++)
            {
                _F_[j] = 0;
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    _F_[i] += A[j, i] * F[j];
                }
            }

            double nm = mNorm(_A_);
            int k = 0;

            double[,] E = new double[n,n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    E[i, j] = 0;
                }
                E[i,i] = 1;
            }
            double[] g = new double[n];

            for (int i = 0; i < n; i++)
            {
                g[i] = _F_[i] / nm;
            }

            double[] x0 = new double[n];
            for (int i = 0; i < n; i++)
            {
                x0[i] = g[i];
            }

            double[] x1 = new double[n];
            do
            {
                for (int i = 0; i < n; i++)
                {
                    x0[i] = x1[i];
                }
                for (int i = 0; i < n; i++)
                {
                    double temp = 0;
                    for (int j = i + 1; j < n; j++)
                    {
                        temp += _A_[i,j] * x0[j] / _A_[i,i];
                    }
                    x1[i] = -temp + (_F_[i] / _A_[i,i]);
                    temp = 0;
                    for (int j = 0; j < i; j++)
                    {
                        temp += _A_[i, j] * x1[j] / _A_[i, i];
                    }
                    x1[i] -= temp;
                    x1[i] *= w;
                    x1[i] += (1 - w) * x0[i];
                }
                k++;
            } while (vNorm(x0, x1) / w >= eps);
            Mandarinka[1,t++] = k;
            return k;
        }

        public static double[] RelaxMethodN(double w, double[,] A, double[] F)
        {
            double[,] _A_ = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    _A_[i, j] = 0;
                }
            }

            double[] _F_ = new double[n];
            for (int j = 0; j < n; j++)
            {
                _F_[j] = 0;
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    for (int o = 0; o < n; o++)
                    {
                        _A_[i, j] += A[o, i] * A[o, j];
                    }
                }
            }

            double nm = mNorm(_A_);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    _F_[i] += A[j, i] * F[j];
                }
            }

            int k = 0;

            double[,] E = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    E[i, j] = 0;
                }
                E[i, i] = 1;
            }
            double[] g = new double[n];

            for (int i = 0; i < n; i++)
            {
                g[i] = _F_[i] / nm;
            }

            double[] x0 = new double[n];
            for (int i = 0; i < n; i++)
            {
                x0[i] = g[i];
            }

            double[] x1 = new double[n];
           
            do
            {
                for (int i = 0; i < n; i++)
                    x0[i] = x1[i];
                for (int i = 0; i < n; i++)
                {
                    double temp = 0;
                    for (int j = i + 1; j < n; j++)
                    {
                        temp += _A_[i, j] * x0[j] / _A_[i, i];
                    }
                    x1[i] = -temp + (_F_[i] / _A_[i, i]);
                    temp = 0;
                    for (int j = 0; j < i; j++)
                    {
                        temp += _A_[i, j] * x1[j] / _A_[i, i];
                    }
                    x1[i] -= temp;
                    x1[i] *= w;
                    x1[i] += (1 - w) * x0[i];                   
                }
            } while (vNorm(x0, x1) / w >= eps);
            for (int i = 0; i < n; i++)
                x0[i] = x1[i];
            for (int i = 0; i < n; i++)
            {
                double temp = 0;
                for (int j = i + 1; j < n; j++)
                {
                    temp += _A_[i, j] * x0[j] / _A_[i, i];
                }
                x1[i] = -temp + (_F_[i] / _A_[i, i]);
                temp = 0;
                for (int j = 0; j < i; j++)
                {
                    temp += _A_[i, j] * x1[j] / _A_[i, i];
                }
                x1[i] -= temp;
                x1[i] *= w;
                x1[i] += (1 - w) * x0[i];

            }
            return x1;
        }
        public static double[] RelaxMethod(double w, double[,] A, double[] F){
        double [,] _A_ = new double[n,n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    _A_[i, j] = 0;
                }
            }

        double [] _F_ = new double[n];
            for (int j = 0; j < n; j++)
            {
                _F_[j] = 0;
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    for (int o = 0; o < n; o++)
                    {
                        _A_[i, j] += A[o, i] * A[o, j];
                    }
                }
            }

        double nm = mNorm(_A_);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    _F_[i] += A[j, i] * F[j];
                }
            }

        int k = 0;

        double[,] E = new double[n,n];
        for (int i = 0; i < n; i++)
        {
                for (int j = 0; j < n; j++)
                {
                    E[i, j] = 0;
                }
            E[i,i] = 1;
        }
        double[] g = new double[n];

            for (int i = 0; i < n; i++)
            {
                g[i] = _F_[i] / nm;
            }

        double[] x0 = new double[n];
            for (int i = 0; i < n; i++)
            {
                x0[i] = g[i];
            }

        double[] x1 = new double[n];
        do {
            for (int i = 0; i < n; i++)
                x0[i] = x1[i];
            for (int i = 0; i < n; i++)
            {
                double temp = 0;
                for (int j = i + 1; j < n; j++)
                {
                    temp += _A_[i,j] * x0[j] / _A_[i,i];
                }
                x1[i] = -temp + (_F_[i] / _A_[i,i]);
                temp = 0;
                    for (int j = 0; j < i; j++)
                    {
                        temp += _A_[i, j] * x1[j] / _A_[i, i];
                    }
                x1[i] -= temp;
                x1[i] *= w;
                x1[i] += (1 - w) * x0[i];
            }
        } while (vNorm(x0, x1) / w >= eps);
        return x1;
    }
    static double mNorm(double[,] m){
        double max = 0;
        for (int i = 0; i < n; i++)
        {
            double temp = 0;
                for (int j = 0; j < n; j++)
                {
                    temp += Math.Abs(m[i, j]);
                }
                if (max < temp)
                {
                    max = temp;
                }
        }
        return max;
    }
        public static double[,] sortingMandarinka(double[,] arr)
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = i + 1; j < 7; j++)
                {
                    if (arr[1,i] > arr[1,j])
                    {
                        double a = arr[0,i];
                        double b = arr[1,i];
                        arr[0,i] = arr[0,j];
                        arr[1,i] = arr[1,j];
                        arr[0,j] = a;
                        arr[1,j] = b;
                    }
                }
            }
            return arr;
        }


        static int Main(string[] args)
        {
            A = Generating(A);
            Console.WriteLine("Исходная матрица: ");
            y = GeneratingY(y);
            f = GeneratingF(f, A, y);
            PrintMatrixWithF(A, f);
            Console.WriteLine("Исходное точное решение:");
            PrintVector(y);
            Console.WriteLine("Требуемая точность нахождения решения:\n" + eps);
            double[] desX = SimpleIterationsMethod(A, f);
            Console.WriteLine("Полученное точное решение методом простых итераций:");
            PrintVector(desX);
            Console.WriteLine("Максимум-норма погрешности метода простых итераций: \n" + MaxNormError(y, desX));
            Console.WriteLine("Значения параметра релаксации w, количества итераций k + 1 для достижения заданной точности и максимум-норма погрещностей метода релаксаций:");
            for (int i = 0; i < 7; i++)
            {
                Console.WriteLine("w = " + Mandarinka[0,i] + ", k + 1 = " + RelaxMethodK(Mandarinka[0,i], A, f) + ", максимум-норма погрешности " + MaxNormError(RelaxMethodN(Mandarinka[0, i], A, f), RelaxMethod(Mandarinka[0,i], A, f)));
            }
            Mandarinka = sortingMandarinka(Mandarinka);
            Console.WriteLine("Полученное точное решение методом релаксаций для параметра w с минимальным количеством итераций w = " + Mandarinka[0,0] + ":");
            desX = RelaxMethod(Mandarinka[0,0], A, f);
            PrintVector(desX);
            Console.WriteLine("Максимум-норма погрешности метода релаксаций для параметра w с минимальным количеством итераций w = " + Mandarinka[0,0] + ":\n" + MaxNormError(y, desX));

            Console.ReadLine();
            return 0;
        }
    }
}

