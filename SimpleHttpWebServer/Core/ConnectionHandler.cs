using SimpleHttpWebServer.Core.Dtos;
using System.Net.Sockets;
using System.Text;

namespace SimpleHttpWebServer.Core;

internal static class ConnectionHandler
{
    internal static void Handle(Socket socket)
    {
        string rawRequest = ReadRequest(socket);

        Request request = RequestParser.Parse(rawRequest);

        var response = "HTTP/1.1 200 OK\r\n" +
                   "Content-Type: text/plain\r\n" +
                   "Content-Length: 13\r\n" +
                   "\r\n" +
                   "Hello, world!";

        socket.Send(Encoding.UTF8.GetBytes(response));

        socket.Close();
    }

    private static string ReadRequest(Socket socket)
    {
        var buffer = new byte[1024];
        int bytesReceived = socket.Receive(buffer);
        return Encoding.UTF8.GetString(buffer, 0, bytesReceived);
    }
}
