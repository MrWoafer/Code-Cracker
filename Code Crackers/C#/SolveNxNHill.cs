using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNxNHill
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            //Console.Write("-- C# NxN Hill Solver --");
            Console.Write("-- C# NxN Hill Brute-Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string msg = System.IO.File.ReadAllText("--NxNHillMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(msg);
            Console.Write("\n\n-----------------------\n\n");

            /*Console.Write(DecodeNxNHill(msg, new int[2][] { new int[2] { 9, 22 }, new int[2] { 21, 15 } }));
            Console.Write("\n\n");*/

            int n;

            if (args.Length > 0)
            {
                n = Int32.Parse(args[0]);
            }
            else
            {
                //n = 2;
                //n = 4;
                n = 5;
            }

            int[][] overallKey = new int[n][];

            int[] currentRow;
            int[][] bestRows = new int[n][];

            float currentScore;
            float[] bestScores = new float[n];

            //Console.Write(bestRows.Length + "\n");

            //Console.Write("Searching " + n + "x" + Math.Pow(26, n).ToString() + " = " + (n * Math.Pow(26, n)).ToString() + " keys...\n\n");
            Console.Write("Searching " + Math.Pow(26, n).ToString() + " keys...\n\n");

            string decipherment;

            //for (int rowNum = 0; rowNum < n; rowNum++)
            for (int rowNum = 0; rowNum < 1; rowNum++)
            {
                int[][] rows = CipherLib.Annealing.NToTheMArrangements(26, n);

                //Console.Write("Now on row: " + (rowNum + 1).ToString()+ "\n----------------\n\n");
                currentRow = new int[n];
                //bestRow = new int[n];
                for (int i = 0; i < n; i++)
                {
                    currentRow[i] = 0;
                }
                //Array.Copy(currentRow, bestRow, n);
                //bestRows = new int[3][];
                bestRows = new int[n][];
                for (int i = 0; i < n; i++)
                {
                    bestRows[i] = new int[n];
                    //bestRows[i] = rows[i];
                    Array.Copy(rows[i], bestRows[i], n);
                    bestScores[i] = Score(msg, bestRows[i]);
                }
                Array.Sort(bestScores, bestRows);
                Array.Sort(bestScores);

                currentScore = Score(msg, currentRow);
                //bestScores = currentScore;
                
                for (int i = 0; i < rows.Length; i++)
                {
                    //currentRow = rows[i];
                    Array.Copy(rows[i], currentRow, n);

                    //DisplayKey(currentRow);
                    //Console.Write("\n");

                    //Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bKeys Searched: " + (i + 1) + " / " + rows.Length.ToString());
                    //if (i % Math.Pow(26, n-1) == 0)
                    if ((i+1) % Math.Pow(26, n - 1) == 0 || i == 0)
                    {
                        Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bKeys Searched: " + (i + 1) + " / " + rows.Length.ToString());
                    }

                    currentScore = Score(msg, currentRow);

                    if (currentScore < bestScores[n-1])
                    {
                        //Console.Write("<\n");

                        //for (int j = n-1; j >= 0; j--)
                        for (int j = n - 1; j >= -1; j--)
                        {
                            //if (currentScore > bestScores[j] || j == -1)
                            if (j == -1 || currentScore > bestScores[j])
                            {
                                /*Console.Write("\n>," + j + "\n");
                                DisplayKey(currentRow);
                                Console.Write("\n");*/
                                for (int k = n - 1; k > j + 1; k--)
                                {
                                    bestScores[k] = bestScores[k - 1];
                                    Array.Copy(bestRows[k - 1], bestRows[k], n);
                                }
                                bestScores[j + 1] = currentScore;
                                //Array.Copy(bestRows[j + 1], currentRow, n);
                                Array.Copy(currentRow, bestRows[j + 1], n);

                                break;
                            }
                        }
                        //bestScores = currentScore;

                        //Array.Copy(currentRow, bestRow, n);

                        /*Console.Write("\n\n\b\b\b\b\b\b\b\b\b");
                        Console.Write("New best row " + (rowNum + 1).ToString() + ":\n\n");
                        Console.Write("Score: " + bestScore + "\n");
                        Console.Write("\n");
                        Console.Write("Row:\n");
                        DisplayKey(bestRow);
                        Console.Write("\n\n");*/
                    }

                    /*if (bestRows.Length > n)
                    {
                        Console.Write(i + "\n");
                        DisplayKey(currentRow);
                        Console.Write("\n");
                    }*/
                }

                //Console.Write("Best row " + (rowNum + 1).ToString() + ":\n\n");
                //Console.Write("\n\nBest row " + (rowNum + 1).ToString() + ":\n\n");
                //DisplayKey(bestRow);

                //overallKey[rowNum] = bestRow;

                //Console.Write("\n\nBest rows " + (rowNum + 1).ToString() + ":\n\n");
                //Console.Write("\n\nBest " + (rowNum + 1).ToString() + " rows:\n\n");
                //Console.Write("\n\nBest " + (n + 1).ToString() + " rows:\n\n");
                Console.Write("\n\nBest " + n.ToString() + " rows:\n\n");
                for (int i = 0; i < n; i++)
                {
                    DisplayKey(bestRows[i]);
                    Console.Write("\n");
                }

                //Console.Write("\n\n-----------------------\n\n");
                Console.Write("\n-----------------------\n\n");
            }

            int[] bestKey = new int[n];
            int[] currentKey = new int[n];

            //int[][] perms = CipherLib.Annealing.Permutations(n);
            int[][] perms = CipherLib.Annealing.NToTheMArrangements(n, n);

            Array.Copy(perms[0], bestKey, n);

            //Console.Write(bestRows.Length + "\n");

            float bestScore = ScoreQuad(msg, ConstructKeyFromOrder(bestRows, bestKey));
            //float currentScore;

            for (int i = 0; i < perms.Length; i++)
            {
                Array.Copy(perms[i], currentKey, n);

                currentScore = ScoreQuad(msg, ConstructKeyFromOrder(bestRows, currentKey));

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    Array.Copy(currentKey, bestKey, n);
                }
            }

            /*Console.Write("Best solution:\n\n");

            decipherment = DecodeNxNHill(msg, overallKey);

            Console.Write(decipherment);*/

            //overallKey = ConstructKeyFromOrder(bestRows, currentKey);
            overallKey = ConstructKeyFromOrder(bestRows, bestKey);

            Console.Write("Best solution:\n\n");

            for (int i = 0; i < n; i++)
            {
                DisplayKey(overallKey[i]);
                Console.Write("\n");
            }
            Console.Write("\n");

            decipherment = DecodeNxNHill(msg, overallKey);

            Console.Write(decipherment);

            Console.Write("\n\n-----------------------\n\nPress ENTER to close...");
            Console.ReadLine();
        }

        static float Score(string msg, int[] row)
        {
            string decodedMsg = "";
            float score = 0;

            if (true)
            {
                decodedMsg = DecodeNxNHill(msg, new int[1][] { row });

                score = CipherLib.Annealing.ChiSquared(decodedMsg);
            }

            return score;
        }

        static float ScoreQuad(string msg, int[][] key)
        {
            string decodedMsg = "";
            float score = 0;

            if (true)
            {
                decodedMsg = DecodeNxNHill(msg, key, true);

                score = CipherLib.Annealing.QuadgramScore(decodedMsg);
            }

            return score;
        }

        static string DecodeNxNHill(string msg, int[][] key, bool debug = false)
        {
            /*if (debug)
            {
                Console.Write("\n");
                //Console.Write(key[0].Length + "\n");
                //DisplayKey(key[0]);
                Console.Write(key.Length);
                Console.Write("\n");
            }
            */

            //string decodedMsg = "";
            StringBuilder decodedMsg = new StringBuilder();

            int sum;

            for (int i = 0; i < msg.Length; i += key[0].Length)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    sum = 0;
                    for (int k = 0; k < key[0].Length; k++)
                    {
                        /*if (debug)
                        {
                            Console.Write(j + "\n");
                            DisplayKey(key[j]); Console.Write(" " + j + "\n");
                        }*/

                        sum += key[j][k] * (int)(msg[i + k] - 97);
                    }

                    //decodedMsg += CipherLib.Annealing.ALPHABET[sum % 26];
                    decodedMsg.Append(CipherLib.Annealing.ALPHABET[sum % 26]);
                }
            }

            //return decodedMsg;
            return decodedMsg.ToString();
        }

        static void DisplayKey(int[] key)
        {
            for (int i = 0; i < key.Length; i++)
            {
                Console.Write(key[i]);

                if (i < key.Length - 1)
                {
                    Console.Write(", ");
                }
            }
        }

        static int[][] ConstructKeyFromOrder(int[][] rows, int[] order)
        {
            int[][] key = new int[rows.Length][];

            //Console.Write(rows.Length);

            //DisplayKey(order); Console.Write("\n");

            for (int i = 0; i < order.Length; i++)
            {
                key[i] = new int[order.Length];
                //key[i] = rows[order[i]];
                //Array.Copy(rows[order[i]], key[i], rows.Length);
                Array.Copy(rows[order[i]], key[i], order.Length);

                //DisplayKey(key[i]); Console.Write("\n");
            }

            return key;
        }
    }
}