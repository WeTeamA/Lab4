using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

namespace Lab4
{
    [Serializable]
    public class Function
    {
        public String Name { get; private set; }
        public String Value { get; set; }

        public Function(String Name, String Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
    }

    public class Functions : IEnumerable<Function>
    {
        private List<Function> _funcs;
        private string _filepath;

        public Functions(String filepath)
        {
            _filepath = filepath;
            _funcs = new List<Function>();
            if (!File.Exists(filepath)) return;

            BinaryFormatter binFormat = new BinaryFormatter();

            using (Stream fStream = new FileStream(_filepath,
               FileMode.Open, FileAccess.Read, FileShare.None))
            {
                _funcs = (List<Function>)binFormat.Deserialize(fStream);
            }
        }

        private void Serialize()
        {
            BinaryFormatter binFormat = new BinaryFormatter();

            using (Stream fStream = new FileStream(_filepath,
               FileMode.Create, FileAccess.Write, FileShare.None))
            {
                binFormat.Serialize(fStream, _funcs);
            }
        }

        public void Add(Function f)
        {
            if (_funcs.Exists(x => x.Name == f.Name))
                _funcs.FirstOrDefault(x => x.Name == f.Name).Value = f.Value;
            else
                _funcs.Add(f);
            Serialize();
        }

        public void Delete(Function f)
        {
            _funcs.Remove(f);
            Serialize();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Function> GetEnumerator()
        {
            return _funcs.GetEnumerator();
        }
    }
}
