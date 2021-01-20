using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Globalization;

namespace BankApplication
{
    public partial class LoggedIn : Form
    {
        BankDetails account;
        SQL sql;
        public LoggedIn(BankDetails _account, SQL _sql)
        {
            sql = _sql;
            account = _account;
            var userName = sql.GetUserName(account.UserID);
            InitializeComponent();
            lblUserName.Text = "Welcome, "+userName;
            LoadSaldo();
            LoadTransactions();
        }

        private void LoggedIn_Load(object sender, EventArgs e)
        { 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double depositValue;
            bool depositing = true;
            var message = "";
            while (depositing)
            {
                var test = Interaction.InputBox("How much would you like to deposit "+message, "Deposit");
                var result = Double.TryParse(test, out depositValue);
                depositing = false;
                if (test == "" || test.Contains("-"))
                {
                    MessageBox.Show("Deposit cancelled.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (result == true)
                {
                    var transaction = new Transaction() { BankID = account.AccountID, Date = new Date().Now().ToString("yyyy-MM-dd H:mm:ss"), Type = "Deposit", Value = depositValue };
                    sql.InsertClass(typeof(Transaction), transaction);
                    sql.UpdateSaldo(depositValue, account.AccountID);
                }
                else if (result == false)
                {
                    message = "(please type in a number)";
                    depositing = true;
                }
            }
            LoadSaldo();
            LoadTransactions();
        }
        private void LoadSaldo()
        {
            lblBalance.Text = sql.GetSaldo(account.AccountID)+ "€"; 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (int.Parse(sql.DailyWithdrawCount(account.AccountID)) < 3)
            {

                var currentBalance = double.Parse(sql.GetSaldo(account.AccountID));
                if (currentBalance > 0)
                {
                    double withdrawValue;
                    bool withdrawing = true;
                    var message = "";
                    while (withdrawing)
                    {
                        var test = Interaction.InputBox("How much would you like to withdraw " + message, "Withdraw");
                        var result = Double.TryParse(test, out withdrawValue);
                        withdrawing = false;
                        if (test == "" || test.Contains("-"))
                        {
                            MessageBox.Show("Withdraw cancelled.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (result == true)
                        {
                            if (withdrawValue > currentBalance)
                            {
                                MessageBox.Show("You dont have enough money", "Withdraw failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else if(withdrawValue > 500)
                            {
                                MessageBox.Show("You have surpassed your withdraw limit of 500€ per transaction", "Withdraw failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                var transaction = new Transaction() { BankID = account.AccountID, Date = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), Type = "Withdraw", Value = -withdrawValue };
                                sql.InsertClass(typeof(Transaction), transaction);
                                sql.UpdateSaldo(-withdrawValue, account.AccountID);
                            }
                        }
                        else if (result == false)
                        {
                            message = "(please type in a number)";
                            withdrawing = true;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You dont have enough money", "Withdraw failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                LoadSaldo();
                LoadTransactions();
            }
            else
            {
                MessageBox.Show("You have surpassed your daily withdraw limit of 3 withdraws", "Withdraw failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void LoadTransactions()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
            dataGridView1.Columns.Add("", "Type");
            dataGridView1.Columns.Add("", "Value");
            dataGridView1.Columns.Add("", "Date");
            var transactions = sql.GetLatestTransactions(account.AccountID);
            foreach (var transaction in transactions)
            {
                dataGridView1.Rows.Add(transaction.Type, transaction.Value.ToString()+ "€", transaction.Date.ToString());
            }
        }
    }
}
