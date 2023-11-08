using System;
using Godot;
using OffWorld.Definitions;

namespace OffWorld.addons.OffWorldDevTools;

[Tool]
public partial class DefinitionsDeserializerDock : Control
{
	[Export] public TextEdit? Editor { get; set; }
	[Export] public Label? Output { get; set; }
	
	public void OnEditorTextChanged()
	{
		if (Editor == null || Output == null)
		{
			GD.PushWarning($"{nameof(Editor)} or {nameof(Output)} isn't set in {nameof(DefinitionsDeserializerDock)}, no operations will be performed.");
			return;
		}
		try
		{
			var definitions = DefinitionsLoader.LoadFromText(Editor.Text);
			Output.Text = $"Deserialized {definitions.Count} definitions";
		}
		catch (Exception e)
		{
			Output.Text = e.ToString();
		}
	}
}