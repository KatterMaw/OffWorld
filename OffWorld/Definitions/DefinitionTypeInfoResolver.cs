using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using CommunityToolkit.Diagnostics;

namespace OffWorld.Definitions;

internal sealed class DefinitionTypeInfoResolver : DefaultJsonTypeInfoResolver
{
	public void AddDefinitionType<TDefinition>() where TDefinition : Definition
	{
		_definitionTypes.Add(new JsonDerivedType(typeof(TDefinition), RemoveDefinitionKeyword(typeof(TDefinition).Name)));
	}

	public void AddDefinitionType(Type definitionType)
	{
		Guard.IsAssignableToType<Definition>(definitionType);
		_definitionTypes.Add(new JsonDerivedType(definitionType, RemoveDefinitionKeyword(definitionType.Name)));
	}

	public void AddDefinitionTypesFromAssembly(Assembly assembly)
	{
		foreach (var definitionType in assembly.GetTypes().Where(type => type.IsAssignableTo(typeof(Definition))))
			AddDefinitionType(definitionType);
	}

	private static string RemoveDefinitionKeyword(string source)
	{
		const string definitionKeyword = "Definition";
		Guard.IsTrue(source.EndsWith(definitionKeyword));
		return source[..^definitionKeyword.Length];
	}
	
	public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
	{
		var jsonTypeInfo = base.GetTypeInfo(type, options);

		var definitionType = typeof(Definition);
		if (jsonTypeInfo.Type == definitionType)
		{
			jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
			{
				TypeDiscriminatorPropertyName = "Type",
				UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization
			};
			foreach (var derivedType in _definitionTypes)
				jsonTypeInfo.PolymorphismOptions.DerivedTypes.Add(derivedType);
		}

		return jsonTypeInfo;
	}

	private readonly List<JsonDerivedType> _definitionTypes = new();
}