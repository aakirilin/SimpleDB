namespace SimpleDB
{
    public struct Row
    {
        private readonly int rowI;
        private readonly DB db;
        public RecordDescription[] Descriptions;
        public byte[] RawValue;

        public Row(int rowI, DB db) : this()
        {
            this.rowI = rowI;
            this.db = db;
        }

        public Column this[int colI]
        {
            get
            {
                var skip = Descriptions.Take(colI).Sum(c => c.Length);
                var take = Descriptions[colI].Length;
                return new Column(rowI, colI, db)
                {
                    Description = Descriptions[colI],
                    //RawValue = RawValue.Skip(skip).Take(take).ToArray()
                };
            }
        }
    }
}
