using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micro800Connection.DriverM800
{
    class Variable
    {
        public String nom { get; set; } = "";
        public String tagPLC { get; set; } = "";
        public String valeur { get; set; } = "";
        public DateTime dateTime { get; set; } = DateTime.Now;
        public Double intervale { get; set; } = 0;
        public byte counter { get; set; } = 0; //Le compteur dans le buffer CIP 
        public byte type { get; set; } = 0x00;
        //Type de variable qui peux avoir dans le programme
        //BOOL(0x00C1, 1),
        //SINT(0x00C2, 1),
        //INT(0x00C3, 2),
        //DINT(0x00C4, 4),
        //REAL(0x00CA, 4),
        //BITS(0x00D3, 4),
        //STRING(0x00DA, 4),

    }
}
