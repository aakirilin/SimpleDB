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
    }
}
