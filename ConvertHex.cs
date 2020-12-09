using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micro800Connection.DriverM800
{
    class ConvertHex
    {


        public ConvertHex()
        {

        }


        public string convert(byte[] bufferIn)
        {
            string valeur = "";

            if (bufferIn.Length>50)
            {
                //Type de variable qui peux avoir dans le programme
                //BOOL(0x00C1, 1),
                //SINT(0x00C2, 1),
                //INT(0x00C3, 2),
                //DINT(0x00C4, 4),
                //REAL(0x00CA, 4),
                //BITS(0x00D3, 4),
                //STRING(0x00DA, 4),
                byte type = bufferIn[50];

                byte longRep = bufferIn[42];
                var reponse = bufferIn.Skip(50).Take(200);
                byte[] breponse=reponse.ToArray();
                


                switch (type) {

                    case 0xc1:
                        valeur = VarBool(breponse);
                        break;
                    case 0xc3:
                        valeur=VarInteger(breponse);
                        break;
                    case 0xc4:
                        valeur = VarDoubleInt(breponse);
                        break;
                    case 0xca:
                        valeur = VarReel(breponse);
                        break;
                    case 0xda:
                        valeur = VarString(breponse,(longRep - 9));
                        break;
                    default:
                        break;
                }
            }

            return valeur;
        }

        private string VarBool(byte[] bufferIn)
        {
            string valeur = "";
            var reponse = bufferIn.Skip(2).Take(2);
            byte[] bvar = reponse.ToArray();
            int b1 = BitConverter.ToInt16(bvar, 0);

            if (b1==1)
            {
                valeur = "1";
            }
            else
            {
                valeur = "0";
            }

            return valeur;
        }

        private string VarInteger(byte[] bufferIn)
        {
            string valeur = "";

            var reponse = bufferIn.Skip(2).Take(2);
            byte[] bvar = reponse.ToArray();
            int b1 = BitConverter.ToInt16(bvar, 0);
            valeur = b1.ToString();

            return valeur;
        }

        private string VarDoubleInt(byte[] bufferIn)
        {
            string valeur = "";

            var reponse = bufferIn.Skip(2).Take(4);
            byte[] bvar = reponse.ToArray();
            double b1 = BitConverter.ToInt32(bvar, 0);
            valeur = b1.ToString();

            return valeur;
        }

        private string VarReel(byte[] bufferIn)
        {
            string valeur = "";

            var reponse = bufferIn.Skip(2).Take(4);
            byte[] bvar = reponse.ToArray();

            double b1 = BitConverter.ToSingle(bvar, 0);
            valeur = b1.ToString();

            return valeur;
        }

        private string VarString(byte[] bufferIn,int longueur)
        {
            string valeur = "";
            var reponse = bufferIn.Skip(3).Take(longueur);
            byte[] rep1 = reponse.ToArray();
            valeur = Encoding.Default.GetString(rep1);

            return valeur;
        }

    }
}
