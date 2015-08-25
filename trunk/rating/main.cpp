//#define ZOJ10
//#define NOTIME

#include <set>
#include <map>
#include <iostream>
#include <sstream>
#include <cstdio>
#include <cstring>
#include <cmath>
#include <string>
#include <vector>
#include <algorithm>
#include "erf.h"
#include "inv_norm.h"

using namespace std;
int trytimes;

const int MAXR = 20;
const int MAXG = 4;
int ngroup;
int rounds[MAXR];
int r;
vector<string> handles[MAXG];
map<string, int> att;

struct Coder
{
	string handle;
	int old_rating;
	int old_vol;
	int matches;
	int solved, penalty;
	double a_rank, e_rank;
	double a_perf, e_perf;
	double perf_as;
	double new_rating;
	int new_rating_caped;
	double new_vol;
	int end_new_vol;
};

vector<Coder> allCoders;

int cmp(const Coder &A, const Coder &B)
{
	if (A.solved != B.solved)
		return A.solved > B.solved ? -1 : 1;
	if (A.penalty != B.penalty)
		return A.penalty < B.penalty ? -2 : 2;
	return 0;
}

struct Division
{
	int num_coders;
	int new_coders;
	double cf;
	double cf_new;
	vector<Coder> coders;
} div1, div2;

