﻿namespace DreamExcel.Core
{
    public class TableStruct
    {
        public string Name;
        public string Type;
        public int Colunm;
        public string Comment;
        public TableStruct(string name, string type,string comment,int colunm)
        {
            Name = name;
            Type = type;
            Colunm = colunm;
            Comment = comment;
        }
    }
}
