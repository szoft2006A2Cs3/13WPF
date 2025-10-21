using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace BrckettAdminApp
{
    class GetMetaData
    {
        public List<string> TableNames;
        public List<Table> Tables = new List<Table>();
        private DataBaseAccessor _dba;

        public GetMetaData(DataBaseAccessor dba)
        {
            _dba = dba;
            TableNames = _dba.GetTableNames();
            foreach(var _tableName in TableNames)
            {
                Tables.Add(new Table(_tableName, _dba));
            }
            MessageBox.Show($"Metadata Loaded{Tables.Count()}");
        }
        
    }
}
