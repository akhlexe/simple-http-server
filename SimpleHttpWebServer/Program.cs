using SimpleHttpWebServer.Core;
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

while (true)
{
    Socket socket = listener.Accept();

    var requestId = Guid.NewGuid();

    Console.WriteLine($"Received request: {requestId}");

    new Thread(() => ConnectionHandler.Handle(socket)).Start();
}

static void HandleRequest(Socket handler, Guid requestId)
{
    var buffer = new byte[1024];
    int bytesReceived = handler.Receive(buffer);
    string request = Encoding.UTF8.GetString(buffer, 0, bytesReceived);

    Console.WriteLine(request);

    var response = "HTTP/1.1 200 OK\r\n" +
                   "Content-Type: text/plain\r\n" +
                   "Content-Length: 13\r\n" +
                   "\r\n" +
                   "Hello, world!";

    handler.Send(Encoding.UTF8.GetBytes(response));

    handler.Close();
}

// static void HandleRequest(Socket handler, string request)
// {
//     string filePath = "wwwroot" + request.Split(" ")[1];

//     if (File.Exists(filePath))
//     {
//         byte[] fileBytes = File.ReadAllBytes(filePath);

//         var responseStr = "HTTP/1.1 200 OK\r\n" +
//                         "Content-Type: text/html\r\n" +
//                         $"Content-Length: {fileBytes.Length}\r\n" +
//                         "\r\n";

//         handler.Send(Encoding.UTF8.GetBytes(responseStr));
//         handler.Send(fileBytes);
//     }
//     else if (request.StartsWith("GET /hello"))
//     {
//         string responseStr = "HTTP/1.1 200 OK\r\n" +
//                          "Content-Type: text/plain\r\n" +
//                          "Content-Length: 13\r\n" +
//                          "\r\n" +
//                          "Hello, world!";

//         handler.Send(Encoding.UTF8.GetBytes(responseStr));

//     }
//     else if (request.StartsWith("POST /data"))
//     {
//         string[] requestParts = request.Split("\r\n\r\n");
//         string body = requestParts.Length > 1 ? requestParts[1] : string.Empty;

//         var responseStr = "HTTP/1.1 200 OK\r\n" +
//                         "Content-Type: text/plain\r\n" +
//                         $"Content-Length: {body.Length}\r\n" +
//                         "\r\n" +
//                         body;

//         handler.Send(Encoding.UTF8.GetBytes(responseStr));
//     }
//     else
//     {
//         string responseStr = "HTTP/1.1 404 Not Found\r\n" +
//                              "Content-Type: text/plain\r\n" +
//                              "Content-Length: 9\r\n" +
//                              "\r\n" +
//                              "Not Found";

//         handler.Send(Encoding.UTF8.GetBytes(responseStr));
//     }
// }