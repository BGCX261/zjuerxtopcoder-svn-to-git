using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJUerXTopCoder
{
    class CoderStat
    {
        public string handle;
        public int total_challenges;
        public int succ_challenges;
        public int rating;
        public int maxrating;
        public int vol;
        public string id;
        public int events;
        public int eventsLastYear;
        public int colorChangeTimes;
        public string myLastDate;
        public bool active;
        public double chaPoints;
        public int submits;
        public int solves;
        public Dictionary<string, int> win, lose, deuce;

        public void DualWith(Coder me, Coder other)
        {
            if (!win.ContainsKey(other.id))
            {
                win.Add(other.id, 0);
                lose.Add(other.id, 0);
                deuce.Add(other.id, 0);
            }
            if (Convert.ToDouble(me.finalPoints) < Convert.ToDouble(other.finalPoints))
            {
                ++lose[other.id];
            }
            else if (Convert.ToDouble(me.finalPoints) > Convert.ToDouble(other.finalPoints))
            {
                ++win[other.id];
            }
            else
            {
                ++deuce[other.id];
            }
        }

        public void UpdateStat(Coder coder, Round round)
        {
            total_challenges += coder.fail_challenges + coder.succ_challenges;
            succ_challenges += coder.succ_challenges;
            rating = coder.newRating;
            maxrating = Math.Max(maxrating, coder.newRating);
            vol = coder.vol;
            chaPoints += Convert.ToDouble(coder.challengePoints);

            myLastDate = round.date;
            ++events;
            if (!HTMLGenerator.RatingToString(coder.oldRating, coder.newCoder).Equals(HTMLGenerator.RatingToString(coder.newRating, false)))
            {
                ++colorChangeTimes;
            }
            if (DateTime.Parse(round.date).AddYears(1) >= DateTime.Now)
            {
                ++eventsLastYear;
            }
            for (int i = 0; i < 3; ++i)
            {
                if (coder.problemStatus[i].Equals("Challenge Succeeded") || coder.problemStatus[i].Equals("Failed System Test"))
                {
                    ++submits;
                }
                else if (coder.problemStatus[i].Equals("Passed System Test"))
                {
                    ++submits;
                    ++solves;
                }
            }
        }

        public void CheckActive(string lastDate)
        {
            DateTime lastEvent = DateTime.Parse(lastDate);
            DateTime myLastEvent = DateTime.Parse(myLastDate);
            active = myLastEvent.AddMonths(6) >= lastEvent;
        }

        public static CoderStat NewCoderStat(Coder coder, Round round)
        {
            CoderStat coderStat = new CoderStat();
            coderStat.handle = coder.handle;
            coderStat.id = coder.id;
            coderStat.win = new Dictionary<string, int>();
            coderStat.lose = new Dictionary<string, int>();
            coderStat.deuce = new Dictionary<string, int>();
            coderStat.UpdateStat(coder, round);
            return coderStat;
        }

        public static int CompareByRating(CoderStat coder1, CoderStat coder2)
        {
            return coder2.rating - coder1.rating;
        }

        public static int CompareByMaxRating(CoderStat coder1, CoderStat coder2)
        {
            return coder2.maxrating - coder1.maxrating;
        }

        public static int CompareByRatingActive(CoderStat coder1, CoderStat coder2)
        {
            if (!coder2.active && !coder1.active)
            {
                return 0;
            }
            else if (!coder2.active)
            {
                return -1;
            }
            else if (!coder1.active)
            {
                return 1;
            }
            return coder2.rating - coder1.rating;
        }

        public static int CompareByMaxRatingActive(CoderStat coder1, CoderStat coder2)
        {
            if (!coder2.active && !coder1.active)
            {
                return 0;
            }
            else if (!coder2.active)
            {
                return -1;
            }
            else if (!coder1.active)
            {
                return 1;
            }
            return coder2.maxrating - coder1.maxrating;
        }

        public static int CompareByChallengeSuccRate(CoderStat coder1, CoderStat coder2)
        {
            if (coder1.total_challenges < 20 && coder2.total_challenges < 20)
            {
                return 0;
            }
            else if (coder2.total_challenges < 20)
            {
                return -1;
            }
            else if (coder1.total_challenges < 20)
            {
                return 1;
            }
            else
            {
                if (coder1.succ_challenges * coder2.total_challenges == coder2.succ_challenges * coder1.total_challenges)
                {
                    return coder2.succ_challenges - coder1.succ_challenges;
                }
                else
                {
                    return coder2.succ_challenges * coder1.total_challenges - coder1.succ_challenges * coder2.total_challenges;
                }
            }
        }

        public static int CompareByChallengePoints(CoderStat coder1, CoderStat coder2)
        {
            if (coder1.total_challenges < 20 && coder2.total_challenges < 20)
            {
                return 0;
            }
            else if (coder2.total_challenges < 20)
            {
                return -1;
            }
            else if (coder1.total_challenges < 20)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(coder2.chaPoints - coder1.chaPoints);
            }
        }

        public static int CompareByAverageChallengePoints(CoderStat coder1, CoderStat coder2)
        {
            if (coder1.total_challenges < 20 && coder2.total_challenges < 20)
            {
                return 0;
            }
            else if (coder2.total_challenges < 20)
            {
                return -1;
            }
            else if (coder1.total_challenges < 20)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(1000 * (coder2.chaPoints / coder2.events - coder1.chaPoints / coder1.events));
            }
        }

        public static int CompareByColorChangeTimes(CoderStat coder1, CoderStat coder2)
        {
            return coder2.colorChangeTimes - coder1.colorChangeTimes;
        }

        public static int CompareByEvents(CoderStat coder1, CoderStat coder2)
        {
            return coder2.events - coder1.events;
        }

        public static int CompareByEventsLastYear(CoderStat coder1, CoderStat coder2)
        {
            return coder2.eventsLastYear - coder1.eventsLastYear;
        }

        public static int CompareBySubmissionSuccRate(CoderStat coder1, CoderStat coder2)
        {
            if (coder1.submits < 20 && coder2.submits < 20)
            {
                return 0;
            }
            else if (coder2.submits < 20)
            {
                return -1;
            }
            else if (coder1.submits < 20)
            {
                return 1;
            }
            else
            {
                if (coder1.solves * coder2.submits == coder2.solves * coder1.submits)
                {
                    return coder2.solves - coder1.solves;
                }
                else
                {
                    return coder2.solves * coder1.submits - coder1.solves * coder2.submits;
                }
            }
        }
    }
}
