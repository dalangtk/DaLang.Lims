namespace DaLang.Lims.Agent.Options;

public class AgentOption
{
    public List<string> ScanAssemblies { get; set; } = new List<string>();
    public string ApiKey { get; set; }
    public string ModelId { get; set; }
    public string Endpoint { get; set; }
}
