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

        public Task[] tasks = null;
        public Task current_task;

        public static string CreateTask(Task task)
        {
            return DB.AddTask(task.header, task.text, task.priority, task.deadline);
        }
    }
}
