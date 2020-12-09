using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micro800Connection.DriverM800
{
    class ConvertToHex
    {

        public String fauteMessage { get; set; }
        public ConvertToHex() 
        {
            
        }


        public byte[] convert(Variable variable, String valeur)
        {
            // on convertir la valeur au depart
            //BOOL(0x00C1, 1),
            //SINT(0x00C2, 1),
            //INT(0x00C3, 2),
            //DINT(0x00C4, 4),
            //REAL(0x00CA, 4),
            //BITS(0x00D3, 4),
            //STRING(0x00DA, 4),
            byte[] bValeur = null;
            try
            {
                switch (variable.type)
                {

                    case 0xc1:
                        bValeur = new byte[1];
                        short repC1 = Convert.ToInt16(valeur);
                        bValeur = BitConverter.GetBytes(repC1);
                        break;

                    case 0xc3:
                        bValeur = new byte[2];
                        short repC3 = Convert.ToInt16(valeur);
                        bValeur = BitConverter.GetBytes(repC3);

                        break;
                    case 0xc4:
                        bValeur = new byte[4];
                        int repC4 = Convert.ToInt32(valeur);
                        bValeur = BitConverter.GetBytes(repC4);

                        break;
                    case 0xca:
                        bValeur = new byte[4];
                        float repCa = (float)Convert.ToDouble(valeur);
                        bValeur = BitConverter.GetBytes(repCa);


                        break;
                    case 0xda:

                        byte longString = (byte)valeur.Length;
                        byte[] bNomValeur = Encoding.Default.GetBytes(valeur);
                        bValeur = new byte[valeur.Length + 1];
                        bValeur.SetValue(longString, 0);
                        int ind = 1;

                        foreach (byte bb in bNomValeur)
                        {
                            bValeur.SetValue(bb, ind);
                            ind++;
                        }


                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                fauteMessage = e.ToString();
            }

            
            return bValeur;
        }



    }
}
