using System;
using System.Collections.Generic;
using System.Text;

namespace TaskBook
{
    public class Task
    {
        public string header;
        public string text;
        public string worker;
        public int priority;
        public string room;
        public bool completed_status;
        public string deadline;

        public static Task[] tasks = new Task[0];
        public static Task current_task;

        public static string CreateTask(Task task)
        {
            return DB.AddTask(task.header, task.text, task.priority, task.deadline);
        }

        public static void ClearTasks()
        {
            tasks = new Task[0];
        }

        public static void AddTask(Task task)
        {
            Task[] new_tasks = new Task[tasks.Length + 1];
            for (int i = 0; i < tasks.Length; i++)
            {
                new_tasks[i] = tasks[i];
            }
            new_tasks[new_tasks.Length - 1] = task;
            tasks = new_tasks;
        }

        public static void AddTaskToFirst(Task task)
        {
            Task[] new_tasks = new Task[tasks.Length + 1];
            new_tasks[0] = task;

            for (int i = 0; i < tasks.Length; i++)
            {
                new_tasks[i + 1] = tasks[i];
            }
            
            tasks = new_tasks;
        }

        public static void SortByCreation(bool NewFirst = true)
        {
            ClearTasks();
            DB.GetTasks();

            if (!NewFirst)
            {
                Array.Reverse(tasks);
            }
        }

        public static void SortByDateTime(bool toLower = true)
        {
            ClearTasks();
            DB.GetTasks();

            if (toLower)
            {
                for (int i = 0; i < tasks.Length - 1; i++)
                {
                    for (int j = 0; j < tasks.Length - 1; j++)
                    {
                        string[] d = tasks[j].deadline.Split(' ');
                        DateTime d1 = new DateTime(2000 + int.Parse(d[2]), int.Parse(d[1]), int.Parse(d[0]), int.Parse(d[3]), int.Parse(d[4]), 0);
                        d = tasks[j + 1].deadline.Split(' ');
                        DateTime d2 = new DateTime(2000 + int.Parse(d[2]), int.Parse(d[1]), int.Parse(d[0]), int.Parse(d[3]), int.Parse(d[4]), 0);

                        if (DateTime.Compare(d1, d2) < 0)
                        {
                            Task temp = tasks[j];
                            tasks[j] = tasks[j + 1];
                            tasks[j + 1] = temp;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < tasks.Length - 1; i++)
                {
                    for (int j = 0; j < tasks.Length - 1; j++)
                    {
                        string[] d = tasks[j].deadline.Split(' ');
                        DateTime d1 = new DateTime(2000 + int.Parse(d[2]), int.Parse(d[1]), int.Parse(d[0]), int.Parse(d[3]), int.Parse(d[4]), 0);
                        d = tasks[j + 1].deadline.Split(' ');
                        DateTime d2 = new DateTime(2000 + int.Parse(d[2]), int.Parse(d[1]), int.Parse(d[0]), int.Parse(d[3]), int.Parse(d[4]), 0);

                        if (DateTime.Compare(d1, d2) > 0)
                        {
                            Task temp = tasks[j];
                            tasks[j] = tasks[j + 1];
                            tasks[j + 1] = temp;
                        }
                    }
                }
            }
        }

        public static void SortByPriority(bool toLower = true)
        {
            ClearTasks();
            DB.GetTasks();

            if (toLower)
            {
                for (int i = 0; i < tasks.Length - 1; i++)
                {
                    for (int j = 0; j < tasks.Length - 1; j++)
                    {
                        if (tasks[j].priority < tasks[j + 1].priority)
                        {
                            Task temp = tasks[j];
                            tasks[j] = tasks[j + 1];
                            tasks[j + 1] = temp;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < tasks.Length - 1; i++)
                {
                    for (int j = 0; j < tasks.Length - 1; j++)
                    {
                        if (tasks[j].priority > tasks[j + 1].priority)
                        {
                            Task temp = tasks[j];
                            tasks[j] = tasks[j + 1];
                            tasks[j + 1] = temp;
                        }
                    }
                }
            }
        }

        public static void SortByFree()
        {
            ClearTasks();
            DB.GetTasks();

            int n = 0;

            foreach (Task item in tasks)
            {
                if (item.worker == null)
                {
                    n++;
                }
            }

            Task[] new_tasks = new Task[n];
            int j = 0;

            for (int i = 0; i < tasks.Length; i++)
            {
                if (tasks[i].worker == null)
                {
                    new_tasks[j++] = tasks[i];
                }
            }

            tasks = new_tasks;
        }
    }
}
