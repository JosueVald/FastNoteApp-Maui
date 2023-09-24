using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastNoteApp.Models
{
    public class AppFolder : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [AutoIncrement, PrimaryKey]
        public int id { get; set; }

        public string name { get; set; } = "";

        public string iconPath { get; set; } = "";

        public string noteCount { get; set; } = "0";

        public AppFolder()
        {

        }

        public AppFolder(string folderName)
        {
            name = folderName;
        }
        public AppFolder(string folderName, string path)
        {
            name = folderName;
            iconPath = path;
        }
        public AppFolder(string folderName, string path, string count)
        {
            name = folderName;
            iconPath = path;
            noteCount = count;
        }
    }
}
