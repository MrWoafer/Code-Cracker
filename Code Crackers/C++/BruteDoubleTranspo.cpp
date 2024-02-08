#include <iostream>
#include <fstream>
#include <stdlib.h>
#include <math.h>
#include <string.h>
#include "Annealing.h"
#include <stdio.h>
#include <streambuf>
#include <algorithm>
#include <time.h>

const int MAXKEYLENGTH = 26;

void DisplayOrder(int ColumnOrder[MAXKEYLENGTH], int ColumnNum)
{
	int i;
	for (i = 0; i < ColumnNum; i++){
		cout << ColumnOrder[i]+1 << " ";
	}
}


void CopyDaKeys(int KeyToCopy[MAXKEYLENGTH], int (&KeyToCopyTo)[MAXKEYLENGTH], int KeyLength)
{
	int i;
	for (i = 0; i < KeyLength; i ++){
		KeyToCopyTo[i] = KeyToCopy[i];
	}
}


void DisplayRows(string Rows[], int RowNum, int ColumnNum)
{
	int i; int j;
	for (i = 0; i < RowNum; i ++){
		for (j = 0; j < ColumnNum; j++){
			if (j < Rows[i].length()){
				cout << Rows[i][j];
			}
		}
		cout << "\n";
	}
}


string Transpose(string Msg, bool RowOrColumn, int ColumnNum, int ColumnOrder[MAXKEYLENGTH])
{
	int RowNum;
	if (Msg.length()%ColumnNum == 0){
		RowNum = Msg.length()/ColumnNum;
	}
	else{
		RowNum = ceil(1 + (Msg.length()-(Msg.length()%ColumnNum))/ColumnNum);
	}
	string Rows[RowNum];
	int i;
	int j;
	string Row;
	if (RowOrColumn == false){
		for (i = 0; i < RowNum; i++){
			Row = "";
			for (j = 0; j < ColumnNum; j++){
				if (i*ColumnNum+j < Msg.length()){
					Row += Msg[i*ColumnNum+j];
				}
			}
			Rows[i] = Row;
		}
	}
	
	else {
		for (i = 0; i < RowNum; i++){
			Row = "";
			for (j = 0; j < ColumnNum; j++){
				if (j*RowNum+i < Msg.length()){
					Row += Msg[j*RowNum+i];
				}
			}
			Rows[i] = Row;
		}
	}

	string NewMsg = "";
	
	for (i = 0; i < RowNum; i++){
		for (j = 0; j < ColumnNum; j++){
			if (ColumnOrder[j] < Rows[i].length()){
				NewMsg += Rows[i][ColumnOrder[j]];
			}
		}
	}
	return NewMsg;
}


string DecodeDoubleTranspo(string Msg, bool TranspoType1, int TranspoLength1, int Order1[MAXKEYLENGTH], bool TranspoType2, int TranspoLength2, int Order2[MAXKEYLENGTH])
{
	string Decode;
	//Decode = Transpose(Msg, TranspoType2, TranspoLength2, TestKey2);
	//Decode = Transpose(Decode, TranspoType1, TranspoLength1, TestKey1);
	Decode = Transpose(Msg, TranspoType2, TranspoLength2, Order2);
	
	/*cout << "\n\n" << Decode << "\n\n";*/
	
	Decode = Transpose(Decode, TranspoType1, TranspoLength1, Order1);
	return Decode;
}


double Score(string Msg, bool TranspoType1, int TranspoLength1, int Order1[MAXKEYLENGTH], bool TranspoType2, int TranspoLength2, int Order2[MAXKEYLENGTH])
{
	string Decipherment = DecodeDoubleTranspo(Msg, TranspoType1, TranspoLength1, Order1, TranspoType2, TranspoLength2, Order2);
	
	double QuadScore;
	QuadScore = QuadgramScore(Decipherment);
	
	return QuadScore;
}


void SwapColumns(int ColumnNum, int (&ColumnOrder)[MAXKEYLENGTH])
{
	int Index1 = rand()%ColumnNum;
	int Index2 = rand()%ColumnNum;
	while (Index1 == Index2){
		Index2 = rand()%ColumnNum;
	}
	
	int Temp = ColumnOrder[Index1];
	ColumnOrder[Index1] = ColumnOrder[Index2];
	ColumnOrder[Index2] = Temp;
}


void ModifyKey(int (&Order1)[MAXKEYLENGTH], int TranspoLength1, int (&Order2)[MAXKEYLENGTH], int TranspoLength2)
{
	int Manoeuvre = rand()%2;
	switch(Manoeuvre){
		case 1: SwapColumns(TranspoLength1, Order1);
		case 0: SwapColumns(TranspoLength2, Order2);
	}
}


