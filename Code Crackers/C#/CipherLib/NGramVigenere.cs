using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    class NGramVigenere
    {
        public static string Decrypt(string ciphertext, int[] key, string[] alphabet, Dictionary<string, int> indices = null)
        {
            StringBuilder plaintext = new StringBuilder();

            /// n is the length of the ngrams;
            int n = alphabet[0].Length;

            /// Go through all n-grams in the ciphertext
            string ngram;
            for (int i = 0; i < ciphertext.Length; i += n)
            {
                /// Construct the n-gram
                ngram = "";
                for (int j = 0; j < n; j++)
                {
                    ngram += ciphertext[i + j];
                }
                if (indices == null)
                {
                    /// Find the ngram in the alphabet
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        if (alphabet[j] == ngram)
                        {
                            /// Decrypt the ngram
                            //plaintext.Append(alphabet[(j - key[(i / n) % key.Length]) % alphabet.Length]);
                            plaintext.Append(alphabet[CipherLib.Utils.Mod(j - key[(i / n) % key.Length], alphabet.Length)]);
                            break;
                        }
                    }
                }
                else
                {
                    plaintext.Append(alphabet[CipherLib.Utils.Mod(indices[ngram] - key[(i / n) % key.Length], alphabet.Length)]);
                }
            }
            return plaintext.ToString();
        }

        public static Tuple<string, int[]> CrackReturnKey(string ciphertext, int period, string[] alphabet, Dictionary<string, int> indices = null)
        {
            int n = alphabet[0].Length;

            int[] key = new int[period];

            float bestScore = float.MinValue;
            int bestShift;
            float newScore;

            for (int i = 0; i < period; i++)
            {
                bestShift = 0;
                bestScore = CipherLib.Annealing.ChiSquared(DecryptPartial(ciphertext, 0, i, period, alphabet, indices));

                for (int j = 1; j < Math.Pow(26, n); j++)
                {
                    newScore = CipherLib.Annealing.ChiSquared(DecryptPartial(ciphertext, j, i, period, alphabet, indices));

                    if (newScore < bestScore)
                    {
                        bestScore = newScore;
                        bestShift = j;
                    }
                }

                key[i] = bestShift;
            }

            return new Tuple<string, int[]>(Decrypt(ciphertext, key, alphabet, indices), key);
        }

        public static string DecryptPartial(string ciphertext, int shift, int keyColumn, int period, string[] alphabet, Dictionary<string, int> indices = null)
        {
            StringBuilder plaintext = new StringBuilder();

            /// n is the length of the ngrams;
            int n = alphabet[0].Length;

            /// Go through all n-grams in the ciphertext
            string ngram;
            //StringBuilder ngram;
            for (int i = keyColumn * n; i < ciphertext.Length; i += n * period)
            {
                /// Construct the n-gram
                ngram = "";
                //ngram = new StringBuilder();
                for (int j = 0; j < n; j++)
                {
                    ngram += ciphertext[i + j];
                    //ngram.Append(ciphertext[i + j]);
                }
                if (indices == null)
                {
                    /// Find the ngram in the alphabet
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        //if (alphabet[j] == ngram)
                        if (alphabet[j] == ngram.ToString())
                        {
                            /// Decrypt the ngram
                            //plaintext.Append(alphabet[(j - key[(i / n) % key.Length]) % alphabet.Length]);
                            plaintext.Append(alphabet[CipherLib.Utils.Mod(j - shift, alphabet.Length)]);
                            break;
                        }
                    }
                }
                else
                {
                    plaintext.Append(alphabet[CipherLib.Utils.Mod(indices[ngram] - shift, alphabet.Length)]);
                    //plaintext.Append(alphabet[CipherLib.Utils.Mod(indices[ngram.ToString()] - shift, alphabet.Length)]);
                }
                //plaintext.Append("hel");
            }
            return plaintext.ToString();
        }
    }
}
