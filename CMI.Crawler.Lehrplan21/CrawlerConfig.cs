namespace CMI.Crawler.Lehrplan21;

public class CrawlerConfig
{
    // Root                           000000000000000000000000000000000
    //   Sprachen                     101fby8NKE8fCB79TRL69VS8VT4HnuHmN
    //     Deutsch                    101ffPWHRKNUdFDK9LRgFbPDLXwTxa4bw
    //       1. Hören                 101kbxe4gXVenMup9zbx8t49eAVq5UbCS



    public string Uid {get; set;}
    public List<string> Cantons { get; set; }
    public List<string> Languages { get; set; }
    public bool DepthFirst { get; set; }
}
