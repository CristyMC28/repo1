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
    public class PlatinumMember : Membership
    {
        //class fields
        internal static decimal pointsFactor;
        internal static decimal minimumBalance;

        //class read only properties
        public static string PointsFactor => PlatinumMember.pointsFactor.ToString("P1");
        public static string MinimumBalance => PlatinumMember.minimumBalance.ToString("C2");

        //instance members
        public override bool Parking => true;

        //constructors
        static PlatinumMember()
        {
            PlatinumMember.pointsFactor = 1M;
            PlatinumMember.minimumBalance = 50000M;
        }
        public PlatinumMember(string name, decimal amount)
            : base(name)
        {
            if (amount < minimumBalance)
                throw new Exception("Incorrect Membership");
            else
            {
                base.balance = amount;
                this.points = 10000;
            }
        }

        //methods
        public override bool RedeemPoints(int points, out decimal credit)
        {
            if (this.points >= points)
            {
                credit = Math.Round((decimal)(points / redeemFactor), 0);
                credit = this.AmountPlayed >= 1000M ? credit + 5M : credit;
                this.points -= points;
                return true;
            }
            else
            {
                credit = 0M;
                return false;
            }
        }
        public override bool AddToBalance(decimal amount)
        {
            if (base.AddToBalance(amount))
            {
                if (amount > 1000M) this.points += 100;
                return true;
            }
            return false;
        }
        public override string ToString() =>
            "Platinum Member: " + base.ToString();
    }
}
