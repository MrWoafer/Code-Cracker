using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    static class Bazeries
    {
        //public static string Decrypt(string ciphertext, int n, string alphabet, int numOfTrials = 1000)
        public static string Decrypt(string ciphertext, int n, string alphabet)
        {
            //return CipherLib.Annealing.CrackCustomMonoSub(ReverseBlocks(ciphertext, n), alphabet, numOfTrials);
            return ReverseBlocks(ciphertext, n);
        }

        public static string ReverseBlocks(string ciphertext, int n)
        {
            StringBuilder reversed = new StringBuilder();

            int[] digits = new int[n.ToString().Length];
            for (int i = 0; i < digits.Length; i++)
            {
                digits[i] = n.ToString()[i] - 48;
            }

            string chunk = "";
            int numberIndex = 0;
            for (int i = 0; i < ciphertext.Length; i++)
            {
                if (digits[numberIndex % digits.Length] != 0)
                {
                    chunk = ciphertext[i] + chunk;
                }
                else
                {
                    /// If nothing was added to the chunk, make sure we don't go onto the next character in the ciphertext without using it.
                    /// This was causing a bug where sub-optimal texts had higher scores than the correct solution because they were shorter in length.
                    i--;
                }
                if (chunk.Length == digits[numberIndex % digits.Length])
                {
                    reversed.Append(chunk);
                    //Console.Write(chunk + " ");
                    chunk = "";
                    numberIndex++;
                }
            }

            if (chunk.Length > 0)
            {
                reversed.Append(chunk);
            }

            return reversed.ToString();
        }

        public static string DecryptCrackMonoSub(string ciphertext, int n, string alphabet, int numOfTrials = 1000)
        {
            return CipherLib.Annealing.CrackCustomMonoSub(ReverseBlocks(ciphertext, n), alphabet, numOfTrials);
        }
    }
}
