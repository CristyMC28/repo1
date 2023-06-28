//PROG1224 Final Exam
//Gaming Business Logic

//Student Name:Cristina Murguia

using System;
using System.Collections.Generic;

namespace CasinoLibrary 
{
    public abstract class Membership
    {
        //Class Fields
        private const string CASINO = "PROG1224 Casino";
        private static int winFactor = 2; 
        protected static int redeemFactor = 10;
        public delegate void CheckWinDelegate(decimal winner);
        public event CheckWinDelegate CheckWinEvent;
        //Class Properties
        public static string CasinoName => CASINO;

        //Instance Fields
        private string name;
        protected decimal balance;
        protected int points;
        private decimal amountPlayed;

        //Instance Read Only Properties
        public int Points => points;
        public decimal Balance => this.balance;
        public decimal AmountPlayed => this.amountPlayed;
        public virtual bool Parking { get; }

        //Instance Read Write Properites
        public string ClientName
        {
            get => this.name;
            set => this.name = (value.Length > 0 && value.Length <= 80) ?
                ProperCase(value) : "Unknown";
        }

        //constructors
        public Membership(string name)
        {
            this.ClientName = name;
            this.balance = 0M;
            this.amountPlayed = 0M;
            this.points = 0;
            this.Parking = false;
        }

        //instance methods
        public int Play(decimal amount, ref int[] luckyNumbers, out int[] winning)
        {
           
            decimal factor = 0M;
            if(this is SilverMember)
            {
                factor = SilverMember.pointsFactor;
            }
            else if (this is GoldMember)
            {
                factor = GoldMember.pointsFactor;
            }
            else if(this is PlatinumMember)
            {
                factor = PlatinumMember.pointsFactor;
            }
            int points = 0;
            if (amount <= this.balance)
            {
                this.balance -= amount;
                this.amountPlayed += amount;
                points = (int)Math.Round(amount * factor, 0);
                this.points += points;
                if (CheckWin(ref luckyNumbers, out winning))
                {
                    Win(this, amount);


                }
                return points;

            }
            else
                throw new Exception("Insficient Founds");
            winning = null;
            return 0;
        }


        private static void Win(Membership member, decimal amount)
        {
            decimal total = (amount * winFactor);
            member.balance += total;
            if (member.CheckWinEvent != null)
                member.CheckWinEvent(total);

        }

        //instance virtual methods
        public virtual bool RedeemPoints(int points)
        {
            decimal amount = Math.Round((decimal)(this.points / redeemFactor), 0);
            decimal redeem = Math.Round((decimal)(points / redeemFactor), 0);
            if (amount > redeem)
            {
                this.points -= points;
                this.balance += redeem;
                return true;
            }
            else
                return false;
        }
        public virtual bool RedeemPoints(int points, out decimal credit)
        {
            credit = 0M;
            return false;
        }
        public virtual bool AddToBalance(decimal amount)
        {
            if (amount > 0M)
            {
                this.balance += amount;
                return true;
            }
            else
                return false;
        }

        //overriden methods
        public override string ToString() =>
            $"{ClientName} - Balance: {Balance.ToString("C2")} Points: {Points}";

        //public class method
        public static List<Membership> GetMembers()
        {
            //return sample memberships
            List<Membership> members = new List<Membership>(10);
            members.Add(new SilverMember("John Brown", 100M));
            members.Add(new SilverMember("Jane Doe", 500M));
            members.Add(new GoldMember("Superman", 1000M));
            members.Add(new GoldMember("Paul Mark", 5000M));
            members.Add(new GoldMember("Susan Brown", 9500M));
            members.Add(new GoldMember("Gale Wind", 10000M));
            members.Add(new GoldMember("Yule Bull", 45000M));
            members.Add(new PlatinumMember("Cliff Biff", 50000M));
            members.Add(new PlatinumMember("Joyce Downing", 65000M));
            members.Add(new PlatinumMember("Peter Vanscoy", 100000M));
            return members;
        }

        //private class methods
        //this method returns true if all the numbers match the winning numbers generated.
        private static bool CheckWin(ref int[] numbers, out int[] winning)
        {
            int count = 0;
            int max = (numbers.Length <= 3) ? numbers.Length : 3;
            winning = GenerateWinningNumbers();
            for (int n = 0; n < max; n++)
                for (int w = 0; w < winning.Length; w++)
                    if (numbers[n] == winning[w]) count++;
            if (count == 3)         
                return true;
                          
            else
                return false;
        }

        private static int[] GenerateWinningNumbers()
        {
            int[] numbers = new int[3];
            Random r = new Random();
            for (int i = 0; i < numbers.Length; i++)
                numbers[i] = r.Next(100);
            //control winning numbers (1,2,3) for testing; uncomment next line:
            numbers[0] = 1;numbers[1] = 2;numbers[2] = 3;
            return numbers;
        }

        private static string ProperCase(string input)
        {
            if (input.Length == 0) return "";
            string output = "";
            input = input.Trim().ToLower();
            string[] names = input.Split(' ');
            foreach (string n in names)
                output += Char.ToUpper(n[0]) + n.Substring(1) + " ";
            return output.Trim();
        }
    }
}
