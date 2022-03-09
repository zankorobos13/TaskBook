using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;

namespace TaskBook
{
    
    public class DB
    {
        readonly MySqlConnection connection = new MySqlConnection("server=remotemysql.com;port=3306;username=FFoXo8zLEg;password=22HWTsEDuI;database=FFoXo8zLEg");

        public static string Login(string login, string password)
        {
            password = Encrypt.Sha256(password);

            try
            {
                DB db = new DB();
                MySqlConnection connection = db.GetConnection();
                db.OpenConnection();
                string sql_command = "SELECT * FROM `users` WHERE login = '" + login + "'";
                MySqlCommand command = new MySqlCommand(sql_command, connection);
                MySqlDataReader reader = command.ExecuteReader();

                string temp_login = null;
                string temp_password = null;

                while (reader.Read())
                {
                    temp_login = reader[0].ToString();
                    temp_password = reader[1].ToString();
                }

                db.CloseConnection();

                if (temp_login == login && temp_password == password)
                    return "ok";
                else
                    return "no";
            }
            catch (Exception)
            {
                return "error";
            }
        }

        public static string Register(string login, string password)
        {
            try
            {
                DB db = new DB();
                MySqlConnection connection = db.GetConnection();
                db.OpenConnection();
                string sql_command = "SELECT * FROM `users` WHERE login = '" + login + "'";
                MySqlCommand command = new MySqlCommand(sql_command, connection);
                MySqlDataReader reader = command.ExecuteReader();

                string temp = null;

                while (reader.Read())
                {
                    temp = reader[0].ToString();
                }

                db.CloseConnection();

                if (temp != null)
                {
                    return "repeat";
                }
                else
                {
                    db.OpenConnection();
                    sql_command = "INSERT INTO `users` (login, password) VALUES ('" + login + "', '" + Encrypt.Sha256(password) + "')";
                    command = new MySqlCommand(sql_command, connection);
                    command.ExecuteNonQuery();

                    return "ok";
                }

            }
            catch (Exception)
            {
                return "error";
            }
            

        }

        public void OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }

        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        public MySqlConnection GetConnection()
        {
            return connection;
        }
    }
}
