using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Serilog;

namespace OffWorld.Definitions;

internal static class DefinitionsLoader
{
	public static ImmutableList<Definition> LoadFilesRecursively(string directoryPath)
	{
		return Directory.EnumerateFiles(directoryPath)
			.Where(filePath => filePath.EndsWith(".json"))
			.SelectMany(DeserializeDefinitionsFromFile)
			.ToImmutableList();
	}

	public static ImmutableList<Definition> LoadFromText(string json) => DeserializeDefinitions(json).ToImmutableList();

	private static readonly ILogger Logger = Log.ForContext(typeof(DefinitionsLoader));
	private static readonly JsonSerializerOptions SerializerOptions = new()
	{
		ReadCommentHandling = JsonCommentHandling.Skip,
		TypeInfoResolver = new DefinitionTypeInfoResolver(),
		Converters = { new JsonStringEnumConverter() }
	};

	private static IEnumerable<Definition> DeserializeDefinitionsFromFile(string filePath)
	{
		var fileContent = File.ReadAllText(filePath);
		return DeserializeDefinitions(fileContent);
	}

	private static IEnumerable<Definition> DeserializeDefinitions(string json)
	{
		var definitions = JsonSerializer.Deserialize<IEnumerable<Definition>>(json, SerializerOptions);
		if (definitions != null)
			return definitions;
		Logger.Warning("Deserialized null when a collection of definitions was expected");
		return Enumerable.Empty<Definition>();
	}
}