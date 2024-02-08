using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTransposition
{
    class Program
    {
        private const int trialNum = 1000;
        //private const int trialNum = 10000;

        private const int refinedTrialNum = 10000;

        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            Console.Write("-- C# Transposition Bruter --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string msg = System.IO.File.ReadAllText("--TranspositionMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(msg);
            Console.Write("\n\n-----------------------\n\n");            

            float currentScore = CipherLib.Annealing.QuadgramScore(CipherLib.Annealing.CrackRowTranspo(msg, 2, trialNum));
            float bestScore = currentScore;

            int bestKeyLength = 2;

            string decipherment;
            int keyLength;

            //Tuple<string, int[]> result = new Tuple<string, int[]>("", new int[keyLength]);
            Tuple<string, int[]> result = new Tuple<string, int[]>("", new int[1]);

            int trial = 0;
            while (true)
            {

                Console.Write("-----------------------\n\n");
                Console.Write("Trial: " + (trial + 1).ToString() + "\n\n");

                Console.Write("Key type: ");

                //keyLength = (trial + 1) % 25 + 1;
                keyLength = trial % 25 + 2;

                if ((trial / 25) % 2 == 0)
                {
                    Console.Write("Row");
                    //decipherment = CipherLib.Annealing.CrackRowTranspo(msg, keyLength, trialNum);
                    result = CipherLib.Annealing.CrackRowTranspoReturnKey(msg, keyLength, trialNum);
                }
                else
                {
                    Console.Write("Column");
                    //decipherment = CipherLib.Annealing.CrackColumnTranspo(msg, keyLength, trialNum);
                    result = CipherLib.Annealing.CrackColumnTranspoReturnKey(msg, keyLength, trialNum);
                }
                decipherment = result.Item1;

                Console.Write("\n\nKey length: " + keyLength.ToString() + "\n\n");

                currentScore = CipherLib.Annealing.QuadgramScore(decipherment);

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;

                    bestKeyLength = keyLength;

                    Console.Write("New best key:\n\n");
                    DisplayKey(result.Item2);
                    Console.Write("\n\n");
                    Console.Write("Score: " + bestScore + "\n\n");
                    Console.Write("Decipherment: " + decipherment);
                    Console.Write("\n\n");
                }
                else
                {
                    Console.Write("Didn't find a better key...");
                    Console.Write("\n\n");
                }

                trial += 1;

                //Console.Write("-----------------------\n\n");

                while (Console.KeyAvailable)
                {
                    Pause(msg, bestKeyLength, (trial / 25) % 2 == 0);
                }
            }


            Console.Write("\n\n-----------------------\n\nPress ENTER to close...");
            Console.ReadLine();
        }

        /*static float Score(string msg, int[] key1, int key2Length, bool row1, bool row2)
        {
            string decodedMsg = "";
            float score = 0;

            if (true)
            {
                decodedMsg = DecodeDoubleTranspo(msg, key1, key2Length, row1, row2);

                score = CipherLib.Annealing.QuadgramScore(decodedMsg);
            }

            return score;
        }

        static string DecodeDoubleTranspo(string msg, int[] key1, int key2Length, bool row1, bool row2, int trialNum = 1000)
        {
            string intermediateMsg;

            if (row1 == true)
            {
                intermediateMsg = CipherLib.Annealing.DecodeRowTranspo(msg, key1);
            }
            else
            {
                intermediateMsg = CipherLib.Annealing.DecodeColumnTranspo(msg, key1);
            }

            if (row2 == true)
            {
                return CipherLib.Annealing.CrackRowTranspo(intermediateMsg, key2Length, trialNum);
            }
            else
            {
                return CipherLib.Annealing.CrackColumnTranspo(intermediateMsg, key2Length, trialNum);
            }
        }*/

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

        static void Pause(string msg, int bestKeyLength, bool isRow)
        {            
            string input = Console.ReadLine();

            string decipherment;

            if (input == "r")
            {
                Console.Write("-----------------------\n\n");
                Console.Write("Refined output of best key so far:\n\n");
                Console.Write("Best key type: ");

                if (isRow)
                {
                    Console.Write("Row");
                    decipherment = CipherLib.Annealing.CrackRowTranspo(msg, bestKeyLength, refinedTrialNum);
                }
                else
                {
                    Console.Write("Column");
                    decipherment = CipherLib.Annealing.CrackColumnTranspo(msg, bestKeyLength, refinedTrialNum);
                }
                Console.Write("\n\n");

                Console.Write("Best key length: " + bestKeyLength.ToString() + "\n\n");
                Console.Write("Decipherment:\n\n");
                Console.Write(decipherment);
                Console.Write("\n\n");
                Console.ReadLine();
            }
        }
    }
}
