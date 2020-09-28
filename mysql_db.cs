using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DbDocjc
{
    public class mysql_db
    {
        private MySqlConnection conn { get; set; }

        public bool hasError { get; private set; }
        public string ErrorMsg { get; private set; }

        public mysql_db()
        {

        }
    }
}
