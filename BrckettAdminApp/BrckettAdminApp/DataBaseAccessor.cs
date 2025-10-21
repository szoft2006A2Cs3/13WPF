using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;

namespace BrckettAdminApp
{
    class DataBaseAccessor
    {
        private string connectionString = "server=localhost;port=3306;uid=root;pwd=;database=testadmin";
        private string _dbName = "testadmin";
        private MySqlConnection _conn;
        public DataBaseAccessor()
        {
            connectionString = "server=localhost;port=3306;uid=root;pwd=;database=testadmin";
            _dbName = "testadmin";
            _conn = new MySqlConnection(connectionString);
            _conn.Open();
        }
        public DataBaseAccessor(string dbName, string dbPassword = "")
        {
            connectionString = $"server=localhost;port=3306;uid=root;pwd={dbPassword};database={dbName}";
            _dbName = dbName;
            _conn = new MySqlConnection(connectionString);
            _conn.Open();
        }
        public string InsertInto(Table table, List<string> data)
        {
            string res = "";
            using (MySqlCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = $"INSERT INTO {table.TableName}({string.Join(",", table.FieldNames.Keys)}) VALUES(";
                
                for (int i = 0;i<data.Count();i++) 
                {
                    if (table.FieldNames.Values.ToList()[i] == "System.Int32")
                    {
                        if (int.TryParse(data[i], out _))
                        {
                            cmd.CommandText += $"{data[i]},";
                        }
                        else 
                        {
                            cmd.CommandText += $"0";
                        }
                            
                    }
                    else
                    {
                        cmd.CommandText += $"\"{data[i]}\",";
                    }
                }
                cmd.CommandText = cmd.CommandText.Substring(0,cmd.CommandText.Length-1);
                
                
                
                
                cmd.CommandText += ")";
                //MessageBox.Show(cmd.CommandText);
                res += $"{cmd.ExecuteNonQuery()}. Rows Affected";

            }
            if (res == "")
            {
                res = "No Changes";
            }
            return res;
        }
        public string Update(string tableName, string pkField, string pk, Dictionary<string, string> fieldValuePairs)
        {
            string res = "";
            using (MySqlCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = $"UPDATE {tableName} SET {fieldValuePairs.Keys.First()}={fieldValuePairs[fieldValuePairs.Keys.First()]}";
                foreach (var field in fieldValuePairs.Keys.Skip(1))
                {
                    cmd.CommandText += $",{field}={fieldValuePairs[field]}";
                }
                cmd.CommandText += $" WHERE {pkField}={pk}";

                res += $"{cmd.ExecuteNonQuery()}. Rows Affected";

            }
            if (res == "")
            {
                res = "No Changes";
            }
            return res;
        }
        public Dictionary<string, Dictionary<string, string>> Read(string tableName, string pkFieldName)
        {
            Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();
            using (MySqlCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = $"SELECT {pkFieldName} FROM {tableName}";
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result[$"{reader.GetValue(0)}"] = new Dictionary<string, string>();
                    }
                }


                cmd.CommandText = $"SELECT * FROM {tableName}";
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        Dictionary<string, string> temp = new Dictionary<string, string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string fieldName = reader.GetName(i);
                            string fieldValue = reader.IsDBNull(i) ? "NULL" : reader.GetValue(i).ToString();

                            temp.Add(fieldName,fieldValue);
                        }
                        result[temp[pkFieldName]] = temp;
                    }
                    
                }
            }

            return result;
        }

        public string Delete(Table table, string pk)
        {
            string res = "";
            using (MySqlCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = $"DELETE FROM {table.TableName} WHERE {table.pkFieldName} = ";
                if (table.FieldNames[table.pkFieldName] == "System.Int32") 
                {
                    cmd.CommandText +=$"{pk}";
                }
                else
                {
                   cmd.CommandText += $"\"{pk}\"";
                }
                
                res += $"{cmd.ExecuteNonQuery()}. Rows Affected";

            }
            if (res == "")
            {
                res = "No Changes";
            }
            return res;
        }
        public bool CloseDbConn()
        {
            try
            {
                _conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public string GetPrimaryKeyName(string tableName)
        {
            string res = "";
            using (MySqlCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '{_dbName}' AND TABLE_NAME = '{tableName}' AND COLUMN_KEY = 'PRI'";
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res = reader.GetString(0);
                    }

                }
            }
            return res;
        }

        public Dictionary<string,string> GetFieldNames(string tableName)
        {
            Dictionary<string,string> res = new Dictionary<string,string>();
            using (MySqlCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '{_dbName}' AND TABLE_NAME = '{tableName}'";
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res.Add(reader.GetString(0), "");
                    }
                }
                cmd.CommandText = $"SELECT * FROM {tableName} LIMIT 1";
                using (MySqlDataReader reader = cmd.ExecuteReader()) 
                {
                    while (reader.Read()) 
                    {
                        for (int i = 0; i < reader.FieldCount; i++) 
                        {
                            res[reader.GetName(i)] = $"{reader.GetFieldType(i)}";
                        }
                    }
                }
            }
            return res;
        }
        public List<string> GetTableNames()
        {
            List<string> res = new List<string>();
            using (MySqlCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{_dbName}'";
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res.Add(reader.GetString(0));
                    }
                }
            }
            return res;
        }
    }
}
