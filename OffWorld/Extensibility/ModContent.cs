using System.Collections.Immutable;
using OffWorld.Definitions;

namespace OffWorld.Extensibility;

public sealed class ModContent
{
	internal static ModContent Load(ModInformation modInformation)
	{
		var definitions = DefinitionsLoader.LoadFilesRecursively(modInformation.Directory);
		return new ModContent(definitions);
	}
	
	public ImmutableList<Definition> Definitions { get; }

	private ModContent(ImmutableList<Definition> definitions)
	{
		Definitions = definitions;
	}
}