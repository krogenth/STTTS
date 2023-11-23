namespace STTTS.Common.Configuration;

public sealed class ConfigurationState
{
	private readonly string _fileLocation = $"{AppDomain.CurrentDomain.BaseDirectory}config.json";

	private static readonly Lazy<ConfigurationState> _instance = new(() => new ConfigurationState());
	public static ConfigurationState Instance => _instance.Value;

	public AudioConfigurationState Audio { get; }
	public RecognizerConfigurationState Recognizer { get; }
	public VoskConfigurationState Vosk { get; }
	public WhisperConfigurationState Whisper { get; }
	public SystemSpeechSynthesizerConfigurationState SystemSpeechSynthesizer { get; }

	private ConfigurationState()
	{
		Audio = new();
		Recognizer = new();
		Vosk = new();
		Whisper = new();
		SystemSpeechSynthesizer = new();
	}

	public void LoadConfiguration()
	{
		if (File.Exists(_fileLocation))
		{
			LoadConfigurationStateFromFile();
		}
		else
		{
			LoadDefaultConfigurationState();
		}
	}

	private void LoadConfigurationStateFromFile()
	{
		if (ConfigurationFileFormat.TryLoad(_fileLocation, out ConfigurationFileFormat? configurationFileFormat))
		{
			Audio.LoadFileConfiguration(configurationFileFormat!);
			Vosk.LoadFileConfiguration(configurationFileFormat!);
			SystemSpeechSynthesizer.LoadFileConfiguration(configurationFileFormat!);
		}
	}

	private void LoadDefaultConfigurationState()
	{
		Audio.LoadDefaultConfiguration();
		Recognizer.LoadDefaultConfiguration();
		Vosk.LoadDefaultConfiguration();
		Whisper.LoadDefaultConfiguration();
		SystemSpeechSynthesizer.LoadDefaultConfiguration();
	}

	public void SaveConfigurationStateToFile() =>
		new ConfigurationFileFormat(this).SaveFile(_fileLocation);
}
