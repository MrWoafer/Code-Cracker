using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    class Gromark
    {
        public static string Decrypt(string ciphertext, string key, string alphabet, string primer)
        {
            StringBuilder plaintext = new StringBuilder();

            primer = ExtendPrimer(primer, ciphertext.Length);

            /*
            Console.Write(primer);
            Console.Write("\n\n");
            Console.Write(alphabet);
            Console.Write("\n");
            Console.Write(key);
            Console.Write("\n\n");
            */

            for (int i = 0; i < ciphertext.Length; i++)
            {
                plaintext.Append(alphabet[CipherLib.Utils.Mod(key.IndexOf(ciphertext[i]) - (primer[i] - 48), alphabet.Length)]);
            }

            return plaintext.ToString();
        }

        public static string ExtendPrimer(string primer, int length)
        {
            StringBuilder newPrimer = new StringBuilder(primer);

            int index = 0;
            while (newPrimer.Length < length)
            {
                //newPrimer.Append((newPrimer[index] + newPrimer[index + 1] - 96) % 10 + 48);
                //Console.Write(newPrimer[index] + " " + (newPrimer[index] - 48) + " " + newPrimer[index + 1] + " " + (newPrimer[index + 1] - 48) + " | " + (((newPrimer[index] - 48) + (newPrimer[index + 1] - 48)) % 10)
                //    + " " + ((((newPrimer[index] - 48) + (newPrimer[index + 1] - 48)) % 10) + 48) + "\n\n");
                //newPrimer.Append(((newPrimer[index] - 48) + (newPrimer[index + 1] - 48)) % 10 + 48);
                newPrimer.Append((char)(((newPrimer[index] - 48) + (newPrimer[index + 1] - 48)) % 10 + 48));
                index++;
            }

            return newPrimer.ToString();
        }

        public static Tuple<string, string> CrackKeyReturnKey(string ciphertext, string alphabet, string primer, int numOfTrials = 1000)
        {
            primer = ExtendPrimer(primer, ciphertext.Length);

            string newKey = alphabet;
            for (int i = 0; i < 100; i++)
            {
                newKey = CipherLib.Annealing.MessWithStringKey(newKey);
            }
            string bestKey = newKey;
            
            float newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey, alphabet, primer));
            float bestScore = newScore;

            for (int i = 0; i < numOfTrials; i++)
            {
                newKey = CipherLib.Annealing.MessWithStringKey(bestKey);

                newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey, alphabet, primer));

                if (newScore > bestScore)
                {
                    bestScore = newScore;
                    bestKey = newKey;
                }
            }

            return new Tuple<string, string>(Decrypt(ciphertext, bestKey, alphabet, primer), bestKey);
        }

        //public static Tuple<string, string, string> CrackReturnKey(string ciphertext, string alphabet, int primerLength, int numOfTrials = 1000)
        public static Tuple<string, string, string> CrackReturnKey(string ciphertext, string alphabet, int primerLength, bool displayCountdown = false)
        {
            //string newPrimer = new string('0', primerLength);
            string newPrimer = "";
            for (int i = 0; i < primerLength; i++)
            {
                newPrimer += (char)(48 + CipherLib.Utils.rand.Next() % 10);
            }
            string bestPrimer = newPrimer;
            string tempPrimer = ExtendPrimer(newPrimer, ciphertext.Length);

            string newKey = alphabet;
            for (int i = 0; i < 100; i++)
            {
                newKey = CipherLib.Annealing.MessWithStringKey(newKey);
            }
            string bestKey = newKey;

            float newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey, alphabet, tempPrimer));
            float bestScore = newScore;

            int operation;

            //for (int i = 0; i < numOfTrials; i++)
            for (float T = 20f; T >= 0f; T -= 0.2f)
            {
                if (displayCountdown)
                {
                    CipherLib.Utils.ClearLine();
                    Console.Write(T.ToString("n1"));
                }

                for (int count  = 0; count < 10000; count++)
                {
                    operation = CipherLib.Utils.rand.Next() % 10;

                    if (operation < 6)
                    {
                        newKey = CipherLib.Annealing.MessWithStringKey(newKey);
                    }
                    else
                    {
                        newPrimer = CipherLib.Annealing.SetChar((char)(48 + CipherLib.Utils.rand.Next() % 10), CipherLib.Utils.rand.Next() % primerLength, newPrimer);
                        tempPrimer = ExtendPrimer(newPrimer, ciphertext.Length);
                    }

                    //ExtendPrimer(newPrimer, ciphertext.Length);
                    newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey, alphabet, tempPrimer));

                    if (newScore > bestScore)
                    {
                        bestScore = newScore;
                        bestKey = newKey;
                        bestPrimer = newPrimer;
                    }
                    else
                    {
                        //bool replaceAnyway = CipherLib.Annealing.AnnealingProbability(newScore - bestScore, 20f * ((float)numOfTrials - i) / numOfTrials);
                        bool replaceAnyway = CipherLib.Annealing.AnnealingProbability(newScore - bestScore, T);

                        if (replaceAnyway)
                        {
                            bestScore = newScore;
                            bestKey = newKey;
                            bestPrimer = newPrimer;
                        }
                        else
                        {
                            newKey = bestKey;
                            newPrimer = bestPrimer;
                            tempPrimer = ExtendPrimer(bestPrimer, ciphertext.Length);
                        }
                    }
                }
            }

            return new Tuple<string, string, string>(Decrypt(ciphertext, bestKey, alphabet, bestPrimer), bestKey, bestPrimer);
        }
    }
}
