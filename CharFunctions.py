#def CountChars(text):
def CountChars(text, prePopulatedCharacters = ""):

    counts = {}

    #for i in prePopulatedCharacters:
    for i in reversed(prePopulatedCharacters): ### It's reversed so that the closer to the front in prePopulatedCharacters you are, the higher up on the list you will be in groups of characters with the same count

        counts[i] = 0

    for i in range(0, len(text)):

        if text[i] in counts:

            counts[text[i]] += 1

        else:

            counts[text[i]] = 1

    return counts

def CharIOC(text):

##    counts = CountChars(text)
##
##    total = 0
##    ioc = 0
##
##    for i in counts:
##
##        ioc += counts[i] * (counts[i] - 1)
##        total += counts[i]
##
##    return ioc / (total * (total - 1))

    return CharNGramIOC(text, 1)

def CountCharNGrams(text, n):

    counts = {}

    i = 0
    while i < len(text) - n + 1:

        ngram = text[i:i+n]

        if ngram in counts:

            counts[ngram] += 1

        else:

            counts[ngram] = 1

        i += 1

    return counts


def CharNGramIOC(text, n):

    counts = CountCharNGrams(text, n)

##    total = 0
##    ioc = 0
##
##    for i in counts:
##
##        ioc += counts[i] * (counts[i] - 1)
##        total += counts[i]
##
##    return ioc / (total * (total - 1))

    return CalculateIOCFromCounts(counts)

def CountCharNGramsBlocked(text, n):

##    counts = {}
##
##    i = 0
##    while i < len(text) - n + 1:
##
##        ngram = text[i:i+n]
##
##        if ngram in counts:
##
##            counts[ngram] += 1
##
##        else:
##
##            counts[ngram] = 1
##
##        i += n
##
##    return counts

    return CountCharNGramsStepped(text, n, n)

def CharNGramIOCBlocked(text, n):

    counts = CountCharNGramsBlocked(text, n)

    return CalculateIOCFromCounts(counts)

def CalculateIOCFromCounts(counts):

    total = 0
    ioc = 0

    for i in counts:

        ioc += counts[i] * (counts[i] - 1)
        total += counts[i]

    return ioc / (total * (total - 1))


def CountCharNGramsStepped(text, n, step, startIndex = 0):

    counts = {}

    i = startIndex
    while i < len(text) - n + 1:

        ngram = text[i:i+n]

        if ngram in counts:

            counts[ngram] += 1

        else:

            counts[ngram] = 1

        i += step

    return counts

def CharNGramIOCStepped(text, n, step):

    counts = CountCharNGramsStepped(text, n, step)

    return CalculateIOCFromCounts(counts)

def CharIOCStepped(text, step):

    return CharNGramIOCStepped(text, 1, step)
