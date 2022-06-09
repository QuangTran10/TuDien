using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuDien
{
    public class ManageDictionary
    {
        public string convert(string data)
        {
            var windows1252 = Encoding.GetEncoding(1252);
            var utf8Bytes = windows1252.GetBytes(data);
            var correct = Encoding.UTF8.GetString(utf8Bytes);
            return correct;
        }

        public ArrayList findWord(MySqlConnection conn, string findwords, int option)
        {
            ArrayList listwords = new ArrayList();
            String sql="";
            findwords = findwords.TrimStart();
            if (option == 0)
            {
                sql = "select * from mst_translate_mean where tm_vietnamese_translate like '%" + findwords+ "%'"
                    + "or tm_english_translate like '%"+ findwords 
                    + "%'or tm_example like '%" + findwords + "%'";
            }else if (option == 1)
            {
                sql = "select * from mst_translate_mean where tm_japanese_hiragana like '%" + findwords +
                    "%' or tm_japanese_translate like '%" + findwords + 
                    "%' or tm_example like '%" + findwords +"%'";
            }

            MySqlCommand command = new MySqlCommand();

            command.Connection = conn;

            command.CommandText = sql;

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Dictionary dic = new Dictionary();
                        dic.Tm_id = reader.GetInt32(0);
                        if (reader.IsDBNull(2))
                        {
                            dic.Tm_japanese_translate = "";
                        }
                        else
                        {
                            dic.Tm_japanese_translate = reader.GetString(2);
                        }

                        dic.Tm_japanese_hiragana = reader.GetString(3);
                        dic.Tm_vietnamese_tranlate = reader.GetString(4);

                        if (reader.IsDBNull(5))
                        {
                            dic.Tm_english_tranlate = "";
                        }
                        else
                        {
                            dic.Tm_english_tranlate = reader.GetString(5);
                        }

                        if (reader.IsDBNull(6))
                        {
                            dic.Tm_example = "";
                        }
                        else
                        {
                            dic.Tm_example = reader.GetString(6);
                        }

                        listwords.Add(dic);
                    }
                }
            }

            return listwords;
        }
        public ArrayList findAllWord(MySqlConnection conn, string findwords)
        {
            ArrayList listwords = new ArrayList();
            String sql = "";

            sql = "";
            
            MySqlCommand command = new MySqlCommand();

            command.Connection = conn;

            command.CommandText = sql;

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Dictionary dic = new Dictionary();
                        dic.Tm_id = reader.GetInt32(0);
                        if (reader.IsDBNull(2))
                        {
                            dic.Tm_japanese_translate = "";
                        }
                        else
                        {
                            dic.Tm_japanese_translate = reader.GetString(2);
                        }

                        dic.Tm_japanese_hiragana = reader.GetString(3);
                        dic.Tm_vietnamese_tranlate = reader.GetString(4);

                        if (reader.IsDBNull(5))
                        {
                            dic.Tm_english_tranlate = "";
                        }
                        else
                        {
                            dic.Tm_english_tranlate = reader.GetString(5);
                        }

                        if (reader.IsDBNull(6))
                        {
                            dic.Tm_example = "";
                        }
                        else
                        {
                            dic.Tm_example = reader.GetString(6);
                        }

                        listwords.Add(dic);
                    }
                }
            }

            return listwords;
        }

        public int save(MySqlConnection conn, Dictionary dic)
        {
            int rows=-1;
            try
            {
                string Query = "insert into mst_translate_mean(sec_id, tm_japanese_translate, tm_japanese_hiragana, tm_vietnamese_translate, tm_english_translate, tm_example, tm_insert_user, tm_flag) " +
                    "values(@secId, @japaneseTranslate, @japaneseHiragana, @vietnameseTranslate, @englishTranslate, @example, @insertUser, @flag);";
                MySqlCommand command = new MySqlCommand(Query, conn);

                command.Parameters.AddWithValue("@secId", 1);
                command.Parameters.AddWithValue("@japaneseTranslate",dic.Tm_japanese_translate);
                command.Parameters.AddWithValue("@japaneseHiragana", dic.Tm_japanese_hiragana);
                command.Parameters.AddWithValue("@vietnameseTranslate", dic.Tm_vietnamese_tranlate);
                command.Parameters.AddWithValue("@englishTranslate", dic.Tm_english_tranlate);
                command.Parameters.AddWithValue("@example", dic.Tm_example);
                command.Parameters.AddWithValue("@insertUser", 1);
                command.Parameters.AddWithValue("@flag", 1);

                rows = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return rows;
        }

    }
}
