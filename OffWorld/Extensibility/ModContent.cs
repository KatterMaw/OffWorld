using System.Collections.Immutable;
using System.IO;
using CommunityToolkit.Diagnostics;
using Godot;
using OffWorld.Definitions;

namespace OffWorld.Extensibility;

public sealed class ModContent
{
	internal static ModContent Load(ModInformation modInformation)
	{
		var definitions = DefinitionsLoader.LoadFilesRecursively(modInformation.Directory);
		var images = Directory.GetFiles(modInformation.Directory, "*.png", SearchOption.AllDirectories)
			.ToImmutableDictionary(GetFileNameWithoutExtension, Image.LoadFromFile);
		return new ModContent(definitions, images);
	}
	
	private static string GetFileNameWithoutExtension(string path)
	{
		var result = Path.GetFileNameWithoutExtension(path);
		Guard.IsNotNull(result);
		return result;
	}

	public ImmutableList<Definition> Definitions { get; }
	public ImmutableDictionary<string, Image> Images { get; }

	private ModContent(ImmutableList<Definition> definitions, ImmutableDictionary<string, Image> images)
	{
		Definitions = definitions;
		Images = images;
	}
}