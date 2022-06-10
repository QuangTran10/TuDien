﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TuDien
{
    class Connection
    {
        public static MySqlConnection getDBConnection(string host, int port, string database, string username, string password)
        {
            String connString = "Server=" + host + ";Database=" + database
                + ";Port=" + port + ";User Id=" + username + ";Password=" + password+ ";charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connString);

            return conn;
        }
    }
}
