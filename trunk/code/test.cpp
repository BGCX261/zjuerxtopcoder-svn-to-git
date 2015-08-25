#include <iostream>
#include <vector>
#include <string>
#include <algorithm>

using namespace std;

const int COL = 16;

int width[COL] = {2,6,0,8,3,4,6,5,2,1,5,6,5,5,6,1};

vector<string> gao(string s)
{
	vector<string> vs;
	for (int i = 0; i < COL - 1; ++i) {
		int ind = s.find('\t');
		if (width[i]) {
			vs.push_back(s.substr(0, ind));
		}
		s = s.substr(ind + 1);
	}
	if (width[COL - 1]) {
		vs.push_back(s);
	}
	return vs;
}

vector<string> res;

void pad(int r, int c, char ch, int tm)
{
	for (int i = 0; i < tm; ++i) {
		res[r][c + i] = ch;
	}
}

void pad(int r, int c, string s, int from, int limit)
{
	for (int i = 0; i < limit; ++i) {
		if (from + i >= s.size()) return;
		res[r][c + i] = s[from + i];
	}
}

int main()
{
	freopen("1.txt", "r", stdin);
	vector<vector<string> > v;
	string s;
	while (getline(cin, s)) {
		v.push_back(gao(s));
	}
	int sumwidth = 0;
	vector<int> w;
	for (int i = 0; i < COL; ++i) {
		sumwidth += width[i];
		if (width[i] != 0) {
			w.push_back(width[i]);
		}
	}
	sumwidth += (w.size() - 1);

	vector<int> heights;
	int sumheight = 0;
	for (int i = 0; i < v.size(); ++i) {
		int maxx = 1;
		for (int j = 0; j < w.size(); ++j) {
			maxx = max(maxx, int((v[i][j].size() - 1) / w[j] + 1));
		}
		heights.push_back(maxx);
		sumheight += (maxx + 1);
	}
	--sumheight;

	res.resize(sumheight);
	string ss;
	for (int j = 0; j < sumwidth; ++j) {
		ss += " ";
	}
	for (int i = 0; i < sumheight; ++i) {
		res[i] = ss;
	}
	int currow = 0;
	for (int i = 0; i < v.size(); ++i) {
		int curcol = 0;
		if (i != 0) {
			for (int j = 0; j < w.size(); ++j) {
				if (j != 0) {
					pad(currow, curcol, '+', 1);
					++curcol;
				}
				pad(currow, curcol, '-', w[j]);
				curcol += w[j];
			}
			//pad(currow, curcol, '+', 1);
			++currow;
		}
		curcol = 0;
		for (int j = 0; j < w.size(); ++j) {
			if (j != 0) {
				for (int k = 0; k < heights[i]; ++k) {
					pad(currow + k, curcol, '|', 1);
				}
				++curcol;
			}
			int from = 0;
			int l = 0;
			while (from < v[i][j].size()) {
				pad(currow + l, curcol, v[i][j], from, w[j]);
				from += w[j];
				++l;
			}
			curcol += w[j];
		}
		/*for (int k = 0; k < heights[i]; ++k) {
			pad(currow + k, curcol, '|', 1);
		}*/
		currow += heights[i];
	}
	/*int curcol = 0;
	for (int j = 0; j < w.size(); ++j) {
		pad(currow, curcol, '+', 1);
		++curcol;
		pad(currow, curcol, '-', w[j]);
		curcol += w[j];
	}
	pad(currow, curcol, '+', 1);*/
	freopen("2.txt", "w", stdout);
	for (int i = 0; i < res.size(); ++i) {
		cout << res[i] << endl;
	}
	return 0;
}