namespace CMI.Crawler.Lehrplan21.Models;

public record PersistContext(TreeNode Node, string OutputDirectory) : CrawlerContext
{
    public PersistContext(TreeNode node, string outputDirectory, string language, string canton) : this(node, outputDirectory)
    {
        Language = language;
        Canton = canton;
    }

    public string GetFileName(string extension = ".csv")
    {
        return $"lp21_{Canton}_{Language}{extension}";
    }
}
