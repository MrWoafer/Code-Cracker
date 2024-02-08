using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    static class Morse
    {
        public static readonly Dictionary<string, string> _CharToMorse = new Dictionary<string, string>()
        {
            {"", ""},
            {"a", ".-"},
            {"b", "-..."},
            {"c", "-.-."},
            {"d", "-.."},
            {"e", "."},
            {"f", "..-."},
            {"g", "--."},
            {"h", "...."},
            {"i", ".."},
            {"j", ".---"},
            {"k", "-.-"},
            {"l", ".-.."},
            {"m", "--"},
            {"n", "-."},
            {"o", "---"},
            {"p", ".--."},
            {"q", "--.-"},
            {"r", ".-."},
            {"s", "..."},
            {"t", "-"},
            {"u", "..-"},
            {"v", "...-"},
            {"w", ".--"},
            {"x", "-..-"},
            {"y", "-.--"},
            {"z", "--.."},
            {"1", ".----"},
            {"2", "..---"},
            {"3", "...--"},
            {"4", "....-"},
            {"5", "....."},
            {"6", "-...."},
            {"7", "--..."},
            {"8", "---.."},
            {"9", "----."},
            {"0", "-----"},
            {"ch", "----"},
            {"á", ".--.-"},
            {"â", ".--.-"},
            {"é", "..-.."},
            {"ñ", "--.--"},
            {"ö", "---."},
            {"ü", "..--"},
            {"<aa>", ".-.-"},
            {"<bk>", "-...-.-"},
            {"<cl>", "-.-..-.."},
            {"<ct>", "-.-.-"},
            {"<do>", "-..---"},
            {"<sk>", "...-.-"},
            {"<sn>", "...-."},
            {"<sos>", "...---..."},
            {"<MISTAKE>", "........"},
            {"&", ".-..."},
            {"'", ".----."},
            {"@", ".--.-."},
            {")", "-.--.-"},
            {"(", "-.--."},
            {":", "---..."},
            {",", "--..--"},
            {"=", "-...-"},
            {"!", "-.-.--"},
            {".", ".-.-.-"},
            {"-", "-....-"},
            {"+", ".-.-."},
            {"\"", ".-..-."},
            {"?", "..--.."},
            {"/", "-..-."},
        };

        public static readonly Dictionary<string, string> _MorseToChar = new Dictionary<string, string>()
        {
            {".-", "a"},
            {"-...", "b"},
            {"-.-.", "c"},
            {"-..", "d"},
            {".", "e"},
            {"..-.", "f"},
            {"--.", "g"},
            {"....", "h"},
            {"..", "i"},
            {".---", "j"},
            {"-.-", "k"},
            {".-..", "l"},
            {"--", "m"},
            {"-.", "n"},
            {"---", "o"},
            {".--.", "p"},
            {"--.-", "q"},
            {".-.", "r"},
            {"...", "s"},
            {"-", "t"},
            {"..-", "u"},
            {"...-", "v"},
            {".--", "w"},
            {"-..-", "x"},
            {"-.--", "y"},
            {"--..", "z"},
            {".----", "1"},
            {"..---", "2"},
            {"...--", "3"},
            {"....-", "4"},
            {".....", "5"},
            {"-....", "6"},
            {"--...", "7"},
            {"---..", "8"},
            {"----.", "9"},
            {"-----", "0"},
            {"----", "ch"},
            {".--.-", "â"},
            {"..-..", "é"},
            {"--.--", "ñ"},
            {"---.", "ö"},
            {"..--", "ü"},
            {".-.-", "<aa>"},
            {"-...-.-", "<bk>"},
            {"-.-..-..", "<cl>"},
            {"-.-.-", "<ct>"},
            {"-..---", "<do>"},
            {"...-.-", "<sk>"},
            {"...-.", "<sn>"},
            {"...---...", "<sos>"},
            {"........", "<MISTAKE>"},
            {".-...", "&"},
            {".----.", "'"},
            {".--.-.", "@"},
            {"-.--.-", ")"},
            {"-.--.", "("},
            {"---...", ":"},
            {"--..--", ","},
            {"-...-", "="},
            {"-.-.--", "!"},
            {".-.-.-", "."},
            {"-....-", "-"},
            {".-.-.", "+"},
            {".-..-.", "\""},
            {"..--..", "?"},
            {"-..-.", "/"}
        };

        private static readonly HashSet<string> restrictedChars = new HashSet<string>()
        {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
        };
        private static readonly HashSet<string> superRestrictedChars = new HashSet<string>()
        {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"
        };

        public static string CharToMorseRestricted(string chr)
        {
            if (restrictedChars.Contains(chr))
            {
                return _CharToMorse[chr];
            }
            else
            {
                return null;
            }
        }
        public static string MorseToCharRestricted(string morse)
        {
            if (_MorseToChar.ContainsKey(morse) && restrictedChars.Contains(_MorseToChar[morse]))
            {
                return _MorseToChar[morse];
            }
            else
            {
                return null;
            }
        }
        public static string CharToMorseSuperRestricted(string chr)
        {
            if (superRestrictedChars.Contains(chr))
            {
                return _CharToMorse[chr];
            }
            else
            {
                return null;
            }
        }
        public static string MorseToCharSuperRestricted(string morse)
        {
            if (_MorseToChar.ContainsKey(morse) && superRestrictedChars.Contains(_MorseToChar[morse]))
            {
                return _MorseToChar[morse];
            }
            else
            {
                return null;
            }
        }

        //public static string DecodeRestricted(string text)
        public static string DecodeRestricted(string text, char separator = '/')
        {
            StringBuilder decoded = new StringBuilder();

            //foreach (string i in text.Split('/'))
            foreach (string i in text.Split(separator))
            {
                //Console.Write(i + "|\n");
                if (i == "")
                {
                    //decoded.Append(' ');
                }
                else if (MorseToCharRestricted(i) != null)
                {
                    decoded.Append(MorseToCharRestricted(i));
                }
                else
                {
                    //return null;
                    decoded.Append('x');
                }
                /*switch (MorseToCharRestricted(i))
                {
                    case null:
                        return null;
                    case "":
                        decoded.Append(' ');
                        break;
                    case " ":
                        decoded.Append(" ");
                        break;
                    default:
                        decoded.Append(MorseToCharRestricted(i));
                        break;
                }*/
            }
            return decoded.ToString();
        }
        public static string DecodeSuperRestricted(string text)
        {
            StringBuilder decoded = new StringBuilder();

            foreach (string i in text.Split('/'))
            {
                if (i == "")
                {
                }
                else if (MorseToCharSuperRestricted(i) != null)
                {
                    decoded.Append(MorseToCharSuperRestricted(i));
                }
                else
                {
                    //return null;
                    decoded.Append('x');
                    //decoded.Append('xxxx');
                    //decoded.Append("xxxx");
                }
            }
            return decoded.ToString();
        }

        public static string DecodePollux(string ciphertext, char[] dashHomophones, char[] dothomophones, char[] separatorHomophones)
        {
            StringBuilder morse = new StringBuilder();

            for (int i = 0; i < ciphertext.Length; i++)
            {
                if (dashHomophones.Contains(ciphertext[i]))
                {
                    morse.Append('-');
                }
                else if (dothomophones.Contains(ciphertext[i]))
                {
                    morse.Append('.');
                }
                else if (separatorHomophones.Contains(ciphertext[i]))
                {
                    morse.Append('/');
                }
                else
                {
                    return null;
                }
            }
            //return DecodeRestricted(morse.ToString());
            return DecodeSuperRestricted(morse.ToString());
        }

        public static string SubPollux(string ciphertext, char[] dashHomophones, char[] dothomophones, char[] separatorHomophones)
        {
            StringBuilder morse = new StringBuilder();

            for (int i = 0; i < ciphertext.Length; i++)
            {
                if (dashHomophones.Contains(ciphertext[i]))
                {
                    morse.Append('-');
                }
                else if (dothomophones.Contains(ciphertext[i]))
                {
                    morse.Append('.');
                }
                else if (separatorHomophones.Contains(ciphertext[i]))
                {
                    morse.Append('/');
                }
                else
                {
                    return null;
                }
            }
            return morse.ToString();
        }
    }


    static class FractionatedMorse
    {
        public static readonly string[] TranslationTable = new string[]
        {
            "...",
            "..-",
            "..x",
            ".-.",
            ".--",
            ".-x",
            ".x.",
            ".x-",
            ".xx",
            "-..",
            "-.-",
            "-.x",
            "--.",
            "---",
            "--x",
            "-x.",
            "-x-",
            "-xx",
            "x..",
            "x.-",
            "x.x",
            "x-.",
            "x--",
            "x-x",
            "xx.",
            "xx-",
        };

        public static string Decrypt(string ciphertext, string key)
        {
            StringBuilder morse = new StringBuilder();

            for (int i = 0; i < ciphertext.Length; i++)
            {
                morse.Append(TranslationTable[key.IndexOf(ciphertext[i])]);
            }

            return CipherLib.Morse.DecodeRestricted(morse.ToString(), 'x');
        }

        public static Tuple<string, string> CrackReturnKey(string ciphertext, string alphabet, int numOfTrials = 1000)
        {
            string newKey = alphabet;
            for (int i = 0; i < 100; i++)
            {
                newKey = CipherLib.Annealing.MessWithStringKey(newKey);
            }
            string bestKey = newKey;

            float newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey));
            float bestScore = newScore;

            for (int i = 0; i < numOfTrials; i++)
            {
                newKey = CipherLib.Annealing.MessWithStringKey(bestKey);

                newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey));

                if (newScore > bestScore)
                {
                    bestScore = newScore;
                    bestKey = newKey;
                }
                else
                {
                    bool ReplaceAnyway;
                    //ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(newScore - bestScore, numOfTrials - i);
                    ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(newScore - bestScore, 20f);

                    if (ReplaceAnyway == true)
                    {
                        bestScore = newScore;
                        bestKey = newKey;
                    }
                }
            }

            return new Tuple<string, string>(Decrypt(ciphertext, bestKey), bestKey);
        }
    }

    static class Morbit
    {
        public static readonly string[] TranslationTable = new string[]
        {
            "..",
            ".-",
            ".x",
            "-.",
            "--",
            "-x",
            "x.",
            "x-",
            "xx",
        };

        public static string Decrypt(string ciphertext, string key)
        {
            StringBuilder morse = new StringBuilder();

            for (int i = 0; i < ciphertext.Length; i++)
            {
                morse.Append(TranslationTable[key.IndexOf(ciphertext[i])]);
            }

            return CipherLib.Morse.DecodeRestricted(morse.ToString(), 'x');
        }

        public static Tuple<string, string> CrackReturnKey(string ciphertext, string alphabet, int numOfTrials = 1000)
        {
            string newKey = alphabet;
            for (int i = 0; i < 100; i++)
            {
                newKey = CipherLib.Annealing.MessWithStringKey(newKey);
            }
            string bestKey = newKey;

            //float newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey));
            float newScore = Score(ciphertext, newKey);
            float bestScore = newScore;

            float newQuadScore = float.MinValue;
            float bestQuadScore = newQuadScore;

            for (int i = 0; i < numOfTrials; i++)
            {
                newKey = CipherLib.Annealing.MessWithStringKey(bestKey);

                //newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey));
                newScore = Score(ciphertext, newKey);

                if (newScore == 0)
                {
                    newQuadScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey));

                    if (newQuadScore > bestQuadScore)
                    {
                        bestKey = newKey;
                        bestQuadScore = newQuadScore;
                    }
                }
                else
                {
                    if (newScore > bestScore)
                    {
                        bestScore = newScore;
                        bestKey = newKey;
                    }
                    else
                    {
                        bool ReplaceAnyway;
                        ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(newScore - bestScore, numOfTrials - i);

                        if (ReplaceAnyway == true)
                        {
                            bestScore = newScore;
                            bestKey = newKey;
                        }
                    }
                }
            }

            return new Tuple<string, string>(Decrypt(ciphertext, bestKey), bestKey);
        }

        private static float Score(string ciphertext, string key)
        {
            StringBuilder morse = new StringBuilder();

            for (int i = 0; i < ciphertext.Length; i++)
            {
                morse.Append(TranslationTable[key.IndexOf(ciphertext[i])]);
            }

            float score = 0f;

            foreach (string i in morse.ToString().Split('x'))
            {
                if (i == "")
                {
                }
                else if (CipherLib.Morse.MorseToCharRestricted(i) == null)
                {
                    score -= 1f;
                }
            }
            return score;
        }
    }
}
