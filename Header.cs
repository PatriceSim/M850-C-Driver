using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Micro800Connection.DriverM800
{
	class Header
    {
		public enum ECommande
		{
			Nop = 0x00,
			ListServices = 0x04,
			ListIdentity = 0x63,
			ListInterfaces = 0x0064,
			RegisterSessions = 0x65,
			UnRegisterSession = 0x66,
			SendRRData = 0x6F,
			SendUnitData = 0x70,
			IndicateStatus = 0x72,
		};

		public byte[] longueur { get; set; }
		public byte[] nHandleSession { get; set; }
		public byte[] nStatus { get; set; }
		public byte[] sSenderContext { get; set; } //"Windev"
		public byte[] nOption { get; set; }
		public short nHeaderSize { get; set; } = 24;
		public ECommande eCommande { get; set; }

		private int ind = 0;

		public Header()
        {
			longueur = new byte[2];
			nHandleSession = new byte[4];
			nStatus = new byte[4];
			sSenderContext = new byte[8]; //"Windev"
			nOption = new byte[4];
			eCommande = new ECommande();
		}

		public byte[] sendHeader()
        {
			ind = 0;
			byte bCommande = (byte)eCommande;

			byte[] bHeader = new byte[24];
			bHeader.SetValue(bCommande, 0);
			ind = 2;

			construireSequence(bHeader,longueur);
			construireSequence(bHeader, nHandleSession);
			construireSequence(bHeader, nStatus);
			construireSequence(bHeader, sSenderContext);
			construireSequence(bHeader, nOption);

			return bHeader;
        }

		public byte[] sendFowardHeader()
        {
			byte[] bForwardHeader = new byte[24];
			byte bCommande = (byte)eCommande;
			bForwardHeader.SetValue(bCommande, 0);
			ind = 2;

			construireSequence(bForwardHeader, longueur);
			construireSequence(bForwardHeader, nHandleSession);

			return bForwardHeader;
        }




		private void construireSequence(byte[] bHeader, byte[] partition)
        {

			foreach (byte bb in partition)
			{
				bHeader.SetValue(bb, ind);
				ind++;
			}

		}

	}
}