//double TranspositionShotgun(string Msg, bool TranspoType1, int TranspoLength1, int (&Order1)[MAXKEYLENGTH], bool TranspoType2, int TranspoLength2, int (&Order2)[MAXKEYLENGTH], double Temperature, double Step, int Count)
double DoubleTranspositionAnnealing(string Msg, bool TranspoType1, int TranspoLength1, int (&Order1)[MAXKEYLENGTH], bool TranspoType2, int TranspoLength2, int (&Order2)[MAXKEYLENGTH], double Temperature, double Step, int Count)
{
	int CurrentOrder1[MAXKEYLENGTH];
	int CurrentOrder2[MAXKEYLENGTH];
	
	int BestOrder1[MAXKEYLENGTH];
	int BestOrder2[MAXKEYLENGTH];
	
	CopyDaKeys(Order1, CurrentOrder1, TranspoLength1);
	CopyDaKeys(Order2, CurrentOrder2, TranspoLength2);
	
	CopyDaKeys(CurrentOrder1, BestOrder1, TranspoLength1);
	CopyDaKeys(CurrentOrder2, BestOrder2, TranspoLength2);
	
	double BestScore;
	
	BestScore = Score(Msg, TranspoType1, TranspoLength1, BestOrder1, TranspoType2, TranspoLength2, BestOrder2);

	double CurrentScore;
	
	bool ReplaceAnyway;
	
	cout << "\n";
	
	/*int LoopNumber = 0;*/
	int count;
	//int T;
	float T;
	for (T = Temperature; T >= 0; T -= Step){
		cout << T << "\n";
		/*if (LoopNumber % 10 == 0){
			cout << "Loop " << LoopNumber << "\n";
		}*/
		for (count = 0; count < Count; count++){
			
			CopyDaKeys(BestOrder1, CurrentOrder1, TranspoLength1);
			CopyDaKeys(BestOrder2, CurrentOrder2, TranspoLength2);
				
			ModifyKey(CurrentOrder1, TranspoLength1, CurrentOrder2, TranspoLength2);
			
			CurrentScore = Score(Msg, TranspoType1, TranspoLength1, CurrentOrder1, TranspoType2, TranspoLength2, CurrentOrder2);
			
			if (CurrentScore > BestScore){
				
				BestScore = CurrentScore;
				CopyDaKeys(CurrentOrder1, BestOrder1, TranspoLength1);
				CopyDaKeys(CurrentOrder2, BestOrder2, TranspoLength2);
				
			}
			
			else if ((CurrentScore < BestScore) && (true)){
				ReplaceAnyway = AnnealingProbability(CurrentScore-BestScore, T);
				
				if (ReplaceAnyway == true){
					BestScore = CurrentScore;
					CopyDaKeys(CurrentOrder1, BestOrder1, TranspoLength1);
					CopyDaKeys(CurrentOrder2, BestOrder2, TranspoLength2);
					
				}
			}
		}
		//LoopNumber += 1
		/*LoopNumber += 1;*/
	}
	
	CopyDaKeys(BestOrder1, Order1, TranspoLength1);
	CopyDaKeys(BestOrder2, Order2, TranspoLength2);
	return BestScore;
}


double DoubleTranspositionHillClimb(string Msg, bool TranspoType1, int TranspoLength1, int (&Order1)[MAXKEYLENGTH], bool TranspoType2, int TranspoLength2, int (&Order2)[MAXKEYLENGTH], int Count)
{
	int CurrentOrder1[MAXKEYLENGTH];
	int CurrentOrder2[MAXKEYLENGTH];
	
	int BestOrder1[MAXKEYLENGTH];
	int BestOrder2[MAXKEYLENGTH];
	
	CopyDaKeys(Order1, CurrentOrder1, TranspoLength1);
	CopyDaKeys(Order2, CurrentOrder2, TranspoLength2);
	
	CopyDaKeys(CurrentOrder1, BestOrder1, TranspoLength1);
	CopyDaKeys(CurrentOrder2, BestOrder2, TranspoLength2);
	
	double BestScore;
	
	BestScore = Score(Msg, TranspoType1, TranspoLength1, BestOrder1, TranspoType2, TranspoLength2, BestOrder2);

	double CurrentScore;
	
	bool ReplaceAnyway;
	
	cout << "\n";
	
	/*int LoopNumber = 0;*/
	int count;
	for (count = 0; count < Count; count++){
		
		CopyDaKeys(BestOrder1, CurrentOrder1, TranspoLength1);
		CopyDaKeys(BestOrder2, CurrentOrder2, TranspoLength2);
			
		ModifyKey(CurrentOrder1, TranspoLength1, CurrentOrder2, TranspoLength2);
		
		CurrentScore = Score(Msg, TranspoType1, TranspoLength1, CurrentOrder1, TranspoType2, TranspoLength2, CurrentOrder2);
		
		if (CurrentScore > BestScore){
			
			BestScore = CurrentScore;
			CopyDaKeys(CurrentOrder1, BestOrder1, TranspoLength1);
			CopyDaKeys(CurrentOrder2, BestOrder2, TranspoLength2);
			
		}
		
	}
	
	CopyDaKeys(BestOrder1, Order1, TranspoLength1);
	CopyDaKeys(BestOrder2, Order2, TranspoLength2);
	return BestScore;
}


