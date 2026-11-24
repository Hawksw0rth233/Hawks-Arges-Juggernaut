using SPTarkov.Server.Core.Models.Spt.Mod;
namespace Hawks.Arges.Juggernaut;
public record ModMetadata : IModMetadata
{
    public string ModGuid { get; init; } = "com.hawks.arges.juggernaut";
    public string Name { get; init; } = "Hawks's Arges Juggernaut Integration";
    public string Author { get; init; } = "Hawks";
    public List<string>? Contributors { get; init; }
    public SemanticVersioning.Version Version { get; init; } = new("1.0.1");
    public SemanticVersioning.Range SptVersion { get; init; } = new("~4.1.6");
    public List<string>? Incompatibilities { get; init; }
    public Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; } = new() { ["team.aminus.arges"] = new(">=1.1.0"), ["com.silian.juggernaut"] = new("~1.1.1") };
    public string? Url { get; init; } = "https://github.com/Hawksw0rth233/Hawks-M135-Arges";
    public string License { get; init; } = "MIT (mod code); third-party game assets excluded";
    public bool HasPrepatcher { get; init; } = false;
}


