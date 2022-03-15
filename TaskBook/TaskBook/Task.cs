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
        public DateTime deadline;

        public Task[] tasks = null;
        public Task current_task;
    }
}
