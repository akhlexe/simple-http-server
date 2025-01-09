namespace SimpleHttpWebServer.Core.Dtos;

internal sealed record RequestStartLine(string Method, string Path, string Protocol);

internal sealed record RequestHeaders(IReadOnlyDictionary<string, string> Headers);

internal sealed record RequestBody(string Body);