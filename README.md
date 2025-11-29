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


for (int i = 0; i < 1000; i++)
{
    var row0 = db.AddRow();

    row0["c1"].SetString("Hello world! 1 12");
    row0["c2"].SetInt(150);
    row0["c3"].SetBool(true);
}

var fileName = "db.xml";
db.Save(fileName);

Console.WriteLine(db.ToString());

Console.ReadLine();
```