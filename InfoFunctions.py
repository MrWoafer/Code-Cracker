from Alphabet import *
#import re

MORSE_PROSIGN_MEANINGS = {"<aa>": "New line",
                          "<ar>": "End of message",
                          "<as>": "Wait",
                          "<bk>": "Break",
                          "<bt>": "New paragraph",
                          "<cl>": "Going of air (clear)",
                          "<ct>": "Start copying",
                          "<do>": "Change to wabun code",
                          "<kn>": "Invite a specific station to transmit",
                          "<sk>": "End of transmission",
                          "<sn>": "Understood (also <ve>)",
                          "<ve>": "Understood (also <sn>)",
                          "<sos>": "Distress message"}

MORSE_ABBREVIATIONS = {"K": "Over",
                       "R": "Roger",
                       "CUL": "See you later",
                       "BCNU": "Be seeing you",
                       "UR": "You're",
                       "RST": "Signal report",
                       "73": "Best regards",
                       "88": "Love and kisses"}

MORSE_Q_CODES = {"QSL": "I acknowledge receipt",
                 "QSL?": "Do you acknowledge?",
                 "QRX": "Wait",
                 "QRX?": "Should I wait?",
                 "QRV": "I am ready to copy",
                 "QRV?": "Are you ready to copy?",
                 "QRL": "The frequency is in use",
                 "QRL?": "Is the frequency in use?",
                 "QTH": "My location is?",
                 "QTH?": "What is your location?"}
                      

def GetMorseInfo(page = 0):

    if page == 0:

        info = "Morse:\n------\n\n"

        for x in MORSE:

            #if "." not in x and "-" not in x:
            if "." not in x and "-" not in x and x != "":

                info = info + x + "\t\t" + MORSE[x] + "\n"

        info = info + "\nProsigns:\n---------\n\n"

        for x in MORSE_PROSIGN_MEANINGS:

            info = info + x + "\t\t" + MORSE_PROSIGN_MEANINGS[x] + "\n"

        info = info + "\nAbbreviations:\n---------\n\n"

        for x in MORSE_ABBREVIATIONS:

            info = info + x + "\t\t" + MORSE_ABBREVIATIONS[x] + "\n"

        info = info + "\nQ Codes:\n---------\n\n"

        for x in MORSE_Q_CODES:

            info = info + x + "\t\t" + MORSE_Q_CODES[x] + "\n"

        info = info.rstrip("\n")

        pageTitle = "Translation Table"

    else:

        #return None
        return None, None

    #return info
    return pageTitle, info


def GetVICInfo(page = 0):

    if page == 0:

        #return open("-VIC Cipher Info-.txt", "r").read()
        #return "Encryption", open("-VIC Cipher Info-.txt", "r").read()
        return "Encryption", open("Info Folder\\-VIC Cipher Info-.txt", "r").read()

    else:

        #return None
        return None, None


WEBSITE_LINE_GAP = 2

def GetWebsitesInfo(page = 0):

    info = ""

    if page == 0:

        #sites = {"How to encipher VIC": "http://www.quadibloc.com/crypto/pp1324.htm",
        sites = {"Letters with accents": "http://usefulshortcuts.com/alt-codes/accents-alt-codes.php",
                 "Morse code table": "https://morsecode.scphillips.com/morse2.html",
                 #"Letters with accents": "http://usefulshortcuts.com/alt-codes/accents-alt-codes.php"}
                 #"How to encipher VIC": "http://www.quadibloc.com/crypto/pp1324.htm",
                 #"How to encipher VIC": "https://everything2.com/user/raincomplex/writeups/VIC+cipher"}
                 "How to encipher VIC": ["http://www.quadibloc.com/crypto/pp1324.htm", "https://everything2.com/user/raincomplex/writeups/VIC+cipher"],
                 "Cipher Cracking And Info": "http://www.practicalcryptography.com/",
                 "Encryption / Cipher Lists:": "https://www.dcode.fr/en"}

        for x in sites:

            info = info + x + ":\n" + "-" * (len(x) + 1) + "\n"

            if isinstance(sites[x], list):

                for y in sites[x]:

                    info = info + y + "\n"

            else:

                #info = info + x + ":\n" + "-" * (len(x) + 1) + "\n" + sites[x] + "\n\n"
                #info = info + x + ":\n" + "-" * (len(x) + 1) + "\n" + sites[x] + "\n" + "\n" * WEBSITE_LINE_GAP
                info = info + sites[x] + "\n"

            info = info + "\n" * WEBSITE_LINE_GAP

        info = info.rstrip("\n")

        pageTitle = "General Sites"

    else:

        #return None
        return None, None

    #return info
    return pageTitle, info


