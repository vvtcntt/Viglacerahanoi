using System;
using System.Collections.Generic;

namespace Viglacera.Models
{
    public partial class tblNewsTag
    {
        public int id { get; set; }
        public Nullable<int> idn { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
    }
}
