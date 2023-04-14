namespace CMI.Crawler.Lehrplan21;

public partial class Worker
{
    public class LpNode
    {
        public const string RootNodeId = "000000000000000000000000000000000";
        public string Id { get; set; }
        public LpNode? Parent { get; set; }
        public List<LpNode> Children { get; set; }
        public bool Existing { get; internal set; }

        public LpNode()
        {
            Id = RootNodeId;
            Children = new List<LpNode>();
        }
    }
}
