def FindPotentialWords(Msg, Dictionary, AppendString):

    Words = []

    for word in Dictionary:

        if len(word) == len(Msg):

            Match = True

            for letter in range(0, len(word)):

                if Match and word[letter] != Msg[letter] and Msg[letter] != "_":

                    Match = False

            if Match:

                #print (word + AppendString)

                Words.append(word + AppendString)

    return Words
