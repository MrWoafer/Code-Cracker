#include "ChiSquare.h"
#include "LetterProbabilities.h"
#include <math.h>
#include <iostream>
#include <fstream>
#include <stdlib.h>
#include <math.h>
#include <string>

using namespace std;

extern double LetterProbs[];

double ChiSquare(string Msg)
{
	double Score;
	Score = 0;
	
	int LetterFreq[26];
	
	int i;
	
	for (i = 0; i < 26; i++){
		LetterFreq[i] = 0;
	}
	
	for (i = 0; i < Msg.length(); i++){
		LetterFreq[Msg[i]-97] += 1;
	}
	
	for (i = 0; i < 26; i++){
		Score += (pow(LetterFreq[i] - (LetterProbs[i]*Msg.length()), 2))/(LetterProbs[i]*Msg.length());
	}
	
	return Score;
}