void SolveTransposition(string Msg, bool TranspoType1, int TranspoLength1, bool TranspoType2, int TranspoLength2)
{
	/*int i;
	for (i = 0; i < CurrentColumnNum; i++){
		CurrentColumnOrder[i] = i;
	}*/
	double BestScore;
	double CurrentScore;
	
	int CurrentOrder1[MAXKEYLENGTH];
	int CurrentOrder2[MAXKEYLENGTH];
	
	int i;
	for (i = 0; i < TranspoLength1; i++){
		CurrentOrder1[i] = i;
	}
	for (i = 0; i < TranspoLength2; i++){
		CurrentOrder2[i] = i;
	}
	
	//for (i =; i < 100; i++){
	for (i = 0; i < 100; i++){
		ModifyKey(CurrentOrder1, TranspoLength1, CurrentOrder2, TranspoLength2);
	}
	
	cout << "Starting Column Order 1: ";
	//DisplayOrder(BestOrder1, TranspoLength1);
	DisplayOrder(CurrentOrder1, TranspoLength1);
	cout << "\nStarting Column Order 2: ";
	//DisplayOrder(BestOrder2, TranspoLength2);
	DisplayOrder(CurrentOrder2, TranspoLength2);
	
	cin.get();
	
	//cout << "\n\n----------------\n\n";
	cout << "\n----------------\n\n";
	
	int BestOrder1[MAXKEYLENGTH];
	int BestOrder2[MAXKEYLENGTH];
	
	CopyDaKeys(CurrentOrder1, BestOrder1, TranspoLength1);
	CopyDaKeys(CurrentOrder2, BestOrder2, TranspoLength2);
	
	BestScore = Score(Msg, TranspoType1, TranspoLength1, BestOrder1, TranspoType2, TranspoLength2, BestOrder2);
	
	int trial;
	trial = 0;
	while(true){
		cout << "Trial: " << trial+1 << "\n\n";
		
		CopyDaKeys(BestOrder1, CurrentOrder1, TranspoLength1);
		CopyDaKeys(BestOrder2, CurrentOrder2, TranspoLength2);
		
		//for (i = 0; i < 100; i++){
		for (i = 0; i < 6; i++){
			ModifyKey(CurrentOrder1, TranspoLength1, CurrentOrder2, TranspoLength2);
		}
		
		cout << "Starting Column Order 1: ";
		DisplayOrder(CurrentOrder1, TranspoLength1);
		cout << "\nStarting Column Order 2: ";
		DisplayOrder(CurrentOrder2, TranspoLength2);
		cout << "\n";
		
		//CurrentScore = TranspositionShotgun(Msg, TranspoType1, TranspoLength1, CurrentOrder1, TranspoType2, TranspoLength2, CurrentOrder2, 20, 0.2, 10000);
		//CurrentScore = TranspositionShotgun(Msg, TranspoType1, TranspoLength1, CurrentOrder1, TranspoType2, TranspoLength2, CurrentOrder2, 10, 1, 10000);
		
		//CurrentScore = DoubleTranspositionAnnealing(Msg, TranspoType1, TranspoLength1, CurrentOrder1, TranspoType2, TranspoLength2, CurrentOrder2, 10, 1, 10000);
		CurrentScore = DoubleTranspositionHillClimb(Msg, TranspoType1, TranspoLength1, CurrentOrder1, TranspoType2, TranspoLength2, CurrentOrder2, 10000);
		
		if (CurrentScore > BestScore){
			CopyDaKeys(CurrentOrder1, BestOrder1, TranspoLength1);
			CopyDaKeys(CurrentOrder2, BestOrder2, TranspoLength2);
			BestScore = CurrentScore;
			//cout << "\n";
			cout << "New Best Key!\n\n";
			cout << "Score: " << fixed << BestScore << "\n";
			cout << "\n";
			cout << "\n";
			//cout << "Columns: " << BestColumnNum <<"\n";
			cout << "Column Order 1: ";
			DisplayOrder(BestOrder1, TranspoLength1);
			cout << "\nColumn Order 2: ";
			DisplayOrder(BestOrder2, TranspoLength2);
			cout << "\n";
			cout << "\n";
			string Decipherment = DecodeDoubleTranspo(Msg, TranspoType1, TranspoLength1, BestOrder1, TranspoType2, TranspoLength2, BestOrder2);
			cout << "Decipherment:\n" << Decipherment;
			cout << "\n\n";
		}
		else {
			cout << "Didn't find a better key...";
			cout << "\n\n";
		}
		cout << "--------------------------------------\n\n";
		trial += 1;
	}
}


