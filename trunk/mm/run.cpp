#include <set>
#include <map>
#include <ctime>
#include <queue>
#include <string>
#include <cstring>
#include <algorithm>

using namespace std;

string str;
string s[4];
int maxlen[14] = {0, 256, 256, 256, 256, 208, 160, 124, 94, 67, 49, 34, 22, 13};
//int r0t;
bool can[512][5] = {false};
int n;
int lim[4];
int mul[4];
int maxholes[5][5];

void initial()
{
	for (int l0 = 2; l0 <= 4; ++l0) {
		for (int l1 = 2; l1 <= 4; ++l1) {
			maxholes[l0][l1] = max(max(l0 * (l1 - 2), (l0 - 1) * (l1 - 1)), max((l0 - 2) * l1, l0 - 1));
		}
	}
}

int charct(const char *str)
{
	set<char> st;
	for (int i = 0; str[i]; ++i) {
		st.insert(str[i]);
	}
	return st.size();
}

bool isok(const char *str, const string &s)
{
	for (int i = 0; i < s[i]; ++i) {
		if (str[i] != s[i]) {
			return false;
		}
	}
	return true;
}

bool run(int lev, int maxlen, int sumlen)
{
	if (lev == 0) {
		//++r0t;
		for (int i = 0; i <= (int)str.size(); ++i) {
			for (int j = 0; j <= lim[0]; ++j) {
				can[i][j] = false;
			}
		}
		can[0][0] = true;
		for (int i = 0; i < (int)str.size(); ++i) {
			for (int j = 0; j < lim[0]; ++j) {
				if (!can[i][j]) continue;
				for (int k = 1; k < n; ++k) {
					if (isok(str.c_str() + i, s[k])) {
						can[i + s[k].size()][j + 1] = true;
					}
				}
				can[i + 1][j + 1] = true;
			}
		}
		for (int i = 1; i <= lim[0]; ++i) {
			if (can[(int)str.size()][i]) {
				return true;
			}
		}
		return false;
	}
	map<string, int> mp;
	mp[""] = 1;
	for (int i = 0; i < (int)str.size(); ++i) {
		queue<pair<int, int> > q;
		q.push(make_pair(i, 0));
		while (!q.empty()) {
			int from = q.front().first, ct = q.front().second;
			q.pop();
			if (ct >= 2 && max(maxlen, (from - i)) * mul[lev] >= (int)str.size()) {
				mp[str.substr(i, from - i)]++;
			}
			if (ct < lim[lev]) {
				for (int ind = lev + 1; ind < n; ++ind) {
					if (s[ind] != "") {
						if (isok(str.c_str() + from, s[ind])) {
							q.push(make_pair(from + s[ind].size(), ct + 1));
						}
					}
				}
				if (from < (int)str.size()) {
					q.push(make_pair(from + 1, ct + 1));
				}
			}
		}
	}
	for (map<string, int>::iterator mi = mp.begin(); mi != mp.end(); ++mi) {
		s[lev] = mi->first;
		int slen = mi->second * s[lev].size();
		if (n == 4 && lev == 2 && slen + sumlen + maxholes[lim[0]][lim[1]] < (int)str.size()) continue; 
		if (run(lev - 1, max((int)s[lev].size(), maxlen), sumlen + slen)) {
			return true;
		}
	}
	return false;
}

bool run()
{
	int c = charct(str.c_str());
	if (c > 13 || (int)str.size() > maxlen[c]) {
		return false;
	}
	return run(n - 1, 0, 0);
}

void gao()
{
	for (int i = 0; i < n; ) {
		if (lim[i] == 1) {
			for (int j = i + 1; j < n; ++j) {
				lim[j - 1] = lim[j];
			}
			--n;
		}
		else {
			++i;
		}
	}
	if (n == 0) {
		n = 1;
		lim[0] = 1;
	}
	mul[0] = 1;
	for (int i = 1; i < n; ++i) {
		mul[i] = mul[i - 1] * lim[i - 1];
	}
}

int main()
{
	freopen("all.txt", "r", stdin);
	freopen("ans.txt", "w", stdout);
	//map<int, int> mp;
	char str[1234];
	int t = 0;
	
	initial();
	clock_t start = clock();
	while (scanf("%d", &n) != EOF) {
		for (int i = 0; i < n; ++i) {
			scanf("%d", lim + i);
		}
		gao();
		scanf("%s", str);
		::str = str;
		//r0t = 0;
		++t;
		/*if (i % 5000 == 0) printf("%d\n", i);
		if (i <= 50000) continue;*/
		//clock_t start = clock();
		bool res = run();
		//clock_t end = clock();
		puts(res ? "Yes" : "No");
		//if (end - start >= 100 || !res) {
			//printf("case=%d time=%d len=%d nc=%d ans=%s\n", t, (end - start), strlen(str), charct(str),  res ? "Yes" : "No");
		//	puts(str);
		//}
		/*
		int c = charct(str);
		mp[c] = max(mp[c], (int)strlen(str));
		*/
	}
	clock_t end = clock();
	//printf("%d\n", end - start);
	/*for (map<int, int>::iterator mi = mp.begin(); mi != mp.end(); ++mi) {
		printf("%d %d\n", mi->first, mi->second);
	}*/
	return 0;
}
