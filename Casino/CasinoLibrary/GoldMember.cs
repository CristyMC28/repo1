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
    public class GoldMember : Membership
    {
        //class fields
        internal static decimal pointsFactor;
        internal static decimal minimumBalance;

        //class read only properties
        public static string PointsFactor => GoldMember.pointsFactor.ToString("P1");
        public static string MinimumBalance => GoldMember.minimumBalance.ToString("C2");
        //constructors
        static GoldMember()
        {
            GoldMember.pointsFactor = .5M;
            GoldMember.minimumBalance = 1000M;
        }
        public GoldMember(string name, decimal amount)
            : base(name)
        {
            if (amount < minimumBalance && amount >= PlatinumMember.minimumBalance)
                throw new Exception("Incorrect Membership");
            else
            {
                base.balance = amount;
                base.points = 100;
            }
        }
        protected GoldMember(string name)
            : base(name) { }

        //methods
        public override bool RedeemPoints(int points, out decimal credit)
        {
            if (this.points >= points)
            {
                credit = Math.Round((decimal)(points / redeemFactor), 0);
                this.points -= points;
                return true;
            }
            else
            {
                credit = 0M;
                return false;
            }
        }
        public override string ToString() =>
            "Gold Member: " + base.ToString();

    }

}
