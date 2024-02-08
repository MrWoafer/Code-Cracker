using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpRunningKeyWorkspace
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("-- C# Running Key Workspace --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            //Console.SetWindowSize(1000, 700);
            //Console.SetWindowSize(200, 30);
            Console.SetWindowSize(170, 40);
            //Console.SetBufferSize(100, 30);
            //Console.SetBufferSize(40, 30);
            //Console.SetBufferSize(15, 15);
            //Console.Write(Console.BufferWidth + " " + Console.BufferHeight);
            //Console.ReadLine();
            //Console.SetBufferSize(1000, 1000);            

            //string msg = System.IO.File.ReadAllText("--RunningKeyMessage.txt");
            string msg = System.IO.File.ReadAllText("--RunningKeyWorkspaceMessage.txt");

            int aIndex = 0;
            CipherLib.DecryptionType decryptType = CipherLib.DecryptionType.subtraction;

            string key = "";

            for (int i = 0; i < msg.Length; i++)
            {
                key += ".";
            }

            //int startIndex = 0;
            int typeIndex = 0;
            ConsoleKeyInfo keyPressed;            

            bool editingKey = true;
            //string plaintext = "";
            string plaintext = key;

            bool doneSomething;
            int maxLength;
            bool updateWholeDisplay;

            string confirmation;

            Console.SetBufferSize(msg.Length + 5 + 1, Console.WindowHeight);

            //DisplayWorkspace(typeIndex, editingKey, msg, key, aIndex, decryptType);
            SetupDisplayWorkspace(typeIndex, editingKey, msg, key, aIndex, decryptType);

            while (true)
            {
                //keyPressed = Console.ReadKey(false);
                keyPressed = Console.ReadKey(true);

                doneSomething = true;
                //maxLength = msg.Length - Console.WindowWidth + 5;
                maxLength = msg.Length;
                updateWholeDisplay = false;

                if (keyPressed.Key.ToString().Length == 1 && CipherLib.Annealing.IsEnglishLetter(keyPressed.Key.ToString()[0]) && (keyPressed.Modifiers & ConsoleModifiers.Control) == 0)
                {
                    if (editingKey)
                    {
                        //key += keyPressed.Key.ToString().ToLower()[0];
                        /*if (typeIndex >= key.Length)
                        {
                            key += keyPressed.Key.ToString().ToLower()[0];
                        }
                        else
                        {
                            key = key.Insert(typeIndex, keyPressed.Key.ToString().ToLower());
                        }*/
                        //typeIndex += 1;
                        key = CipherLib.Annealing.SetChar(keyPressed.Key.ToString().ToLower()[0], typeIndex, key);
                    }
                    else
                    {
                        //plaintext += keyPressed.Key.ToString().ToLower()[0];
                        /*if (typeIndex >= plaintext.Length)
                        {
                            plaintext += keyPressed.Key.ToString().ToLower()[0];
                        }
                        else
                        {
                            plaintext = plaintext.Insert(typeIndex, keyPressed.Key.ToString().ToLower());                            
                        }*/
                        //typeIndex += 1;
                        plaintext = CipherLib.Annealing.SetChar(keyPressed.Key.ToString().ToLower()[0], typeIndex, plaintext);
                    }
                    typeIndex += 1;
                }
                else if (keyPressed.Key.ToString().Length == 1 && CipherLib.Annealing.IsEnglishLetter(keyPressed.Key.ToString()[0]) && (keyPressed.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    //if (keyPressed.Key == ConsoleKey.M)
                    if (keyPressed.Key == ConsoleKey.I)
                    {
                        //Console.Write("\n\nEnter index to move cursor to: ");
                        if (editingKey)
                        {
                            Console.Write("\n");
                        }
                        //Console.Write("\nEnter index to move cursor to: ");
                        Console.Write("\n\nEnter index to move cursor to: ");

                        try
                        {
                            //confirmation = Console.ReadLine();
                            //int newIndex = Int32.Parse(confirmation);
                            int newIndex = Int32.Parse(Console.ReadLine());

                            typeIndex = newIndex;

                            updateWholeDisplay = true;
                        }
                        catch
                        {
                            Console.Write("Invalid integer.");
                        }
                    }
                    else if (keyPressed.Key == ConsoleKey.R)
                    {
                        SetupDisplayWorkspace(typeIndex, editingKey, msg, key, aIndex, decryptType);
                        //doneSomething = false;
                        ///SetupDisplayWorkspace(typeIndex, editingKey, msg, key, aIndex, decryptType);
                        updateWholeDisplay = true;

                        /// /// For some reason, I need to do this command ( SetupDisplayWorkspace() ) twice. Otherwise, it doesn't quite reset the display. ///
                    }
                    else if (keyPressed.Key == ConsoleKey.P)
                    {
                        if (editingKey)
                        {
                            Console.Write("\n");
                        }
                        Console.Write("\n\nEnter text to paste: ");
                        //Console.Write("\nEnter text to paste: ");
                        confirmation = Console.ReadLine();
                        if (editingKey)
                        {
                            for (int i = typeIndex; i < key.Length && i - typeIndex < confirmation.Length; i++)
                            {
                                key = CipherLib.Annealing.SetChar(confirmation[i - typeIndex], i, key);
                            }
                        }
                        else
                        {
                            for (int i = typeIndex; i < plaintext.Length && i - typeIndex < confirmation.Length; i++)
                            {
                                plaintext = CipherLib.Annealing.SetChar(confirmation[i - typeIndex], i, plaintext);
                            }
                        }

                        updateWholeDisplay = true;
                    }
                }
                //else if (keyPressed.Key.ToString() == "BACK")
                else if (keyPressed.Key == ConsoleKey.Backspace)
                {
                    if (editingKey && key.Length >= 1)
                    {
                        /*if (key[key.Length - 1] == ' ')
                        {
                            //msg = msg.Remove(key.Length - 1, 1);
                            msg = msg.Remove(typeIndex - 1, 1);
                        }
                        key = key.Remove(key.Length - 1, 1);*/
                        /*if (typeIndex >= key.Length)
                        {
                            key = key.Remove(key.Length - 1, 1);
                        }
                        else
                        {
                            key = key.Remove(typeIndex - 1, 1);
                        }*/
                        if (typeIndex > 0)
                        {
                            key = CipherLib.Annealing.SetChar('.', typeIndex - 1, key);
                        }                        
                    }
                    else if (!editingKey && plaintext.Length >= 1)
                    {
                        /*if (plaintext[plaintext.Length - 1] == ' ')
                        {
                            //msg = msg.Remove(plaintext.Length - 1, 1);
                            msg = msg.Remove(typeIndex - 1, 1);
                        }
                        plaintext = plaintext.Remove(plaintext.Length - 1, 1);
                    }*/
                        /*if (typeIndex >= plaintext.Length)
                        {
                            plaintext = plaintext.Remove(plaintext.Length - 1, 1);
                        }
                        else
                        {
                            plaintext = plaintext.Remove(typeIndex - 1, 1);
                        }*/
                        if (typeIndex > 0)
                        {
                            plaintext = CipherLib.Annealing.SetChar('.', typeIndex - 1, plaintext);
                        }                        
                    }
                    typeIndex -= 1;
                }
                else if (keyPressed.Key == ConsoleKey.Delete && (keyPressed.Modifiers & ConsoleModifiers.Control) == 0)
                {
                    if (editingKey && key.Length >= 1)
                    {
                        if (typeIndex < maxLength)
                        {
                            //key = CipherLib.Annealing.SetChar('.', typeIndex + 1, key);
                            key = CipherLib.Annealing.SetChar('.', typeIndex, key);
                        }
                    }
                    else if (!editingKey && plaintext.Length >= 1)
                    {
                        if (typeIndex < maxLength)
                        {
                            //plaintext = CipherLib.Annealing.SetChar('.', typeIndex + 1, plaintext);
                            plaintext = CipherLib.Annealing.SetChar('.', typeIndex, plaintext);
                        }
                    }
                    typeIndex += 1;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    editingKey = true;
                }
                else if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    editingKey = false;
                }
                else if (keyPressed.Key == ConsoleKey.LeftArrow)
                {
                    typeIndex -= 1;
                    if (typeIndex < 0)
                    {
                        typeIndex = 0;
                    }
                }
                else if (keyPressed.Key == ConsoleKey.RightArrow)
                {
                    typeIndex += 1;
                    if (typeIndex > maxLength)
                    {
                        typeIndex = maxLength;
                    }
                }
                else if (keyPressed.Key == ConsoleKey.Home)
                {
                    typeIndex = 0;
                }
                else if (keyPressed.Key == ConsoleKey.End)
                {
                    typeIndex = maxLength;
                }
                else if (keyPressed.Key == ConsoleKey.OemPlus)
                {
                    if (editingKey)
                    {
                        //key = "." + key;
                        key = ShiftTextRight(key, 1);
                    }
                    else
                    {
                        //plaintext = "." + plaintext;
                        plaintext = ShiftTextRight(plaintext, 1);
                    }
                }
                else if (keyPressed.Key == ConsoleKey.OemMinus)
                {
                    if (editingKey)
                    {
                        /*if (key[0] == '.')
                        {
                            //key = key.Remove(0, 1);                            
                        }*/
                        key = ShiftTextLeft(key, 1);
                    }
                    else
                    {
                        /*if (plaintext[0] == '.')
                        {
                            plaintext = plaintext.Remove(0, 1);
                            key = ShiftTextLeft(key, 1);
                        }*/
                        plaintext = ShiftTextLeft(plaintext, 1);
                    }
                }
                else if (keyPressed.Key == ConsoleKey.PageUp)
                {
                    aIndex = CipherLib.Annealing.Mod(aIndex + 1, 26);
                    updateWholeDisplay = true;
                }
                else if (keyPressed.Key == ConsoleKey.PageDown)
                {
                    aIndex = CipherLib.Annealing.Mod(aIndex - 1, 26);
                    updateWholeDisplay = true;
                }
                //else if (keyPressed.Key == ConsoleKey.Delete && (keyPressed.Modifiers & ConsoleModifiers.Control) == 0)
                //else if (keyPressed.Key == ConsoleKey.Pause)
                else if (keyPressed.Key == ConsoleKey.Multiply)
                {
                    if (decryptType == CipherLib.DecryptionType.subtraction)
                    {
                        decryptType = CipherLib.DecryptionType.addition;
                    }
                    else if (decryptType == CipherLib.DecryptionType.addition)
                    {
                        decryptType = CipherLib.DecryptionType.subtraction;
                    }
                    updateWholeDisplay = true;
                }
                /*else if (keyPressed.Key == ConsoleKey.Spacebar)
                {
                    //msg = msg.Insert(key.Length, " ");
                    msg = msg.Insert(typeIndex, " ");
                    //key += " ";
                    //plaintext += " ";
                    key.
                    typeIndex += 1;
                }*/
                else if (keyPressed.Key == ConsoleKey.Tab)
                {
                    if (editingKey)
                    {
                        /*for (int i = 0; i < key.Length; i++)
                        {
                            if (key[0] == '.')
                            {
                                key = key.Remove(0, 1);
                            }
                        }*/

                        key = SlideCrib(msg, key, decryptType, aIndex);

                        /*for (int i = 0; i < key.Length; i++)
                        {
                            if (key[i] != '.')
                            {
                                typeIndex = i;
                                break;
                            }
                        }*/
                    }
                    else
                    {
                        /*for (int i = 0; i < plaintext.Length; i++)
                        {
                            if (plaintext[0] == '.')
                            {
                                plaintext = plaintext.Remove(0, 1);
                            }
                        }*/

                        plaintext = SlideCrib(msg, plaintext, decryptType, aIndex);

                        /*for (int i = 0; i < plaintext.Length; i++)
                        {
                            if (plaintext[i] != '.')
                            {
                                typeIndex = i;
                                break;
                            }
                        }*/
                    }                    
                }
                else if (keyPressed.Key == ConsoleKey.Delete && (keyPressed.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if (editingKey)
                    {
                        Console.Write("\n");
                    }
                    Console.Write("\n\nAre you sure you want to clear everything? [y/n]: ");
                    //Console.Write("\nAre you sure you want to clear everything? [y/n]: ");
                    //confirmation = Console.Read().ToString().ToLower();
                    confirmation = Console.ReadLine().ToString().ToLower();

                    Console.Write(confirmation);

                    if (confirmation == "y")
                    {
                        //key = "";
                        //plaintext = "";
                        for (int i = 0; i < key.Length; i++)
                        {
                            key = CipherLib.Annealing.SetChar('.', i, key);
                            plaintext = CipherLib.Annealing.SetChar('.', i, plaintext);
                        }
                    }
                    //confirmation = "";
                    //doneSomething = false;
                    updateWholeDisplay = true;
                }
                else
                {
                    doneSomething = false;
                }

                typeIndex = CipherLib.Annealing.RoundToRange(typeIndex, 0, msg.Length - 1);

                if (doneSomething)
                {
                    if (editingKey)
                    {
                        plaintext = CipherLib.RunningKey.DecryptRunningKeyPartial(msg, key, decryptType, aIndex);
                    }
                    else
                    {
                        key = CipherLib.RunningKey.DeduceKey(plaintext, msg, decryptType, aIndex);
                    }

                    if (updateWholeDisplay)
                    {
                        SetupDisplayWorkspace(typeIndex, editingKey, msg, key, aIndex, decryptType);
                    }
                    //DisplayWorkspace(startIndex, editingKey, msg, key, aIndex, decryptType);
                    else
                    {
                        DisplayWorkspace(typeIndex, editingKey, msg, key, aIndex, decryptType);
                    }
                }                
            }

            Console.Write("\n\n-----------------------\n\n");
            Console.Write("Program ended.\n\nPress ENTER to exit...");
            Console.Read();
        }

        //private static void DisplayWorkspace(int startIndex, bool editingKey, string msg, string key, int aIndex, CipherLib.DecryptionType decryptType)
        //private static void DisplayWorkspace(int typeIndex, bool editingKey, string msg, string key, int aIndex, CipherLib.DecryptionType decryptType)
        private static void SetupDisplayWorkspace(int typeIndex, bool editingKey, string msg, string key, int aIndex, CipherLib.DecryptionType decryptType)
        {
            int leftIndex = Console.WindowLeft;

            Console.Clear();
            /*for (int i = 0; i < 100; i++)
            {
                Console.Write("\n");
            }*/
            Console.Write("-- C# Running Key Workspace --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");
            Console.Write("Using Alphabet: " + CipherLib.RunningKey.ALPHABET);
            Console.Write("\n\nIndex Of 'A': " + aIndex.ToString());
            Console.Write("\n\nDecryption Type: ");

            if (decryptType == CipherLib.DecryptionType.subtraction)
            {
                Console.Write("Subtraction");
            }
            else
            {
                Console.Write("Addition");
            }

            Console.Write("\n\n-----------------------\n\n");

            string decipherment = CipherLib.RunningKey.DecryptRunningKeyPartial(msg, key, decryptType, aIndex);

            //int lowerBound = startIndex;
            int lowerBound = 0;
            //int upperBound = startIndex + Console.WindowWidth;
            //int upperBound = startIndex + Console.WindowWidth - 5;
            int upperBound = msg.Length;

            if (upperBound > msg.Length)
            {
                lowerBound -= upperBound - msg.Length;
                upperBound = msg.Length;
            }
            if (lowerBound < 0)
            {
                upperBound -= lowerBound;
                lowerBound = 0;
            }

            msg = msg.ToUpper();

            //bool placedUnderscore = false;
            //int leftIndex = 0;            

            //Console.Write("     " + startIndex);
            //Console.Write("IND: " + startIndex);
            Console.Write("IND: " + 0);
            //for (int i = ("IND: " + startIndex).Length; i < Console.WindowWidth - (Console.WindowWidth - 5 + startIndex).ToString().Length; i++)
            //for (int i = ("IND: " + startIndex).Length; i < msg.Length + 5 - (msg.Length - 1).ToString().Length; i++)
            for (int i = ("IND: " + 0).Length; i < msg.Length + 5 - (msg.Length - 1).ToString().Length; i++)
            //for (int i = ("IND: " + 0).Length; i < msg.Length + 5 - (msg.Length - 1).ToString().Length - 1; i++)
            {
                //if (i - 4 + (Math.Ceiling(((decimal)i - 5) / 50) * 50).ToString().Length == Math.Ceiling(((decimal)i - 5) / 50) * 50)
                //if (i - 5 + (Math.Ceiling(((decimal)i - 5) / 50) * 50).ToString().Length == Math.Ceiling(((decimal)i - 5) / 50) * 50)
                //if (i - 4 + (Math.Ceiling(((decimal)i - 4) / 50) * 50).ToString().Length == Math.Ceiling(((decimal)i - 4) / 50) * 50)
                //if (i - 4 + (Math.Ceiling(((decimal)i - 5) / 50) * 50 - 1).ToString().Length == Math.Ceiling(((decimal)i - 5) / 50) * 50 - 1)
                if (i - 5 + (Math.Ceiling(((decimal)i - 5) / 50) * 50).ToString().Length - 1 == Math.Ceiling(((decimal)i - 5) / 50) * 50)
                {
                    //Console.Write("  ");
                    //Console.Write(" ");
                    Console.Write((Math.Ceiling(((decimal)i - 5) / 50) * 50).ToString());
                    //Console.Write((Math.Ceiling(((decimal)i - 5) / 50) * 50 - 1).ToString());
                    //i += (Math.Ceiling(((decimal)i - 5) / 50) * 50).ToString().Length;
                    //i += (Math.Ceiling(((decimal)i - 5) / 50) * 50 - 1).ToString().Length;
                    i += (Math.Ceiling(((decimal)i - 5) / 50) * 50).ToString().Length - 1;
                }
                else
                {
                    Console.Write(" ");
                }                
            }
            //Console.Write(Console.WindowWidth - 5 + startIndex);
            Console.Write(msg.Length - 1);
            //Console.Write("\n\n");
            Console.Write("\n");
            Console.Write("CIP: ");
            for (int i = lowerBound; i < upperBound; i++)
            {
                Console.Write(msg[i]);
            }
            Console.Write("\n");
            Console.Write("KEY: ");            
            for (int i = lowerBound; i < upperBound; i++)
            {
                if (i < key.Length)
                {
                    Console.Write(key[i]);
                }
                else
                {
                    /*if (editingKey && !placedUnderscore)
                    {
                        Console.Write("_");
                        placedUnderscore = true;
                    }
                    else
                    {
                        Console.Write(".");
                    }*/
                    Console.Write(".");
                }
                /*if (editingKey && i == key.Length - 1)
                {
                    Console.SetCursorPosition(4, 4 + i);
                }*/
            }
            Console.Write("\n");
            Console.Write("PLA: ");
            for (int i = lowerBound; i < upperBound; i++)
            {
                if (i < decipherment.Length)
                {
                    Console.Write(decipherment[i]);
                }
                else
                {
                    /*if (!editingKey && !placedUnderscore)
                    {
                        Console.Write("_");
                        placedUnderscore = true;
                    }
                    else
                    {
                        Console.Write(".");
                    }*/
                    Console.Write(".");
                }
            }
            Console.Write("\n\n");
            Console.Write("\n\n");
            //Console.Write("BACKSPACE:\t\tDelete character");
            Console.Write("BACKSPACE:\t\tDelete previous character");
            Console.Write("\n");
            //Console.Write("DELETE:\t\t\tDelete next character");
            Console.Write("DELETE:\t\t\tDelete current character");
            Console.Write("\n");
            //Console.Write("SPACE:\t\tAdd a space");
            //Console.Write("SPACE:\t\t\tAdd a space");
            //Console.Write("\n");
            Console.Write("UP ARROW:\t\tChange to editing key");
            Console.Write("\n");
            //Console.Write("Down ARROW:\t\tChange to editing plaintext");
            Console.Write("DOWN ARROW:\t\tChange to editing plaintext");
            Console.Write("\n");
            //Console.Write("LEFT/RIGHT ARROW:\tMove view");
            Console.Write("LEFT/RIGHT ARROW:\tMove cursor");
            Console.Write("\n");
            Console.Write("HOME/END:\t\tView beginning/end of text");
            Console.Write("\n");
            Console.Write("PLUS/MINUS:\t\tShift key/plaintext");
            Console.Write("\n");
            Console.Write("TAB:\t\t\tSlide crib");
            Console.Write("\n");
            //Console.Write("PAGE UP/PAGE DOWN:\t\t\tChange index of 'a'");
            //Console.Write("PAGE UP/PAGE DOWN:\t\tChange index of 'a'");
            Console.Write("PAGE UP/PAGE DOWN:\tChange index of 'a'");
            Console.Write("\n");
            //Console.Write("DELETE:\t\t\tChange decryption type");
            //Console.Write("PAUSE BREAK:\t\tChange decryption type");
            Console.Write("ASTERISK:\t\tChange decryption type");
            Console.Write("\n");
            //Console.Write("CTRL + DELETE:\tClear key/plaintext (asks for confirmation)");
            Console.Write("CTRL + DELETE:\t\tClear key/plaintext (asks for confirmation)");
            Console.Write("\n");
            //Console.Write("CTRL + M:\t\tMoved cursor to entered index");
            //Console.Write("CTRL + I:\t\tMoved cursor to entered index");
            Console.Write("CTRL + I:\t\tMove cursor to entered index");
            //Console.Write("\n\n");
            Console.Write("\n");
            Console.Write("CTRL + R:\t\tReload the display (doesn't delete the text; it is to use if you resize the window and the display goes all weird)");
            Console.Write("\n");
            Console.Write("CTRL + P:\t\tPaste entered text");

            Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight);
            //Console.SetWindowPosition(key.IndexOf('.'), Console.WindowTop);

            /*for (int i = 0; i < key.Length; i++)
            {
                if (i > 0 && key[i] == '.' && CipherLib.Annealing.IsEnglishLetter(key[i - 1]))
                {
                    leftIndex = i;
                    break;
                }
            }*/
            //Console.SetWindowPosition(leftIndex, Console.WindowTop);
            //Console.SetWindowPosition(key.Length, Console.WindowTop);

            //int leftIndex = 0;            
            Console.SetBufferSize(msg.Length + 5 + 1, Console.WindowHeight);
            //Console.SetCursorPosition(5 + startIndex, 16);
            if (editingKey)
            {
                Console.SetCursorPosition(5 + typeIndex, 13);
            }
            else
            {
                Console.SetCursorPosition(5 + typeIndex, 14);
            }
            Console.SetWindowPosition(leftIndex, Console.WindowTop);
        }

        private static string SlideCrib(string msg, string key, CipherLib.DecryptionType decryptType, int aIndex)
        {
            while (key[0] == '.')
            {
                key = ShiftTextLeft(key, 1);
            }
            for (int i = key.Length - 1; key.Length < msg.Length; i++)
            {
                key += '.';
            }
            //Console.Write("\n" + key);
            string bestKey = key;
            float bestScore = CipherLib.Annealing.QuadgramScore(CipherLib.RunningKey.DecryptRunningKeyPartial(msg, key, decryptType, aIndex));

            string newKey = key;
            float newScore;

            //string msgSection;

            //for (int i = 1; i + key.Length <= msg.Length; i++)
            for (int i = 0; i < msg.Length; i++)
            {
                //Console.Write("\n" + i);
                //newKey = "." + newKey;
                //newKey = newKey.Remove(newKey.Length - 1, 1);
                newKey = ShiftTextRight(newKey, 1);

                //Console.Write("\n" + newKey);

                /*msgSection = "";
                for (int j = i; j < i + key.Length; j++)
                {
                    msgSection += msg[j];
                }

                newScore = CipherLib.Annealing.QuadgramScore(CipherLib.RunningKey.DecryptRunningKeyPartial(msgSection, key, decryptType, aIndex));*/

                newScore = CipherLib.Annealing.QuadgramScore(CipherLib.RunningKey.DecryptRunningKeyPartial(msg, newKey, decryptType, aIndex));

                if (newScore > bestScore)
                {
                    bestScore = newScore;
                    bestKey = newKey;
                }
            }

            //Console.Write("\n" + bestKey);
            //Console.Read();

            return bestKey;
        }

        private static string ShiftTextRight(string text, int amount = 1)
        {
            for (int i = text.Length - 1; i >= amount; i--)
            {
                text = CipherLib.Annealing.SetChar(text[i - amount], i, text);
            }
            for (int i = 0; i < amount; i++)
            {
                text = CipherLib.Annealing.SetChar('.', i, text);
            }
            return text;
        }

        private static string ShiftTextLeft(string text, int amount = 1)
        {
            for (int i = 0; i < text.Length - amount; i++)
            {
                text = CipherLib.Annealing.SetChar(text[i + amount], i, text);
            }
            for (int i = text.Length - amount; i < text.Length; i++)
            {
                text = CipherLib.Annealing.SetChar('.', i, text);
            }
            return text;
        }

        private static void DisplayWorkspace(int typeIndex, bool editingKey, string msg, string key, int aIndex, CipherLib.DecryptionType decryptType)
        {
            //int leftIndex = Console.WindowLeft;

            string decipherment = CipherLib.RunningKey.DecryptRunningKeyPartial(msg, key, decryptType, aIndex);

            int lowerBound = 0;
            int upperBound = msg.Length;

            //msg = msg.ToUpper();

            /*Console.SetCursorPosition(5, 12);
            for (int i = lowerBound; i < upperBound; i++)
            {
                Console.Write(msg[i]);
            }*/
            Console.SetCursorPosition(5, 13);
            for (int i = lowerBound; i < upperBound; i++)
            {
                if (i < key.Length)
                {
                    Console.Write(key[i]);
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.SetCursorPosition(5, 14);
            for (int i = lowerBound; i < upperBound; i++)
            {
                if (i < decipherment.Length)
                {
                    Console.Write(decipherment[i]);
                }
                else
                {
                    Console.Write(".");
                }
            }        

            if (editingKey)
            {
                Console.SetCursorPosition(5 + typeIndex, 13);
            }
            else
            {
                Console.SetCursorPosition(5 + typeIndex, 14);
            }
            //Console.SetWindowPosition(leftIndex, Console.WindowTop);
        }
    }
}
