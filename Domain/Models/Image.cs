using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Image
    {
        public string Id { get; set; } = null!;

        public string Url { get; set; } = null!;

        public string AbsolutePath { get; set; } = null!;
    }
}
