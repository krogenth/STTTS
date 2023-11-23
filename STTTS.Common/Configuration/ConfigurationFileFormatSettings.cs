using STTTS.IO.Json;

namespace STTTS.Common.Configuration;

internal class ConfigurationFileFormatSettings
{
	public static readonly ConfigurationJsonSerializerContext SerializerContext =
		new(JsonHelper.GetDefaultSerializerOptions());
}
