using CStringLib;

namespace SimpleDB
{
    public struct RecordDescription
    {
        public CString10 Name;
        public int Length;
        public RecordTypes Type;
    }
}
