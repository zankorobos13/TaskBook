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

        public static void ClearTaks()
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
    }
}
