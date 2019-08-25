using Blish_HUD;
using Blish_HUD.Content;
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

        public string Description { get; set; }
        public string Category { get; set; }
        //public bool isCompleted { get; set; }
        //public bool isRepeatable { get; set; }
        //public bool timesCompleted { get; set; }
        private string _icon;
        public string Icon
        {
            get => _icon;
            set
            {
                if (_icon == value) return;

                _icon = value;

                if (!string.IsNullOrEmpty(_icon))
                {
                    this.Texture = GameService.Content.GetTexture(_icon);
                }
            }
        }
        public AsyncTexture2D Texture { get; private set; } = new AsyncTexture2D(GameService.Content.GetTexture("102377"));
        public static void Load()
        {
            var tdTasks = new List<tdTask>();

            tdTasks.Add(new tdTask() { Description = "First Daily", Category = "Achievements"});
            tdTasks.Add(new tdTask() { Description = "Second Daily", Category = "Crafting"});
            tdTasks.Add(new tdTask() { Description = "This is a long title example, possibly will switch to content", Category = "Duplicate"});
            tdTasks.Add(new tdTask() { Description = "This is an even longer description to see just how much the text can wrap before it runs into an issue of some kind.", Category = "Duplicate"});
            tdTasks.Add(new tdTask() { Description = "Winterberry Farm", Category = "Routines"});
            tdTasks.Add(new tdTask() { Description = "Do the dishes!", Category = "Personal"});

            Tasks = tdTasks;
        }

    }
}