void solve(Division &div)
{
	vector<Coder> &C = div.coders;

	//process without new coders
	if (div.num_coders - div.new_coders <= 1)
	{
		for (int i = 0; i < div.num_coders; i++) if (C[i].matches > 0)
		{
			C[i].new_rating_caped = C[i].old_rating;
			C[i].end_new_vol = C[i].old_vol;
		}
	}
	else
	{
		double ave_rating = 0.0, a = 0.0, b = 0.0;

		for (int i = 0; i < div.num_coders; i++) if (C[i].matches > 0)
			ave_rating += C[i].old_rating;
		ave_rating /= div.num_coders - div.new_coders;

		for (int i = 0; i < div.num_coders; i++) if (C[i].matches > 0)
		{
			a += C[i].old_vol * C[i].old_vol;
			b += (C[i].old_rating - ave_rating) * (C[i].old_rating - ave_rating);
		}

		div.cf = sqrt(a / (div.num_coders - div.new_coders)
			+ b / (div.num_coders - div.new_coders - 1));

		for (int i = 0; i < div.num_coders; i++) if (C[i].matches > 0)
		{
			int lose = 0, tie = 0, losetie = 0, wintie = 0;
			for (int j = 0; j < div.num_coders; j++) if (C[j].matches > 0)
			{
				int r = cmp(C[i], C[j]);
				if (r == 1) lose++;
				else if (r == 0) tie++;
				else if (r == 2) losetie++;
				else if (r == -2) wintie++;
			}
			C[i].a_rank = (lose * 4 + 2 + losetie * 3 + tie * 2 + wintie) / 4.0;
		}

		for (int i = 0; i < div.num_coders; i++) if (C[i].matches > 0)
		{
			C[i].e_rank = 0.5;
			for (int j = 0; j < div.num_coders; j++) if (C[j].matches > 0)
			{
				C[i].e_rank += 0.5 * erf((C[j].old_rating - C[i].old_rating) / 
					sqrt(2.0 * (C[i].old_vol * C[i].old_vol + C[j].old_vol * C[j].old_vol))) + 0.5;
			}
		}

		for (int i = 0; i < div.num_coders; i++) if (C[i].matches > 0)
		{
			C[i].a_perf = -inv_norm((C[i].a_rank - 0.5) / (div.num_coders - div.new_coders));
			C[i].e_perf = -inv_norm((C[i].e_rank - 0.5) / (div.num_coders - div.new_coders));
		}

		for (int i = 0; i < div.num_coders; i++) if (C[i].matches > 0)
		{
			C[i].perf_as = C[i].old_rating + div.cf * (C[i].a_perf - C[i].e_perf);

			double weight = 1.0 / (1 - (0.42 / (C[i].matches + 1) + 0.18)) - 1.0;
			if (C[i].old_rating >= 2000 && C[i].old_rating <= 2500) weight = weight * 0.9;
			else if (C[i].old_rating > 2500) weight = weight * 0.8;
			double cap = 150.0 + 1500.0 / (C[i].matches + 2.0);

			C[i].new_rating = (C[i].old_rating + weight * C[i].perf_as) / (1.0 + weight);

			if (C[i].new_rating > C[i].old_rating && C[i].new_rating - C[i].old_rating > cap)
				C[i].new_rating = C[i].old_rating + cap;

			if (C[i].new_rating < C[i].old_rating && C[i].new_rating - C[i].old_rating < -cap)
				C[i].new_rating = C[i].old_rating - cap;

			C[i].new_vol = sqrt((C[i].new_rating - C[i].old_rating) * (C[i].new_rating - C[i].old_rating) / weight
				+ C[i].old_vol * C[i].old_vol / (weight + 1.0));

			C[i].new_rating_caped = int(C[i].new_rating + 0.5);
			C[i].end_new_vol = int(C[i].new_vol + 0.5);
		}
	}

	//process new coders
	if (div.num_coders <= 1)
	{
		for (int i = 0; i < div.num_coders; i++) if (C[i].matches == 0)
		{
			C[i].new_rating_caped = C[i].old_rating;
			C[i].end_new_vol = C[i].old_vol;
		}
	}
	else
	{
		double ave_rating = 0.0, a = 0.0, b = 0.0;

		for (int i = 0; i < div.num_coders; i++)
			ave_rating += C[i].old_rating;
		ave_rating /= div.num_coders;

		for (int i = 0; i < div.num_coders; i++)
		{
			a += C[i].old_vol * C[i].old_vol;
			b += (C[i].old_rating - ave_rating) * (C[i].old_rating - ave_rating);
		}

		div.cf_new = sqrt(a / (div.num_coders)
			+ b / (div.num_coders - 1));

		for (int i = 0; i < div.num_coders; i++) if (C[i].matches == 0)
		{
			int lose = 0, tie = 0, losetie = 0, wintie = 0;
			for (int j = 0; j < div.num_coders; j++)
			{
				int r = cmp(C[i], C[j]);
				if (r == 1) lose++;
				else if (r == 0) tie++;
				else if (r == 2) losetie++;
				else if (r == -2) wintie++;
			}
			C[i].a_rank = (lose * 4 + 2 + losetie * 3 + tie * 2 + wintie) / 4.0;
		}

		for (int i = 0; i < div.num_coders; i++) if (C[i].matches == 0)
		{
			C[i].e_rank = 0.5;
			for (int j = 0; j < div.num_coders; j++)
			{
				C[i].e_rank += 0.5 * erf((C[j].old_rating - C[i].old_rating) / 
					sqrt(2.0 * (C[i].old_vol * C[i].old_vol + C[j].old_vol * C[j].old_vol))) + 0.5;
			}
		}

		for (int i = 0; i < div.num_coders; i++) if (C[i].matches == 0)
		{
			C[i].a_perf = -inv_norm((C[i].a_rank - 0.5) / (div.num_coders));
			C[i].e_perf = -inv_norm((C[i].e_rank - 0.5) / (div.num_coders));
		}

		for (int i = 0; i < div.num_coders; i++) if (C[i].matches == 0)
		{
			C[i].perf_as = C[i].old_rating + div.cf_new * (C[i].a_perf - C[i].e_perf);
			double weight = 1.0 / (1 - (0.42 / (C[i].matches + 1) + 0.18)) - 1.0;
			if (C[i].old_rating >= 2000 && C[i].old_rating <= 2500) weight = weight * 0.9;
			else if (C[i].old_rating > 2500) weight = weight * 0.8;
			double cap = 150.0 + 1500.0 / (C[i].matches + 2.0);

			C[i].new_rating = (C[i].old_rating + weight * C[i].perf_as) / (1.0 + weight);

			if (C[i].new_rating > C[i].old_rating && C[i].new_rating - C[i].old_rating > cap)
				C[i].new_rating = C[i].old_rating + cap;

			if (C[i].new_rating < C[i].old_rating && C[i].new_rating - C[i].old_rating < -cap)
				C[i].new_rating = C[i].old_rating - cap;

			C[i].new_vol = sqrt((C[i].new_rating - C[i].old_rating) * (C[i].new_rating - C[i].old_rating) / weight
				+ C[i].old_vol * C[i].old_vol / (weight + 1.0));

			C[i].new_rating_caped = int(C[i].new_rating + 0.5);
			C[i].end_new_vol = int(C[i].new_vol + 0.5);
		}
	}
}

bool hasCoder(string handle)
{
	for (int i = 0; i < (int)allCoders.size(); ++i) {
		if (allCoders[i].handle == handle) return true;
	}
	return false;
}

