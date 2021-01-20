﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAdmin
{
    public class SQL
    {

        // make sure you can access/use the Database.cs files functions in order to 'launch' these SQL queries so you can change the database
        Database database;
        public SQL(Database db)
        {
            database = db;
        }

        public User GetSingleUser(string id)
        {
            return User.LoadUser(database.QueryToDictionary("select * from user where user_id = "+id, ClassReader.ClassToDictionary(typeof(User))));
        }
        public List<string> GetUserNames()
        {
            return database.List("select voornaam from user", "voornaam");
        }
        public List<User> GetUserNamesBankNumbers()
        {
            var result = new List<User>();
            var users = database.QueryToDictionaries("select user.user_id, voornaam, bank_rekeningnummer from user inner join bankdetails on user.user_id = bankdetails.user_id AND bankdetails.deleted_state = 0", new Dictionary<string, string>() { { "voornaam", "FirstName" }, { "user_id", "UserId" } }, "bank_rekeningnummer"); //you can use inline custom dictionaries to query specific collumns and save memory
            foreach(var user in users)
            {
                result.Add(User.LoadUser(user,"bank_rekeningnummer"));
            }
            return result;
        }
        public List<User> GetDeletedUserNamesBankNumbers()
        {
            var result = new List<User>();
            var users = database.QueryToDictionaries("select user.user_id, voornaam, bank_rekeningnummer from user inner join bankdetails on user.user_id = bankdetails.user_id AND bankdetails.deleted_state = 1", new Dictionary<string, string>() { { "voornaam", "FirstName" }, { "user_id", "UserId" } }, "bank_rekeningnummer"); //you can use inline custom dictionaries to query specific collumns and save memory
            foreach (var user in users)
            {
                result.Add(User.LoadUser(user, "bank_rekeningnummer"));
            }
            return result;
        }
        public string LastId(string table, string idColName)
        {
            return database.SingleResultQuery(String.Format("select MAX({0}) from {1}", idColName, table));
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
        public void EditClass(Type t, object instance)
        {
            Field Id = ClassReader.IdInfo(t, instance);
            var collumns = ClassReader.FieldNames(t);
            var values = ClassReader.ValuesList(t, instance);
            var sb = new StringBuilder();
            for (int i = 0; i < collumns.Count; i++)
            {
                sb.Append(collumns[i] + "=" + "'"+values[i]+"'" + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            var tablename = t.GetCustomAttributes(true).OfType<TableName>().First().value;
            string query = String.Format("UPDATE {0} SET {1} WHERE {2}={3}", tablename, sb.ToString(), Id.Name, Id.Value);
            database.CustomQuery(query);
        }
        public void UpdatePin(string newPin, string id)
        {
            database.CustomQuery(String.Format("UPDATE bankdetails SET pin='{0}' WHERE user_id={1}", newPin, id));
        }
        public void ChangeAccountState(string id)
        {
            database.CustomQuery(String.Format("UPDATE bankdetails SET deleted_state=NOT deleted_state WHERE user_id={0}", id));
        }

    }
}
