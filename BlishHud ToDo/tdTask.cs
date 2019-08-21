using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoModule
{
    public class tdTask
    {
        public static List<tdTask> Tasks;

        public string Name { get; set; }
        public string Category { get; set; }
        public string Content { get; set; }
        //public bool isCompleted { get; set; }
        //public bool isRepeatable { get; set; }
        //public bool timesCompleted { get; set; }

        public static void Load()
        {

            var tdTasks = new List<tdTask>();

            tdTasks.Add(new tdTask() { Name = "First Daily", Category = "Achivements", Content = "This is example content, please read me!" });
            tdTasks.Add(new tdTask() { Name = "Second Daily", Category = "Crafting", Content = "This is example content, please don't read me!" });
            tdTasks.Add(new tdTask() { Name = "Dupliated for Testing", Category = "Duplicate", Content = "This is example content, please don't read me!" });
            tdTasks.Add(new tdTask() { Name = "Dupliated for Testing", Category = "Duplicate", Content = "This is example content, please don't read me!" });
            tdTasks.Add(new tdTask() { Name = "Dupliated for Testing", Category = "Duplicate", Content = "This is example content, please don't read me!" });
            tdTasks.Add(new tdTask() { Name = "Dupliated for Testing", Category = "Duplicate", Content = "This is example content, please don't read me!" });
            tdTasks.Add(new tdTask() { Name = "Dupliated for Testing", Category = "Duplicate", Content = "This is example content, please don't read me!" });
            tdTasks.Add(new tdTask() { Name = "Dupliated for Testing", Category = "Duplicate", Content = "This is example content, please don't read me!" });
            tdTasks.Add(new tdTask() { Name = "Dupliated for Testing", Category = "Duplicate", Content = "This is example content, please don't read me!" });
            tdTasks.Add(new tdTask() { Name = "Dupliated for Testing", Category = "Duplicate", Content = "This is example content, please don't read me!" });
            tdTasks.Add(new tdTask() { Name = "Dupliated for Testing", Category = "Duplicate", Content = "This is example content, please don't read me!" });
            tdTasks.Add(new tdTask() { Name = "Dupliated for Testing", Category = "Duplicate", Content = "This is example content, please don't read me!" });
            tdTasks.Add(new tdTask() { Name = "Dupliated for Testing", Category = "Duplicate", Content = "This is example content, please don't read me!" });

            Tasks = tdTasks;
        }

    }
}
