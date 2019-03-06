namespace DreamExcel.Core
{
    public class TableStruct
    {
        public string Name;
        public string Type;
        public int Colunm;
        public bool IsArray
        {
            get { return Type.EndsWith("[]"); }
        }
        public TableStruct(string name, string type,int colunm)
        {
            Name = name;
            Type = type;
            Colunm = colunm;
        }
    }
}
