//PROG1224
//Final Test
//Do not modify

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoLibrary
{
    public class SilverMember : Membership
    {
        //class fields
        internal static decimal pointsFactor;
        internal static decimal minimumBalance;

        //class read only properties
        public static string PointsFactor => SilverMember.pointsFactor.ToString("P1");
        public static string MinimumBalance => SilverMember.minimumBalance.ToString("C2");

        //constructors
        static SilverMember()
        {
            SilverMember.pointsFactor = .25M;
            SilverMember.minimumBalance = 0M;
        }
        public SilverMember(string name, decimal amount)
            : base(name)
        {
            if (amount >= GoldMember.minimumBalance)
                throw new Exception("Incorrect Membership");
            else
            {
                base.balance = (amount > 0M) ? amount : 0M;
                base.points = (amount > 100M) ? 10 : 0;
            }
        }

        //methods
        public override string ToString() =>
            "Silver Member: " + base.ToString();
    }

}
