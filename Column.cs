using System.Text;

namespace SimpleDB
{
    public struct Column
    {
        private readonly int rowI;
        private readonly int colI;
        private readonly DB db;
        public RecordDescription Description;

        public Column(int rowI, int colI, DB db) : this()
        {
            this.rowI = rowI;
            this.colI = colI;
            this.db = db;
        }

        public int GetInt()
        {
            return BitConverter.ToInt32(db[rowI, colI]);
        }

        public void SetInt(int value)
        {
            db[rowI, colI] = BitConverter.GetBytes(value);
        }

        public string GetString()
        {
            return Encoding.UTF8.GetString(db[rowI, colI]);
        }

        public void SetString(string value)
        {
            db[rowI, colI] = Encoding.UTF8.GetBytes(value)
                .Take(Description.Length)
                .ToArray();
        }

        public bool GetBool()
        {
            return BitConverter.ToBoolean(db[rowI, colI]);
        }

        public void SetBool(bool value)
        {
            db[rowI, colI] = BitConverter.GetBytes(value);
        }

        public static string ToString(byte[] value, RecordTypes type)
        {
            switch (type)
            {
                case RecordTypes.INT: return BitConverter.ToInt32(value).ToString();
                case RecordTypes.STRING: return Encoding.UTF8.GetString(value);
                case RecordTypes.BOOL: return BitConverter.ToBoolean(value).ToString();
                default: throw new Exception("Тип не реализован");
            }
        }
    }
}
