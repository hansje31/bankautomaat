using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication
{
    public class Date
    {
        public DateTime Now() 
        {
            var request = (HttpWebRequest)WebRequest.Create("http://www.google.com");
            var response = request.GetResponse();
            string date = response.Headers["date"];
            return DateTime.Parse(date);
        }
    }
                
}
