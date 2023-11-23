using System.Text.Json.Serialization;

namespace STTTS.Common.Configuration;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(ConfigurationFileFormat))]
internal partial class ConfigurationJsonSerializerContext : JsonSerializerContext
{
}
