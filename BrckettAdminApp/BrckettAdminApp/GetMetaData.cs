using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BrckettAdminApp
{
    class GetMetaData
    {
        private DataBaseAccessor _dba;

        public GetMetaData(DataBaseAccessor dba)
        {
            _dba = dba;
        }

    }
}
