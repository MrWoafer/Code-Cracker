using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    class ABCDEFGHIK
    {
        //public static string TransposeABCDEFGHIK(string msg, int[] key, CipherLib.TranspositionType transpoType, string alphabet, bool turnToMonograms = true)
        public static string TransposeABCDEFGHIK(string msg, int[] key, CipherLib.TranspositionType transpoType, int ngramLength, string alphabet, bool turnToMonograms = true)
        {
            string decodedMsg;
            if (transpoType == CipherLib.TranspositionType.column)
            {
                //decodedMsg = CipherLib.Annealing.DecodeColumnTranspo(msg, key);
                //decodedMsg = IncompleteColumnarTranspoABCDEFGHIK(msg, key);
                decodedMsg = CipherLib.Annealing.IncompleteColumnarTranspo(msg, key);
            }
            //else
            else if (transpoType == TranspositionType.row)
            {
                decodedMsg = CipherLib.Annealing.DecodeRowTranspo(msg, key);
            }
            else if (transpoType == TranspositionType.myszkowski)
            {
                decodedMsg = CipherLib.Annealing.DecodeMyszkowskiTranspo(msg, key);
            }
            else if (transpoType == TranspositionType.amsco)
            {
                decodedMsg = CipherLib.Annealing.DecodeAMSCO(msg, 2, key, new int[] { 0, 1 });
            }
            else
            {
                return null;
            }

            if (turnToMonograms)
            {
                //string replacedMsg = DigramToMonogramABCDEFGHIK(decodedMsg, alphabet);

                //return replacedMsg;
                //return DigramToMonogramABCDEFGHIK(decodedMsg, alphabet);
                return NGramToMonogramABCDEFGHIK(decodedMsg, ngramLength, alphabet);
            }
            else
            {
                return decodedMsg;
            }
        }

        //public static float IOCABCDEFGHIK(string msg, int[] key, CipherLib.TranspositionType transpoType, string alphabet)
        public static float IOCABCDEFGHIK(string msg, int[] key, CipherLib.TranspositionType transpoType, int ngramLength, string alphabet)
        {
            //string decoded = TransposeABCDEFGHIK(msg, key, transpoType, alphabet);
            string decoded = TransposeABCDEFGHIK(msg, key, transpoType, ngramLength, alphabet);
            if (decoded == null)
            {
                return -1f;
            }
            else
            {
                return CipherLib.Annealing.IOCAlphanumeric(decoded);
            }
        }

        //public static string DigramToMonogramABCDEFGHIK(string text, string alphabet)
        public static string NGramToMonogramABCDEFGHIK(string text, int ngramLength, string alphabet)
        {
            StringBuilder subbedText = new StringBuilder();

            Dictionary<string, char> ngramToLetter = new Dictionary<string, char>();

            string ngram;
            int index = 0;
            //for (int i = 0; i < text.Length; i += 2)
            for (int i = 0; i < text.Length; i += ngramLength)
            {
                ngram = "";
                //for (int j = 0; j < 2; j++)
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

        //public static string DecodeABCDEFGHIK(string msg, int[] key, CipherLib.TranspositionType transpoType, string alphabet, int trials = 2500)
        public static string DecodeABCDEFGHIK(string msg, int[] key, CipherLib.TranspositionType transpoType, int ngramLength, string alphabet, int trials = 2500)
        {
            //return CipherLib.Annealing.CrackCustomMonoSub(TransposeABCDEFGHIK(msg, key, transpoType, alphabet), alphabet, trials, CipherLib.SolutionType.QuadgramScore);
            return CipherLib.Annealing.CrackCustomMonoSub(TransposeABCDEFGHIK(msg, key, transpoType, ngramLength, alphabet), alphabet, trials, CipherLib.SolutionType.QuadgramScore);
        }

        //public static Dictionary<string, char> DeduceKey(string plaintext, string transposedText, string alphabet)
        public static Dictionary<string, char> DeduceKey(string plaintext, string transposedText, int ngramLength, string alphabet)
        {
            Dictionary<string, char> nGramsToPlaintext = new Dictionary<string, char>();
            string ngram;
            for (int i = 0; i < plaintext.Length; i++)
            {
                //ngram = transposedText[i * 2].ToString() + transposedText[i * 2 + 1];
                ngram = "";
                for (int j = 0; j < ngramLength; j++)
                {
                    ngram += transposedText[i * ngramLength + j];
                }

                if (!nGramsToPlaintext.ContainsKey(ngram))
                {
                    nGramsToPlaintext[ngram] = plaintext[i];
                }
            }
            return nGramsToPlaintext;
        }

        //public static void DisplayKey(Dictionary<string, char> key)
        public static void DisplayKey(Dictionary<string, char> key, int ngramLength)
        {
            foreach (string i in key.Keys)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n");
            foreach (char i in key.Values)
            {
                for (int j = 0; j < ngramLength - 1; j++)
                {
                    Console.Write(" ");
                }
                Console.Write(i);
                Console.Write(" ");
            }
            //Console.Write("\n\n");
        }

        /*public static string IncompleteColumnarTranspoABCDEFGHIK(string msg, int[] key)
        {
            List<char>[] grid = new List<char>[key.Length];
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = new List<char>();
            }

            //int columnNum = msg.Length / key.Length;
            int rowNum = msg.Length / key.Length;

            int index = 0;
            for (int i = 0; i < key.Length; i++)
            {
                for (int j = 0; j < rowNum; j++)
                {
                    grid[i].Add(msg[index]);
                    index++;
                }
                //if (key[i] <= msg.Length % key.Length)
                if (key[i] < msg.Length % key.Length)
                {
                    grid[i].Add(msg[index]);
                    index++;
                }
            }

            /*for (int i = 0; i < rowNum; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    Console.Write(grid[j][i] + " ");
                }
                Console.Write("\n");
            }*/

            /*StringBuilder transpoed = new StringBuilder();*/

            /*for (int i = 0; i < key.Length; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    if (key[j] == i)
                    {
                        for (int k = 0; k < grid[j].Count; k++)
                        {
                            transpoed.Append(grid[j][k]);
                        }
                        break;
                    }
                }
            }*/

            /*for (int i = 0; i < rowNum; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    for (int k = 0; k < key.Length; k++)
                    {
                        if (key[k] == j)
                        {
                            transpoed.Append(grid[k][i]);
                            break;
                        }
                    }
                }
            }

            return transpoed.ToString();
        }*/
    }
}
