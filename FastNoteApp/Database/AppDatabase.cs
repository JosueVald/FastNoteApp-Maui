﻿using CommunityToolkit.Maui.Core.Primitives;
using FastNoteApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastNoteApp.Database
{
    public static class Constants
    {
        public const string DatabaseFilename = "FastNoteApp.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }


    public class AppDatabase
    {      
        public static AppDatabase m_instance = null;
        public static AppDatabase Instance()
        {
            if (m_instance == null) m_instance = new AppDatabase();

            return m_instance;
        }

        //SQLite database = null;
        SQLiteConnection dbConnection = null;

        public AppDatabase()
        {
        }

        public void Init()
        {
            if (dbConnection is not null)
                return;

            dbConnection = new SQLiteConnection(Constants.DatabasePath, Constants.Flags);

            //string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FastNoteApp.sqlite");
            //db = new SQLiteConnection(dbPath);

            dbConnection.CreateTable<AppFolder>();
            dbConnection.CreateTable<AppNote>();

            if(GetFolderList().Count == 0)
                InsertFolder(new AppFolder("My Note", "ic_folder_special_black.png"));
        }

        public List<AppFolder> GetFolderList()
        {
            return dbConnection.Table<AppFolder>().ToList();
        }
        public AppFolder GetFirstFolder()
        {
            return dbConnection.Table<AppFolder>().FirstOrDefault();
        }

        public AppFolder GetFolder(int id)
        {
            return dbConnection.Table<AppFolder>().Where(folder => folder.id == id).FirstOrDefault();
        }

        public int UpdateFolder(AppFolder folder)
        {
            return dbConnection.Update(folder);
        }

        public int InsertFolder(AppFolder folder)
        {
            return dbConnection.Insert(folder);
        }

        public int DeleteFolder(AppFolder folder)
        {
            return dbConnection.Delete(folder);
        }

        public List<AppNote> GetNoteList(int folderID)
        {
            return dbConnection.Table<AppNote>().Where(note => note.folderID == folderID).ToList();
        }

        public int UpdateNote(AppNote note)
        {
            return dbConnection.Update(note);
        }

        public int InsertNote(AppNote note)
        {
            AppFolder folder = GetFolder(note.folderID);

            int count = Convert.ToInt32(folder.noteCount) + 1;
            folder.noteCount = count.ToString();

            dbConnection.Update(folder);

            return dbConnection.Insert(note);
        }

        public int DeleteNote(AppNote note)
        {
            AppFolder folder = GetFolder(note.folderID);

            int count = Convert.ToInt32(folder.noteCount) - 1;
            if (count < 0) count = 0;
            folder.noteCount = count.ToString();

            dbConnection.Update(folder);

            return dbConnection.Delete(note);
        }

        public void DeleteAllNotes(int folderID)
        {
            List<AppNote> noteList = GetNoteList(folderID);

            foreach(AppNote note in noteList)
            {
                dbConnection.Delete(note);
            }
        }
    }
}
