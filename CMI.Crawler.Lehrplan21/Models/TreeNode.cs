namespace CMI.Crawler.Lehrplan21.Models;

public record TreeNode(string? Id, Lp21? Data = null, IEnumerable<Uri>? Urls = null);

