using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Options
{
    public class ConnectionStringOptions
    {
        public const string ConnectionStringSectionName = "ConnectionStrings";

        public string DefaultConnection { get; set; }
    }
}
