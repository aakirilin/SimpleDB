using System.Diagnostics.CodeAnalysis;

namespace SimpleDB
{
    public struct CString10
    {
        public char c0;
        public char c1;
        public char c2;
        public char c3;
        public char c4;
        public char c5;
        public char c6;
        public char c7;
        public char c8;
        public char c9;

        public char this[int i]
        {
            get
            {
                object instance = (object)this;
                var type = this.GetType();
                var fields = type.GetFields()
                    .OrderBy(f => f.Name)
                    .ToArray();

                if (fields.Length < 0) throw new IndexOutOfRangeException();
                if (fields.Length < i) throw new IndexOutOfRangeException();

                var result = (char)fields[i].GetValue(instance);
                return result;

            }
            set
            {
                var type = this.GetType();
                var fields = type.GetFields()
                    .OrderBy(f => f.Name)
                    .ToArray();

                if (fields.Length < 0) throw new IndexOutOfRangeException();
                if (fields.Length < i) throw new IndexOutOfRangeException();

                object obj = (object)this;
                fields[i].SetValue(obj, value);
                this = (CString10)obj;
            }
        }

        public void SetString(char[] value)
        {
            object obj = (object)this;
            var type = this.GetType();
            var fields = type.GetFields()
                .OrderBy(f => f.Name)
                .ToArray();

            for (int i = 0; i < fields.Length; i++)
            {
                if (i < value.Length) fields[i].SetValue(obj, value[i]);
                else fields[i].SetValue(obj, new char());
            }

            this = (CString10)obj;
        }

        public void SetString(string value)
        {
            object obj = (object)this;
            var type = this.GetType();
            var fields = type.GetFields()
                .OrderBy(f => f.Name)
                .ToArray();

            for (int i = 0; i < fields.Length; i++)
            {
                if (i < value.Length) fields[i].SetValue(obj, value[i]);
                else fields[i].SetValue(obj, new char());
            }

            this = (CString10)obj;
        }

        public string GetString()
        {
            object instance = (object)this;
            var type = this.GetType();
            var fields = type.GetFields()
                .OrderBy(f => f.Name)
                .Select(f => f.GetValue(instance));

            var result = string.Join("", fields);
            return result.Trim();
        }

        public override string ToString()
        {
            return GetString();
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is CString10) return this == (CString10)obj;

            var c1 = new CString10();
            c1.SetString((string)obj);
            return this == c1;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(CString10 a, CString10 b)
        {
            var a1 = (object)a;
            var b1 = (object)b;

            var type = a.GetType();
            var fields = type.GetFields();
            var result = fields.All(f => f.GetValue(a1).Equals(f.GetValue(b1)));

            return result;
        }

        public static bool operator !=(CString10 a, CString10 b)
        {
            var a1 = (object)a;
            var b1 = (object)b;

            var type = a.GetType();
            var fields = type.GetFields();
            var result = fields.Any(f => !f.GetValue(a1).Equals(f.GetValue(b1)));

            return result;
        }


        public static CString10 New(string s)
        {
            var result = new CString10();
            result.SetString(s);

            return result;
        }
    }
}
