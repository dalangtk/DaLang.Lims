namespace DaLang.Lims.Agent.Attibutes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class AgentToolAttribute : Attribute
{
    public string Name { get; }
    public string Description { get; }

    public AgentToolAttribute(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
