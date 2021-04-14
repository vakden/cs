using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMA_4
{
    public class iterativePowerMethod
    {
        int size = 6;
        //Максимальное значение вектора
        private static double vectorNorm(double[] y0)
        {
            double yNorm = Math.Abs(y0[0]);
            for (int i = 1; i < y0.Length; i++)
            {
                if (yNorm < Math.Abs(y0[i]))
                {
                    yNorm = Math.Abs(y0[i]);
                }
            }
            return yNorm;
        }
        //Норма разности векторов
        private static double vectorsDifferenceNorm(double[] y1, double[] y2)
        {
            double yNorm = Math.Abs(y1[0] - y2[0]);
            for (int i = 1; i < y1.Length; i++)
            {
                if (yNorm < Math.Abs(y1[i] - y2[i]))
                    yNorm = Math.Abs(y1[i] - y2[i]);
            }
            return yNorm;
        }
        //Деление вектора на скаляр
        private static double[] divisionVectorScalar(double[] y, double n)
        {
            double[] yN = new double[y.Length];
            for (int i = 0; i < y.Length; i++)
                yN[i] = y[i] / n;
            return yN;
        }
        //Умножение мтарицы на вектор
        private static double[] multiplicationMatrixVector(double[,] A, double[] y)
        {
            double[] y1 = new double[y.Length];
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                    y1[i] += A[i,j] * y[j];
            }
            return y1;
        }
        //Максимальное значение вектора
        private static double[] divisionVectors(double[] y1, double[] y2)
        {
            double[] y = new double[y1.Length];
            for (int i = 0; i < y1.Length; i++)
                y[i] = y1[i] / y2[i];
            return y;
        }
        //Среднее значение координат векторов
        private static double averageEigenValue(double[] y)
        {
            double sum = 0;
            for (int i = 0; i < y.Length; i++)
                sum += y[i];
            return Math.Abs(sum / y.Length);
        }
        //Максимальное собственное значение матрицы
        public static double maxEigenValue(double[,] A, double eps, double[] y0)
        {
            double[] lBefore = new double[y0.Length]; //Вектор собственных значений на k-ой итерации
            double[] lAfter = new double[y0.Length]; //Вектор собственных значений k+1 -ой итерации

            double[] yBefore = new double[y0.Length];
            Array.Copy(y0, yBefore, y0.Length); //Вектор значений на k-ой итерации
            double[] yNormBefore = divisionVectorScalar(yBefore, vectorNorm(yBefore)); //Вектор нормированных значений на k-ой итерации

            double[] yAfter; //Вектор значений на k+1 -ой итерации
            double delta = 1.0; //Условие выхода из цикла (норма разности векторов собственных значений на k-ой и k+1 -ой итерациях

            while (delta > eps)
            { //Пока не достигнута заданная точность
                yAfter = multiplicationMatrixVector(A, yNormBefore);

                lAfter = divisionVectors(yAfter, yNormBefore);

                double norm = vectorNorm(yAfter); //норма вектора значений на k+1 -ой итерации
                yNormBefore = divisionVectorScalar(yAfter, norm);

                delta = vectorsDifferenceNorm(lAfter, lBefore);
                                
                Array.Copy(lAfter, lBefore, lAfter.Length);
            }

            return averageEigenValue(lAfter);
        }
        //Собственный вектор, соответствующий максимальному собственному значению
        //Смысл переменных такой же, как в методе выше, отличие в возвращаемом значении
        public static double[] vectorEigenValues(double[,] A, double eps, double[] y0)
        {
            double[] lBefore = new double[y0.Length];
            double[] lAfter = new double[y0.Length];

            double[] yBefore = new double[y0.Length];    
               Array.Copy(y0, yBefore, y0.Length);
            double[] yNormBefore = divisionVectorScalar(yBefore, vectorNorm(yBefore));

            double[] yAfter;
            double delta = 1.0;

            while (delta > eps)
            {
                yAfter = multiplicationMatrixVector(A, yNormBefore);

                lAfter = divisionVectors(yAfter, yNormBefore);

                double norm = vectorNorm(yAfter);
                yNormBefore = divisionVectorScalar(yAfter, norm);

                delta = vectorsDifferenceNorm(lAfter, lBefore);

                Array.Copy(lAfter, lBefore, lAfter.Length);
            }

            return yNormBefore;
        }
        //Количество итераций метода
        //Смысл переменных такой же, как в методе выше, отличие в возвращаемом значении
        public static int iterationsNum(double[,] A, double eps, double[] y0)
        {
            double[] lBefore = new double[y0.Length];
            double[] lAfter = new double[y0.Length];

            double[] yBefore = new double[y0.Length];
            Array.Copy(y0, yBefore, y0.Length);
            double[] yNormBefore = divisionVectorScalar(yBefore, vectorNorm(yBefore));

            double[] yAfter;
            double delta = 1.0;

            int k = 0;

            while (delta > eps)
            {
                yAfter = multiplicationMatrixVector(A, yNormBefore);

                lAfter = divisionVectors(yAfter, yNormBefore);

                double norm = vectorNorm(yAfter);
                yNormBefore = divisionVectorScalar(yAfter, norm);

                delta = vectorsDifferenceNorm(lAfter, lBefore);

                Array.Copy(lAfter, lBefore, lAfter.Length);

                k++;
            }

            return k;
        }
    }

    public class dotProductMethod
    {
        //Максимальное значение в векторе
        private static double vectorNorm(double[] y0)
        {
            double yNorm = Math.Abs(y0[0]);
            for (int i = 0; i < y0.Length; i++)
            {
                if (yNorm < Math.Abs(y0[i]))
                {
                    yNorm = Math.Abs(y0[i]);
                }
            }
            return yNorm;
        }
        //Деление вектора на скаляр
        private static double[] divisionVectorScalar(double[] y, double n)
        {
            double[] yN = new double[y.Length];
            for (int i = 0; i < y.Length; i++)
            {
                yN[i] = y[i] / n;
            }
            return yN;
        }
        //Умножение матрицы на вектор
        private static double[] multiplicationMatrixVector(double[,] A, double[] y)
        {
            double[] y1 = new double[y.Length];
            for (int i = 0; i < y1.Length; i++)
            {
                for (int j = 0; j < y1.Length; j++)
                {
                    y1[i] += A[i, j] * y[j];
                }
            }
            return y1;
        }
        //Скалярное произведение векторов
        private static double scalarMultiplicationVectors(double[] y1, double[] y2)
        {
            double y = 0;
            for (int i = 0; i < y1.Length; i++)
                y += y1[i] * y2[i];
            return y;
        }
        //Максимальное собственное значение
        public static double maxEigenValue(double[,] A, double eps, double[] y0)
        {
            double lBefore = 0;
            double lAfter = 0;

            double[] yBefore = new double[y0.Length];
            Array.Copy(y0, yBefore, y0.Length);

            double[] yNormBefore = divisionVectorScalar(yBefore, vectorNorm(yBefore));

            double[] yAfter;
            double delta = 1.0;

            while (delta > eps)
            {
                yAfter = multiplicationMatrixVector(A, yNormBefore);

                lAfter = scalarMultiplicationVectors(yAfter, yNormBefore) / scalarMultiplicationVectors(yNormBefore, yNormBefore);

                double norm = vectorNorm(yAfter);
                yNormBefore = divisionVectorScalar(yAfter, norm);

                delta = Math.Abs(lAfter - lBefore);

                lBefore = lAfter;
            }

            return Math.Abs(lAfter);
        }
        //Собственный вектор, соответствующий максимальному собственному значению
        //Смысл переменных такой же, как в методе выше, отличие в возвращаемом значении
        public static double[] vectorEigenValues(double[,] A, double eps, double[] y0)
        {
            double lBefore = 0;
            double lAfter = 0;

            double[] yBefore = new double[y0.Length];
            Array.Copy(y0, yBefore, y0.Length);
            double[] yNormBefore = divisionVectorScalar(yBefore, vectorNorm(yBefore));

            double[] yAfter;
            double delta = 1.0;

            while (delta > eps)
            {
                yAfter = multiplicationMatrixVector(A, yNormBefore);

                lAfter = scalarMultiplicationVectors(yAfter, yNormBefore) / scalarMultiplicationVectors(yNormBefore, yNormBefore);

                double norm = vectorNorm(yAfter);
                yNormBefore = divisionVectorScalar(yAfter, norm);

                delta = Math.Abs(lAfter - lBefore);

                lBefore = lAfter;
            }

            return yNormBefore;
        }
        //Количество итераций метода
        //Смысл переменных такой же, как в методе выше, отличие в возвращаемом значении
        public static int iterationsNum(double[,] A, double eps, double[] y0)
        {
            double lBefore = 0;
            double lAfter = 0;

            double[] yBefore = new double[y0.Length];
            Array.Copy(y0, yBefore, y0.Length);
            double[] yNormBefore = divisionVectorScalar(yBefore, vectorNorm(yBefore));

            double[] yAfter;
            double delta = 1.0;

            int k = 0;

            while (delta > eps)
            {
                yAfter = multiplicationMatrixVector(A, yNormBefore);

                lAfter = scalarMultiplicationVectors(yAfter, yNormBefore) / scalarMultiplicationVectors(yNormBefore, yNormBefore);

                double norm = vectorNorm(yAfter);
                yNormBefore = divisionVectorScalar(yAfter, norm);

                delta = Math.Abs(lAfter - lBefore);

                lBefore = lAfter;

                k++;
            }

            return k;
        }
    }

    public class Test
    {

        public static double eps = 1e-6;
        public static double[,] B = {{ 1.342,  0.432, -0.599,  0.202,  0.603, -0.202},
                                     { 0.432,  1.342,  0.256, -0.599,  0.204,  0.304},
                                     {-0.599,  0.256,  1.342,  0.532,  0.101,  0.506},
                                     { 0.202, -0.599,  0.532,  1.342,  0.106, -0.311},
                                     { 0.603,  0.204,  0.101,  0.106,  1.342,  0.102},
                                     {-0.202,  0.304,  0.506, -0.311,  0.102,  1.342}};
        public static int k = 2;
        //public static double[,] C = {{ 0.05, 0,    0,    0,    0,    0    },
        //                             { 0,    0.03, 0,    0,    0,    0    },
        //                             { 0,    0,    0.02, 0,    0,    0    },
        //                             { 0,    0,    0,    0.04, 0,    0    },
        //                             { 0,    0,    0,    0,    0.06, 0    },
        //                             { 0,    0,    0,    0,    0,    0.07 } };
        public static double[] C = { 0.05, 0.03, 0.02, 0.04, 0.06, 0.07 };
        //Получение матрицы А
        public static double[,] getA(double[,] gB, int gK, double[] gC)
        {
            double[,] gA = new double[gB.Length,gB.Length];
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 6; j++)
                    gA[i, j] = gB[i, j];
            for (int i = 0; i < 6; i++)
            {
                gA[i,i] = gB[i,i] + gK * gC[i];
            }
            return gA;
        }

        //Печать матрицы
        public static void matrixPrinting(double[,] M, int m, int p)
        {
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < p; j++)
                {
                    Console.Write("{0:f7}", M[i,j] + "  ");
                }
                Console.WriteLine("\n");
            }
            Console.WriteLine("\n");
        }
        //Печать вектора
        public static void vectorPrinting(double[] M)
        {
            for (int i = 0; i < M.Length; i++)
            {
                Console.WriteLine("{0:f7}", M[i]);
            }
        }
        //Получение вектора начальных значений
        public static double[] getY0(int n)
        {
            double[] y = new double[n];
            for (int i = 0; i < n; i++)
                y[i] = 1;
            return y;
        }
        //Умножение матрицы на вектор
        public static double[] multiplicationMatrixVector(double[,] A, double[] y)
        {
            double[] y1 = new double[y.Length];
            for (int i = 0; i < y1.Length; i++)
            {
                for (int j = 0; j < y1.Length; j++)
                    y1[i] += A[i,j] * y[j];
            }
            return y1;
        }
        //Умножение вектора на скаляр
        private static double[] multiplicationVectorScalar(double[] y, double n)
        {
            double[] yN = new double[y.Length];
            for (int i = 0; i < y.Length; i++)
                yN[i] = y[i] * n;
            return yN;
        }
        //Вычисление невязки (вектора r1)
        private static double[] vectorR1(double[,] A, double l, double[] x)
        {
            double[] c = new double[x.Length];
            double[] ax = multiplicationMatrixVector(A, x);
            double[] lx = multiplicationVectorScalar(x, l);
            for (int i = 0; i < x.Length; i++)
                c[i] = ax[i] - lx[i];
            return c;
        }

        public static void Main(String[] args)
        {
            int size = 6;
            double[,] A = getA(B, k, C);
            Console.WriteLine("Исходная матрица: ");
            matrixPrinting(A, size, size);

            double[] yIPM = getY0(size);
            double[] yDPM = getY0(size);
            Console.WriteLine("\nНачальный вектор для итерационно степенного метода: ");
            vectorPrinting(yIPM);
            Console.WriteLine("\nНачальный вектор для метода скалярных произведений:");
            vectorPrinting(yDPM);

            double maxEigenIPM = iterativePowerMethod.maxEigenValue(A, eps, yIPM);
            double maxEigenDPM = dotProductMethod.maxEigenValue(A, eps, yDPM);
            Console.WriteLine("\nМаксимальное приближенное по модулю собственное значение по итерационно степенному методу: \n" + maxEigenIPM +
                    "\nМаксимальное приближенное по модулю собственное значение по методу скалярных произведений: \n" + maxEigenDPM + "\n");

            double[] eigenvaluesIPM = iterativePowerMethod.vectorEigenValues(A, eps, yIPM);
            double[] eigenvaluesDPM = dotProductMethod.vectorEigenValues(A, eps, yDPM);
            Console.WriteLine("\nСобственных вектор по итерационно степенному методу: ");
            vectorPrinting(eigenvaluesIPM);
            Console.WriteLine("\nСобственных вектор по методу скалярных произведений:");
            vectorPrinting(eigenvaluesDPM);

            double[] r1IPM = vectorR1(A, maxEigenIPM, eigenvaluesIPM);
            double[] r1DMP = vectorR1(A, maxEigenDPM, eigenvaluesDPM);
            Console.WriteLine("\nВектор r1 для итерационно степенного метода: ");
            vectorPrinting(r1IPM);
            Console.WriteLine("\nВектор r1 для метода скалярных произведений:");
            vectorPrinting(r1DMP);

            int iterationsIPM = iterativePowerMethod.iterationsNum(A, eps, yIPM);
            int iterationsDPM = dotProductMethod.iterationsNum(A, eps, yDPM);
            Console.WriteLine("\nЧисло итераций для заданной точности по итерационно степенного метода: " + iterationsIPM +
                    "\nЧисло итераций для заданной точности по методу скалярных произведений: " + iterationsDPM);
            Console.ReadLine();
        }
    }

}

