using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpDoubleTranspo
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

            Console.Write("-- C# Double Transposition Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string msg = System.IO.File.ReadAllText("--DoubleTranspoMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(msg);
            Console.Write("\n\n-----------------------\n\n");

            /*Console.Write("\n\n");
            Console.Write(DecodeDoubleTranspo(msg, new int[4] { 0, 2, 1, 3 }, 8, true, false));
            Console.Write("\n\n");*/
            /*Console.Write("\n\n");
            Console.Write(Score(msg, new int[4] { 0, 2, 1, 3 }, 8, true, false));
            Console.Write("\n\n");*/

            /*Console.Write("\n\n");
            //Console.Write(DecodeDoubleTranspo(msg, new int[7] { 3, 4, 2, 5, 6, 0, 1 }, 5, false, false));
            //Console.Write(DecodeDoubleTranspo(msg, new int[7] { 3, 4, 2, 5, 6, 1, 0 }, 5, false, false));
            //Console.Write(DecodeDoubleTranspo(msg, new int[5] { 2, 0, 1, 3, 4 }, 7, false, false));
            //Console.Write(DecodeDoubleTranspo(msg, new int[7] { 4, 3, 2, 5, 6, 1, 0 }, 5, false, false));
            Console.Write(DecodeDoubleTranspo(msg, new int[7] { 6, 5, 2, 1, 0, 3, 4 }, 5, false, false));
            Console.Write("\n\n");*/

            //Console.ReadLine();

            int trial = 0;

            int[] currentKey1;
            int key2Length;
            bool row1;
            bool row2;

            if (args.Length > 0)
            {
                currentKey1 = new int[Int32.Parse(args[0])];
                key2Length = Int32.Parse(args[1]);

                row1 = Boolean.Parse(args[2]);
                row2 = Boolean.Parse(args[3]);
            }
            else
            {
                //currentKey1 = new int[4];
                currentKey1 = new int[7];
                //key2Length = 8;
                key2Length = 5;

                row1 = false;
                //row1 = true;
                row2 = false;
            }

            for (int i = 0; i < currentKey1.Length; i++)
            {
                currentKey1[i] = i;
            }

            for (int i = 0; i < 100; i++)
            {
                currentKey1 = CipherLib.Annealing.MessWithIntKey(currentKey1);
            }

            int[] bestKey1 = new int[currentKey1.Length];
            Array.Copy(currentKey1, bestKey1, currentKey1.Length);

            float currentScore = Score(msg, currentKey1, key2Length, row1, row2);
            float bestScore = currentScore;

            Console.Write("Starting with key: ");
            DisplayKey(currentKey1);
            Console.Write("\n\n");
            Console.Write("Starting with score: " + bestScore.ToString() + "\n\n");
            Console.Write("-----------------------\n\n");

            Tuple<int[], float> result;
            string decipherment;
            Tuple<string, int[]> result2 = new Tuple<string, int[]>("", new int[1]);

            for (trial = 0; trial >= 0; trial++)
            {
                Console.Write("Trial: " + (trial + 1) + "\n\n");

                Array.Copy(bestKey1, currentKey1, bestKey1.Length);

                //result = AnnealingDoubleTranspo(msg, currentKey1, key2Length, 20, 1f, 1000);
                //result = AnnealingDoubleTranspo(msg, currentKey1, key2Length, 10, 1f, 1000);
                //result = AnnealingDoubleTranspo(msg, currentKey1, key2Length, 0, 1f, 1000);
                //result = AnnealingDoubleTranspo(msg, currentKey1, key2Length, row1, row2, 0, 1f, 100);
                //result = AnnealingDoubleTranspo(msg, bestKey1, currentKey1, key2Length, row1, row2, 0, 1f, 10);
                //result = AnnealingDoubleTranspo(msg, bestKey1, currentKey1, key2Length, row1, row2, 0, 1f, 100);
                result = AnnealingDoubleTranspo(msg, bestKey1, currentKey1, key2Length, row1, row2, 10, 1f, 10);

                Array.Copy(result.Item1, currentKey1, currentKey1.Length);
                currentScore = result.Item2;

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;

                    Array.Copy(currentKey1, bestKey1, currentKey1.Length);

                    result2 = DecodeDoubleTranspo(msg, bestKey1, key2Length, row1, row2);
                    decipherment = result2.Item1;

                    Console.Write("\b\b\b\b\b\b\b\b\b");
                    Console.Write("New best key:\n\n");
                    Console.Write("Score: " + bestScore + "\n");
                    Console.Write("\n");
                    Console.Write("Key:\n");
                    DisplayKey(result2.Item2);
                    Console.Write("\n");
                    DisplayKey(currentKey1);
                    Console.Write("\n\n");
                    //decipherment = DecodeDoubleTranspo(msg, bestKey1, key2Length, row1, row2);
                    Console.Write("Decipherment: " + decipherment);
                    Console.Write("\n\n");
                }

                else
                {
                    Console.Write("\b\b\b\b\b\b\b\b\b\b\b");
                    Console.Write("Didn't find a better key...");
                    Console.Write("\n\n");
                }
                Console.Write("--------------------------------------\n\n");
            }


            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }

        static float Score(string msg, int[] key1, int key2Length, bool row1, bool row2)
        {
            string decodedMsg = "";
            float score = 0;

            if (true)
            {
                //decodedMsg = DecodeDoubleTranspo(msg, key1, key2Length, row1, row2);
                decodedMsg = DecodeDoubleTranspo(msg, key1, key2Length, row1, row2).Item1;

                score = CipherLib.Annealing.QuadgramScore(decodedMsg);
            }

            return score;
        }

        //static string DecodeDoubleTranspo(string msg, int[] key1, int key2Length, bool row1, bool row2, int trialNum = 1000)
        static Tuple<string, int[]> DecodeDoubleTranspo(string msg, int[] key1, int key2Length, bool row1, bool row2, int trialNum = 1000)
        //static string DecodeDoubleTranspo(string msg, int[] key1, int key2Length, bool row1, bool row2, int trialNum = 100)
        {
            //return CipherLib.Annealing.CrackColumnTranspo(CipherLib.Annealing.DecodeColumnTranspo(msg, key1), key2Length, 1000);
            string intermediateMsg;

            if (row1 == true)
            {
                intermediateMsg = CipherLib.Annealing.DecodeRowTranspo(msg, key1);
            }
            else
            {
                //intermediateMsg = CipherLib.Annealing.DecodeColumnTranspo(msg, key1);
                intermediateMsg = CipherLib.Annealing.IncompleteColumnarTranspo(msg, key1);
            }

            //Console.Write("\n\n" + intermediateMsg + "\n\n");

            if (row2 == true)
            {
                //return CipherLib.Annealing.CrackRowTranspo(intermediateMsg, key2Length, trialNum);
                return CipherLib.Annealing.CrackRowTranspoReturnKey(intermediateMsg, key2Length, trialNum);
            }
            else
            {
                //return CipherLib.Annealing.CrackColumnTranspo(intermediateMsg, key2Length, trialNum);
                return CipherLib.Annealing.CrackColumnTranspoReturnKey(intermediateMsg, key2Length, trialNum);
                //return intermediateMsg;
            }
        }

        static Tuple<int[], float> AnnealingDoubleTranspo(string msg, int[] bestkeyOverall, int[] key1, int key2Length, bool row1, bool row2, float temperature, float step, int count)
        {
            int[] currentKey1 = new int[key1.Length];
            Array.Copy(key1, currentKey1, key1.Length);

            int[] bestKey1 = new int[key1.Length];
            Array.Copy(key1, bestKey1, key1.Length);

            float currentScore = Score(msg, currentKey1, key2Length, row1, row2);
            float bestScore = currentScore;

            for (float t = temperature; t >= 0; t -= step)
            {
                string zeroes = "";

                for (int i = 0; i < 2 - t.ToString().Length; i++)
                {
                    zeroes += "0";
                }

                Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b" + zeroes + t.ToString("n1"));

                //Console.Write("\n");

                for (int trial = 0; trial < count; trial++)
                {
                    /*if (true)
                    {
                        //Console.Write((count - trial) + "\n");
                    }*/

                    while (Console.KeyAvailable)
                    {
                        //Console.Write("Hello");
                        ProducePlaintext(msg, bestkeyOverall, key2Length, row1, row2);
                    }


                    //for (int i = 0; i < CipherLib.Annealing.rand.Next() % 6 + 1; i++)
                    //for (int i = 0; i < 10; i++)
                    //for (int i = 0; i < 1; i++)
                    //for (int i = 0; i < CipherLib.Annealing.rand.Next() % (key1.Length * 5) + 1; i++)
                    for (int i = 0; i < CipherLib.Annealing.rand.Next() % (key1.Length * 6) + 1; i++)
                    {
                        Array.Copy(CipherLib.Annealing.MessWithIntKey(currentKey1), currentKey1, currentKey1.Length);
                    }

                    /*if (currentKey1 == new int[7] { 6, 5, 2, 1, 0, 3, 4 })
                    {
                        Console.Write("\nI got it!\n");
                    }*/

                    currentScore = Score(msg, currentKey1, key2Length, row1, row2);

                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;
                        Array.Copy(currentKey1, bestKey1, currentKey1.Length);
                    }
                    else if (currentScore < bestScore)
                    {

                        bool ReplaceAnyway;
                        ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(currentScore - bestScore, t);
                        //ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(currentScore - bestScore, count - trial);

                        if (ReplaceAnyway == true)
                        {
                            bestScore = currentScore;
                            Array.Copy(currentKey1, bestKey1, currentKey1.Length);
                        }
                        else
                        {
                            Array.Copy(bestKey1, currentKey1, bestKey1.Length);
                        }
                    }
                    else
                    {
                        Array.Copy(bestKey1, currentKey1, bestKey1.Length);
                    }
                }
            }

            return new Tuple<int[], float>(bestKey1, bestScore);
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

        static void ProducePlaintext(string msg, int[] key1, int key2Length, bool row1, bool row2)
        {
            Console.Write("\n---------------\n");
            Console.Write("Refined output from best key so far:\n\n");
            //string decodedMsg = DecodeDoubleTranspo(msg, key1, key2Length, row1, row2, 10000);
            Tuple<string, int[]> result = DecodeDoubleTranspo(msg, key1, key2Length, row1, row2, 10000);
            string decodedMsg = result.Item1;
            Console.Write(decodedMsg);
            Console.Write("\n");
            /*Console.Write("\nEnter Row Rearrange Length: ");
            int rowArrangeLength = Int32.Parse(Console.ReadLine());
            Console.Write(CipherLib.Annealing.CrackRowTranspo(decodedMsg, rowArrangeLength, 10000));*/
            Console.Write("\n");
            Console.ReadLine();
            //Console.Write("---------------\n\n");
            Console.Write("---------------\n");
        }
    }
}
