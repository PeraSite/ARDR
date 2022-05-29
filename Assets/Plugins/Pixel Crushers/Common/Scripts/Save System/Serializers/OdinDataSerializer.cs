using System.Text;
using PixelCrushers;
using Sirenix.Serialization;

public class OdinDataSerializer : DataSerializer {
	public DataFormat Format;

	public override string Serialize(object data) {
		return Encoding.UTF8.GetString(SerializationUtility.SerializeValue(data, Format));
	}

	public override T Deserialize<T>(string s, T data = default) {
		var bytes = Encoding.UTF8.GetBytes(s);
		return SerializationUtility.DeserializeValue<T>(bytes, Format);
	}
}
