﻿using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;
using Xamarin.Essentials;


namespace TaskBook
{

    public class DB
    {
        public readonly MySqlConnection connection = new MySqlConnection("server=remotemysql.com;port=3306;username=FFoXo8zLEg;password=22HWTsEDuI;database=FFoXo8zLEg");

        public static string DeclineTask(string header)
        {
            try
            {
                DB db = new DB();
                MySqlConnection connection = db.GetConnection();
                db.OpenConnection();
                string sql_command = "UPDATE `tasks` SET worker = NULL WHERE header = '" + header + "'";
                MySqlCommand command = new MySqlCommand(sql_command, connection);
                command.ExecuteNonQuery();
                db.CloseConnection();

                foreach (Task item in Task.tasks)
                {
                    if (item.header == Task.current_task.header)
                    {
                        item.worker = null;
                    }
                }

                return "ok";
            }
            catch (Exception)
            {
                return "error";
            }
        }

        public static string AcceptTask(string header)
        {
            try
            {
                DB db = new DB();
                MySqlConnection connection = db.GetConnection();
                db.OpenConnection();
                string sql_command = "UPDATE `tasks` SET worker = '" + Preferences.Get("login", null) + "' WHERE header = '" + header + "'";
                MySqlCommand command = new MySqlCommand(sql_command, connection);
                command.ExecuteNonQuery();
                db.CloseConnection();

                foreach (Task item in Task.tasks)
                {
                    if (item.header == Task.current_task.header)
                    {
                        item.worker = Preferences.Get("login", null);
                    }
                }

                return "ok";
            }
            catch (Exception)
            {
                return "error";
            }
        }

        public static string GetTasks()
        {
            try
            {
                DB db = new DB();
                MySqlConnection connection = db.GetConnection();
                db.OpenConnection();
                string sql_command = "SELECT * FROM `tasks` WHERE room = '" + Preferences.Get("room", null) + "'";
                MySqlCommand command = new MySqlCommand(sql_command, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Task task = new Task()
                    {
                        header = reader[0].ToString(),
                        text = reader[1].ToString(),
                        worker = reader[2].ToString(),
                        priority = int.Parse(reader[3].ToString()),
                        room = reader[4].ToString(),
                        completed_status = bool.Parse(reader[5].ToString()),
                        deadline = reader[6].ToString()
                    };

                    if (task.worker.Trim(' ') == "")
                    {
                        task.worker = null;
                    }

                    if (task.completed_status == false)
                    {
                        Task.AddTask(task);
                    }
                }
                return "ok";
            }
            catch (Exception)
            {
                return "error";
            }
        }

        public static string AddTask(string header, string text, int priority, string deadline)
        {
            try
            {
                DB db = new DB();
                MySqlConnection connection = db.GetConnection();
                db.OpenConnection();
                string sql_command = "INSERT INTO `tasks` (header, text, room, priority, deadline) VALUES ('" + header + "', '" + text + "', '" + Preferences.Get("room", null) + "', " + priority + ", '" + deadline + "');";
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
