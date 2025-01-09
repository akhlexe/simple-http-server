using System.Net;
using System.Net.Sockets;
using System.Text;

// Creates a TCP Socket
using Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

// Creates an Endpoint
var localEndPoint = new IPEndPoint(IPAddress.Loopback, 8080);

listener.Bind(localEndPoint);

// Start listening for incoming connections
listener.Listen(backlog: 10);

Console.WriteLine("Server listening on http://localhost:8080/");

while(true)
{
    Socket handler = listener.Accept();

    var buffer = new byte[1024];
    int bytesReceived = handler.Receive(buffer);

    // convert the byte array to a string
    string request = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
    Console.WriteLine($"Received request:\n{request}");

    string responseStr = "HTTP/1.1 200 OK\r\n" +
                         "Content-Type: text/plain\r\n" +
                         "Content-Length: 13\r\n" +
                         "\r\n" +
                         "Hello, world!";

    handler.Send(Encoding.UTF8.GetBytes(responseStr));

    handler.Close();
}
