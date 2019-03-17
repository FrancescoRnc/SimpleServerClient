using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SimpleServerClient.Test
{
    class TestServer
    {
        Server server;
        FakeTransport serverTransport;

        Client client;
        FakeTransport clientTransport;

        [SetUp]
        public void SetUpTests()
        {
            serverTransport = new FakeTransport();
            serverTransport.Bind("tester", 00);
            server = new Server(serverTransport);

            clientTransport = new FakeTransport();
            client = new Client(clientTransport);
        }

        [Test]
        public void TestReceiveMessage()
        {
            client.SendMessage("Ciao");
            client.Step();
            serverTransport.ClientEnqueue(clientTransport.ClientDequeue());
            server.Step();

            Assert.That(server.LastMessageReceived, Is.EqualTo("Ciao"));
        }

        [Test]
        public void TestReceiveMessageAndInverted()
        {
            client.SendMessage("Ciao");
            client.Step();
            serverTransport.ClientEnqueue(clientTransport.ClientDequeue());
            server.Step();         
            
            Assert.That(server.LastInvertedMessage, Is.EqualTo("oaiC"));
        }

        [Test]
        public void TestReceiveMessageIsEmpty()
        {
            client.SendMessage("");
            client.Step();

            Assert.That(() => clientTransport.ClientDequeue(), Throws.InstanceOf<FakeQueueEmpty>());            
        }

        [Test]
        public void TestClientFirstStepWorks()
        {
            Assert.That(() => client.Step(), Throws.Nothing);
        }

        [Test]
        public void TestClientSendedMessageIsOk()
        {
            client.SendMessage("Ciao");
            client.Step();
            string messageTaken = Encoding.UTF8.GetString(clientTransport.ClientDequeue().Data);

            Assert.That(messageTaken, Is.EqualTo("Ciao"));
        }

        [Test]
        public void TestClientSendMessageToServerQueueNotEmpty()
        {
            client.SendMessage("Ciao");
            client.Step();

            serverTransport.ClientEnqueue(clientTransport.ClientDequeue());
            server.Step();
            
            Assert.That(() => serverTransport.ClientDequeue(), Throws.Nothing);
        }

        [Test]
        public void TestClientSendMessageToServerNotNull()
        {
            client.SendMessage("Ciao");
            client.Step();

            serverTransport.ClientEnqueue(clientTransport.ClientDequeue());
            server.Step();

            Assert.That(() => serverTransport.ClientDequeue(), Is.Not.Null);
        }

        [Test]
        public void TestClientSendMessageToServerWork()
        {
            client.SendMessage("Ciao");
            client.Step();

            serverTransport.ClientEnqueue(clientTransport.ClientDequeue());
            server.Step();

            clientTransport.ClientEnqueue(serverTransport.ClientDequeue());
            client.Step();

            Assert.That(() => client.LastInvertedMessageReceived, Is.Not.EqualTo("Ciao"));
            Assert.That(() => client.LastInvertedMessageReceived, Is.EqualTo("oaiC"));
        }
    }
}
