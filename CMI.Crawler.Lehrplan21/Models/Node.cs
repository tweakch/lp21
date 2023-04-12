namespace CMI.Crawler.Lehrplan21.Models;

public record TreeNode(string Id, string Kind, string Language, string Canton, string Description, string Data, IEnumerable<string> Urls);
