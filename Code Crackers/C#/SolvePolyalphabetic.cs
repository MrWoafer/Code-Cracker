using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPolyalphabetic
{
    class Program
    {
        //const int numOfTrials = 1000;
        //const int numOfTrials = 10000;
        //const int numOfTrials = 1;
        //const int numOfTrials = 10;

        //const int numOfTrials2 = 10000;
        const int numOfAnnealingTrials = 10000;
        const float temperature = 20f;
        //const float step = 1f;
        //const float step = 20f;
        //const float step = 2f;

        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            //Console.Write("-- C# Polyalphabetic Solver --");
            Console.Write("-- C# Polyalphabetic Substitution Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--PolyalphabeticMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            int period;
            string alphabet;
            float step;
            int alphabetChangePeriod;
            if (args.Length > 0)
            {
                alphabet = args[0];
                period = Int32.Parse(args[1]);
                step = float.Parse(args[2]);
                alphabetChangePeriod = int.Parse(args[3]);
            }
            else
            {
                alphabet = "abcdefghijklmnopqrstuvwxyz";
                //period = 7;
                period = 8;
                //step = 20f;
                //step = 0.2f;
                step = 2f;
                alphabetChangePeriod = 5;
            }

            Console.Write("Alphabet: " + alphabet + "\n\n");
            Console.Write("Period: " + period.ToString() + "\n\n");
            Console.Write("Step: " + step.ToString() + "\n\n");
            Console.Write("Alphabet Change Period: " + alphabetChangePeriod.ToString() + "\n\n");
            Console.Write("-----------------------\n\n");

            string[] key = new string[period];
            
            string partOfCiphertext;
            string partOfSolution;

            for (int i = 0; i < period; i++)
            {
                //Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b");
                //Console.Write("Solving key column " + (i + 1).ToString() + " / " + period.ToString());
                //Console.Write("Solving key column " + (i + 1).ToString() + " / " + period.ToString() + "...");
                //Console.Write("Initial guess for key column " + (i + 1).ToString() + " / " + period.ToString() + "...");

                partOfCiphertext = "";
                for (int j = i; j < ciphertext.Length; j += period)
                {
                    partOfCiphertext += ciphertext[j];
                }
                //Console.Write(partOfCiphertext);

                //partOfSolution = CipherLib.Annealing.CrackCustomMonoSub(ciphertext, alphabet, numOfTrials, CipherLib.SolutionType.ChiSquared);
                //partOfSolution = CipherLib.Annealing.CrackCustomMonoSub(partOfCiphertext, alphabet, numOfTrials, CipherLib.SolutionType.ChiSquared);
                partOfSolution = CipherLib.Annealing.CrackCustomMonoSub(partOfCiphertext, alphabet, 0, CipherLib.SolutionType.ChiSquared);

                key[i] = CipherLib.Annealing.GetKeyFromPlaintext(partOfSolution, partOfCiphertext, alphabet);
                //key[i] = CipherLib.Annealing.GetKeyFromPlaintext(partOfCiphertext, partOfSolution, alphabet);
            }

            /// Then mess around with the key;
            float score;
            float bestScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, key, alphabet, alphabetChangePeriod));
            string[] bestKey = new string[period];

            int outerIndex;
            int innerIndex1;
            int innerIndex2;

            CipherLib.Utils.CopyArray(key, bestKey);

            //Console.Write("\n\n");
            //for (float T = 20f; T >= 0f; T -= 0.2f)
            //for (float T = 20f; T >= 0f; T -= 1f)
            for (float T = temperature; T >= 0f; T -= step)
            {
                //CipherLib.Utils.ClearLine();
                //Console.Write("Countdown: " + T.ToString("n1"));

                for (int trial = 0; trial < numOfAnnealingTrials; trial++)
                {
                    //Console.Write(trial + " " + 10 * trial / numOfAnnealingTrials + "\n");
                    /// Display countdown
                    //if (trial % numOfAnnealingTrials / 10 == 0)
                    if (trial % (numOfAnnealingTrials / 10) == 0)
                    {
                        CipherLib.Utils.ClearLine();
                        //Console.Write("Countdown: " + (10 * (1 + temperature/step) - (10 - 10 * trial / numOfAnnealingTrials) - 10 * (temperature - T) / step).ToString("n1"));
                        Console.Write("Countdown: " + (10 * (1 + temperature / step) - (10 * trial / numOfAnnealingTrials + 10 * (temperature - T) / step)).ToString("n1"));
                        //Console.Write("Countdown: " + (10 * (1 + temperature / step) - (100 * trial / numOfAnnealingTrials + 10 * (temperature - T) / step)).ToString("n1"));
                        Console.Write("      ");
                    }

                    outerIndex = CipherLib.Utils.rand.Next() % period;

                    innerIndex1 = CipherLib.Utils.rand.Next() % key[outerIndex].Length;
                    innerIndex2 = CipherLib.Utils.rand.Next() % key[outerIndex].Length;

                    key[outerIndex] = CipherLib.Annealing.Swap(key[outerIndex], innerIndex1, innerIndex2);

                    score = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, key, alphabet, alphabetChangePeriod));

                    if (score > bestScore)
                    {
                        bestScore = score;
                        CipherLib.Utils.CopyArray(key, bestKey);
                    }
                    else
                    {
                        bool replaceAnyway;
                        //replaceAnyway = CipherLib.Annealing.AnnealingProbability(score - bestScore, numOfTrials2 - trial);
                        replaceAnyway = CipherLib.Annealing.AnnealingProbability(score - bestScore, T);

                        if (replaceAnyway == true)
                        {
                            bestScore = score;
                            CipherLib.Utils.CopyArray(key, bestKey);
                        }
                        else
                        {
                            CipherLib.Utils.CopyArray(bestKey, key);
                        }
                    }
                }
            }

            /// Display the key;
            //Console.Write("\n\n");
            //Console.Write("Best Key\n\n");
            CipherLib.Utils.ClearLine();
            Console.Write("Best Key:");
            /// (Remove any existing characters on the line. There may be some left over if what you print over it is shorter than what was there before.)
            Console.Write("                                      ");
            Console.Write("\n\n");
            Console.Write("  |");
            for (int i = 0; i < alphabet.Length; i++)
            {
                Console.Write(" " + alphabet[i]);
            }
            Console.Write("\n");
            Console.Write("--+");
            for (int i = 0; i < alphabet.Length; i++)
            {
                Console.Write("--");
            }
            Console.Write("\n");
            for (int i = 0; i < period; i++)
            {
                //for (int k = 0; k < 2 - i.ToString().Length; k++)
                for (int k = 0; k < 2 - (i + 1).ToString().Length; k++)
                {
                    Console.Write(" ");
                }
                Console.Write(i + 1);
                Console.Write("|");
                for (int j = 0; j < key[i].Length; j++)
                {
                    Console.Write(" " + key[i][j]);
                }
                Console.Write("\n");
            }

            /// Display the plaintext;
            //Console.Write("\n\n");
            Console.Write("\n");
            Console.Write(Decrypt(ciphertext, key, alphabet, alphabetChangePeriod));

            Console.Write("\n\n-----------------------\n\n");
            Console.Write("Program finished.");
            Console.Write("\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }

        public static string Decrypt(string ciphertext, string[] key, string alphabet, int alphabetChangePeriod)
        {
            StringBuilder plaintext = new StringBuilder();

            for (int i = 0; i < ciphertext.Length; i++)
            {
                //plaintext.Append(key[i % key.Length][alphabet.IndexOf(ciphertext[i])]);
                plaintext.Append(key[(i / alphabetChangePeriod) % key.Length][alphabet.IndexOf(ciphertext[i])]);
            }

            return plaintext.ToString();
        }
    }
}
