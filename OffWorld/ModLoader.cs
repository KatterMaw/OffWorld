using System.IO;
using System.Linq;
using System.Text.Json;
using CommunityToolkit.Diagnostics;

namespace OffWorld;

public static class ModLoader
{
	public static ModInformation LoadInformation(string directory)
	{
		var informationFilePath = Path.Combine(directory, InformationFileName);
		var informationJson = File.ReadAllText(informationFilePath);
		var info = JsonSerializer.Deserialize<ModInfo>(informationJson);
		Guard.IsNotNull(info);
		return new ModInformation(info.Id, info.Name, directory);
	}
	
	public static Mod Load(ModInformation modInformation)
	{
		var hasAnyAssemblies = ModAssembly.EnumerateDlls(modInformation).Any();
		if (!hasAnyAssemblies)
			return new Mod(modInformation, ModContent.Load(modInformation));
		return new AssemblyMod(modInformation, ModContent.Load(modInformation), ModAssembly.Load(modInformation));
	}
	
	private const string InformationFileName = "Information.json";
	
	private sealed class ModInfo
	{
		public required string Id { get; init; }
		public required string Name { get; init; }
	}
}