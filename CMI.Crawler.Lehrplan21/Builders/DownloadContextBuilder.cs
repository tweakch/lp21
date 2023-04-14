using CMI.Crawler.Lehrplan21.Models;
using System.Net;
using static CMI.Crawler.Lehrplan21.Worker;

namespace CMI.Crawler.Lehrplan21.Builders
{

    public class DownloadContextBuilder
    {
        private string _nodeId = string.Empty;
        private string _language = string.Empty;
        private string _canton = string.Empty;
        private bool _existing = false;

        public DownloadContext Build()
        {
            if (_nodeId.Length < 4) { throw new ArgumentException("nodeId must be 4 characters or longer"); }
            return new DownloadContext(_nodeId, _existing, _language, _canton);
        }

        public DownloadContextBuilder WithNodeId(string nodeId)
        {
            _nodeId = nodeId;
            return this;
        }
        public DownloadContextBuilder WithLanguage(string language)
        {
            _language = language;
            return this;
        }
        public DownloadContextBuilder WithCanton(string canton)
        {
            _canton = canton;
            return this;
        }
        public DownloadContextBuilder WithExisting(bool existing)
        {
            _existing = existing;
            return this;
        }

        public static DownloadContext FromConfig(CrawlerConfig config)
        {
            return new DownloadContextBuilder().WithNodeId(config.Uid).WithCanton(config.Canton).WithLanguage(config.Language).Build();
        }

        public bool TryBuild(out DownloadContext context)
        {
            try
            {
                context = Build();
                return true;
            }
            catch (Exception)
            {
                context = FromConfig(CrawlerConfig.Default);
                return false;
            }
        }

        private DownloadContext? FromConfig(object @default)
        {
            throw new NotImplementedException();
        }
    }
}