int getOldRating(string handle)
{
	for (int i = 0; i < (int)allCoders.size(); ++i) {
		if (allCoders[i].handle == handle) return allCoders[i].old_rating;
	}
	return -1;
}

int getOldVol(string handle)
{
	for (int i = 0; i < (int)allCoders.size(); ++i) {
		if (allCoders[i].handle == handle) return allCoders[i].old_vol;
	}
	return -1;
}

int getMatches(string handle)
{
	for (int i = 0; i < (int)allCoders.size(); ++i) {
		if (allCoders[i].handle == handle) return allCoders[i].matches;
	}
	return -1;
}

void setNewRating(string handle, int newRating)
{
	for (int i = 0; i < (int)allCoders.size(); ++i) {
		if (allCoders[i].handle == handle) allCoders[i].old_rating = newRating;
	}
}

void setNewVol(string handle, int newVol)
{
	for (int i = 0; i < (int)allCoders.size(); ++i) {
		if (allCoders[i].handle == handle) allCoders[i].old_vol = newVol;
	}
}

void addMatch(string handle)
{
	for (int i = 0; i < (int)allCoders.size(); ++i) {
		if (allCoders[i].handle == handle) ++allCoders[i].matches;
	}
}

void subMatch(string handle)
{
	for (int i = 0; i < (int)allCoders.size(); ++i) {
		if (allCoders[i].handle == handle) --allCoders[i].matches;
	}
}

vector<vector<string> > paras;

string trim(string s)
{
	while (!s.empty() && s[s.size() - 1] == ' ') s = s.substr(0, s.size() - 1);
	return s;
}

int preread()
{
	paras.clear();
	string buff;
	vector<string> v;
	bool beg = true;
	r = 0;
	while (1)
	{
		if (!getline(cin, buff)) {
			break;
		}
		if (trim(buff).size() == 1) {
			rounds[r] = buff[0] - 'A';
		}
		else if (buff == "") {
			paras.push_back(v);
			v.clear();
			++r;
			continue;
		}
		else {
			v.push_back(buff);
		}
	}
	if (!v.empty()) {
		paras.push_back(v);
	}
	return (int)paras.size();
}

void read(int r)
{
	//div1.num_coders = div1.new_coders = 0; div1.coders.clear();
	//div2.num_coders = div2.new_coders = 0; div2.coders.clear();
	div2.coders.clear();
	div2.num_coders = div2.new_coders = 0;
	Coder x;
	string buff;
	set<string> app;
	for (int i = 0; i < ngroup; ++i)
	{
		if (i == rounds[r]) continue;
		for (int j = 0; j < (int)handles[i].size(); ++j)
		{
			string handle = handles[i][j];
			app.insert(handle);
		}
	}
	for (int i = 0; i < (int)paras[r].size(); ++i)
	{
		buff = paras[r][i];
		stringstream S(buff);
		string foo;
		S >> foo >> x.handle >> x.solved;
#ifndef ZOJ10
		// 2.0
		while (S >> foo);
		stringstream SS(foo);
		SS >> x.penalty;
#else
		//	1.0
		string ss = buff.substr(buff.size() - 8);
		int a, b, c;
		sscanf(ss.c_str(), "%d:%d:%d", &a, &b, &c);
		x.penalty = (a * 60 + b) * 60 + c;
#endif
		if (!app.count(x.handle)) {
			continue;
		}
#ifdef NOTIME
		x.penalty = 0; // no penalty
#endif
		app.erase(app.find(x.handle));
		if (trytimes == 0) ++att[x.handle];
		x.matches = getMatches(x.handle);
		x.old_rating = getOldRating(x.handle);
		x.old_vol = getOldVol(x.handle);
		//cin >> x.matches >> x.old_rating >> x.old_vol >> x.solved >> x.penalty;
		//if (x.matches > 0 && x.old_rating >= 1200) //div 1
		//{
		//   div1.num_coders++;
		//   div1.coders.push_back(x);
		//}
		//else //div2
		{
			div2.num_coders++;
			if (x.matches == 0)
			{
				div2.new_coders++;
				x.old_rating = 1200; x.old_vol = 515;
			}
			div2.coders.push_back(x);
		}
		/*if (x.handle == "X" || x.handle == "X_ray") {
			cout << x.handle << ' ' << x.solved << ' ' << x.penalty << endl;
		}*/
	}
	for (set<string>::iterator si = app.begin(); si != app.end(); ++si) {
		string handle = *si;
		x.handle = handle;
		x.solved = 0;
		x.penalty = 0;
		x.matches = getMatches(x.handle);
		x.old_rating = getOldRating(x.handle);
		x.old_vol = getOldVol(x.handle);
		{
			div2.num_coders++;
			if (x.matches == 0)
			{
				div2.new_coders++;
				x.old_rating = 1200; x.old_vol = 515;
			}
			div2.coders.push_back(x);
			/*subMatch(handle);
			if (x.handle == "X" || x.handle == "X_ray") {
				cout << x.handle << ' ' << x.solved << ' ' << x.penalty << endl;
			}*/
		}
	}
}

