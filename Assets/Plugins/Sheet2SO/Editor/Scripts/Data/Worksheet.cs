using Sheet2SO.Editor;
using Sirenix.OdinInspector;

public class Worksheet {
	public string Name;
	public string PathName;
	public int SkipRow;

	[Required]
	public SheetSerializer Serializer;
}
