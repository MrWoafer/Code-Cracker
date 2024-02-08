using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    class Ragbaby
    {
        public static string Decrypt(string ciphertext, string key, int startIndex)
        {
            StringBuilder plaintext = new StringBuilder();

            string[] split = ciphertext.Split();

            for(int i = 0; i < split.Length; i++)
            {
                for (int j = 0; j < split[i].Length; j++)
                {
                    //plaintext.Append(key[CipherLib.Utils.Mod(key.IndexOf(split[i][j]) - i - startIndex - j, 26)]);
                    plaintext.Append(key[CipherLib.Utils.Mod(key.IndexOf(split[i][j]) - i - startIndex - j, key.Length)]);
                }
            }

            return plaintext.ToString();
        }

        public static Tuple<string, string> CrackReturnKey(string ciphertext, string alphabet, int startIndex, int numOfTrials = 1000)
        {
            string newKey = alphabet;
            for (int i = 0; i < 100; i++)
            {
                newKey = CipherLib.Annealing.MessWithStringKey(newKey);
            }
            string bestKey = newKey;

            float newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey, startIndex));
            float bestScore = newScore;

            for (int i = 0; i < numOfTrials; i++)
            {
                //newKey = CipherLib.Annealing.MessWithStringKey(newKey);
                newKey = CipherLib.Annealing.MessWithStringKey(bestKey);

                newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey, startIndex));

                if (newScore > bestScore)
                {
                    bestScore = newScore;
                    bestKey = newKey;
                }
            }

            return new Tuple<string, string>(Decrypt(ciphertext, bestKey, startIndex), bestKey);
        }
    }
}