void write(Division &div)
{
	vector<Coder> &C = div.coders;
	for (int i = 0; i < div.num_coders; i++)
		printf("%s\t%d\t%d\t%d\n", C[i].handle.c_str(), C[i].matches + 1, C[i].new_rating_caped, C[i].end_new_vol);
	//for (int i = 0; i < div.num_coders; i++) if (C[i].handle == "supernova")
	//   printf("%s %f %f %f %f %f %f\n", C[i].handle.c_str(), C[i].a_rank, C[i].a_perf, C[i].e_perf, C[i].perf_as, C[i].new_rating, C[i].new_vol);
}

void readCoderInfo()
{
	FILE *fin = fopen("handlelist.txt", "r");
	Coder x;
	char buff[1234];
	allCoders.clear();
	ngroup = 0;
	while (fgets(buff, 1000, fin)) {
		handles[ngroup].clear();
		stringstream S(buff);
		while (S >> buff) {
			x.handle = buff;
			x.matches = x.old_rating = x.old_vol = 0;
			allCoders.push_back(x);
			handles[ngroup].push_back(buff);
		}
		++ngroup;
	}
	fclose(fin);
}

void updateCoderInfo()
{
	for (int i = 0; i < (int)div2.coders.size(); ++i) {
		Coder x = div2.coders[i];
		setNewRating(x.handle, x.new_rating_caped);
		setNewVol(x.handle, x.end_new_vol);
		addMatch(x.handle);
	}
}

bool cmprating(const Coder &a, const Coder &b)
{
	return a.old_rating > b.old_rating;
}

void writeCoderInfo()
{
	sort(allCoders.begin(), allCoders.end(), cmprating);
	FILE *fin = fopen("result.txt", "w");
	Coder x;
	fprintf(fin, "%-7s%-17s%13s%10s%10s\n", "rank", "handle", "# matches", "rating", "vol");
	int lastrank = -1;
	for (int i = 0; i < (int)allCoders.size(); ++i) {
		x = allCoders[i];
		if (i == 0 || x.old_rating != allCoders[i - 1].old_rating) {
			lastrank = i + 1;
		}
		fprintf(fin, "%-7d%-17s%13d%10d%10d\n", lastrank, x.handle.c_str(), att[x.handle], x.old_rating, x.old_vol);
	}
	fclose(fin);
}

bool cango(int *order, int r)
{
	int beg = 0;
	while (beg + ngroup < r) beg += ngroup;
	int end = r;
	while (1) {
		if (next_permutation(order + beg, order + end)) {
			return true;
		}
		else {
			if (beg == 0) {
				return false;
			}
			else {
				beg -= ngroup;
				end = beg + ngroup;
			}
		}
	}
	return false;
}

int main()
{
	readCoderInfo();
	r = preread();

	int order[MAXR];
	for (int i = 0; i < r; ++i) {
		order[i] = i;
	}
	vector<Coder> sum = allCoders;
	trytimes = 0;
	do {
		for (int i = 0; i < (int)allCoders.size(); ++i) {
			allCoders[i].matches = allCoders[i].old_vol = allCoders[i].old_rating = 0;
		}
		for (int i = 0; i < r; ++i) {
			read(order[i]);

			solve(div2);

			updateCoderInfo();
		}
		for (int i = 0; i < (int)allCoders.size(); ++i) {
			sum[i].old_rating += allCoders[i].old_rating;
			sum[i].old_vol += allCoders[i].old_vol;
		}
		++trytimes;
		printf("%d\n", trytimes);
	}
	while (cango(order, r));

	for (int i = 0; i < (int)allCoders.size(); ++i) {
		allCoders[i].old_rating = sum[i].old_rating / trytimes;
		allCoders[i].old_vol = sum[i].old_vol / trytimes;
	}

	writeCoderInfo();

	return 0;
}
