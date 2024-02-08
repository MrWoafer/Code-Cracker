using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    static class Playfair
    {
        public static string Decrypt(string ciphertext, char[][] key)
        {
            StringBuilder plaintext = new StringBuilder();

            int[] index1 = new int[2];
            int[] index2 = new int[2];

            //for (int i = 0; i < plaintext.Length - 1; i += 2)
            for (int i = 0; i < ciphertext.Length - 1; i += 2)
            {
                Array.Copy(CipherLib.Utils.FindIn2DCharArray(ciphertext[i], key), index1, 2);
                Array.Copy(CipherLib.Utils.FindIn2DCharArray(ciphertext[i + 1], key), index2, 2);

                /*Console.Write(ciphertext[i] + " " + index1[0] + " " + index1[1]);
                Console.Write("\n");
                Console.Write(ciphertext[i + 1] + " " + index2[0] + " " + index2[1]);
                Console.Write("\n\n");*/

                /// Same row
                if (index1[0] == index2[0])
                {
                    plaintext.Append(key[index1[0]][CipherLib.Utils.Mod(index1[1] - 1, key[index1[0]].Length)]);
                    plaintext.Append(key[index2[0]][CipherLib.Utils.Mod(index2[1] - 1, key[index2[0]].Length)]);
                }
                /// Same column
                else if (index1[1] == index2[1])
                {
                    plaintext.Append(key[CipherLib.Utils.Mod(index1[0] - 1, key.Length)][index1[1]]);
                    plaintext.Append(key[CipherLib.Utils.Mod(index2[0] - 1, key.Length)][index2[1]]);
                }
                /// Form a rectangle
                else
                {
                    plaintext.Append(key[index1[0]][index2[1]]);
                    plaintext.Append(key[index2[0]][index1[1]]);
                }
            }

            return plaintext.ToString();
        }

        //public static Tuple<string, char[][]> CrackPlayfairReturnKey(string ciphertext, int height, int width, string alphabet, int numOfTrials = 1000)
        public static Tuple<string, char[][]> CrackPlayfairReturnKey(string ciphertext, int height, int width, string alphabet, bool displayCountdown = false)
        {
            char[][] currentKey = new char[height][];

            int index = 0;
            for (int i = 0; i < height; i++)
            {
                currentKey[i] = new char[width];
                for (int j = 0; j < width; j++)
                {
                    //currentKey[i][j] = alphabet[i * height + width];
                    //currentKey[i][j] = alphabet[i * height + j];
                    currentKey[i][j] = alphabet[index];
                    index++;
                }
            }
            for (int i = 0; i < 100; i++)
            {
                CipherLib.Utils.Copy2DArray(CipherLib.Annealing.MessWith2DCharKey(currentKey), currentKey);
            }

            //DisplayKey(currentKey);

            /*char[][] testKey = new char[][] {
                new char[] { 'm', 'y', 'f', 'a', 'c' },
                new char[] { 'e', 'i', 's', 'o', 'l' },
                new char[] { 'b', 'd', 'g', 'h', 'k' },
                new char[] { 'n', 'p', 'q', 'r', 't' },
                new char[] { 'u', 'v', 'w', 'x', 'z' } };
            CipherLib.Utils.Copy2DArray(testKey, currentKey);*/

            char[][] bestKey = new char[height][];
            for (int i = 0; i < height; i++)
            {
                bestKey[i] = new char[width];
            }
            char[][] overallBestKey = new char[height][];
            for (int i = 0; i < height; i++)
            {
                overallBestKey[i] = new char[width];
            }

            CipherLib.Utils.Copy2DArray(currentKey, bestKey);
            CipherLib.Utils.Copy2DArray(currentKey, overallBestKey);

            float currentScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, currentKey));
            float bestScore = currentScore;
            float overallBestScore = currentScore;

            string decodedMsg;

            //for (int trial = 0; trial < numOfTrials; trial++)
            for (float T = 20; T >= 0; T -= 0.2f)
            //for (float T = 20; T >= 0; T -= 20f)
            //for (float T = 20; T >= 0; T -= 2f)
            //for (float T = 1; T >= 0; T -= 0.2f)
            {
                //Console.Write("\b\b\b\b\b" + T);
                if (displayCountdown)
                {
                    //Console.Write("\b\b\b\b\bb\b\b\b" + T.ToString("n2"));
                    Console.Write("\b\b\b\b\bb\b\b\b" + T.ToString("n1"));
                }
                for (int count  = 0; count < 10000; count++)
                //for (int count = 0; count < 1; count++)
                {
                    //for (int i = 0; i < CipherLib.Utils.rand.Next() % 5 + 1; i++)
                    for (int i = 0; i < 1; i++)
                    {
                        //CipherLib.Utils.Copy2DArray(CipherLib.Annealing.MessWith2DCharKey(bestKey), currentKey);
                        //DisplayKey(bestKey);
                        //CipherLib.Utils.Copy2DArray(MessWithKey(bestKey), currentKey);
                        CipherLib.Utils.Copy2DArray(MessWithKey(currentKey), currentKey);
                        //Console.Write("\n");
                        //DisplayKey(bestKey);
                        //Console.Write("\n\n");
                    }

                    decodedMsg = Decrypt(ciphertext, currentKey);

                    currentScore = CipherLib.Annealing.QuadgramScore(decodedMsg);

                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;
                        CipherLib.Utils.Copy2DArray(currentKey, bestKey);

                        if (bestScore > overallBestScore)
                        {
                            overallBestScore = bestScore;
                            CipherLib.Utils.Copy2DArray(bestKey, overallBestKey);
                        }
                    }
                    else if (currentScore < bestScore)
                    {
                        bool ReplaceAnyway;
                        //ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(currentScore - bestScore, numOfTrials - trial);
                        ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(currentScore - bestScore, T);

                        if (ReplaceAnyway == true)
                        {
                            bestScore = currentScore;
                            CipherLib.Utils.Copy2DArray(currentKey, bestKey);
                        }
                        else
                        {
                            CipherLib.Utils.Copy2DArray(bestKey, currentKey);
                        }
                    }
                    else
                    {
                        CipherLib.Utils.Copy2DArray(bestKey, currentKey);
                    }
                }
            }

            //return new Tuple<string, char[][]>(Decrypt(ciphertext, bestKey), bestKey);
            return new Tuple<string, char[][]>(Decrypt(ciphertext, overallBestKey), overallBestKey);
        }

        public static void DisplayKey(char[][] key, int spaceNum = 2)
        {
            for (int i = 0; i < key.Length; i++)
            {
                for (int j = 0; j < key[i].Length; j++)
                {
                    Console.Write(key[i][j]);
                    if (j < key[i].Length - 1)
                    {
                        for (int k = 0; k < spaceNum - key[i][j].ToString().Length; k++)
                        {
                            Console.Write(" ");
                        }
                    }
                }
                if (i < key.Length - 1)
                {
                    Console.Write("\n");
                }
            }
        }

        private static char[][] MessWithKey(char[][] key)
        {
            //Console.Write("\n\n" + key.Length + " " + key[0].Length);
            int operation = CipherLib.Utils.rand.Next() % 50;
            switch (operation)
            {
                case 0:
                    return ReverseKey(key);
                case 1:
                    return ReverseRowOrder(key);
                case 2:
                    return ReverseColumnOrder(key);
                case 3:
                    return SwapRows(key, CipherLib.Utils.rand.Next() % key.Length, CipherLib.Utils.rand.Next() % key.Length);
                case 4:
                    return SwapColumns(key, CipherLib.Utils.rand.Next() % key[0].Length, CipherLib.Utils.rand.Next() % key[0].Length);
                default:
                    return CipherLib.Annealing.MessWith2DCharKey(key);
            }
        }

        private static char[][] SwapRows(char[][] key, int row1, int row2)
        {
            //Console.Write("\n\n" + row1 + " " + row2);
            for (int k = 0; k < key[row1].Length; k++)
            {
                char temp = key[row1][k];
                key[row1][k] = key[row2][k];
                key[row2][k] = temp;
            }
            return key;
        }

        private static char[][] SwapColumns(char[][] key, int column1, int column2)
        {
            //Console.Write("\n\n" + column1 + " " + column2);
            for (int i = 0; i < key.Length; i++)
            {
                char temp = key[i][column1];
                key[i][column1] = key[i][column2];
                key[i][column2] = temp;
            }
            return key;
        }

        private static char[][] ReverseRowOrder(char[][] key)
        {
            for (int i = 0; i < key.Length / 2; i++)
            {
                //Utils.Copy2DArray(SwapRows(key, i, key.Length - i), key);
                Utils.Copy2DArray(SwapRows(key, i, key.Length - 1 - i), key);
            }
            return key;
        }

        private static char[][] ReverseColumnOrder(char[][] key)
        {
            for (int i = 0; i < key[0].Length / 2; i++)
            {
                //Utils.Copy2DArray(SwapColumns(key, i, key[0].Length - i), key);
                Utils.Copy2DArray(SwapColumns(key, i, key[0].Length - 1 - i), key);
            }
            return key;
        }

        private static char[][] ReverseKey(char[][] key)
        {
            return ReverseColumnOrder(ReverseRowOrder(key));
        }
    }
}
