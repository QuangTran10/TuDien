using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuDien
{
    public class ConnectDB
    {
        public static MySqlConnection Connect()
        {
            MySqlConnection conn = null;
            string ip="", user="", pass = "", database = "";
            int port=3306;
            if (File.Exists("dic.txt"))
            {
                string[] db = File.ReadAllLines("dic.txt");
                ip = Md5.Decrypt(db[0]);
                port =  Convert.ToInt32(Md5.Decrypt(db[1]));
                user = Md5.Decrypt(db[2]);
                pass = Md5.Decrypt(db[3]);
                database = Md5.Decrypt(db[4]);
            }
            
            return conn = Connection.getDBConnection(ip, port, database, user, pass);
        }
    }
}
