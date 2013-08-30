using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Signatory.Framework
{
    // Modified from https://gist.github.com/simonech/4104490
    public class CsvActionResult<T> : FileResult
    {
        public CsvActionResult(IEnumerable<T> list, string fileDownloadName, char delimiter = ',') : base("text/csv")
        {
            List = list;
            FileDownloadName = fileDownloadName;
            Delimiter = delimiter;
        }

        internal IEnumerable<T> List { get; private set; }
        internal char Delimiter { get; private set; }

        protected override void WriteFile(HttpResponseBase response)
        {
            using (var memoryStream = new MemoryStream())
            {
                WriteList(memoryStream);
                response.OutputStream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }
        }

        private void WriteList(Stream stream)
        {
            var streamWriter = new StreamWriter(stream, Encoding.Default);

            WriteHeaderLine(streamWriter);
            streamWriter.WriteLine();
            WriteDataLines(streamWriter);

            streamWriter.Flush();
        }

        private void WriteHeaderLine(StreamWriter streamWriter)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                WriteValue(streamWriter, property.Name);
            }
        }

        private void WriteDataLines(StreamWriter streamWriter)
        {
            foreach (T line in List)
            {
                foreach (var property in typeof(T).GetProperties())
                {
                    WriteValue(streamWriter, GetPropertyValue(line, property.Name));
                }
                streamWriter.WriteLine();
            }
        }

        private void WriteValue(StreamWriter writer, String value)
        {
            writer.Write("\"");
            writer.Write(value.Replace("\"", "\"\""));
            writer.Write("\"" + Delimiter);
        }

        private static string GetPropertyValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null).ToString();
        }
    }
}