using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;

namespace TaskBook
{
    // Класс для работы с базой данных 
    public class DB
    {
        readonly MySqlConnection connection = new MySqlConnection("server=remotemysql.com;port=3306;username=NqaN7lnsn5;password=WGxtoGYopq;database=NqaN7lnsn5");

        // Открыть соединение
        public void OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }
        // Закрыть соединение
        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }
        // Вернуть соединение
        public MySqlConnection GetConnection()
        {
            return connection;
        }
    }
}
