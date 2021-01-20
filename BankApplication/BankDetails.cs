using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication
{
    [TableName("bankdetails")]
    public class BankDetails
    {
        [FieldName("bank_nummer_id")]
        public int AccountID { get; set; }

        [FieldName("bank_rekeningnummer")]
        public int AccountNumber { get; set; }

        [FieldName("deleted_state")]
        public bool DeletedState { get; set; }

        [FieldName("saldo")]
        public double Balance { get; set; }

        [FieldName("pin")]
        public string PinHash { get; set; }

        [FieldName("user_id")]
        public string UserID { get; set; }

        public static BankDetails LoadAccount(Dictionary<string, string> dict)
        {
            var account = new BankDetails();
            PropertyInfo[] props = typeof(BankDetails).GetProperties();
            foreach (PropertyInfo prop in props.Where(x => x.CustomAttributes.Count() != 0).Where(x => x.CustomAttributes.First().AttributeType == typeof(FieldName)))
            {
                if (dict.ContainsKey(prop.Name))
                {
                    if (prop.PropertyType == typeof(int))
                    {
                        prop.SetValue(account, int.Parse(dict[prop.Name]));
                    }
                    else if(prop.PropertyType == typeof(double))
                    {
                        prop.SetValue(account, double.Parse(dict[prop.Name]));
                    }
                    else if(prop.PropertyType == typeof(bool))
                    {
                        prop.SetValue(account, bool.Parse(dict[prop.Name]));
                    }
                    else
                    {
                        prop.SetValue(account, dict[prop.Name]);
                    }
                }
            }
            return account;
        }
    }

}
