using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Interfaces
{
    public interface ICsvExporter
    {
        public byte[] GetToCsvExport<T>(IEnumerable<T> toExport);
    }
}
