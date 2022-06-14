using System.Collections.Generic;
using Sirenix.OdinInspector;

public class Sheet {
	public string Id;

	[FolderPath]
	public string Path;

	[HideReferenceObjectPicker]
	public List<Worksheet> Worksheets = new();
}