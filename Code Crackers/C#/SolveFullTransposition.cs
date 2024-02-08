using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFullTransposition
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

            Console.Write("-- C# Full Transposition Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--FullTranspoMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            string alphabet;
            CipherLib.TranspositionType transpoType;
            int columnNum;
            if (args.Length > 0)
            {
                alphabet = args[0];
                transpoType = (CipherLib.TranspositionType)Int32.Parse(args[1]);
                columnNum = Int32.Parse(args[2]);
            }
            else
            {
                alphabet = "abcdefghijklmnopqrstuvwxyz";
                transpoType = CipherLib.TranspositionType.column;
                columnNum = 8;
            }

            Console.Write("Using Alphabet: " + alphabet);
            Console.Write("\n\nTransposition Type: ");
            if (transpoType == CipherLib.TranspositionType.column)
            {
                Console.Write("Column");
            }
            else
            {
                Console.Write("Row");
            }
            Console.Write("\n\nNumber Of Columns: ");
            Console.Write(columnNum);
            //Console.Write("\n\n-----------------------\n\n");
            Console.Write("\n\n");

            int numOfOuterTrials = 10;

            int numOfInnerTrials = 1000;
            //int numOfInnerTrials = 10000;

            int[] currentKey = new int[columnNum];
            int[] bestKey = new int[columnNum];
            int[] bestOuterKey = new int[columnNum];

            currentKey = CipherLib.Utils.IntRangeArray(0, columnNum - 1);
            for (int i = 0; i < 100; i++)
            {
                //currentKey = CipherLib.Annealing.MessWithIntKey(currentKey);
                currentKey = CipherLib.Annealing.BlockMessWithIntKey(currentKey);
            }
            Array.Copy(currentKey, bestKey, columnNum);
            Array.Copy(currentKey, bestOuterKey, columnNum);

            int[] bestOverallKey = new int[columnNum];

            float currentScore;
            float bestScore;
            float bestOuterScore = float.MinValue;
            float bestOverallScore = float.MinValue;

            string decodedMsg;
            string decipherment;

            for (int outerTrial = 0; outerTrial < numOfOuterTrials; outerTrial++)
            {
                Console.Write("-----------------------\n\n");
                Console.Write("Trial: " + (outerTrial + 1).ToString() + " / " + numOfOuterTrials.ToString() + "\n\n");

                /*currentKey = CipherLib.Utils.IntRangeArray(0, columnNum - 1);
                for (int i = 0; i < 100; i++)
                {
                    //currentKey = CipherLib.Annealing.MessWithIntKey(currentKey);
                    currentKey = CipherLib.Annealing.BlockMessWithIntKey(currentKey);
                }*/

                //bestKey = new int[columnNum];

                //Array.Copy(currentKey, bestKey, columnNum);
                //Array.Copy(bestKey, currentKey, columnNum);
                Array.Copy(bestOuterKey, currentKey, columnNum);
                Array.Copy(currentKey, bestKey, columnNum);

                currentScore = CipherLib.Annealing.QuadgramScore(CipherLib.Annealing.DecodeColumnTranspo(ciphertext, currentKey));
                bestScore = currentScore;

                decodedMsg = "";

                for (int innerTrial = 0; innerTrial < numOfInnerTrials; innerTrial++)
                {
                    Array.Copy(CipherLib.Annealing.BlockMessWithIntKey(currentKey), currentKey, columnNum);
                    //Array.Copy(CipherLib.Annealing.MessWithIntKey(currentKey), currentKey, columnNum);

                    decodedMsg = CipherLib.Annealing.DecodeTranspo(ciphertext, currentKey, transpoType);

                    currentScore = CipherLib.Annealing.QuadgramScore(decodedMsg);

                    if (currentScore > bestOverallScore)
                    {
                        bestOverallScore = currentScore;
                        Array.Copy(currentKey, bestOverallKey, columnNum);
                    }

                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;
                        Array.Copy(currentKey, bestKey, columnNum);
                    }
                    else if (currentScore < bestScore)
                    {
                        bool ReplaceAnyway;
                        ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(currentScore - bestScore, numOfInnerTrials - innerTrial);

                        if (ReplaceAnyway == true)
                        {
                            bestScore = currentScore;
                            Array.Copy(currentKey, bestKey, columnNum);
                        }
                        else
                        {
                            Array.Copy(bestKey, currentKey, columnNum);
                        }
                    }
                    else
                    {
                        Array.Copy(bestKey, currentKey, columnNum);
                    }
                }

                if (outerTrial == 0 || bestScore > bestOuterScore)
                {
                    Array.Copy(bestKey, bestOuterKey, columnNum);
                    bestOuterScore = bestScore;

                    decipherment = CipherLib.Annealing.DecodeTranspo(ciphertext, bestOuterKey, transpoType);

                    Console.Write("New best key:\n\n");
                    Console.Write("Score: " + bestScore + "\n\n");
                    Console.Write("Key:\n\n");
                    CipherLib.Utils.DisplayArray(bestOuterKey);
                    Console.Write("\n\n");
                    //Console.Write("Decipherment: " + decipherment);
                    Console.Write("Decipherment:\n\n");
                    Console.Write(decipherment);
                    Console.Write("\n\n");
                }
                else
                {
                    Console.Write("Didn't find a better key...");
                    Console.Write("\n\n");
                }
            }

            Console.Write("-----------------------\n\n");
            decipherment = CipherLib.Annealing.DecodeTranspo(ciphertext, bestOverallKey, transpoType);

            Console.Write("Overall best  key:\n\n");
            Console.Write("Score: " + bestOverallScore + "\n\n");
            Console.Write("Key:\n\n");
            CipherLib.Utils.DisplayArray(bestOverallKey);
            Console.Write("\n\n");
            Console.Write("Decipherment:\n\n");
            Console.Write(decipherment);
            Console.Write("\n\n");

            //Console.Write("\n\n-----------------------\n\n");
            Console.Write("-----------------------\n\n");
            Console.Write("Program finished.");
            Console.Write("\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }
    }
}
