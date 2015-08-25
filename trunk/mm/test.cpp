#include <cstdio>
#include <cctype>
#include <cstdlib>
#include <string>
#include <algorithm>

using namespace std;

char randchar()
{
	return char('a' + rand() % 26);
}

string s[4];

string gen(int ind)
{
	string ret = "";
	for (int i = 0; i < 4; ++i) {
		if (isdigit(s[ind][i])) {
			ret += gen(s[ind][i] - '0');
		}
		else {
			ret += s[ind][i];
		}
	}
	return ret;
}

void yes1()
{
	freopen("Yes1.txt", "w", stdout);
	for (int i = 0; i < 4; ++i) {
		s[i] = "    ";
	}
	int ct[4] = {0, 0, 0, 0};
	int xx = 0;
	for (int a = 0; a < 4; ++a) {
		s[0][0] = (a == 0 ? randchar() : char('0' + a));
		if (a) ct[a]++;
		for (int b = 0; b < 4; ++b) {
			s[0][1] = (b == 0 ? randchar() : char('0' + b));
			if (b) ct[b]++;
			for (int c = 0; c < 4; ++c) {
				s[0][2] = (c == 0 ? randchar() : char('0' + c));
				if (c) ct[c]++;
				for (int d = 0; d < 4; ++d) {
					s[0][3] = (d == 0 ? randchar() : char('0' + d));
					if (d) ct[d]++;
					if (!ct[1]) {
						if (d) ct[d]--;
						continue;
					}
					for (int e = 0; e < 3; ++e) {
						s[1][0] = (e == 0 ? randchar() : char('1' + e));
						if (e) ct[1 + e]++;
						for (int f = 0; f < 3; ++f) {
							s[1][1] = (f == 0 ? randchar() : char('1' + f));
							if (f) ct[1 + f]++;
							for (int g = 0; g < 3; ++g) {
								s[1][2] = (g == 0 ? randchar() : char('1' + g));
								if (g) ct[1 + g]++;
								for (int h = 0; h < 3; ++h) {
									s[1][3] = (h == 0 ? randchar() : char('1' + h));
									if (h) ct[1 + h]++;
									if (!ct[2]) {
										if (h) ct[1 + h]--;
										continue;
									}
									for (int i = 0; i < 2; ++i) {
										s[2][0] = (i == 0 ? randchar() : char('2' + i));
										if (i) ct[3]++;
										for (int j = 0; j < 2; ++j) {
											s[2][1] = (j == 0 ? randchar() : char('2' + j));
											if (j) ct[3]++;
											for (int k = 0; k < 2; ++k) {
												s[2][2] = (k == 0 ? randchar() : char('2' + k));
												if (k) ct[3]++;
												for (int l = 0; l < 2; ++l) {
													s[2][3] = (l == 0 ? randchar() : char('2' + l));
													if (l) ct[3]++;
													if (!ct[3]) continue;
													for (int x = 0; x < 4; ++x) {
														s[3][x] = randchar();
													}
													if (l) ct[3]--;
													if (xx % 5000 == 0) {
														puts("4");
														puts("4 4 4 4");
														printf("%s\n", gen(0).c_str());
													}
													++xx;
												}
												if (k) ct[3]--;
											}
											if (j) ct[3]--;
										}
										if (i) ct[3]--;
									}
									if (h) ct[1 + h]--;
								}
								if (g) ct[1 + g]--;
							}
							if (f) ct[1 + f]--;
						}
						if (e) ct[1 + e]--;
					}
					if (d) ct[d]--;
				}
				if (c) ct[c]--;
			}
			if (b) ct[b]--;
		}
		if (a) ct[a]--;
	}
}

