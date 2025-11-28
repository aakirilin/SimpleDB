using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleDB
{
    public class DB
    {
        public RecordDescription[] RecordDescriptions { get; private set; }

        public int RowLength { get; private set; }

        public List<byte[]> Values { get; private set; }

        public DB(params RecordDescription[] recordDescriptions)
        {
            this.RecordDescriptions = recordDescriptions;
            this.RowLength = recordDescriptions.Sum(r => r.Length);
            Values = new List<byte[]>();
        }

        public int ColumnLength(int colI) => RecordDescriptions[colI].Length;

        public byte[] this[int rowI, int colI]
        {
            get
            {
                var skip = RecordDescriptions.Take(colI).Sum(c => c.Length);
                return this.Values[rowI]
                    .Skip(skip)
                    .Take(ColumnLength(colI))
                    .ToArray();
            }
            set
            {
                var skip = RecordDescriptions.Take(colI).Sum(c => c.Length);
                var length = ColumnLength(colI);

                for (int i = 0; i < length; i++)
                {
                    if (i < value.Length) Values[rowI][i + skip] = value[i];
                }
            }
        }

        public Row this[int rowI]
        {
            get
            {
                return new Row(rowI, this)
                {
                    Descriptions = RecordDescriptions,
                    RawValue = Values[rowI]
                };
            }
        }

        public Row AddRow()
        {
            Values.Add(new byte[RowLength]);
            var index = Values.Count - 1;
            return this[index];
        }

        public void Save(string fileName)
        {
            // создаем объект BinaryFormatter
            BinaryFormatter formatter = new BinaryFormatter();
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, this);

                Console.WriteLine("Объект сериализован");
            }
        }

        public static DB Read(string fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("people.dat", FileMode.OpenOrCreate))
            {
                var db = (DB)formatter.Deserialize(fs);

                return db;
            }
        }
    }
}
