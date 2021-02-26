using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.Common;
//using Microsoft.Data.Sqlite;

namespace KiraDX
{
    class SQLiteDB {
        SQLiteConnection con;
        public void Open() {
            con = new SQLiteConnection("Data Source=" + path + "");

            SQLiteCommand com = new SQLiteCommand();
            com.Connection = con;
            com.CommandType = CommandType.Text;
            con.Open();

        }
        public SQLiteCommand com;
        public string path;
        public void setcmd(string cmdstr) {
            com.CommandText = cmdstr;
            
        }
        public SQLiteDB(string path)
        {
            this.path = path;
            this.com = new SQLiteCommand();
            Open();
        }
        public void addParameters(string ParaName,string value)
        {
            SQLiteParameter para = new SQLiteParameter(); //声明参数
            para = new SQLiteParameter($"@{ParaName}", value);//生成一个名字为@Id的参数,必须以@开头表示是添加的参数，并设置其类型长度，类型长度与数据库中对应字段相同，但是不能超出数据库字段大小的范围，否则报错。
            //para.Value = value;
            com.Parameters.Add(para);
        }
        public DataTable execute() {

            using (con = new SQLiteConnection("Data Source=" + path + ""))
            {
                //SQLiteCommand com = new SQLiteCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                con.Open();
                //com.CommandText = cmd;
                //IDataReader dr = com.ExecuteReader();//执行Sqlite语句
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(com);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                adapter.Dispose();
                return dataTable;
            }

        }
    }
    static class  DB{

       public  static DataTable execute(string path,string cmd) {
            SQLiteConnection con;
            using (con = new SQLiteConnection("Data Source=" + path + "") )
            {
                SQLiteCommand com = new SQLiteCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                con.Open();
                com.CommandText = cmd;
                //IDataReader dr = com.ExecuteReader();//执行Sqlite语句
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(com);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                adapter.Dispose();
                return dataTable;
            }
        
        }
}

    class DataBase
    {
        static string Path;
        static SQLiteConnection con;
        static SQLiteCommand com = new SQLiteCommand();
        
        //DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite.EF6");
        public DataBase(string path)
        {
            con=new SQLiteConnection();
            
            //SQLitePCL.raw.SetProvider(new  SQLitePCL.SQLite3Provider_sqlite3());
            Path = path;
        }
        public void open() {

            con.ConnectionString = "Data Source=" + Path + "";
            
            con.Open();
            com.Connection = con;
            com.CommandType = CommandType.Text;
        }
        public DataTable Run(string SqliteCOMMAND) {
            com.CommandText = SqliteCOMMAND;
            //IDataReader dr = com.ExecuteReader();//执行Sqlite语句
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(com);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            adapter.Dispose();
            return dataTable;
            
        }
        public  void close() { 
            con.Close();
            con.Dispose();
        }
    }
}
