namespace OffWorld.Definitions;

public sealed class TerrainDefinition : Definition
{
	public required string Name { get; init; }
	public string? TextureName { get; init; }
}