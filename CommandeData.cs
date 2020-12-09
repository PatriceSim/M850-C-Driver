using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micro800Connection.DriverM800
{
    class CommandeData
    {
        public byte[] sendRRData { get; set; }
        public byte[] timeOut { get; set; }
        public byte[] itemCount { get; set; }
        public byte[] typeEncapsulation { get; set; }
        public byte[] lengt { get; set; }
        public byte[] typeID { get; set; }
        public byte[] length { get; set; }

        private int ind = 0;

        public CommandeData()
        {
            sendRRData =new byte[4];
            timeOut = new byte[2];
            itemCount = new byte[2];
            typeEncapsulation = new byte[2];
            lengt = new byte[2];
            typeID = new byte[2];
            length = new byte[2];
    }

        public byte[] sendHeader()
        {
            byte[] enipHeader = new byte[4];
            enipHeader[0] = 0x01;

            return enipHeader;
        }


        public byte[] sendForward()
        {
            ind = 0;
            sendRRData = new byte[] { 0, 0, 0, 0 };
            timeOut[0] =0x05;
            itemCount[0] =0x02 ;
            typeEncapsulation = new byte[] { 0, 0 } ;
            lengt = new byte[] { 0, 0 };
            typeID = new byte[] { 0xb2, 0 };
            
            byte[] bCommande = new byte[16];

            construireSequence(bCommande, sendRRData);
            construireSequence(bCommande, timeOut);
            construireSequence(bCommande, itemCount);
            construireSequence(bCommande, typeEncapsulation);
            construireSequence(bCommande, lengt);
            construireSequence(bCommande, typeID);
            construireSequence(bCommande, length);

            return bCommande;
        }

        public byte[] forwardClose()
        {
            ind = 0;
            sendRRData = new byte[] { 0, 0, 0, 0 };
            timeOut[0] = 0x05;
            itemCount[0] = 0x02;
            typeEncapsulation = new byte[] { 0, 0 };
            lengt = new byte[] { 0, 0 };
            typeID = new byte[] { 0xb2, 0 };

            byte[] bCommande = new byte[16];

            construireSequence(bCommande, sendRRData);
            construireSequence(bCommande, timeOut);
            construireSequence(bCommande, itemCount);
            construireSequence(bCommande, typeEncapsulation);
            construireSequence(bCommande, lengt);
            construireSequence(bCommande, typeID);
            construireSequence(bCommande, length);

            return bCommande;
        }

        public byte[] commandRead()
        {
            ind = 0;
            sendRRData = new byte[] { 0, 0, 0, 0 };
            timeOut[0] = 0x00;
            itemCount[0] = 0x02;
            typeID = new byte[] { 0xa1, 0 };
            length[0] = 0x04;

            byte[] bCommandeRead=new byte[12];
            construireSequence(bCommandeRead, sendRRData);
            construireSequence(bCommandeRead, timeOut);
            construireSequence(bCommandeRead, itemCount);
            construireSequence(bCommandeRead, typeID);
            construireSequence(bCommandeRead, length);

            return bCommandeRead;
        }


        private void construireSequence(byte[] bCIP, byte[] partition)
        {

            foreach (byte bb in partition)
            {
                bCIP.SetValue(bb, ind);
                ind++;
            }

        }


    }
}
