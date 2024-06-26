using STTTS.IO.Json;

namespace STTTS.Common.Configuration;

public class ConfigurationFileFormat
{
	public string VoskModelDirectory { get; set; }
	public string SystemSpeechVoiceID { get; set; }
	public string InputDeviceID { get; set; }
	public string OutputDeviceID { get; set; }
	public string PlaybackDeviceID { get; set; }
	public int SendPort { get; set; }

	public ConfigurationFileFormat() { }

	public ConfigurationFileFormat(ConfigurationState state)
	{
		VoskModelDirectory = state.Vosk.ModelDirectory.Value;
		SystemSpeechVoiceID = state.SystemSpeechSynthesizer.VoiceID.Value;
		InputDeviceID = state.Audio.InputDeviceID.Value;
		OutputDeviceID = state.Audio.OutputDeviceID.Value;
		PlaybackDeviceID = state.Audio.PlaybackDeviceID.Value;
		SendPort = state.OSC.SendPort.Value;
	}

	public static bool TryLoad(string filepath, out ConfigurationFileFormat? configurationFileFormat)
	{
		try
		{
			configurationFileFormat = JsonHelper.DeserializeFromFile(
				filepath,
				ConfigurationFileFormatSettings.SerializerContext.ConfigurationFileFormat
			);
			return configurationFileFormat != null;
		}
		catch
		{
			configurationFileFormat = null;
			return false;
		}
	}

	public void SaveFile(string filepath) =>
		JsonHelper.SerializeToFile(
			filepath,
			this,
			ConfigurationFileFormatSettings.SerializerContext.ConfigurationFileFormat
		);
}
