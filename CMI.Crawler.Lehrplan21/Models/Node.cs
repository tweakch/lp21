namespace CMI.Crawler.Lehrplan21;

public record TreeNode(string Id, string Kind, string Data, IEnumerable<string> Urls);
