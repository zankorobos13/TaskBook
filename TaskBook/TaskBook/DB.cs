using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;
using Xamarin.Essentials;


namespace TaskBook
{

    public class DB
    {
        // public readonly MySqlConnection connection = new MySqlConnection("server=remotemysql.com;port=3306;username=FFoXo8zLEg;password=22HWTsEDuI;database=FFoXo8zLEg");

        public readonly MySqlConnection connection = new MySqlConnection("server=sql8.freemysqlhosting.net;port=3306;username=sql8593991;password=ncCxPsNXdE;database=sql8593991");


        public static string IncreaseUser(string username)
        {
            try
            {
                DB db = new DB();
                MySqlConnection connection = db.GetConnection();
                db.OpenConnection();
                string sql_command = "UPDATE `users` SET role = 'admin' WHERE login = '" + username + "'";
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

        public static Task[] GetTasks(string username)
        {
            try
            {
                DB db = new DB();
                MySqlConnection connection = db.GetConnection();
                db.OpenConnection();
                string sql_command = "SELECT * FROM `tasks` WHERE worker = '" + username + "' AND room = '" + Preferences.Get("room", null) + "'";
                MySqlCommand command = new MySqlCommand(sql_command, connection);
                MySqlDataReader reader = command.ExecuteReader();

                Task[] tasks = new Task[0];

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

                    Task[] new_tasks = new Task[tasks.Length + 1];

                    for (int i = 0; i < tasks.Length; i++)
                    {
                        new_tasks[i] = tasks[i];
                    }

                    new_tasks[tasks.Length] = task;
                    tasks = new_tasks;
                }

                db.CloseConnection();

                return tasks;
            }
            catch (Exception)
            {
                return new Task[0];
            }
        }

        public static string[] GetUsers()
        {
            try
            {
                DB db = new DB();
                MySqlConnection connection = db.GetConnection();
                db.OpenConnection();
                string sql_command = "SELECT * FROM `users` WHERE room = '" + Preferences.Get("room", null) + "' AND role = 'user'";
                MySqlCommand command = new MySqlCommand(sql_command, connection);
                MySqlDataReader reader = command.ExecuteReader();

                string[] users = new string[0];

                while (reader.Read())
                {
                    string[] new_users = new string[users.Length + 1];

                    for (int i = 0; i < users.Length; i++)
                    {
                        new_users[i] = users[i];
                    }

                    new_users[users.Length] = reader[0].ToString();
                    users = new_users;
                }

                db.CloseConnection();

                return users;
            }
            catch (Exception)
            {
                return new string[0];
            }
        }

        public static string DeleteTask(string header)
        {
            try
            {
                DB db = new DB();
                MySqlConnection connection = db.GetConnection();
                db.OpenConnection();
                string sql_command = "DELETE FROM `tasks` WHERE header = '" + header + "' AND room = '" + Preferences.Get("room", null) + "'";
                MySqlCommand command = new MySqlCommand(sql_command, connection);
                command.ExecuteNonQuery();
                db.CloseConnection();

                int id = 0;

                for (int i = 0; i < Task.tasks.Length; i++)
                {
                    if (Task.tasks[i].header == Task.current_task.header)
                    {
                        id = i;
                        break;
                    }
                }

                Task[] new_tasks = new Task[Task.tasks.Length - 1];

                for (int i = 0; i < id; i++)
                {
                    new_tasks[i] = Task.tasks[i];
                }

                for (int i = id; i < new_tasks.Length; i++)
                {
                    new_tasks[i] = Task.tasks[i + 1];
                }

                Task.tasks = new_tasks;

                return "ok";
            }
            catch (Exception)
            {
                return "error";
            }
        }

        public static string CompleteTask(string header)
        {
            try
            {
                DB db = new DB();
                MySqlConnection connection = db.GetConnection();
                db.OpenConnection();
                string sql_command = "UPDATE `tasks` SET completed_status = true WHERE header = '" + header + "' AND room = '" + Preferences.Get("room", null) + "'";
                MySqlCommand command = new MySqlCommand(sql_command, connection);
                command.ExecuteNonQuery();
                db.CloseConnection();

                int id = 0;

                for (int i = 0; i < Task.tasks.Length; i++)
                {
                    if (Task.tasks[i].header == Task.current_task.header)
                    {
                        id = i;
                        break;
                    }
                }

                Task[] new_tasks = new Task[Task.tasks.Length - 1];

                for (int i = 0; i < id; i++)
                {
                    new_tasks[i] = Task.tasks[i];
                }

                for (int i = id; i < new_tasks.Length; i++)
                {
                    new_tasks[i] = Task.tasks[i + 1];
                }

                Task.tasks = new_tasks;

                return "ok";
            }
            catch (Exception)
            {
                return "error";
            }
        }

        public static string DeclineTask(string header)
        {
            try
            {
                DB db = new DB();
                MySqlConnection connection = db.GetConnection();
                db.OpenConnection();
                string sql_command = "UPDATE `tasks` SET worker = NULL WHERE header = '" + header + "' AND room = '" + Preferences.Get("room", null) + "'";
                MySqlCommand command = new MySqlCommand(sql_command, connection);
                command.ExecuteNonQuery();
                db.CloseConnection();

                foreach (Task item in Task.tasks)
                {
                    if (item.header == Task.current_task.header)
                    {
                        item.worker = null;
                        break;
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
                string sql_command = "UPDATE `tasks` SET worker = '" + Preferences.Get("login", null) + "' WHERE header = '" + header + "' AND room = '" + Preferences.Get("room", null) + "'";
                MySqlCommand command = new MySqlCommand(sql_command, connection);
                command.ExecuteNonQuery();
                db.CloseConnection();

                foreach (Task item in Task.tasks)
                {
                    if (item.header == Task.current_task.header)
                    {
                        item.worker = Preferences.Get("login", null);
                        break;
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

                    if (Preferences.Get("role", null) == "admin" || task.completed_status == false)
                    {
                        Task.AddTask(task);
                    }
                }

                Array.Reverse(Task.tasks);
                db.CloseConnection();

                return "ok";
            }
            catch (Exception)
            {
                Task[] a = Task.tasks;
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

                Task.AddTaskToFirst(new Task()
                {
                    header = header,
                    text = text,
                    priority = priority,
                    deadline = deadline,
                    room = Preferences.Get("room", null),
                    worker = null,
                    completed_status = false,
                });

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