Permute2(Message, TranspoType1, TranspoLength1, Permutation1, TranspoType2, TranspoLength2, Permutation2);


int main(int argc, char *argv[])
{
	srand(time(0));
	
	cout << "Welcome To The Double Transposition Cipher Creaker!\n";
	
	cout << "\n\nToday, We're Gonna BRUTE FORCE IT!!!\n\n";
	
	/* Get Message */
	string MESSAGE;
	ifstream File("Double Transposition Message.txt");
	File.seekg(0, ios::end);
	MESSAGE.reserve(File.tellg());
	File.seekg(0, ios::beg);
	MESSAGE.assign((istreambuf_iterator<char>(File)), istreambuf_iterator<char>());
	transform(MESSAGE.begin(), MESSAGE.end(), MESSAGE.begin(), ::tolower);
	
	cout.precision(17);
	
	bool TranspoType1;
	bool TranspoType2;
	int TranspoLength1;
	int TranspoLength2;
	
	if (string(argv[1]) == "Column"){
        TranspoType1 = true;
    } 
	else {
		TranspoType1 = false;
    }
	
	TranspoLength1 = atof(argv[2]);
	
	if (string(argv[3]) == "Column"){
        TranspoType2 = true;
    } 
	else {
        TranspoType2 = false;
    }
	
	TranspoLength2 = atof(argv[4]);
	
	cout << "----------------\n";
	
	cin.get();
	
	/*string Permutation1;
	Permutation1 = "";
	string Permutation2;
	Permutation2 = "";*/
	int i;
	int j;
	/*for (i = 1; i < TranspoLength1+1; i++){
		for (j = 1; j < TranspoLength2+1; j++){
			cout
		}
	}*/
	/*for (i = 1; i < TranspoLength1+1; i++){
		//Permutation1 += i;
		Permutation1 += string(i);
	}
	for (j = 1; j < TranspoLength2+1; j++){
		//Permutation2 += j;
		Permutation2 += string(j);
	}
	
	cout << Permutation1 << " " << Permutation2 << "\n";
	
	do{
		//cout << Permutation1 
		string Permutation2;
		Permutation2 = "";
		for (j = 1; j < TranspoLength2+1; j++){
			//Permutation2 += j;
			Permutation2 += string(j);
		}
		
		do {
			
			cout << Permutation1 << " " << Permutation2 << "\n";
			
		//} while (next_permutation(Permutation2.start(), Permutation2.end()));
		} while (next_permutation(Permutation2.begin(), Permutation2.end()));
		
	//} while (next_permutation(Permutation1.start(), Permutation1.end()));
	} while (next_permutation(Permutation1.begin(), Permutation1.end()));*/
	
	int Permutation1[MAXKEYLENGTH];
	int Permutation2[MAXKEYLENGTH];
	
	for (i = 0; i < TranspoLength1; i++){
		Permutation1[i] = i;
	}
	for (j = 0; j < TranspoLength2; j++){
		Permutation2[j] = j;
	}
	
	Permute2(Message, TranspoType1, TranspoLength1, Permutation1, TranspoType2, TranspoLength2, Permutation2);
	
	/*int Len1 = strlen(Permutation1);
	int Len2 = strlen(Permutation2);
	
	do {
		DisplayOrder(Permutation1, TranspoLength1); cout << "\n";
	} while (next_permutation(Permutation1, Permutation1+Len1));*/
	
	SolveTransposition(MESSAGE, TranspoType1, TranspoLength1, TranspoType2, TranspoLength2);
	
	cin.get();
	
	return 1;
}