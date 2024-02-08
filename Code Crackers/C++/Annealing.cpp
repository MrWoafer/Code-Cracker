//#include "Annealing.hpp"
#include "Annealing.h"
//#include "QuadgramScores.hpp"
#include "QuadgramScores.h"
#include <math.h>
#include <iostream>
#include <fstream>
#include <stdlib.h>
#include <math.h>
#include <string>

using namespace std;

//float QuadgramScores[];
//extern float QuadgramScores[];
extern double QuadgramScores[];

//float QuadgramScore(string Msg)
double QuadgramScore(string Msg)
{
	//return 1;
	////cout << "Letter 1: " << Msg[0] << " " << (int)Msg[0] << "\n";
	//int Score;
	//float Score;
	double Score;
	Score = 0;
	int i;
	//string Letter;
	////string Letter1;
	////string Letter2;
	////string Letter3;
	////string Letter4;
	//char Letters[4];
	int Letters[4];
	//for (i = 0; i < Msg.length()-3; i += 4){
	for (i = 0; i < Msg.length()-3; i ++){
		//const char * Letter1 = Msg[i].c_str();
		//const char * Letter1 = Msg.at(i).c_str();
		//const char Letter1 = Msg[i];
		//const char *Letter1 = Msg[i].c_str();
		//const char *Letter1 = Msg[i].c_str()[i];
		//int Alpha1 = AlphabetIndex(Letter1);
		//int Alpha1 = AlphabetIndex(str(Msg1[i]));
		//Letter = Msg1[i];
		//Letter = Msg[i];
		////Letter1 = Msg[i];
		////Letter2 = Msg[i+1];
		////Letter3 = Msg[i+2];
		////Letter4 = Msg[i+3];
		//Letters[0] = Msg[i] - 'a';
		//Letters[1] = Msg[i+1] - 'a';
		//Letters[2] = Msg[i+2] - 'a';
		//Letters[3] = Msg[i+3] - 'a';
		//Letters[0] = Msg[i] - 87;
		//Letters[1] = Msg[i+1] - 87;
		//Letters[2] = Msg[i+2] - 87;
		//Letters[3] = Msg[i+3] - 87;
		//Letters[0] = (int)Msg[i] - 87;
		//Letters[1] = (int)Msg[i+1] - 87;
		//Letters[2] = (int)Msg[i+2] - 87;
		//Letters[3] = (int)Msg[i+3] - 87;
		//cout << Letters[0] << "," << Letters[1] << "," << Letters[2] << "," << Letters[3] << "\n";
		//cout << Msg[i] << "," << Msg[i+1] << "," << Msg[i+2] << "," << Msg[i+3] << "\n";
		//Letters[0] = (int)Msg[i] - 97;
		//Letters[1] = (int)Msg[i+1] - 97;
		//Letters[2] = (int)Msg[i+2] - 97;
		//Letters[3] = (int)Msg[i+3] - 97;
		//Letters[0] = AlphabetIndexMsg(i);
		//Letters[1] = AlphabetIndexMsg(i+1);
		//Letters[2] = AlphabetIndexMsg(i+2);
		//Letters[3] = AlphabetIndexMsg(i+3);
		//Letters[0] = AlphabetIndex(Msg[i]);
		//Letters[0] = AlphabetIndex((str)Msg[i]);
		//Letters[0] = AlphabetIndex((string)Msg[i]);
		//Letters[1] = AlphabetIndex(Msg[i+1]);
		//Letters[2] = AlphabetIndex(Msg[i+2]);
		//Letters[3] = AlphabetIndex(Msg[i+3]);
		//cout << "Letter A: " << (char)char((char)'a'-97);
		//cout << Letters[0] << "," << Letters[1] << "," << Letters[2] << "," << Letters[3] << "\n";
		//cout << "\n\n\n";
		////cout << Letters[0] << "," << Letters[1] << "," << Letters[2] << "," << Letters[3] << "\n";
		//int Alpha1 = AlphabetIndex(Letter);
		//int Alpha1 = AlphabetIndex(Letter1);
		//int Alpha2 = AlphabetIndex(Letter2);
		//int Alpha3 = AlphabetIndex(Letter3);
		//int Alpha4 = AlphabetIndex(Letter4);
		////int Alpha1 = 5;
		////int Alpha2 = 5;
		////int Alpha3 = 5;
		////int Alpha4 = 5;
		//cout << Msg[i];
		//Score += Alpha1*26*26*26 + Alpha2*26*26 + Alpha3*26 + Alpha4;
		//int Index;
		//int Index += Alpha1*26*26*26 + Alpha2*26*26 + Alpha3*26 + Alpha4;
		//Index += Alpha1*26*26*26 + Alpha2*26*26 + Alpha3*26 + Alpha4;
		//int Index = Alpha1*26*26*26 + Alpha2*26*26 + Alpha3*26 + Alpha4;
		//Score += QuadgramScores[Index];
		////////Score += QuadgramScores[Letters[0]*26*26*26 + Letters[1]*26*26 + Letters[2]*26 + Letters[3]];
		//Score += QuadgramScores[Letters[0]*17576 + Letters[1]*676 + Letters[2]*26 + Letters[3]];
		//cout << (Msg[i+0]-97)*17576 + (Msg[i+1]-97)*676 + (Msg[i+2]-97)*26 + (Msg[i+3]-97) << "\n";
		if ((Msg[i+0]-97)*17576 + (Msg[i+1]-97)*676 + (Msg[i+2]-97)*26 + (Msg[i+3]-97) >= 0){
			Score += QuadgramScores[(Msg[i+0]-97)*17576 + (Msg[i+1]-97)*676 + (Msg[i+2]-97)*26 + (Msg[i+3]-97)];
		}
		//Score += 1;
	}
	//return 1.0;
	return Score;
}


//int AlphabetIndex(char Letter)
int AlphabetIndex(string Letter)
//int AlphabetIndex(const char Letter)
//int AlphabetIndex(char Letter)
{
	string Alphabet = "abcdefghijklmnopqrstuvwxyz";
	string NewLetter;
	int i;
	for ( i = 0; i < 26; i++){
		//if (Alphabet[i] == Letter){
		//if (Alphabeta.at(i) == Letter){
		//if (Alphabet.at(i) == Letter){
		//#if (str(Alphabet.at(i)) == Letter){
		NewLetter = Alphabet.at(i);
		if (NewLetter == Letter){
			return i;
		}
	}
	return 26;
}


bool AnnealingProbability(float Difference, float Temperature)
{
	float Probability = exp(Difference/Temperature);
	//float ProbabilityNum = rand()/RAND_MAX;
	//float ProbabilityNum = rand()/<float>RAND_MAX;
	float ProbabilityNum = rand()/(float)RAND_MAX;
	
	//cout << "Probabilities: " << Probability << " " << ProbabilityNum << "\n\n";
	
	if (Probability >= ProbabilityNum){
		//cout << "True\n";
		return true;
	}
	else {
		//cout << "False\n";
		return false;
	}
}