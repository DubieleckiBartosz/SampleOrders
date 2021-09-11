using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using ShopApi.Interfaces;

namespace ShopApi.Services
{
    public class CsvExporter : ICsvExporter
    {
        public byte[] GetToCsvExport<T>(IEnumerable<T> toExport)
        {
            using var memory = new MemoryStream();
            using (var streamWriter = new StreamWriter(memory))
            {
                using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
                csvWriter.WriteRecords(toExport);
            }
            return memory.ToArray();
        }
    }
}
