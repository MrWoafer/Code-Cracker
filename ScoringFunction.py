def LoadQuadgramFrequencyTable(ListOrDictionary, CountUpTo):

    List = open("Quadgram Frequencies.txt", "r").read()

    if ListOrDictionary == "List":

        FrequencyTable = []

    elif ListOrDictionary == "Dictionary":

        FrequencyTable = {}

    TotalFreq = 0

    CurrentQuadgram = ""

    CurrentNumber = ""

    NumberOfQuadgrams = 0

    for character in List:

        if CountUpTo == "All" or NumberOfQuadgrams < CountUpTo:

            letter = character.lower()

            if letter in alphabetset:

                if CurrentNumber != "":

                    if ListOrDictionary == "List":

                        FrequencyTable.append([CurrentQuadgram, int(CurrentNumber)])

                    elif ListOrDictionary == "Dictionary":

                        FrequencyTable[CurrentQuadgram] = int(CurrentNumber)

                    TotalFreq += int(CurrentNumber)
                    
                    CurrentQuadgram, CurrentNumber = letter, ""

                    NumberOfQuadgrams += 1

                else:

                    CurrentQuadgram = CurrentQuadgram + letter

            elif letter == " ":

                pass

            elif letter in digitset:

                CurrentNumber = CurrentNumber + letter


    for item in FrequencyTable:

        if ListOrDictionary == "List":

            item[1] = item[1]*100/TotalFreq

        if ListOrDictionary == "Dictionary":

            FrequencyTable[item] = FrequencyTable[item]*100/TotalFreq

    if ListOrDictionary == "List":

        FrequencyTable.insert(0, TotalFreq)

    if ListOrDictionary == "Dictionary":

        FrequencyTable["Total"] = TotalFreq

    return FrequencyTable



def QuadgramScore(Msg, QuadgramFrequencies):

    NumberOfQuadgrams = len(QuadgramFrequencies)

    Msg = RemoveSpaces(RemovePunctuation(Msg))

    Score = 0

    Quadgram = ""

    UsedQuadgrams = set()

    for letter in range(0, len(Msg)):
            
        if letter <= len(Msg)-4:

            Quadgram = Msg[letter:letter+4]

            if Quadgram in QuadgramFrequencies and Quadgram not in UsedQuadgrams:

                Score += log10(QuadgramFrequencies[Quadgram]/100)

            else:

                Score += log10(0.01/QuadgramFrequencies["Total"])

    return Score
