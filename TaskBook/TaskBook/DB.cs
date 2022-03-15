using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;
using Xamarin.Essentials;

namespace TaskBook
{

    public class DB
    {
        public readonly MySqlConnection connection = new MySqlConnection("server=remotemysql.com;port=3306;username=FFoXo8zLEg;password=22HWTsEDuI;database=FFoXo8zLEg");

        public static string LeaveRoom()
        {
            try
            {
                DB db = new DB();
                MySqlConnection connection = db.GetConnection();
                db.OpenConnection();
                string sql_command = "UPDATE `users` SET room = NULL, role = NULL WHERE login = '" + Preferences.Get("login", null) + "'";
                MySqlCommand command = new MySqlCommand(sql_command, connection);
                command.ExecuteNonQuery();
                db.CloseConnection();

                return "ok";
            }
            catch (Exception)
            {
                return "error";
            }
        }

        public static string EnterRoom(string name, string password)
        {
            password = Encrypt.Sha256(password);

            try
            {
                DB db = new DB();
                MySqlConnection connection = db.GetConnection();
                db.OpenConnection();
                string sql_command = "SELECT * FROM `rooms` WHERE name = '" + name + "'";
                MySqlCommand command = new MySqlCommand(sql_command, connection);
                MySqlDataReader reader = command.ExecuteReader();

                string temp_name = null;
                string temp_password = null;

                while (reader.Read())
                {
                    temp_name = reader[0].ToString();
                    temp_password = reader[1].ToString();
                }

                db.CloseConnection();

                if (temp_name == name && temp_password == password)
                {
                    db.OpenConnection();
                    sql_command = "UPDATE `users` SET room = '" + name + "', role = 'user' WHERE login = '" + Preferences.Get("login", null) + "'";
                    command = new MySqlCommand(sql_command, connection);
                    command.ExecuteNonQuery();
                    db.CloseConnection();

                    return "ok";
                }
                else
                {
                    return "no";
                }
            }
            catch (Exception)
            {
                return "error";
            }

        }

        public static string AddRoom(string name, string password)
        {
            try
            {
                DB db = new DB();
                MySqlConnection connection = db.GetConnection();
                db.OpenConnection();
                string sql_command = "SELECT * FROM `rooms` WHERE name = '" + name + "'";
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
                    sql_command = "INSERT INTO `rooms` (name, password) VALUES ('" + name + "', '" + Encrypt.Sha256(password) + "'); UPDATE `users` SET room = '" + name + "', role = 'admin' WHERE login = '" + Preferences.Get("login", null) + "'";
                    command = new MySqlCommand(sql_command, connection);
                    command.ExecuteNonQuery();
                    db.CloseConnection();

                    return "ok";
                }
            }
            catch (Exception)
            {
                return "error";
            }




        }

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

                    if (reader[2].ToString() == null || reader[2].ToString().Trim(' ') == "")
                        Preferences.Set("room", null);
                    else
                        Preferences.Set("room", reader[2].ToString());

                    if (reader[3].ToString() == null || reader[3].ToString().Trim(' ') == "")
                        Preferences.Set("role", null);
                    else
                        Preferences.Set("role", reader[3].ToString());
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
                    db.CloseConnection();

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