def MatchToClosestString(string, matchStrings):

    #string = string.lower()

    #for i in range(0, len(matchStrings)):

        #matchStrings[i] = matchStrings[i].lower()

    for x in matchStrings:

        if string.lower() in x.lower():

            return x

    lowestLevenshValue = LevenshteinMetric(string, matchStrings[0])
    lowestLevenshStr = matchStrings[0]

    for x in matchStrings[1:]:

        newLevenshVal = LevenshteinMetric(string, x)

        if newLevenshVal < lowestLevenshValue:

            lowestLevenshStr = x

            lowestLevenshValue = newLevenshVal

    return lowestLevenshStr

    return None


def LevenshteinMetric(string1, string2):

    string1 = string1.lower()
    string2 = string2.lower()

    n = len(string1)
    m = len(string2)

    if n == 0:
        return m
    if m == 0:
        return n

    d = [[None for x in range(0, m+1)] for x in range(0, n+1)]

    for i in range(0, n+1):

        d[i][0] = i

    for j in range(0, m+1):

        d[0][j] = j

    for i in range(1, n+1):

        for j in range(1, m+1):

            #cost = 1 if string2[j-1] == string1[i-1] else 0
            cost = 0 if string2[j-1] == string1[i-1] else 1

            #print (i, j)
            #print (d)

            #d[i, j] = min(d[i-1][j] + 1, d[i][j-1] + 1, d[i-1][j-1] + cost)
            d[i][j] = min(d[i-1][j] + 1, d[i][j-1] + 1, d[i-1][j-1] + cost)

    return d[n][m]


def GetWSACCInfo(page = 0):

    info = ""

    if page == 0:

        info = "Press Ctrl+F to view a frequency chart of the text in the input box."
        info = info + "\n\nCtrl+UpArrow to move text from the output box into the input box."
        info = info + "\n\nCtrl+DownArrow to move text from the input box into the output box."

        pageTitle = "Keyboard shortcuts"

    else:

        return None

    info = info.rstrip("\n")

    return pageTitle, info


def GetLevenshInfo(page = 0):

    info = ""

    if page == 0:

        info = info + "Wikipedia:\n----------\n\nhttps://en.wikipedia.org/wiki/Levenshtein_distance\n\n\n"
        info = info + "Implementation:\n---------------\n\nhttps://stackoverflow.com/questions/13793560/find-closest-match-to-input-string-in-a-list-of-strings\n\n\n"

        pageTitle = "Info web links"

    else:

        return None

    info = info.rstrip("\n")

    return pageTitle, info


def GetEnigmaInfo(page = 0):

    info = ""

    if page == 0:

        file = open("Info Folder\\Enigma Rotors.txt", "r").readlines()

        for line in file:

            #info = info + "\t\t".join(column for column in line.split(";"))
            info = info + "\t\t".join(column for column in line.split(";")) + "\n"
            #info = info + "\t".join(column for column in line.split(";"))
            #info = info + "\t".join(column for column in line.split(";")) + "\n"

        info = info + "\n(From https://en.wikipedia.org/wiki/Enigma_rotor_details)"

        pageTitle = "Rotors & Reflectors"

    elif page == 1:

        info = "Whenever a rotor is in the position where its notch is ready to cause a turn in the next rotor, it will turn with the next press regardless of whether the previous rotor turns.\n\n\
E.g. let's have Rotor I, II, and III (from left to right with the rightmost being the fastest rotor), starting in position ADU. Rotor III will progress to give ADV, then AEW (as the move from V to W on Rotor III causes it to turn\
the next rotor). But, Rotor II is now touching its notch (as it is in position E, and the move from E to F causes the next rotor to turn). So, even though the next Rotor III move will not cause Rotor II to turn, Rotor II will turn\
anyway because it's touching the notch. So, on the next button press this gives BFX (Rotor II's movement past the notch causes Rotor I to turn as well). Then we continue to get BFY as Rotor III turns."

        info = info + "\n\nIn simple steps:\n\n\
(starting position)\n\
ADU \n\
(Rotor III's normal movement)\n\
ADV\n\
(Rotor III passes notch, causing Rotor II to turn)\n\
AEW\n\
(Rotor II is touching its notch, so it moves autonomously, causing Rotor III to turn. Rotor III does its normal movement)\n\
BFX\n\
(Rotor III's normal movement)\n\
BFY"

        source = "https://en.wikipedia.org/wiki/Enigma_rotor_details"

        #info = info + "\n\n" + source
        info = info + "\n\n(Adapted from: " + source + ")"

        pageTitle = "Double-Stepping"

    return pageTitle, info


