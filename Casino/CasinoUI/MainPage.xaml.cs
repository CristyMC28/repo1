//PROG1224
//Final Test

//Name:

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CasinoLibrary;
using Windows.UI.Popups;

namespace CasinoUI
{
    public sealed partial class MainPage : Page
    {
        List<Membership> members;
        public static Membership selectedMember;
        
        public MainPage()
        {
            this.InitializeComponent();
            members = new List<Membership>(Membership.GetMembers().ToList());
            foreach(Membership m in members)
            {
                cboNames.Items.Add(m.ClientName);
                m.CheckWinEvent += winnerHandler;
            }

            loadCombos();
        }

        private void Name_Selection(object sender, SelectionChangedEventArgs e)
        {
            UpdateInfo();
           
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (selectedMember != null)
            {
                try
                {

                    decimal amountPlay = decimal.TryParse(txtPlay.Text, out amountPlay) == true ? amountPlay : throw new Exception("Amont to play must be a decimal value");
                    int[] luckyNumbers = new int[3];
                    int[] win = new int[3];

                    luckyNumbers[0] = (int)cboOne.SelectedItem;
                    luckyNumbers[1] = (int)cboTwo.SelectedItem;
                    luckyNumbers[2] = (int)cboThree.SelectedItem;

                    
                    selectedMember.Play(amountPlay, ref luckyNumbers, out win);

                    tbWinOne.Text = win[0].ToString();
                    tbWinTwo.Text = win[1].ToString();
                    tbWinThree.Text = win[2].ToString();
                    UpdateInfo();
                }
                catch (Exception ex)
                {
                    MessageDialog ms = new MessageDialog(ex.Message, "Error");
                    ms.ShowAsync();

                }
            }
            else
            {

            }
            txtPlay.Text = "";

        }

        private void Redeem_Click(object sender, RoutedEventArgs e)
        {
            if (selectedMember != null)
            {
                int points;
                if (int.TryParse(txtPlay.Text, out points))
                {
                    if (selectedMember.RedeemPoints(points))
                    {
                        UpdateInfo();
                    }
                    txtPlay.Text = "";
                    txtPlay.Focus(FocusState.Keyboard);
                   
                }
            }
        }

        private void Gift_Click(object sender, RoutedEventArgs e)
        {
            if (selectedMember != null)
            {
                decimal amount;
                int points;
                if (int.TryParse(txtPlay.Text, out points))
                {
                    if (selectedMember.RedeemPoints(points,out amount))
                    {
                        MessageDialog msg = new MessageDialog("Gift");
                        msg.ShowAsync();
                        UpdateInfo();
                    }
                    txtPlay.Text = "";
                    txtPlay.Focus(FocusState.Keyboard);

                }
            }
        }

        private void Deposit_Click(object sender, RoutedEventArgs e)
        {
            if(selectedMember != null)
            {
                decimal amount;
                if(decimal.TryParse(txtPlay.Text,out amount))
                {
                    selectedMember.AddToBalance(amount);
                    UpdateInfo();
                }
            }
            
        }

        private void winnerHandler(decimal amount)
        {

            MessageDialog msg = new MessageDialog($"All numbers match You won {amount.ToString("C2")}", "Congratulations!!");
            msg.ShowAsync();
        }
        public void UpdateInfo()
        {
            cboOne.IsEnabled = true;
            cboTwo.IsEnabled = true;
            cboThree.IsEnabled = true;
            txtPlay.IsEnabled = true;
            btnDeposit.IsEnabled = true;
            btnPlay.IsEnabled = true;
            btnGift.IsEnabled = true;
            btnRedeem.IsEnabled = true;
            
            selectedMember = members[cboNames.SelectedIndex];
            txtBalance.Text = selectedMember.Balance.ToString("C");
            txtAmount.Text = selectedMember.AmountPlayed.ToString("C");
            txtPoint.Text = selectedMember.Points.ToString();


            tbParking.Text = (selectedMember.Parking) ? "Free Parking" : "No Parking";

            if(selectedMember is SilverMember)
            {
                tbPointFactor.Text = SilverMember.PointsFactor;
                
            }
            else if(selectedMember is GoldMember)
            {
                tbPointFactor.Text = GoldMember.PointsFactor;
            }
            else if(selectedMember is PlatinumMember)
            {
                tbPointFactor.Text = PlatinumMember.PointsFactor;
                
            }

            tbMember.Text = selectedMember.ToString();

           
            CalculateTotals();
            
        }
        public void loadCombos()
        {
            for (int i = 1; i <= 100; i++)
            {
                cboOne.Items.Add(i);
                cboTwo.Items.Add(i);
                cboThree.Items.Add(i);
            }
        }
        private void CalculateTotals()
        {
            txtTotals.Text = Membership.CasinoName + " Totals:\n " + System.Environment.NewLine;
            var result1 = members.GroupBy(r => r.GetType().Name, emp => emp.GetType().Name,
                (TypeMember, Type) => new
                {
                    Key = TypeMember,
                    Count = Type.Count()
                });
            foreach(var t in result1)
            {
                txtTotals.Text += $"\n{t.Key} : {t.Count}";
            }

            decimal totalBalance = members.Sum(b => b.Balance);
            decimal totalAmount = members.Sum(a=> a.AmountPlayed);
            decimal totalPoints = members.Sum(p => p.Points);

            txtTotals.Text += $"\nTotal Balance: {totalBalance.ToString("C2")}  ";
            txtTotals.Text += $"Total Amount Played: {totalAmount.ToString("C2")}  ";
            txtTotals.Text += $"Total Points: {totalPoints}";

            var result2 = members.OrderByDescending(h => h.AmountPlayed).Select(h => new { h.ClientName,h.AmountPlayed}).Take(1);
            
            foreach(var ha in result2)
                txtTotals.Text += $"\nHighest Amount Played \nName: {ha.ClientName}  Amount: {ha.AmountPlayed.ToString("C2")} ";

            var result3 = members.Where(amount => amount.AmountPlayed > 500).Select(name => name.ClientName);
            txtTotals.Text += $"\nPlayers with more than $500.00 played: ";
            foreach (var names in result3)
                txtTotals.Text += $"\n{names}\n";
           
        }
    }
}
