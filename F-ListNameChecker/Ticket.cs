using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F_ListNameChecker
{
    public class Ticket
    {
        public Ticket (string name, string key)
        {
            Name = name;
            Key = key;
        }
        public string Name { get; set; }
        public string Key { get; set; }
    }
}
