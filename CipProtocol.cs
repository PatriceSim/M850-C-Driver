using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micro800Connection.DriverM800
{
    class CipProtocol
    {
		enum EService {
			Get_Attribute_All = 0x01,
			Get_Attributes_List = 0x03,
			Get_Attribute_Single = 0x0E,
			CIP_MultiRequest = 0x0A,
			CIP_ReadData = 0x4C,
			CIP_ReadDataFragmented = 0x52,
			CIP_WriteData = 0x4D,
			CIP_WriteDataFragmented = 0x53,
			CM_Unconnected_Send = 0x52,
			CM_FowardOpen = 0x54,
			CM_FowardClose = 0x4e,
			Get_Instance_Attribute_List = 0x55,
			Get_Connection_Data = 0x56
			//  Copier du java donc va falloir découvrir comment faire le | 0x80!!!! 			
			//	Get_Attribute_All_Reply=(0x01 | 0x80)
			//	Get_Attributes_List_Reply=(0x03 | 0x80)		
			//	Get_Attribute_Single_Reply=(0x0E | 0x80)
			//	CIP_MultiRequest_Reply=(0x0A | 0x80)		
			//	CIP_ReadData_Reply=(0x4C | 0x80)		
			//	CIP_ReadDataFragmented_Reply=(0x52 | 0x80)
			//	CIP_WriteData_Reply=(0x4D | 0x80)
			//	CIP_WriteDataFragmente_Reply=(0x53 | 0x80)	
			//	Get_Instance_Attribute_List_Reply=(0x55 | 0x80)		
			//	CM_Unconnected_Send_Reply=(0x52 | 0x80)
		}

		public byte[] requestPathSize { get; set; } 
		public byte[] classSegment { get; set; }
		public byte[] instanceSegment { get; set; }
		public byte[] actualTimeOut { get; set; }
		public byte[] O_TNetworkID { get; set; }
		public byte[] T_ONetworkID { get; set; }
		public byte[] connectionSerialNumber { get; set; }
		public byte[] vendorId { get; set; }
		public byte[] originalSerialNumber { get; set; }
		public byte[] timeOutMultiplier { get; set; }
		public byte[] reserved { get; set; }
		public byte[] O_T_RPI { get; set; }
		public byte[] O_T_NetworkParametre { get; set; }
		public byte[] T_O_RPI { get; set; }
		public byte[] T_O_NetworkParametre { get; set; }
		public byte[] triggerType { get; set; }
		public byte[] pathSize { get; set; }
		public byte[] portSegment { get; set; }
		public byte[] pathSegment { get; set; }
		public byte[] CIPClassSegment { get; set; }
		public byte[] CIPInstanceSegment { get; set; }
		public byte[] dataSize { get; set; }
		public byte[] bufferSpecefic { get; set; }
		public byte[] typeID { get; set; }
		public byte[] length { get; set; }
		public byte[] sequenceCount { get; set; }
		private short compteur = 1;


		private int ind=0;


		EService eService { get; set; }


		public CipProtocol()
        {
			requestPathSize = new byte[] {0x02};
			classSegment = new byte[] {0x20,0x06};
			instanceSegment = new byte[] {0x24,0x01};
			actualTimeOut = new byte[] {0x07,0xc3};
			O_TNetworkID = new byte[4];
			T_ONetworkID = new byte[4];
			connectionSerialNumber = new byte[2];
			vendorId = new byte[2];
			vendorId=Encoding.ASCII.GetBytes("PA");
			originalSerialNumber = new byte[4];
			originalSerialNumber= Encoding.ASCII.GetBytes("TSIM");
			timeOutMultiplier = new byte[] {0x02};
			reserved = new byte[3];
			O_T_RPI = new byte[] {0x80,0x84,0x1e,0x00};
			O_T_NetworkParametre = new byte[] {0xfc,0x43};
			T_O_RPI = new byte[] {0x80, 0x84, 0x1e, 0x00};
			T_O_NetworkParametre = new byte[] {0xfc, 0x43};
			triggerType = new byte[] {0xA3};
			pathSize = new byte[] {0x02};
			portSegment = new byte[] {0x01,0x00};
			pathSegment = new byte[1];
			CIPClassSegment = new byte[] {0x20,0x02};
			CIPInstanceSegment = new byte[] {0x24,0x01};
			dataSize = new byte[1];
			bufferSpecefic = new byte[4];
			typeID = new byte[2];
			length= new byte[2];
			sequenceCount = new byte[2];

		}



		public byte[] fowardOpen()
        {

			ind = 1;
			Random random = new Random();
			T_ONetworkID[0] = (byte)random.Next(1, 255);
			T_ONetworkID[1] = (byte)random.Next(1, 255);
			T_ONetworkID[2] = (byte)random.Next(1, 255);
			T_ONetworkID[3] = (byte)random.Next(1, 255);
			connectionSerialNumber[0] = (byte)random.Next(1, 255);
			connectionSerialNumber[1] = (byte)random.Next(1, 255);
			pathSize[0] = 0x02;


			Byte bEservice = (byte)EService.CM_FowardOpen;
			byte[] bFoward = new byte[46];
			bFoward.SetValue(bEservice,0);
			construireSequence(bFoward,requestPathSize);
			construireSequence(bFoward, classSegment);
			construireSequence(bFoward, instanceSegment);
			construireSequence(bFoward, actualTimeOut);
			construireSequence(bFoward, O_TNetworkID);
			construireSequence(bFoward, T_ONetworkID);
			construireSequence(bFoward, connectionSerialNumber);
			construireSequence(bFoward, vendorId);
			construireSequence(bFoward, originalSerialNumber);
			construireSequence(bFoward, timeOutMultiplier);
			construireSequence(bFoward, reserved);
			construireSequence(bFoward, O_T_RPI);
			construireSequence(bFoward, O_T_NetworkParametre);
			construireSequence(bFoward, T_O_RPI);
			construireSequence(bFoward, T_O_NetworkParametre);
			construireSequence(bFoward, triggerType);
			construireSequence(bFoward, pathSize);
			construireSequence(bFoward, CIPClassSegment);
			construireSequence(bFoward, CIPInstanceSegment);

			return bFoward;
		}


		public byte[] fowardClose()
		{

			pathSize[0] = 0x02;
			ind = 1;
			Byte bEservice = (byte)EService.CM_FowardClose;
			byte[] bFoward = new byte[22];
			bFoward.SetValue(bEservice, 0);
			construireSequence(bFoward, requestPathSize);
			construireSequence(bFoward, classSegment);
			construireSequence(bFoward, instanceSegment);
			construireSequence(bFoward, actualTimeOut);
			construireSequence(bFoward, connectionSerialNumber);
			construireSequence(bFoward, vendorId);
			construireSequence(bFoward, originalSerialNumber);
			construireSequence(bFoward, pathSize);
			byte[] reserve = new byte[] {0x00};
			construireSequence(bFoward,reserve);
			construireSequence(bFoward, CIPClassSegment);
			construireSequence(bFoward, CIPInstanceSegment);

			return bFoward;
		}

		public byte[] CIPRead(String nomVariable)
        {
			ind = 0;
			//on doit trouver la taille de la valeur sVaraible et la mettre dans dataSize
			int nbrByte = nomVariable.Length;
			dataSize[0] = (byte)nbrByte;
			pathSegment[0] = 0x91;

			//Le buffer de la lecture doit toujours être pair. Donc si la variable est impaire, on ajoute " "
			String sNomVariable="";
			if (nbrByte %2==0){
				sNomVariable = nomVariable;
			}
            else
            {
				sNomVariable = nomVariable+" ";
			}

			//on trouve ensuite le nbrByte de sNomVariable
			int nbrbyteVar = sNomVariable.Length;
			pathSize[0] = (byte)((nbrbyteVar / 2) + 1);

			//ensuite on insère le commande Spécifique data qui est 0x00000001
			bufferSpecefic[0] = 0x01;


			//on construit ensuite notre buffer
			
			typeID[0] = 0xb1;
			//on trouve ensuite la longueur du buffer
			length[0] = (byte)(10 + nbrbyteVar);

			//on incremente de 1 a chaque fois pour le compteur

			sequenceCount= BitConverter.GetBytes(sendCompteur());

			Byte[] bEservice = new byte[] { (byte)EService.CIP_ReadData};
			Byte[] bNomVariable = Encoding.Default.GetBytes(sNomVariable);


			int nbrbyteCip = 18 + nbrbyteVar;
			byte[] bCIPRead = new byte[nbrbyteCip];
			construireSequence(bCIPRead, O_TNetworkID);
			construireSequence(bCIPRead, typeID);
			construireSequence(bCIPRead, length);
			construireSequence(bCIPRead, sequenceCount);
			construireSequence(bCIPRead, bEservice);
			construireSequence(bCIPRead, pathSize);
			construireSequence(bCIPRead, pathSegment);
			construireSequence(bCIPRead, dataSize);
			construireSequence(bCIPRead, bNomVariable);
			construireSequence(bCIPRead, bufferSpecefic);

			return bCIPRead;
		}



		public byte[] CIPWrite(Variable variable,String valeur)
        {
			ind = 0;
			//on doit trouver la taille de la valeur sVaraible et la mettre dans dataSize
			int nbrByte = variable.tagPLC.Length;
			dataSize[0] = (byte)nbrByte;
			pathSegment[0] = 0x91;

			//Le buffer de la lecture doit toujours être pair. Donc si la variable est impaire, on ajoute " "
			String sNomVariable = "";
			if (nbrByte % 2 == 0)
			{
				sNomVariable = variable.tagPLC;
			}
			else
			{
				sNomVariable = variable.tagPLC + " ";
			}


			//on trouve ensuite le nbrByte de sNomVariable
			int nbrbyteVar = sNomVariable.Length;
			pathSize[0] = (byte)((nbrbyteVar / 2) + 1);

			typeID[0] = 0xb1;
			//on trouve ensuite la longueur du buffer

			int longValue = 0;

			switch (variable.type)
			{

				case 0xc1:
					longValue = 1;
					break;
				case 0xc3:
					longValue = 2;
					break;
				case 0xc4:
					longValue = 4;
					break;
				case 0xca:
					longValue = 4;
					break;
				case 0xda:
					longValue = valeur.Length+1;
					break;
				default:
					break;
			}

			length[0] = (byte)(10 + nbrbyteVar+ longValue);

			//on incremente de 1 a chaque fois pour le compteur

			sequenceCount = BitConverter.GetBytes(sendCompteur());
			

			Byte[] bEservice = new byte[] { (byte)EService.CIP_WriteData };
			Byte[] bNomVariable = Encoding.Default.GetBytes(sNomVariable);

			//on insere ensuite le type de variable
			byte[] bType = new byte[4];
			bType[0] = variable.type;
			bType[2] = 0x01;

			int nbrbyteCip = 18 + nbrbyteVar;
			byte[] bCIPWrite = new byte[nbrbyteCip];
			construireSequence(bCIPWrite, O_TNetworkID);
			construireSequence(bCIPWrite, typeID);
			construireSequence(bCIPWrite, length);
			construireSequence(bCIPWrite, sequenceCount);
			construireSequence(bCIPWrite, bEservice);
			construireSequence(bCIPWrite, pathSize);
			construireSequence(bCIPWrite, pathSegment);
			construireSequence(bCIPWrite, dataSize);
			construireSequence(bCIPWrite, bNomVariable);
			construireSequence(bCIPWrite, bType);

			return bCIPWrite;
        }







		private void construireSequence(byte[] bCIP, byte[] partition)
		{

			foreach (byte bb in partition)
			{
				bCIP.SetValue(bb, ind);
				ind++;
			}

		}

		private short sendCompteur()
		{
			if (compteur > 32000)
			{
				compteur = 1;
			}
			compteur++;

			return compteur;
		}


	}
	
}
