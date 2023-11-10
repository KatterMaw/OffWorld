using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using OffWorld.Definitions;

namespace OffWorld;

public sealed class ModAssembly : IDisposable
{
	internal static ModAssembly Load(ModInformation modInformation)
	{
		var loadContext = new AssemblyLoadContext(modInformation.Name, true);
		foreach (var dll in EnumerateDlls(modInformation))
			loadContext.LoadFromAssemblyPath(dll);
		var definitionTypes = GetDefinitionTypes(loadContext);
		return new ModAssembly(loadContext, definitionTypes);
	}

	internal static IEnumerable<string> EnumerateDlls(ModInformation modInformation) =>
		Directory.EnumerateFiles(modInformation.Directory, "*.dll", SearchOption.AllDirectories);

	private static ImmutableList<Type> GetDefinitionTypes(AssemblyLoadContext assemblyLoadContext) =>
		assemblyLoadContext.Assemblies
			.SelectMany(assembly => assembly.GetTypes())
			.Where(type => type.IsAssignableFrom(typeof(Definition)) && !type.IsAbstract)
			.ToImmutableList();

	public ImmutableList<Type> DefinitionTypes { get; }

	public void Dispose() => _loadContext.Unload();

	private readonly AssemblyLoadContext _loadContext;

	private ModAssembly(AssemblyLoadContext loadContext, ImmutableList<Type> definitionTypes)
	{
		_loadContext = loadContext;
		DefinitionTypes = definitionTypes;
	}
}