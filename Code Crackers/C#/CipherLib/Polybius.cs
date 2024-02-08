using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    class Polybius
    {
        public static string NGramToMonogram(string text, int ngramLength, string alphabet)
        {
            StringBuilder subbedText = new StringBuilder();

            Dictionary<string, char> ngramToLetter = new Dictionary<string, char>();

            string ngram;
            int index = 0;
            for (int i = 0; i < text.Length; i += ngramLength)
            {
                ngram = "";
                for (int j = 0; j < ngramLength; j++)
                {
                    ngram += text[i + j];
                }
                if (!ngramToLetter.ContainsKey(ngram))
                {
                    if (index >= alphabet.Length)
                    {
                        return null;
                    }
                    else
                    {
                        ngramToLetter[ngram] = alphabet[index];
                        index++;
                    }
                }
                subbedText.Append(ngramToLetter[ngram]);
            }

            return subbedText.ToString();
        }

        public static Tuple<string, string> CrackReturnKey(string ciphertext, int ngramLength, string alphabet, int numOfTrials)
        {
            return CipherLib.Annealing.CrackCustomMonoSubReturnKey(NGramToMonogram(ciphertext, ngramLength, alphabet), alphabet, numOfTrials);
        }
    }

    class PolybiusVigenere
    {
        public static Tuple<string, string> CrackPolybiusGrid(string ciphertext, int ngramLength, string vigenKey, string alphabet, int numOfTrials = 1000)
        {
            //return CipherLib.Annealing.CrackCustomMonoSubReturnKey(CipherLib.Annealing.DecodeVigenere(ciphertext, vigenKey), alphabet, numOfTrials);
            //return CipherLib.Annealing.CrackCustomMonoSubReturnKey(CipherLib.Polybius.NGramToMonogram(CipherLib.Annealing.DecodeVigenere(ciphertext, vigenKey), ngramLength, alphabet), alphabet, numOfTrials);

            return CipherLib.Polybius.CrackReturnKey(CipherLib.Annealing.DecodeVigenere(ciphertext, vigenKey), ngramLength, alphabet, numOfTrials);
        }

        //public static Tuple<string, string, string> CrackReturnKey(string ciphertext, int ngramLength, int period, string alphabet, int numOfTrials = 1000)
        private static Tuple<string, string, string> CrackReturnKey2(string ciphertext, int ngramLength, int period, string alphabet, int numOfTrials = 1000)
        {
            string newVigenKey = "";
            for (int i = 0; i < period; i++)
            {
                newVigenKey += alphabet[CipherLib.Utils.rand.Next() % alphabet.Length];
            }
            string bestVigenKey = newVigenKey;

            float newScore = float.MinValue;
            float bestScore = newScore;

            string undonePolybius;

            for (int i = 0; i < numOfTrials; i++)
            {
                //newVigenKey = CipherLib.Annealing.MessWithStringKey(newVigenKey);
                newVigenKey = CipherLib.Annealing.SetChar(alphabet[CipherLib.Utils.rand.Next() % alphabet.Length], CipherLib.Utils.rand.Next() % period, newVigenKey);

                /*Console.Write("----\n");
                Console.Write(CipherLib.Annealing.DecodeVigenere(ciphertext, newVigenKey));
                Console.Write("\n\n");
                Console.Write(CipherLib.Polybius.NGramToMonogram(CipherLib.Annealing.DecodeVigenere(ciphertext, newVigenKey), ngramLength, alphabet));*/

                undonePolybius = CipherLib.Polybius.NGramToMonogram(CipherLib.Annealing.DecodeVigenere(ciphertext, newVigenKey), ngramLength, alphabet);

                if (undonePolybius != null)
                {
                    //newScore = CipherLib.Annealing.IOCAlphanumeric(CipherLib.Polybius.NGramToMonogram(CipherLib.Annealing.DecodeVigenere(ciphertext, newVigenKey), ngramLength, alphabet));
                    newScore = CipherLib.Annealing.IOCAlphanumeric(undonePolybius);

                    if (newScore > bestScore)
                    {
                        bestScore = newScore;
                        bestVigenKey = newVigenKey;
                    }
                    else
                    {
                        newVigenKey = bestVigenKey;
                    }
                }
                else
                {
                    newVigenKey = bestVigenKey;
                }
            }

            Tuple<string, string> result = CrackPolybiusGrid(ciphertext, ngramLength, bestVigenKey, alphabet, 1000);

            return new Tuple<string, string, string>(result.Item1, result.Item2, bestVigenKey);
        }

        //public static Tuple<string, string, string> CrackReturnKey(string ciphertext, int ngramLength, int period, string alphabet, int numOfTrials = 1000)
        //public static Tuple<string, string, string> CrackReturnKey(string ciphertext, int ngramLength, int period, string alphabet, bool exactMatch = false)
        public static Tuple<string, string, string> CrackReturnKey(string ciphertext, int ngramLength, int period, string alphabet, int numAllowedIncorrectChars = 0)
        {
            //HashSet<char> usedSymbols = new HashSet<char>();
            //HashSet<char>[] usedSymbols = new HashSet<char>[ngramLength];
            HashSet<char>[] usedSymbols = new HashSet<char>[ngramLength];
            for (int i = 0; i < ngramLength; i++)
            {
                usedSymbols[i] = new HashSet<char>();
            }

            //for (int i = 0; i < ciphertext.Length; i += period)
            for (int j = 0; j < ngramLength; j++)
            {
                //for (int i = 0; i < ciphertext.Length; i += period * ngramLength)
                for (int i = j; i < ciphertext.Length; i += period * ngramLength)
                {
                    //usedSymbols.Add(ciphertext[i]);
                    usedSymbols[j].Add(ciphertext[i]);
                }
            }

            bool isValid;
            string partialDecrypt;

            //string bestVigenKey = "a";
            //string bestVigenKey = "";

            int count;
            HashSet<char> alreadyCounted;
            //for (int i = 1; i < period; i++)
            //bool alreadyDone;
            List<string> potentialKeys = new List<string>();
            List<string> temp = new List<string>();
            List<char> potential = new List<char>();

            potentialKeys.Add("a");

            //for (int i = 1; i < ngramLength * period; i++)
            for (int i = 1; i < CipherLib.Utils.LCM(ngramLength, period); i++)
            {
                //alreadyDone = false;

                potential = new List<char>();

                for (int j = 0; j < alphabet.Length; j++)
                {
                    //partialDecrypt = CipherLib.Annealing.DecodePartialVigenere(ciphertext, j, i, period);
                    partialDecrypt = CipherLib.Annealing.DecodePartialVigenere(ciphertext, j, i, ngramLength * period);

                    isValid = true;
                    count = 0;
                    alreadyCounted = new HashSet<char>();
                    for (int k = 0; k < partialDecrypt.Length; k++)
                    {
                        //if (!usedSymbols.Contains(partialDecrypt[k]))
                        if (!usedSymbols[i % ngramLength].Contains(partialDecrypt[k]))
                        {
                            //isValid = true;
                            if (!alreadyCounted.Contains(partialDecrypt[k]))
                            {
                                count++;
                                alreadyCounted.Add(partialDecrypt[k]);
                            }
                        }
                    }
                    //if (count > 1)
                    //if (count > 0)
                    //if ((exactMatch && count > 0) || (!exactMatch && count > 1))
                    if (count > numAllowedIncorrectChars)
                    {
                        isValid = false;
                    }

                    //if (isValid && !alreadyDone)
                    if (isValid)
                    {
                        //bestVigenKey += alphabet[j];
                        //alreadyDone = true;
                        potential.Add(alphabet[j]);
                    }
                }

                temp = new List<string>();

                foreach (char t in potential)
                {
                    foreach (string key in potentialKeys)
                    {
                        temp.Add(key + t);
                    }
                }
                potentialKeys = new List<string>(temp);
            }

            //Console.Write("\n\n" + potentialKeys.Count + " potential keys...\n\n");
            Console.Write("I identified " + potentialKeys.Count + " potential keys...\n\n");

            //Console.Write(bestVigenKey);

            //Tuple<string, string> result = CrackPolybiusGrid(ciphertext, ngramLength, bestVigenKey, alphabet, 1000);
            Tuple<string, string> result;

            float newScore = float.MinValue;
            float bestScore = newScore;

            string bestVigenKey = null;

            foreach (string key in potentialKeys)
            {
                try
                {
                    result = CrackPolybiusGrid(ciphertext, ngramLength, key, alphabet, 1000);

                    newScore = CipherLib.Annealing.QuadgramScore(result.Item1);

                    if (newScore > bestScore)
                    {
                        bestScore = newScore;
                        bestVigenKey = key;
                    }
                }
                catch
                {

                }
            }

            if (bestVigenKey == null)
            {
                return null;
            }
            else
            {
                result = CrackPolybiusGrid(ciphertext, ngramLength, bestVigenKey, alphabet, 1000);

                return new Tuple<string, string, string>(result.Item1, result.Item2, bestVigenKey);
            }
        }
    }
}
