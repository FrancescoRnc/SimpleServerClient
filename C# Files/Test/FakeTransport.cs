using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace SimpleServerClient.Test
{
    public class FakeQueueEmpty : Exception
    {

    }

    public struct FakeData
    {
        public FakeEndpoint EndPoint;
        public byte[] Data;
    }

    class FakeTransport : ITransport
    {
        FakeEndpoint boundAddress;
        Queue<FakeData> recvQueue;
        Queue<FakeData> sendQueue;


        public FakeTransport()
        {
            recvQueue = new Queue<FakeData>();
            sendQueue = new Queue<FakeData>();
        }


        public void ClientEnqueue(FakeData fakeData)
        {
            recvQueue.Enqueue(fakeData);
        }

        public void ClientEnqueue(byte[] packet, string address, int port)
        {
            recvQueue.Enqueue(new FakeData() { Data = packet, EndPoint = new FakeEndpoint(address, port) });
        }

        public FakeData ClientDequeue()
        {
            if (sendQueue.Count <= 0)
                throw new FakeQueueEmpty();
            return sendQueue.Dequeue();
        }

        public void Bind(string address, int port)
        {
            boundAddress = new FakeEndpoint(address, port);
        }

        public EndPoint CreateEndPoint()
        {
            return new FakeEndpoint();
        }

        public byte[] Receive(int buffersize, ref EndPoint sender)
        {
            if (recvQueue.Count == 0)
                return null;
            byte[] data = new byte[buffersize];
            FakeData fakeData = recvQueue.Dequeue();
            if (fakeData.Data.Length > buffersize)
                return null;

            sender = fakeData.EndPoint;
            return fakeData.Data;
        }

        public bool Send(byte[] data, EndPoint destination)
        {
            FakeData fakeData = new FakeData();
            fakeData.Data = data;
            fakeData.EndPoint = destination as FakeEndpoint;
            sendQueue.Enqueue(fakeData);
            return true;
        }
    }
}
