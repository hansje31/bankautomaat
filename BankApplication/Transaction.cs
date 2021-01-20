using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication
{
    [TableName("transacties")]
    public class Transaction
    {
        [FieldName("transactie_id",true)]
        public int TransactionID { get; set; }

        [FieldName("datum")]
        public string Date { get; set; }
        
        [FieldName("type")]
        public string Type { get; set; }

        [FieldName("bedrag")]
        public double Value { get; set; }

        [FieldName("bank_nummer_id")]
        public int BankID { get; set; }

        public static List<Transaction> GetTransactions(List<Dictionary<string, string>> dicts)
        {
            var result = new List<Transaction>();
            foreach (var dict in dicts)
            {
                var transaction = new Transaction();
                PropertyInfo[] props = typeof(Transaction).GetProperties();
                foreach (PropertyInfo prop in props.Where(x => x.CustomAttributes.Count() != 0).Where(x => x.CustomAttributes.First().AttributeType == typeof(FieldName)))
                {
                    if (dict.ContainsKey(prop.Name))
                    {
                        if (prop.PropertyType == typeof(int))
                        {
                            prop.SetValue(transaction, int.Parse(dict[prop.Name]));
                        }
                        else if (prop.PropertyType == typeof(double))
                        {
                            prop.SetValue(transaction, double.Parse(dict[prop.Name]));
                        }
                        else if (prop.PropertyType == typeof(bool))
                        {
                            prop.SetValue(transaction, bool.Parse(dict[prop.Name]));
                        }
                        else
                        {
                            prop.SetValue(transaction, dict[prop.Name]);
                        }
                    }
                }
                result.Add(transaction);
            }
            return result;
        }
    }
}
