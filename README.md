# SimpleDB

```C#
using CStringLib;
using SimpleDB;

var db = new DB(
    new RecordDescription() { Name = CString10.New("c1"), Length = 18, Type = RecordTypes.STRING },
    new RecordDescription() { Name = CString10.New("c2"), Length = 4, Type = RecordTypes.INT },
    new RecordDescription() { Name = CString10.New("c3"), Length = 1, Type = RecordTypes.BOOL },
    new RecordDescription() { Name = CString10.New("c4"), Length = 4, Type = RecordTypes.INT }
);


for (int i = 0; i < 20; i++)
{
    var row0 = db.AddRow();

    row0["c1"].SetString("Hello world! " + i);
    row0["c2"].SetInt(i);
    row0["c3"].SetBool(true);
    row0["c4"].SetInt(i * 100);
}

db.DeleteRow(2);
db.DeleteRow(5);
db.DeleteRow(10);


var fileName = "db.xml";
db.Save(fileName);

Console.WriteLine(db.ToString());

Console.ReadLine();
```