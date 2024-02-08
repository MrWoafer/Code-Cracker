using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    class ProgressiveKey
    {
        public static string DecryptPartial(string ciphertext, int shift, int progressionIndex, int keyColumn, int period, string alphabet)
        {
            StringBuilder decodedMsg = new StringBuilder();
            for (int i = keyColumn; i < ciphertext.Length; i += period)
            {
                //decodedMsg.Append(alphabet[(int)Annealing.Mod((ciphertext[i] - 97 - shift - progressionIndex * i), alphabet.Length)]);
                decodedMsg.Append(alphabet[(int)Annealing.Mod((ciphertext[i] - 97 - shift - progressionIndex * (i / period)), alphabet.Length)]);
            }
            return decodedMsg.ToString();
        }

        public static string Decrypt(string ciphertext, string key, int progressionIndex, string alphabet)
        {
            StringBuilder decodedMsg = new StringBuilder();
            for (int i = 0; i < ciphertext.Length; i++)
            {
                decodedMsg.Append(alphabet[(int)Annealing.Mod((ciphertext[i] - key[i % key.Length] - progressionIndex * (i / key.Length)), alphabet.Length)]);
            }
            return decodedMsg.ToString();
        }

        public static Tuple<string, string> CrackReturnKey(string ciphertext, int period, int progressionIndex, string alphabet = "abcdefghijklmnopqrstuvwxyz")
        {
            string key = "";

            float bestScore = float.MinValue;
            int bestKey;
            float newScore;

            for (int i = 0; i < period; i++)
            {
                bestKey = 0;
                bestScore = CipherLib.Annealing.ChiSquared(DecryptPartial(ciphertext, 0, progressionIndex, i, period, alphabet));

                for (int j = 1; j < 26; j++)
                {
                    newScore = CipherLib.Annealing.ChiSquared(DecryptPartial(ciphertext, j, progressionIndex, i, period, alphabet));

                    if (newScore < bestScore)
                    {
                        bestScore = newScore;
                        bestKey = j;
                    }
                }

                key += alphabet[bestKey];
            }

            return new Tuple<string, string>(Decrypt(ciphertext, key, progressionIndex, alphabet), key);
        }
    }
}