void no3()
{
	freopen("No3.txt", "w", stdout);
	for (int i = 0; i < 4; ++i) {
		s[i] = "    ";
	}
	int ct[4] = {0, 0, 0, 0};
	int xx = 0;
	for (int a = 0; a < 4; ++a) {
		s[0][0] = (a == 0 ? randchar() : char('0' + a));
		if (a) ct[a]++;
		for (int b = 0; b < 4; ++b) {
			s[0][1] = (b == 0 ? randchar() : char('0' + b));
			if (b) ct[b]++;
			for (int c = 0; c < 4; ++c) {
				s[0][2] = (c == 0 ? randchar() : char('0' + c));
				if (c) ct[c]++;
				for (int d = 0; d < 4; ++d) {
					s[0][3] = (d == 0 ? randchar() : char('0' + d));
					if (d) ct[d]++;
					if (!ct[1]) {
						if (d) ct[d]--;
						continue;
					}
					for (int e = 0; e < 3; ++e) {
						s[1][0] = (e == 0 ? randchar() : char('1' + e));
						if (e) ct[1 + e]++;
						for (int f = 0; f < 3; ++f) {
							s[1][1] = (f == 0 ? randchar() : char('1' + f));
							if (f) ct[1 + f]++;
							for (int g = 0; g < 3; ++g) {
								s[1][2] = (g == 0 ? randchar() : char('1' + g));
								if (g) ct[1 + g]++;
								for (int h = 0; h < 3; ++h) {
									s[1][3] = (h == 0 ? randchar() : char('1' + h));
									if (h) ct[1 + h]++;
									if (!ct[2]) {
										if (h) ct[1 + h]--;
										continue;
									}
									for (int i = 0; i < 2; ++i) {
										s[2][0] = (i == 0 ? randchar() : char('2' + i));
										if (i) ct[3]++;
										for (int j = 0; j < 2; ++j) {
											s[2][1] = (j == 0 ? randchar() : char('2' + j));
											if (j) ct[3]++;
											for (int k = 0; k < 2; ++k) {
												s[2][2] = (k == 0 ? randchar() : char('2' + k));
												if (k) ct[3]++;
												for (int l = 0; l < 2; ++l) {
													s[2][3] = (l == 0 ? randchar() : char('2' + l));
													if (l) ct[3]++;
													if (!ct[3]) continue;
													for (int x = 0; x < 4; ++x) {
														s[3][x] = randchar();
													}
													if (l) ct[3]--;
													if (xx % 10000 == 2000) {
														puts("4");
														puts("4 4 4 4");
														string ret = gen(0);
														ret[rand() % ret.size()] = ret[rand() % ret.size()];
														printf("%s\n", ret.c_str());
													}
													++xx;
												}
												if (k) ct[3]--;
											}
											if (j) ct[3]--;
										}
										if (i) ct[3]--;
									}
									if (h) ct[1 + h]--;
								}
								if (g) ct[1 + g]--;
							}
							if (f) ct[1 + f]--;
						}
						if (e) ct[1 + e]--;
					}
					if (d) ct[d]--;
				}
				if (c) ct[c]--;
			}
			if (b) ct[b]--;
		}
		if (a) ct[a]--;
	}
}

void no()
{
	freopen("No.txt", "w", stdout);
	int arr[26];
	for (int i = 0; i < 26; ++i) {
		arr[i] = i;
	}
	for (int nchar = 1; nchar <= 20; ++nchar) {
		for (int i = 0; i < 2; ++i) {
			int len = rand() % 200 + 10;
			if (i == 0) {
				len = 40 + (rand() % 11) - 5;
			}
			random_shuffle(arr, arr + 26);
			puts("4");
			puts("4 4 4 4");
			for (int i = 0; i < len; ++i) {
				putchar(arr[rand() % nchar] + 'a');
			}
			puts("");
		}
	}
}

void no2()
{
	freopen("No2.txt", "w", stdout);
	int arr[26];
	for (int i = 0; i < 26; ++i) {
		arr[i] = i;
	}
	for (int nchar = 1; nchar <= 10; ++nchar) {
		int len = rand() % 100 + 10;
		random_shuffle(arr, arr + 26);
		int n = rand() % 3 + 2;
		printf("%d\n", n);
		for (int i = 0; i < n; ++i) {
			printf("%d%c", rand() % 4 + 1, i == n - 1 ? '\n' : ' ');
		}
		for (int i = 0; i < len; ++i) {
			putchar(arr[rand() % nchar] + 'a');
		}
		puts("");
	}
}

void no4()
{
	freopen("No4.txt", "w", stdout);
	int arr[26];
	for (int i = 0; i < 26; ++i) {
		arr[i] = i;
	}
	for (int nchar = 1; nchar <= 5; ++nchar) {
		int len = rand() % 100 + 257;
		random_shuffle(arr, arr + 26);
		int n = 4;
		printf("%d\n", n);
		for (int i = 0; i < n; ++i) {
			printf("%d%c", 4, i == n - 1 ? '\n' : ' ');
		}
		for (int i = 0; i < len; ++i) {
			putchar(arr[rand() % nchar] + 'a');
		}
		puts("");
	}
}

void yes2()
{
	freopen("Yes2.txt", "w", stdout);
	s[0] = "1111";
	s[1] = "2222";
	s[2] = "3333";
	s[3] = "tttt";
	puts("4");
	puts("4 4 4 4");
	printf("%s\n", gen(0).c_str());
	s[3] = "kjfh";
	puts("4");
	puts("4 4 4 4");
	printf("%s\n", gen(0).c_str());
	s[0] = "1k11";
	puts("4");
	puts("4 4 4 4");
	printf("%s\n", gen(0).c_str());
	s[0] = "11p1";
	puts("4");
	puts("4 4 4 4");
	printf("%s\n", gen(0).c_str());
	s[1] = "qp23";
	puts("4");
	puts("4 4 4 4");
	printf("%s\n", gen(0).c_str());
	s[1] = "2222";
	s[0] = "1211";
	puts("4");
	puts("4 4 4 4");
	printf("%s\n", gen(0).c_str());
	s[0] = "1311";
	puts("4");
	puts("4 4 4 4");
	printf("%s\n", gen(0).c_str());
	s[0] = "1231";
	puts("4");
	puts("4 4 4 4");
	printf("%s\n", gen(0).c_str());
	s[0] = "1111";
	s[1] = "2223";
	puts("4");
	puts("4 4 4 4");
	printf("%s\n", gen(0).c_str());
	s[0] = "1123";
	puts("4");
	puts("4 4 4 4");
	printf("%s\n", gen(0).c_str());
}

int main()
{
	//no();
	//no2();
	//yes();
	//yes1();
	//yes2();
	//no3();
	no4();
	return 0;
}
