using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication
{
    public class SQL
    {

        // make sure you can access/use the Database.cs files functions in order to 'launch' these SQL queries so you can change the database
        Database database;
        public SQL(Database db)
        {
            database = db;
        }
        public BankDetails GetAccount(string accountNumber,string pinHash)
        {
            return BankDetails.LoadAccount(database.QueryToDictionary(String.Format("select * from bankdetails where bank_rekeningnummer = '{0}' AND pin = '{1}'",accountNumber,pinHash), ClassReader.ClassToDictionary(typeof(BankDetails))));
        }
        public List<Transaction> GetLatestTransactions(int accountID)
        {
            return Transaction.GetTransactions(database.QueryToDictionaries("select * from (select * from transacties ORDER BY DATE(datum) DESC) sub ORDER BY datum DESC LIMIT 3", ClassReader.ClassToDictionary(typeof(Transaction))));
        }
        public string GetHash(string accountNumber)
        {
            return database.SingleResultQuery(String.Format("select pin from bankdetails where bank_rekeningnummer = '{0}'",accountNumber));
        }
        public string GetUserName(string id)
        {
            return database.SingleResultQuery("select voornaam from user where user_id = " + id);
        }
        public void InsertClass(Type t, object instance)
        {
            var collumns = ClassReader.FieldNames(t);
            var values = ClassReader.ValuesList(t, instance);
            var sb = new StringBuilder();
            var sb2 = new StringBuilder();
            foreach(string collumn in collumns)
            {
                sb.Append(collumn + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            foreach (string value in values)
            {
                if(value=="")
                    sb2.Append("'0'" + ",");
                else
                    sb2.Append("'"+value+"'" + ",");
            }
            sb2.Remove(sb2.Length - 1, 1);
            var tablename = t.GetCustomAttributes(true).OfType<TableName>().First().value;
            string query = String.Format("INSERT INTO {0} ({1}) VALUES({2})",tablename,sb.ToString(),sb2.ToString());
            database.CustomQuery(query);
        }
        public void UpdatePin(string newPin, string id)
        {
            database.CustomQuery(String.Format("UPDATE bankdetails SET pin='{0}' WHERE user_id={1}", newPin, id));
        }
        public void UpdateSaldo(double value, int id)
        {
            database.CustomQuery(String.Format("UPDATE bankdetails SET saldo = saldo + {0} WHERE bank_nummer_id={1}",value,id));
        }
        public string GetSaldo(int id)
        {
            return database.SingleResultQuery(String.Format("SELECT saldo from bankdetails WHERE bank_nummer_id={0}",id));
        }
        public string DailyWithdrawCount(int id)
        {
            return database.SingleResultQuery(String.Format("SELECT COUNT(bank_nummer_id) as transactions from transacties where DATE(datum) = '{0}' AND type='Withdraw' AND bank_nummer_id={1}",new Date().Now().ToString("yyyy-MM-dd"),id));
        }
    }
}
