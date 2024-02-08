using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    class Straddle
    {
        /// Decrypt the ciphertext if the message was left as digits after the straddle checkerboard step
        public static string Decrypt(string ciphertext, char[][] key)
        {
            StringBuilder plaintext = new StringBuilder();

            List<int> dotsInFirstRow = new List<int>();
            for (int j = 0; j < key[0].Length; j++)
            {
                //if (key[0][j] == '.')
                if (key[0][j] == ' ')
                {
                    dotsInFirstRow.Add(j);
                }
            }

            int i = 0;
            while (i < ciphertext.Length)
            {
                //if (key[0][ciphertext[i] - '0'] == '.')
                if (key[0][ciphertext[i] - '0'] == ' ')
                {
                    //plaintext.Append(key[ciphertext[i] - '0'][ciphertext[i + 1] - '0']);
                    //plaintext.Append(key[ciphertext[i] - '0'][dotsInFirstRow.IndexOf(ciphertext[i + 1] - '0') + 1]);
                    if (i + 1 < ciphertext.Length)
                    {
                        plaintext.Append(key[dotsInFirstRow.IndexOf(ciphertext[i] - '0') + 1][ciphertext[i + 1] - '0']);
                    }
                    i += 2;
                }
                else
                {
                    plaintext.Append(key[0][ciphertext[i] - '0']);
                    i += 1;
                }
            }

            return plaintext.ToString();
        }

        /// Encrypt the plaintext and leave it as the digits
        public static string Encrypt(string plaintext, char[][] key)
        {
            StringBuilder ciphertext = new StringBuilder();

            List<int> dotsInFirstRow = new List<int>();
            for (int j = 0; j < key[0].Length; j++)
            {
                //if (key[0][j] == '.')
                if (key[0][j] == ' ')
                {
                    dotsInFirstRow.Add(j);
                }
            }

            int[] index = new int[2];

            for (int i = 0; i < plaintext.Length; i++)
            {
                Array.Copy(CipherLib.Utils.FindIn2DCharArray(plaintext[i], key), index, 2);

                /// In first row
                if (index[0] == 0)
                {
                    ciphertext.Append((char)(index[1] + '0'));
                }
                /// Not in first row
                else
                {
                    //ciphertext.Append((char)(dotsInFirstRow[index[0]] + '0'));
                    ciphertext.Append((char)(dotsInFirstRow[index[0] - 1] + '0'));
                    ciphertext.Append((char)(index[1] + '0'));
                }
            }
            return ciphertext.ToString();
        }

        public static void DisplayKey(char[][] key)
        {
            for (int i = 0; i < key.Length; i++)
            {
                for (int j = 0; j < key[i].Length; j++)
                {
                    Console.Write(key[i][j]);
                    if (j < key[i].Length - 1)
                    {
                        Console.Write(" ");
                    }
                }
                if (i < key.Length - 1)
                {
                    Console.Write("\n");
                }
            }
        }

        //public static string Crack(string ciphertext, string alphabet = "abcdefghijklmnopqrstuvwxyz#/", int numOfTrials = 1000)
        public static string Crack(string ciphertext, int rows = 3, int columns = 10, string alphabet = "abcdefghijklmnopqrstuvwxyz#/", int numOfTrials = 10000)
        {
            //alphabet = ".." + alphabet;
            //alphabet = Enumerable.Repeat(".", rows - 1).ToString() + alphabet;
            //alphabet = new string('.', rows - 1) + alphabet;
            //alphabet = new string(' ', rows - 1) + alphabet;
            //alphabet = ".." + alphabet;
            alphabet = "  " + alphabet;

            char[][] currentKey = new char[rows][];
            int index = 0;
            for (int i = 0; i < currentKey.Length; i++)
            {
                currentKey[i] = new char[columns];
                for (int j = 0; j < currentKey[i].Length; j++)
                {
                    currentKey[i][j] = alphabet[index];
                    index++;
                }
            }
            for (int i = 0; i < 100; i++)
            {
                CipherLib.Utils.Copy2DArray(MessWithKey(currentKey), currentKey);
            }
            //DisplayKey(currentKey);

            char[][] bestKey = new char[rows][];
            for (int i = 0; i < bestKey.Length; i++)
            {
                bestKey[i] = new char[columns];
            }
            char[][] overallBestKey = new char[rows][];
            for (int i = 0; i < overallBestKey.Length; i++)
            {
                overallBestKey[i] = new char[columns];
            }

            CipherLib.Utils.Copy2DArray(currentKey, bestKey);
            CipherLib.Utils.Copy2DArray(currentKey, overallBestKey);

            float currentScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, currentKey));
            float bestScore = currentScore;
            float overallBestScore = currentScore;

            string decodedMsg;

            for (int i = 0; i < numOfTrials; i++)
            //for (float T = 20; T >= 0; T -= 0.2f)
            //for (float T = 1; T >= 0; T -= 1f)
            //for (float T = 1; T >= 0; T -= 0.2f)
            {
                //for (int trial = 0; trial < numOfTrials; trial++)
                for (int trial = 0; trial < 1; trial++)
                //for (int trial = 0; trial < 2; trial++)
                {
                    CipherLib.Utils.Copy2DArray(MessWithKey(currentKey), currentKey);

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
                    /*else if (currentScore < bestScore)
                    {
                        bool ReplaceAnyway;
                        //ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(currentScore - bestScore, numOfTrials - i);
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
                    }*/
                    else
                    {
                        CipherLib.Utils.Copy2DArray(bestKey, currentKey);
                    }
                }
            }

            /*Console.Write("\n");
            Console.Write("\n");
            DisplayKey(overallBestKey);
            Console.Write("\n");
            Console.Write("\n");*/

            return Decrypt(ciphertext, overallBestKey);
            //return Decrypt(ciphertext, bestKey);
        }

        public static char[][] MessWithKey(char[][] key)
        {
            //int operation = Utils.rand.Next() % 6;
            //int operation = Utils.rand.Next() % 2;
            int operation = Utils.rand.Next() % 20;
            char temp;
            switch (operation)
            {
                // Swap two things in the first row
                //case 0:
                //case int n when (n > 2 && n < 10):
                /*case int n when (n > 2 && n < 15):
                //case int n when (n > 2 && n < 18):
                    int index1 = Utils.rand.Next() % key[0].Length;
                    int index2 = Utils.rand.Next() % key[0].Length;
                    temp = key[0][index1];
                    key[0][index1] = key[0][index2];
                    key[0][index2] = temp;
                    break;
                case 1:
                    return ReverseColumnOrder(key);
                case 2:
                    return SwapColumns(key, Utils.rand.Next() % key[0].Length, Utils.rand.Next() % key[0].Length);
                // Swap two things anywhere, as long as they aren't fullstops
                default:
                    int column1 = Utils.rand.Next() % key[0].Length;
                    int row1 = Utils.rand.Next() % key.Length;
                    while (key[row1][column1] == '.')
                    {
                        column1 = Utils.rand.Next() % key[0].Length;
                        row1 = Utils.rand.Next() % key.Length;
                    }
                    int column2 = Utils.rand.Next() % key[0].Length;
                    int row2 = Utils.rand.Next() % key.Length;
                    while (key[row2][column2] == '.')
                    {
                        column2 = Utils.rand.Next() % key[0].Length;
                        row2 = Utils.rand.Next() % key.Length;
                    }
                    temp = key[row1][column1];
                    key[row1][column1] = key[row2][column2];
                    key[row2][column2] = temp;
                    break;*/
                /// Reverse column order
                case 0:
                    return ReverseColumnOrder(key);
                /// Swap two columns
                case 1:
                    return SwapColumns(key, Utils.rand.Next() % key[0].Length, Utils.rand.Next() % key[0].Length);
                // Swap two things anywhere
                default:
                    int column1 = Utils.rand.Next() % key[0].Length;
                    int row1 = Utils.rand.Next() % key.Length;
                    /*while (key[row1][column1] == '#' || key[row1][column1] == '/')
                    {
                        column1 = Utils.rand.Next() % key[0].Length;
                        row1 = Utils.rand.Next() % key.Length;
                    }*/
                    int column2 = column1;
                    int row2 = row1;
                    //if (key[row1][column1] == '.')
                    if (key[row1][column1] == ' ')
                    {
                        //while (key[row2][column2] == '.')
                        while (key[row2][column2] == ' ')
                        {
                            column2 = Utils.rand.Next() % key[0].Length;
                        }
                    }
                    else
                    {
                        column2 = Utils.rand.Next() % key[0].Length;
                        row2 = Utils.rand.Next() % key.Length;
                        //while (key[row2][column2] == '.' || key[row2][column2] == '#' || key[row2][column2] == '/')
                        //while (key[row2][column2] == '.')
                        while (key[row2][column2] == ' ')
                        {
                            column2 = Utils.rand.Next() % key[0].Length;
                            row2 = Utils.rand.Next() % key.Length;
                        }
                    }
                    
                    temp = key[row1][column1];
                    key[row1][column1] = key[row2][column2];
                    key[row2][column2] = temp;
                    break;
            }
            return key;
        }

        public static char[][] SwapColumns(char[][] key, int column1, int column2)
        {
            for (int i = 0; i < key.Length; i++)
            {
                char temp = key[i][column1];
                key[i][column1] = key[i][column2];
                key[i][column2] = temp;
            }
            return key;
        }

        public static char[][] ReverseColumnOrder(char[][] key)
        {
            for (int i = 0; i < key[0].Length / 2; i++)
            {
                Utils.Copy2DArray(SwapColumns(key, i, key[0].Length - 1 - i), key);
            }
            return key;
        }

        public static string Crack2(string ciphertext, string alphabet = "abcdefghijklmnopqrstuvwxyz#/", int numOfTrials = 1000)
        {
            char[][] currentKey = new char[3][];
            for (int i = 0; i < currentKey.Length; i++)
            {
                currentKey[i] = new char[10];
            }
            for (int i = 0; i < 100; i++)
            {
                CipherLib.Utils.Copy2DArray(MessWithKey2(currentKey), currentKey);
            }

            char[][] bestKey = new char[3][];
            for (int i = 0; i < bestKey.Length; i++)
            {
                bestKey[i] = new char[10];
            }
            char[][] overallBestKey = new char[3][];
            for (int i = 0; i < overallBestKey.Length; i++)
            {
                overallBestKey[i] = new char[10];
            }

            CipherLib.Utils.Copy2DArray(currentKey, bestKey);
            CipherLib.Utils.Copy2DArray(currentKey, overallBestKey);

            float currentScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, currentKey));
            float bestScore = currentScore;
            float overallBestScore = currentScore;

            string decodedMsg;

            //for (int i = 0; i < 9; i++)
            for (int i = 0; i + 1 < currentKey[0].Length; i++)
            {
                //for (int j = 0; j + i < 10; j++)
                //for (int j = i + 1; j + i < currentKey.Length; j++)
                for (int j = i + 1; j + i < currentKey[0].Length; j++)
                {
                    //currentKey[0][i] = '.';
                    currentKey[0][i] = ' ';
                    //currentKey[0][j] = '.';
                    currentKey[0][j] = ' ';
                    int index = 0;
                    for (int a = 0; a < currentKey.Length; a++)
                    //for (int a = 0; a + 1 < currentKey.Length; a++)
                    {
                        for (int b = 0; b < currentKey[a].Length; b++)
                        //for (int b = a + 1; b < currentKey[a].Length; b++)
                        {
                            if (!(a == 0 && ((b == i) || (b == j))))
                            {
                                currentKey[a][b] = alphabet[index];
                                index++;
                            }
                        }
                    }
                    ////Console.Write("After set-up\n");
                    //DisplayKey(currentKey);
                    //Console.Write("\n\n");
                    for (int mess = 0; mess < 100; mess++)
                    {
                        CipherLib.Utils.Copy2DArray(MessWithKey2(currentKey), currentKey);
                    }
                    //Console.Write("After mess-up\n");
                    //DisplayKey(currentKey);
                    //Console.Write("\n\n");

                    CipherLib.Utils.Copy2DArray(currentKey, bestKey);

                    currentScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, currentKey));
                    bestScore = currentScore;

                    for (int trial = 0; trial < numOfTrials; trial++)
                    {
                        CipherLib.Utils.Copy2DArray(MessWithKey2(currentKey), currentKey);

                        decodedMsg = Decrypt(ciphertext, currentKey);

                        currentScore = CipherLib.Annealing.QuadgramScore(decodedMsg);

                        if (currentScore > bestScore)
                        {
                            bestScore = currentScore;
                            CipherLib.Utils.Copy2DArray(currentKey, bestKey);
                        }
                        else
                        {
                            CipherLib.Utils.Copy2DArray(bestKey, currentKey);
                        }
                    }

                    if (bestScore > overallBestScore)
                    {
                        overallBestScore = bestScore;
                        CipherLib.Utils.Copy2DArray(bestKey, overallBestKey);
                    }
                }
            }

            return Decrypt(ciphertext, overallBestKey);
        }

        private static char[][] MessWithKey2(char[][] key)
        {
            int column1 = Utils.rand.Next() % key[0].Length;
            int row1 = Utils.rand.Next() % key.Length;
            //while (key[row1][column1] == '.')
            while (key[row1][column1] == ' ')
            {
                column1 = Utils.rand.Next() % key[0].Length;
                row1 = Utils.rand.Next() % key.Length;
            }
            int column2 = Utils.rand.Next() % key[0].Length;
            int row2 = Utils.rand.Next() % key.Length;
            //while (key[row2][column2] == '.')
            while (key[row2][column2] == ' ')
            {
                column2 = Utils.rand.Next() % key[0].Length;
                row2 = Utils.rand.Next() % key.Length;
            }

            char temp = key[row1][column1];
            key[row1][column1] = key[row2][column2];
            key[row2][column2] = temp;
            return key;
        }

        public static string Crack3(string ciphertext, string alphabet = "abcdefghijklmnopqrstuvwxyz#/", int numOfTrials = 1000)
        {
            char[][] currentKey = new char[3][];
            for (int i = 0; i < currentKey.Length; i++)
            {
                currentKey[i] = new char[10];
            }
            for (int i = 0; i < 100; i++)
            {
                CipherLib.Utils.Copy2DArray(MessWithKey2(currentKey), currentKey);
            }

            //int[] bestDotPlacement = new int[2];

            //float bestIOC = 0;
            //float newIOC;

            //string newMonoSub;
            //string bestMonoSub = "";

            float bestScore = float.MinValue;
            float currentScore;

            string bestDecrypt = "";
            string currentDecrypt;

            for (int i = 0; i + 1 < currentKey[0].Length; i++)
            {
                for (int j = i + 1; j + i < currentKey[0].Length; j++)
                {
                    //currentKey[0][i] = '.';
                    currentKey[0][i] = ' ';
                    //currentKey[0][j] = '.';
                    currentKey[0][j] = ' ';
                    int index = 0;
                    for (int a = 0; a < currentKey.Length; a++)
                    {
                        for (int b = 0; b < currentKey[a].Length; b++)
                        {
                            if (!(a == 0 && ((b == i) || (b == j))))
                            {
                                currentKey[a][b] = alphabet[index];
                                index++;
                            }
                        }
                    }

                    //newMonoSub = Decrypt(ciphertext, currentKey);
                    currentDecrypt = Annealing.CrackCustomMonoSub(Decrypt(ciphertext, currentKey), alphabet: alphabet, numOfTrials: 1000);

                    //newIOC = Math.Abs(CipherLib.Annealing.IOCAlphanumeric(newMonoSub) - 0.065f);
                    //newIOC = Math.Abs(CipherLib.Annealing.IOCAlphanumeric(newMonoSub) - 0.067f);
                    currentScore = CipherLib.Annealing.QuadgramScore(currentDecrypt);

                    //if ((i == 0 && j == 1) || newIOC > bestIOC)
                    //if ((i == 0 && j == 1) || newIOC < bestIOC)
                    if (currentScore > bestScore)
                    {
                        //bestIOC = newIOC;
                        //bestMonoSub = newMonoSub;
                        //Console.Write(i + " " + j + "\n");
                        bestScore = currentScore;
                        bestDecrypt = currentDecrypt;
                    }
                }
            }

            //return Annealing.CrackMonoSub(bestMonoSub, numOfTrials);
            //return Annealing.CrackCustomMonoSub(bestMonoSub, alphabet: alphabet, numOfTrials: 1000);
            return bestDecrypt;
        }
    }

    class StraddleVigenere
    {
        public static string Decrypt(string ciphertext, char[][] checkerboard, string key)
        {
            /*StringBuilder undoneVigenere = new StringBuilder();
            for (int i = 0; i < ciphertext.Length; i++)
            {
                //Console.Write(ciphertext[i] - key[i % key.Length]);
                undoneVigenere.Append(Utils.Mod(ciphertext[i] - key[i % key.Length], 10).ToString());
            }
            //Console.Write("\n\n" + undoneVigenere.ToString() + "\n\n");
            return Straddle.Decrypt(undoneVigenere.ToString(), checkerboard);*/
            return Straddle.Decrypt(UndoPeriodicKey(ciphertext, key), checkerboard);
        }

        public static string UndoPeriodicKey(string ciphertext, string key)
        {
            StringBuilder undoneVigenere = new StringBuilder();
            for (int i = 0; i < ciphertext.Length; i++)
            {
                undoneVigenere.Append(Utils.Mod(ciphertext[i] - key[i % key.Length], 10).ToString());
            }
            return undoneVigenere.ToString();
        }

        //public static Tuple<string, char[][], string> CrackReturnKey(string ciphertext, int period, int rows = 3, int columns = 10, string alphabet = "abcdefghijklmnopqrstuvwxyz#/", bool displayCountdown = false)
        //public static Tuple<string, char[][], string> CrackReturnKey(string ciphertext, int period, int rows = 3, int columns = 10, string alphabet = "abcdefghijklmnopqrstuvwxyz#/", int numOfTrials = 10000, bool displayCountdown = false)
        public static Tuple<string, char[][], string> CrackReturnKey(string ciphertext, int period, int rows = 3, int columns = 10, string alphabet = "abcdefghijklmnopqrstuvwxyz#/", bool isAllNumbers = true, int numOfTrials = 10000, bool displayCountdown = false)
        {
            //alphabet = Enumerable.Repeat(".", rows - 1).ToString() + alphabet;
            //alphabet = Enumerable.Repeat('.', rows - 1).ToString() + alphabet;
            //alphabet = (string)Enumerable.Repeat('.', rows - 1) + alphabet;
            //alphabet = new string('.', rows - 1) + alphabet;
            alphabet = new string(' ', rows - 1) + alphabet;
            //Console.Write(alphabet + "\n");

            //bool isAllNumbers = System.Text.RegularExpressions.Regex.IsMatch(ciphertext, @"^\d+$");
            //Console.Write(isAllNumbers);

            char[][] currentBoard = new char[rows][];
            int index = 0;
            for (int i = 0; i < currentBoard.Length; i++)
            {
                currentBoard[i] = new char[columns];
                for (int j = 0; j < currentBoard[i].Length; j++)
                {
                    currentBoard[i][j] = alphabet[index];
                    index++;
                }
            }
            for (int i = 0; i < 100; i++)
            {
                CipherLib.Utils.Copy2DArray(CipherLib.Straddle.MessWithKey(currentBoard), currentBoard);
            }

            char[][] bestBoard = new char[rows][];
            for (int i = 0; i < bestBoard.Length; i++)
            {
                bestBoard[i] = new char[columns];
            }
            char[][] overallBestBoard = new char[rows][];
            for (int i = 0; i < overallBestBoard.Length; i++)
            {
                overallBestBoard[i] = new char[columns];
            }

            char[] currentKey = Enumerable.Repeat('0', period).ToArray();
            //string bestKey = "";
            string bestKey = new string(currentKey);
            string overallBestKey = bestKey;

            //bestKey = "0545";
            //overallBestKey = "0545";

            CipherLib.Utils.Copy2DArray(currentBoard, bestBoard);
            CipherLib.Utils.Copy2DArray(currentBoard, overallBestBoard);

            float currentScore;
            if (isAllNumbers)
            {
                currentScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, currentBoard, new string(currentKey)));
            }
            else
            {
                currentScore = CipherLib.Annealing.QuadgramScore(DecryptConvertedBack(ciphertext, currentBoard, new string(currentKey)));
            }
            float bestScore = currentScore;
            float overallBestScore = currentScore;

            string decodedMsg;
            int operation;

            //for (float T = 20; T >= 0; T -= 0.2f)
            for (int trial = 0; trial < numOfTrials; trial++)
            {
                //if (displayCountdown)
                if (false)
                {
                    //Console.Write("\b\b\b\b\bb\b\b\b" + T.ToString("n1"));
                    Console.Write("\n");
                    CipherLib.Straddle.DisplayKey(overallBestBoard);
                    Console.Write("\n");
                    Console.Write(overallBestKey);
                    Console.Write("\n");
                    Console.Write("\n");
                    Console.Write(Decrypt(ciphertext, overallBestBoard, overallBestKey));
                    Console.Write("\n");
                    Console.Write("\n");
                    Console.Write("----\n\n");
                }
                //for (int count = 0; count < 10000; count++)
                for (int count = 0; count < 1; count++)
                {
                    if (isAllNumbers)
                    {
                        operation = CipherLib.Utils.rand.Next() % 10;
                        if (operation <= 2)
                        {
                            CipherLib.Utils.CopyArray(MessWithPeriodicKey(currentKey), currentKey);
                        }
                        else
                        {
                            CipherLib.Utils.Copy2DArray(CipherLib.Straddle.MessWithKey(currentBoard), currentBoard);
                        }
                    }
                    else
                    {
                        operation = CipherLib.Utils.rand.Next() % 10;
                        if (operation <= 2)
                        {
                            CipherLib.Utils.CopyArray(MessWithPeriodicKey(currentKey), currentKey);
                        }
                        else if (operation <= 3)
                        {
                            for (int i = 0; i < period; i++)
                            {
                                CipherLib.Utils.CopyArray(MessWithPeriodicKey(currentKey), currentKey);
                            }
                            CipherLib.Utils.Copy2DArray(CipherLib.Straddle.MessWithKey(currentBoard), currentBoard);
                        }
                        else
                        {
                            CipherLib.Utils.Copy2DArray(CipherLib.Straddle.MessWithKey(currentBoard), currentBoard);
                        }
                    }
                    //currentKey = new char[] { '0', '5', '4', '5' };

                    if (isAllNumbers)
                    {
                        decodedMsg = Decrypt(ciphertext, currentBoard, new string(currentKey));
                    }
                    else
                    {
                        decodedMsg = DecryptConvertedBack(ciphertext, currentBoard, new string(currentKey));
                    }

                    currentScore = CipherLib.Annealing.QuadgramScore(decodedMsg);

                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;
                        CipherLib.Utils.Copy2DArray(currentBoard, bestBoard);
                        bestKey = new string(currentKey);

                        if (bestScore > overallBestScore)
                        {
                            overallBestScore = bestScore;
                            CipherLib.Utils.Copy2DArray(bestBoard, overallBestBoard);
                            overallBestKey = bestKey;
                        }
                    }
                    else if (currentScore < bestScore)
                    {
                        bool ReplaceAnyway;
                        //ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(currentScore - bestScore, T);
                        //ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(currentScore - bestScore, numOfTrials - trial);
                        ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(currentScore - bestScore, 20f);

                        if (ReplaceAnyway == true)
                        {
                            bestScore = currentScore;
                            CipherLib.Utils.Copy2DArray(currentBoard, bestBoard);
                            bestKey = new string(currentKey);
                        }
                        else
                        {
                            CipherLib.Utils.Copy2DArray(bestBoard, currentBoard);
                            CipherLib.Utils.CopyArray(bestKey.ToCharArray(), currentKey);
                        }
                    }
                    else
                    {
                        CipherLib.Utils.Copy2DArray(bestBoard, currentBoard);
                        CipherLib.Utils.CopyArray(bestKey.ToCharArray(), currentKey);
                    }
                }
            }

            if (isAllNumbers)
            {
                return new Tuple<string, char[][], string>(Decrypt(ciphertext, overallBestBoard, overallBestKey), overallBestBoard, overallBestKey);
            }
            else
            {
                return new Tuple<string, char[][], string>(DecryptConvertedBack(ciphertext, overallBestBoard, overallBestKey), overallBestBoard, overallBestKey);
            }
        }

        private static char[] MessWithPeriodicKey(char[] key)
        {
            int index = CipherLib.Utils.rand.Next() % key.Length;
            key[index] = (char)(CipherLib.Utils.rand.Next() % 10 + '0');
            return key;
        }

        /// Decrypt the ciphertext if the message was converted back to characters after the straddle checkerboard step
        public static string DecryptConvertedBack(string ciphertext, char[][] checkerboard, string key)
        {
            return Decrypt(CipherLib.Straddle.Encrypt(ciphertext, checkerboard), checkerboard, key);
        }

        /// Work out if the ciphertext was left and digits or not and decrypt accordingly
        /*public static string DecryptGeneral(string ciphertext, char[][] checkerboard, string key)
        {
            if ()
        }*/
    }
}
