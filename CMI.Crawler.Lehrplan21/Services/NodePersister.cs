using CMI.Crawler.Lehrplan21.Models;
using System.Reflection;
using System.Text;

namespace CMI.Crawler.Lehrplan21.Services;

public class NodePersister : INodePersister
{
    public async Task PersistNodeAsync(PersistContext context, CancellationToken cancellationToken)
    {
        // write the node to file
        var filename = new FileInfo(Path.Combine(context.OutputDirectory, context.Canton, context.GetFileName()));
        var sb = new StringBuilder();

        try
        {
            if (context.Node.Id == null) sb.AppendLine(CsvHeader);
            sb.AppendLine(string.Join(";", SerializeableProperties.Select(p => EscapeCsvValue(p.GetValue(context.Node.Data)))));
            
            filename.Directory?.Create();

            await File.AppendAllTextAsync(filename.FullName, sb.ToString(), Encoding.Unicode, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while persisting the node to the file: {ex.Message}");
        }
    }

    public static string EscapeCsvValue(object? value)
    {
        var stringValue = value?.ToString() ?? string.Empty;
        if (stringValue.Contains(';')) {
            stringValue = stringValue.Replace(';','_');
        }
        if (stringValue.Contains('\n'))
        {
            stringValue = stringValue.Replace('\n', '_');
        }
        if (stringValue.Contains('\r'))
        {
            stringValue = stringValue.Replace('\r', '_');
        }

        if (stringValue.Contains('"'))
        {
            stringValue = stringValue.Replace("\"", "\"\"");
        }
        return stringValue;
    }

    private static string CsvHeader => string.Join(";", SerializeableProperties.Select(p => p.Name));

    private static IEnumerable<PropertyInfo> SerializeableProperties => typeof(Lp21).GetProperties().Where(p => p.PropertyType == typeof(string) || p.PropertyType == typeof(Uri)).ToArray();
}
