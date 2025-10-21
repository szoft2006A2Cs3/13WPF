using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrckettAdminApp
{
    class Table
    {
        public string TableName { get; set; }
        public string pkFieldName { get; set; }
        public List<string> FieldNames { get; set; }

        public Table(string tableName, DataBaseAccessor _db)
        {
            TableName = tableName;
            pkFieldName = _db.GetPrimaryKeyName(tableName);
            FieldNames = _db.GetFieldNames(tableName);
        }
    }
}
