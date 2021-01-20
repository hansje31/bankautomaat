using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCrypt.Net;

namespace BankApplication
{
    public partial class Form1 : Form
    {
        Database database;
        SQL sql;
        public Form1()
        {   
            InitializeComponent();
            string connectionString = "";
            try
            {
                // connectionstring is de file waar de connection naar database staat
                connectionString = File.ReadAllText(Directory.GetCurrentDirectory() + @"\connectionstring.txt");
            }
            catch
            {
                MessageBox.Show("Please make sure connectionstring.txt is in the same folder as this program.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            // connection naar de database, deze variabele connectionstring (met wachtwoordgegevens van de database en meer) word doorgevoerd naar Database class
            database = new Database(connectionString);
            
            // quizz class can now make use of Database class to connect to database
            sql = new SQL(database);        
        }

        bool loginEnabled1;
        bool loginEnabled2;
        private void txtAccountNumber_TextChanged(object sender, EventArgs e)
        {
            loginEnabled1 = txtAccountNumber.Text != "";
            CheckButton();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            loginEnabled2 = txtPassword.Text != "";
            CheckButton();
        }
        private void CheckButton()
        {
            btnLogin.Enabled = loginEnabled1 && loginEnabled2;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            BankDetails account;
            var hash = sql.GetHash(txtAccountNumber.Text);
            if (hash == "")
            {
                MessageBox.Show("Account not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (BCrypt.Net.BCrypt.EnhancedVerify(txtPassword.Text, hash))
                {
                    account = sql.GetAccount(txtAccountNumber.Text, hash);
                    if(account.DeletedState==false)
                    {
                        account.PinHash = null;
                        new LoggedIn(account,sql).ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("This account has been suspended or deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            Cursor.Current = Cursors.Default;
        }
        private void Login(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin_Click(sender, null);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            database.Connect(); // the first database connection takes longer, and since the first query already takes long its better to connect once before instead of connecting while doing the long query
            txtAccountNumber.Enabled = true;
            txtPassword.Enabled = true;
            Cursor.Current = Cursors.Default;
        }
    }
}
