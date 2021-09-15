using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseHelper
{

    public class Block
    {
        public int ID { get; set; }
        public BlockType Type { get; set; }
        public string Content { get; set; }
        public string Value { get; set; }
        public string MinInterval { get; set; }
        public string MaxInterval { get; set; }

        public Block(int id)
        {
            ID = id;
            Type = new BlockType();
        }
    }
}
