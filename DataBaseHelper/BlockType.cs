using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseHelper
{
    public class BlockType
    {
        public enum _typeLabel { Zero, Empty, RndString, RndNumber, CustomDict, RndDate, RndTime, RndDateTime, LocalAutoIncrement, GlobalAutoIncrement, CustomString, LinkedParam, DuplicateParam };

        public int typeID { get; set; }
        public _typeLabel typeLabel { get; set; }
        public string typeName { get; set; }

        public BlockType() { }

        public BlockType(int _id, _typeLabel _label, string _name)
        {
            typeID = _id;
            typeLabel = _label;
            typeName = _name;
        }
    }
}
