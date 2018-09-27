using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login
{
    class Task
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan WorkingTime { get; set; }
        public TimeSpan NonWorkingTime { get; set; }

        public Task(string taskName)
        {
            this.TaskName = taskName;
            StartTime = DateTime.Now;
        }
        public Task()
        {

        }
    }
}
