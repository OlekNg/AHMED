using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Genetics.Statistics
{
    public class CsvImport<T> where T : new()
    {
        private Dictionary<Type, Func<string, object>> convertFunctions = new Dictionary<Type, Func<string, object>>();

        public List<T> Objects;

        public CsvImport()
        {
            InitConvertFunctions();
        }

        public void Import(string path)
        {
            Objects = new List<T>();
            var lines = File.ReadAllLines(path);
            var header = lines[0].Split(';');
            var props = header.Select(x => typeof(T).GetProperties().Single(p => p.Name == x)).ToArray();

            for (int i = 1; i < lines.Length; i++)
            {
                var obj = new T();
                var data = lines[i].Split(';');
                for (int j = 0; j < props.Length; j++)
                {
                    var prop = props[j];
                    var convertedValue = convertFunctions[prop.PropertyType](PrepareString(data[j]));
                    prop.SetValue(obj, convertedValue, null);
                }

                Objects.Add(obj);
            }
        }

        private string PrepareString(string csvValue)
        {
            if (csvValue == null)
                return csvValue;

            if (csvValue.StartsWith("\""))
                csvValue = csvValue.Substring(1);

            if (csvValue.EndsWith("\""))
                csvValue = csvValue.Substring(0, csvValue.Length - 1);

            return csvValue;
        }

        private void InitConvertFunctions()
        {
            convertFunctions.Add(typeof(int), ConvertToInt);
            convertFunctions.Add(typeof(double), ConvertToDouble);
        }

        private object ConvertToDouble(string arg)
        {
            return Double.Parse(arg);
        }

        private object ConvertToInt(string arg)
        {
            return Int32.Parse(arg);
        }
    }
}