def GetInfo(infoToLoad, page):

    info = None

    if infoToLoad == "Morse Code":

        pageTitle, info = GetMorseInfo(page)

    elif infoToLoad == "VIC Cipher":

        pageTitle, info = GetVICInfo(page)

    elif infoToLoad == "Websites":

        pageTitle, info = GetWebsitesInfo(page)

    elif infoToLoad == "William's Super Awesome Code Creaker":

        pageTitle, info = GetWSACCInfo(page)

    elif infoToLoad == "Levenshtein Metric":

        pageTitle, info = GetLevenshInfo(page)

    elif infoToLoad == "Enigma":

        pageTitle, info = GetEnigmaInfo(page)

    elif infoToLoad == "Double Transposition":

        pageTitle = "Instructions"

        info = "The top box is the transposition that was performed first in encryption.\n\nEnter the type of transposition and the length of the transposition key (number of columns)."

    elif infoToLoad == "ASCII":

        pageTitle = "ASCII Table"

        info = ""

        for i in range(0, 128):

            if i == 32:

                info = info + str(i) + "\t" + "\\s" + "\n"

            else:
                
                #info = info + str(i) + "\t" + chr(i) + "\n"
                #info = info + str(i) + "\t" + chr(i).replace("\\", "\\\\") + "\n"
                #info = info + str(i) + "\t" + re.escape(chr(i)) + "\n"
                #info = info + str(i) + "\t" + repr(chr(i)) + "\n"
                info = info + str(i) + "\t" + repr(chr(i))[1:-1] + "\n"

        info = info.rstrip("\n")

    elif infoToLoad == "ADFGVX":

        pageTitle = "Using the creakers"

        info = "I would recommend using the solver for keys of length >= 9, and the bruter for keys of length < 9. The reason is that the bruter is perhaps more likely to get the correct key, but is only tractable in time up to and \
including 8 columns.\n\nAlso, perhaps run the solver and bruter twice or thrice as it might not always get the right decryption for the mono-sub first time round.\n\nI'm also not sure if the solver will work for keys longer than 8.\
\n\nSo, having said that, the bruter does complete a key of length 9 in about 20 minutes, so is feasible. And, the solver doesn't really work for keys longer than 8. Scrap that, the solver literally just solved a key of length 9. It\
did so on trial 6.\n\nIt appears, from empirical data, that for odd-length keys the bruter will only identify 1 possible key. For even keys, it identifies many, but - according to my findings - it is a feasible number."

    elif infoToLoad == "Running Key":

        pageTitle = "Distribution of letters"

        #info = "Letter\t\tExpected frequency\n------\t\t--------------------\n"
        info = "Letter\t\tExpected frequency\n------\t\t------------------\n"

        for i in range(0, len(alphabetlist)):

            info = info + alphabetlist[i] + "\t\t" + str(runningkeyletterfrequencies[i]) + "\n"

        info = info + "\nIOC: " + str(runningkeyioc)

        info = info.rstrip("\n")

    elif infoToLoad == "Homophonic Substitution":

        if page == 0:

            pageTitle = "Using the program(s)"

            info = "Use the workspace to manually decrypt/encrypt, and to correct solutions. Enter the ciphertext into the normal input box, then enter the key in the designated key box.\n\n"
            info = info + "Format the key in lines, with the ciphertext symbol, then a space, then the plaintext symbol. E.g.:\n\n00 z\n01 a\n02 c\n\netc."
            info = info + "---\n\nThere are two solvers: a quick one (a few seconds), and a longer one (20-30 mins). Use the quick one to see if the ciphertext is likely to be a homo-sub; any resemblance of English is indicative of\
it being a homo-sub. The quick solver should get over 80% of the ciphertext correct for texts of ~5000 characters and ~50 symbols.\n\nIf the ciphertext is shorter and you cannot complete the quick solution manually, then use the long/\
thorough solver. It will take much longer, but should get over 80% no matter what. The longer the text and the more symbols, the longer it will take to run. It takes ~25 mins for 1800-chars & 52 symbols, and ~15 mins for 2000-chars &\
 43 symbols."

            info = info.rstrip("\n")

    elif infoToLoad == "ABCDEFGHIK":

        if page == 0:

            pageTitle = "What is it?"

            info = "You probably haven't heard of this cipher before, because I invented the name. Essentially, it's an ADFG(V)X but the Polybius grid doesn't have to have the same alphabet for the rows as it does for the columns.\
\n\nThe solver works by not even using the Polybius grid. It just turns each unique digram into a unique letter, then solves it like a mono-sub."

    elif infoToLoad == "Letter Frequencies":

        if page == 0:

            pageTitle = "English letters in order of frequency"

            info = ""

            #for i in orderedlettersonaveragefrequencies:

                #info = info + i + "\n"

            for i in range(0, len(orderedlettersonaveragefrequencies)):

                info = info + str(i) + ":\t" + orderedlettersonaveragefrequencies[i] + "\n"

            info = info.strip()

    #return info, pageTitle
    return pageTitle, info
