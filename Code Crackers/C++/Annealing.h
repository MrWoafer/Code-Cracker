//#pragma once
#ifndef ANNEALING_H
#define ANNEALING_H
#include <iostream>
#include <fstream>
#include <stdlib.h>
#include <math.h>
#include <string>

using namespace std;

//float QuadgramScore(string Msg);
double QuadgramScore(string Msg);

int AlphabetIndex(string Letter);

bool AnnealingProbability(float Difference, float Temperature);

#endif