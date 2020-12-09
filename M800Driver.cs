using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;


namespace Micro800Connection.DriverM800
{
    class M800Driver
    {
        private String adresseIP = "";
        private int port = 0;
        private SocketSend socketSend;
        private Header header;
        private CommandeData commandeData;
        private CipProtocol cipProtocol;
        private byte[] sessionHandle;
        private ConvertHex convertHex;
        private ConvertToHex convertToHex;
        public ArrayList MesVariable { set; get; }
        private System.Timers.Timer timerSendBuffer;
        private Queue qIn;
        private Queue qOut;
        public String fauteMessage {get; set;}


        public M800Driver(String adresseIp,int port)
        {
            this.adresseIP = adresseIp;
            this.port = port;
            header = new Header();
            commandeData = new CommandeData();
            cipProtocol = new CipProtocol();
            sessionHandle = new byte[4];
            convertHex = new ConvertHex();
            convertToHex = new ConvertToHex();
            qIn =new Queue();
            qOut = new Queue();
            socketSend = new SocketSend(this.adresseIP, this.port, qIn, qOut);
            MesVariable = new ArrayList();
            fauteMessage = "";

            //configuration du timerSendBuffer
            timerSendBuffer = new System.Timers.Timer();
            timerSendBuffer.Elapsed += new ElapsedEventHandler(sendBuffer);
            timerSendBuffer.Interval = 10;
        }


        public void connection()
        {
            registerSession();
        }

        private void registerSession()
        {
            header.eCommande = Header.ECommande.RegisterSessions;
            header.longueur[0]=0x04;
            byte[] bheader = new byte[24];
            bheader = header.sendHeader();
            byte[] bCommandeData = new byte[4];
            bCommandeData = commandeData.sendHeader();

            byte[] bregister = new byte[28];
            construireByte(bregister,bheader,0);
            construireByte(bregister,bCommandeData, 24);

            qIn.Enqueue(bregister);
                        
            byte[] bReceive = new byte[255];
            int count = 0;
                      
            while (count == 0)
            {
                count = qOut.Count;
                Thread.Sleep(100);
            }

            bReceive = (byte[])qOut.Dequeue();

            sessionHandle[0] = bReceive[4];
            sessionHandle[1] = bReceive[5];
            sessionHandle[2] = bReceive[6];
            sessionHandle[3] = bReceive[7];
            header.nHandleSession = sessionHandle;
                        
            FowardOpen();

        }

        private void FowardOpen()
        {
            byte[] bCipFoward = new byte[46];
            bCipFoward= cipProtocol.fowardOpen();
            commandeData.length[0] =(byte) bCipFoward.Length;
            byte[] bCommandData = new byte[16];
            bCommandData = commandeData.sendForward();

            header.longueur[0] = (byte)(bCipFoward.Length + bCommandData.Length);
            header.eCommande = Header.ECommande.SendRRData;
            byte[] bheader = new byte[24];
            bheader = header.sendFowardHeader();

            byte[] bFoward = new byte[86];
            construireByte(bFoward, bheader,0);
            construireByte(bFoward, bCommandData, bheader.Length);
            construireByte(bFoward, bCipFoward, (bheader.Length+ bCommandData.Length));
            byte[] bReceive = new byte[255];

            qIn.Enqueue(bFoward);

            int count = 0; 
               
            while (count == 0)
            {
                count = qOut.Count;
                Thread.Sleep(100);
            }

            bReceive = (byte[])qOut.Dequeue();

            cipProtocol.O_TNetworkID[0] = bReceive[44];
            cipProtocol.O_TNetworkID[1] = bReceive[45];
            cipProtocol.O_TNetworkID[2] = bReceive[46];
            cipProtocol.O_TNetworkID[3] = bReceive[47];

            timerSendBuffer.Enabled = true;

        }

        public void close()
        {
            fowardClose();
            //registerClose();
        }

        private void fowardClose()
        {
            byte[] bCipFoward = new byte[22];
            bCipFoward = cipProtocol.fowardClose();
            commandeData.length[0] = (byte)bCipFoward.Length;
            byte[] bCommandData = new byte[16];
            bCommandData = commandeData.forwardClose();

            header.longueur[0] = (byte)(bCipFoward.Length + bCommandData.Length);
            header.eCommande = Header.ECommande.SendRRData;
            byte[] bheader = new byte[24];
            bheader = header.sendFowardHeader();

            byte[] bFoward = new byte[86];
            construireByte(bFoward, bheader, 0);
            construireByte(bFoward, bCommandData, bheader.Length);
            construireByte(bFoward, bCipFoward, (bheader.Length + bCommandData.Length));
            byte[] bReceive = new byte[255];

            qIn.Enqueue(bFoward);
        }

