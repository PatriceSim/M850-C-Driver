using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;


namespace Micro800Connection.DriverM800
{
    class SocketSend
    {
        private String adresseIP = "";
        private int port = 0;
        private Socket s;
        private Queue qIn;
        private Queue qOut;
        private Thread t;
        
        public SocketSend(String adresseIp, int port, Queue qIn,Queue qOut )
        {
            this.adresseIP = adresseIp;
            this.port = port;
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.SendTimeout = 1000;
            s.ReceiveTimeout = 1000;
            s.Connect(this.adresseIP, this.port);
            this.qIn = qIn;
            this.qOut = qOut;
            t=new Thread(new ThreadStart(receiveSocket));
            t.Start();
                 
        }

        private void receiveSocket()
        {

            int ind = 0;
            while (true)
            {
                if (qIn != null)
                {
                    ind = qIn.Count;
                }
                
                if (ind > 0)
                {

                    Byte[] br = new Byte[255];
                    int lenth = 0;
                    try
                    {
                        s.Send((Byte[]) qIn.Dequeue());
                        lenth = s.Receive(br);
                    }
                    catch (Exception e)
                    {
                        String error = e.ToString();
                    }

                    if (lenth !=0)
                    {
                        qOut.Enqueue(br);
                    }

                    ind = qIn.Count;
                }
                Thread.Sleep(5);
            }
            
            
        }

    }
}
