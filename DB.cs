using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SimpleDB
{
    [DataContract]
    public class DB
    {
        public event Action<Row> AddRowEvent;

        [DataMember]
        public RecordDescription[] RecordDescriptions { get; set; }

        [DataMember]
        public int RowLength { get; set; }

        [DataMember]
        public List<byte[]> Values { get; set; }

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
            var row = this[index];
            AddRowEvent?.Invoke(row);

            return row;
        }

        private string CloumnToString(int rowI, int colI, int length)
        {
            return Column.ToString(this[rowI, colI], RecordDescriptions[colI].Type)
                .TrimEnd(new char())
                .PadRight(length, ' ');
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();

            var columnLength = Values
                .Select((r, i) => new { 
                    i, 
                    values = RecordDescriptions.Select((c, j) => CloumnToString(i, j, 0)).ToArray() })    
                .SelectMany(v => v.values.Select((c, j) => new { v.i, j, l = c.Length > 10 ? c.Length : 10 }))
                .GroupBy(v => v.j)
                .ToDictionary(v => v.Key, v => new { v.Key, l = v.Max(c => c.l) })
                .ToArray();

            var delimiter = '|';
            var headersNames = RecordDescriptions
                .Select((h, i) => h.Name.ToString().Replace(new char(), ' ').PadRight(columnLength[i].Value.l, ' '));

            strBuilder.AppendLine(string.Join(delimiter, headersNames));
            strBuilder.AppendLine(new string('-', columnLength.Sum(l => l.Value.l)));
 
            var sVal = Values
                .Select((r, i) => RecordDescriptions.Select((c, j) => 
                    CloumnToString(i, j, columnLength[j].Value.l)).ToArray())
                .Select(r => string.Join(delimiter, r))
                .ToList();

            sVal.ForEach(r => strBuilder.AppendLine(r));

            return strBuilder.ToString();   
        }

        public void Save(string fileName)
        {
            DataContractSerializer s = new DataContractSerializer(typeof(DB));
            using (FileStream fs = File.Open(fileName, FileMode.Create))
            {
                Console.WriteLine("Testing for type: {0}", typeof(DB));
                s.WriteObject(fs, this);
            }
        }

        public static DB Read(string fileName)
        {
            DataContractSerializer s = new DataContractSerializer(typeof(DB));
            using (FileStream fs = File.Open(fileName, FileMode.Open))
            {
                object s2 = s.ReadObject(fs);
                return s2 as DB;
            }
        }
    }
}
