using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJUerXTopCoder
{
    class Record
    {
        public Coder coder;
        public Round round;

        public static int CompareByChaPoints(Record record1, Record record2)
        {
            return Convert.ToInt32(Convert.ToDouble(record2.coder.challengePoints) - Convert.ToDouble(record1.coder.challengePoints));
        }

        public static int CompareByVolatility(Record record1, Record record2)
        {
            return record2.coder.vol - record1.coder.vol;
        }
    }
}
