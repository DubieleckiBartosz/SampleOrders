using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit;

namespace ShopApi.Models
{
    public class File
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Body { get; set; }
    }
}
