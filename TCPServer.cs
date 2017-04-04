using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using UnityEngine;
using System.Net.Sockets;
using System.Text;

public class TCPServer : MonoBehaviour {

    public int port = 50000;
    public string host = "127.0.0.1";

    public void ListenForMessages()
    {
        TcpListener server = null;
        try
        {
            IPAddress localaddr = IPAddress.Parse(host);

            // tcplistener server = new tcplistener(port);
              server = new TcpListener(localaddr, port);
        
            // start listening for client requests.
            server.Start();

            // buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;

            // enter the listening loop.
            while (true)
            {
                Debug.Log("waiting for a connection... ");

                // perform a blocking call to accept requests.
                // you could also user server.acceptsocket() here.
                TcpClient client = server.AcceptTcpClient();
                Debug.Log("connected!");

                data = null;

                // get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                int i;

                // loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // translate data bytes to a ascii string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Debug.Log(String.Format("received: {0}", data));

                    // process the data sent by the client.
                    data = data.ToUpper();

                    Byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                    // send back a response.
                    stream.Write(msg, 0, msg.Length);
                    Debug.Log(String.Format("sent: {0}", data));
                }

                // shutdown and end connection
                client.Close();
            }
        }
        catch (SocketException e)
        {
            Debug.Log(String.Format("socketexception: {0}", e));
        }
        finally
        {
            // stop listening for new clients.
            server.Stop();
        }
    }
}