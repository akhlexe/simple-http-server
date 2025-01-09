namespace SimpleHttpWebServer.Core.Dtos;

public sealed record Request(
    string Method,
    string Path,
    string Protocol,
    IReadOnlyDictionary<string, string> Headers,
    string Body);