        private void registerClose()
        {
            header.eCommande = Header.ECommande.UnRegisterSession;
            header.longueur[0] = 0x00;
            byte[] bheader = new byte[24];
            bheader = header.sendFowardHeader();
            byte[] bReceive = new byte[255];
            qIn.Enqueue(bheader);
        }


        private byte variableRead(String NomVariable)
        {
            byte[] bCipRead = cipProtocol.CIPRead(NomVariable);
            byte[] bCommandData = commandeData.commandRead();
            header.eCommande = Header.ECommande.SendUnitData;
            header.longueur[0] =(byte) (bCipRead.Length+ bCommandData.Length);
            byte[] bheader = header.sendHeader();

            int longBuff = bCipRead.Length + bCommandData.Length + 24;

            byte[] bRead = new byte[longBuff];
            construireByte(bRead, bheader, 0);
            construireByte(bRead, bCommandData, bheader.Length);
            construireByte(bRead, bCipRead, (bheader.Length + bCommandData.Length));

            qIn.Enqueue(bRead);

            return (bRead[44]);
        }



        public String read(String nomVariable)
        {
            String valeur="";
            foreach (Variable variable in MesVariable)
            {
                if (variable.nom == nomVariable)
                {
                    valeur = variable.valeur;
                    break;
                }

            }
            return valeur;
        }

        public String readFloat(String nomVariable,int nbrDecimal)
        {
            String valeur = "";
            foreach (Variable variable in MesVariable)
            {
                if (variable.nom == nomVariable)
                {
                    valeur = variable.valeur;
                    break;
                }

            }
            string s = "0";
            if (valeur != "")
            {
                double var = Convert.ToDouble(valeur);
                s = string.Format("{0:N"+nbrDecimal+"}", var);
            }

            return s;
        }





        public void write(String nomVariable,String Valeur)
        {
            foreach (Variable variable in MesVariable)
            {
                if (variable.nom==nomVariable)
                {
                    //Quand la variable est trouve, il faut trouver son type
                    byte type = variable.type;
                    if (type != 0x00)
                    {
                        byte[] bValeur=convertToHex.convert(variable,Valeur);
                        fauteMessage = convertToHex.fauteMessage;
                        byte[] bCipWrite = cipProtocol.CIPWrite(variable, Valeur);
                        byte[] bCommandData = commandeData.commandRead();
                        header.eCommande = Header.ECommande.SendUnitData;
                        header.longueur[0] = (byte)(bValeur.Length+bCipWrite.Length + bCommandData.Length);
                        byte[] bheader = header.sendHeader();

                        int longBuff = bValeur.Length+bCipWrite.Length + bCommandData.Length + 24;

                        byte[] bWrite = new byte[longBuff];

                        construireByte(bWrite, bheader, 0);
                        construireByte(bWrite, bCommandData, bheader.Length);
                        construireByte(bWrite, bCipWrite, (bheader.Length + bCommandData.Length));
                        construireByte(bWrite, bValeur, (bheader.Length + bCommandData.Length+ bCipWrite.Length));

                        qIn.Enqueue(bWrite);
                    }



                    break;

                }

            }

        }


        public void createVariable(String nom, String tagPLC, double intervale)
        {
            Variable variable = new Variable();
            variable.nom = nom;
            variable.tagPLC = tagPLC;
            variable.intervale = intervale;
            variable.dateTime= DateTime.Now;
            variable.valeur = "";
            MesVariable.Add(variable);
        }




        private void construireByte(byte[] byteIn, byte[] partition,int indice)
        {
            foreach (byte bb in partition)
            {
                byteIn.SetValue(bb, indice);
                indice++;
            }
        }


        private void sendBuffer(object source, ElapsedEventArgs e)
        {
            foreach (Variable variable in MesVariable)
            {
                DateTime dateTimeNow = DateTime.Now;
                DateTime dateTimeNext = variable.dateTime.AddMilliseconds(variable.intervale);
                if (dateTimeNow > dateTimeNext)
                {
                    variable.counter= variableRead(variable.tagPLC);

                    variable.dateTime = dateTimeNow;
                }
            }

            while (qOut.Count>0)
            {
                byte[] bReceive = new byte[255];
                bReceive = (byte[])qOut.Dequeue();
                String Valeur = convertHex.convert(bReceive);
                byte countRep = bReceive[44];
                byte type = bReceive[50];
                foreach (Variable variable in MesVariable)
                {
                    if (variable.counter == countRep)
                    {
                        variable.valeur = Valeur;
                        variable.type = type;
                    }

                }
            }

        }

    }
        
}