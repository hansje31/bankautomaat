using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class TableName : System.Attribute
    {
        public string value;
        public TableName(string value)
        {
            this.value = value;
        }
    }
}
