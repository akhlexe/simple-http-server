using SimpleHttpWebServer.Core.Dtos;

namespace SimpleHttpWebServer.Core;

internal static class RequestParser
{
    public static Request Parse(string request)
    {
        string[] headersAndBody = request.Split("\r\n\r\n");

        if (headersAndBody.Length != 2 )
        {
            throw new ArgumentException("HTTP Requests should have only one separation between headers and body.");
        }

        string[] headersWithStartLine = headersAndBody[0].Split("\r\n");

        RequestStartLine requestStartLine = ProduceStartLine(headersWithStartLine[0].Split(" "));
        RequestHeaders requestHeaders = ProduceHeaders(headersWithStartLine[1..]);
        RequestBody requestBody = ProduceBody(headersAndBody[1]);

        return new Request(
            requestStartLine.Method,
            requestStartLine.Path,
            requestStartLine.Protocol,
            requestHeaders.Headers,
            requestBody.Body);

    }

    private static RequestStartLine ProduceStartLine(string[] startLine)
    {
        if (startLine.Length != 3)
        {
            throw new InvalidDataException("Startline of request has wrong format.");
        }

        string method = startLine[0];
        string path = startLine[1];
        string protocol = startLine[2];

        return new RequestStartLine(method, path, protocol);
    }

    private static RequestHeaders ProduceHeaders(string[] headersPart)
    {
        var headersDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        Dictionary<string, string> requestHeaders = headersPart
            .Where(header => !string.IsNullOrEmpty(header))
            .Select(CollectKeyValueHeader)
            .ToDictionary();

        return new RequestHeaders(requestHeaders.AsReadOnly());
    }

    private static KeyValuePair<string, string> CollectKeyValueHeader(string header)
    {
        int separatorIndex = header.IndexOf(':');

        if (separatorIndex < 0)
        {
            throw new InvalidDataException($"Request headers is malformed for header: {header}");
        }

        string key = header[..separatorIndex].Trim();
        string value = header[(separatorIndex + 1)..].Trim();

        return KeyValuePair.Create(key, value);
    }

    private static RequestBody ProduceBody(string secondPart)
    {
        return new RequestBody(secondPart);
    }
}
