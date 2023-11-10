using System.Collections.Generic;
using System.Linq;
using Godot;
using OffWorld.Extensibility;

namespace OffWorld;

public static class ImagesResolver
{
	internal static void SetData(IEnumerable<ModContent> modContents)
	{
		Images.Clear();
		foreach (var (imageName, image) in modContents.SelectMany(modContent => modContent.Images))
			Images[imageName] = image;
	}
	
	public static Image Resolve(string imageName) => Images[imageName];

	// TODO Use frozen dictionary when it becomes available (.net 8)
	private static readonly Dictionary<string, Image> Images = new();
}