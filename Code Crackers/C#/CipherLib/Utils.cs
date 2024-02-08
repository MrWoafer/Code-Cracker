using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    static class Utils
    {
        public static Random rand = new Random();

        public static dynamic PickRandom(dynamic[] array)
        {
            return array[rand.Next() % array.Length];
        }

        public static int PickRandom(HashSet<int> hashSet)
        {
            return hashSet.ToArray()[rand.Next() % hashSet.Count];
        }

        public static int[] IntRangeArray(int start, int end)
        {
            int[] array = new int[end - start + 1];

            for (int i = start; i <= end; i++)
            {
                array[i - start] = i;
            }

            return array;
        }

        public static void Copy2DArray(Array[] sourceArray, Array[] destinationArray)
        {
            for (int i = 0; i < sourceArray.Length; i++)
            {
                Array.Copy(sourceArray[i], destinationArray[i], sourceArray[i].Length);
            }
        }

        public static bool Compare2DArray(int[][] array1, int[][] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i].Length != array2[i].Length)
                {
                    return false;
                }
                for (int j = 0; j < array1[i].Length; j++)
                {
                    if (array1[i][j] != array2[i][j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        //public static void DisplayArray(Array[] array)
        //public static void DisplayArray(Array array)
        public static void DisplayArray(Array array, bool printComma = false)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array.GetValue(i));
                if (i < array.Length - 1)
                {
                    if (printComma)
                    {
                        Console.Write(", ");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
            }
        }

        public static float RandFloat()
        {
            return (rand.Next() % 100000) / 100000f;
        }

        public static int RandIntFromRange(int min, int max)
        {
            return rand.Next() % (max - min + 1) + min;
        }

        /*public static float RandFloatFromRange(int min, int max)
        {
            return rand.Next() % (max - min) + min + RandFloat();
        }*/
        public static float RandFloatFromRange(float min, float max)
        {
            return (float)(rand.NextDouble() * (max - min) + min);
        }

        public static int[] FindIn2DCharArray(char item, char[][] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array[i].Length; j++)
                {
                    if (array[i][j] == item)
                    {
                        return new int[2] { i, j };
                    }
                }
            }
            return new int[2] { -1, -1 };
        }

        public static int Mod(int x, int y)
        {
            return (x % y + y) % y;
        }
        public static float Mod(float x, float y)
        {
            return (x % y + y) % y;
        }

        public static void CopyArray(Array sourceArray, Array destinationArray)
        {
            Array.Copy(sourceArray, destinationArray, sourceArray.Length);
        }

        public static void ClearLine()
        {
            Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b");
        }

        public static int GCD(int x, int y)
        {
            if (y == 0)
            {
                return x;
            }
            else
            {
                return GCD(y, x % y);
            }
        }

        public static int LCM(int x, int y)
        {
            return x * y / GCD(x, y);
        }
    }
}
