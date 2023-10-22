﻿namespace STTTS.Common.Configuration;

public sealed class ConfigurationState
{
	private readonly string _fileLocation = $"{AppDomain.CurrentDomain.BaseDirectory}config.json";

	private static readonly Lazy<ConfigurationState> _instance = new(() => new ConfigurationState());
	public static ConfigurationState Instance => _instance.Value;

	public AudioConfigurationState Audio { get; }
	public VoskConfigurationState Vosk { get; }
	public SystemSpeechSynthesizerConfigurationState SystemSpeechSynthesizer { get; }

	private ConfigurationState()
	{
		Audio = new();
		Vosk = new();
		SystemSpeechSynthesizer = new();

		LoadConfiguration();
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

	}

	private void LoadDefaultConfigurationState()
	{
		Audio.LoadDefaultConfiguration();
		Vosk.LoadDefaultConfiguration();
		SystemSpeechSynthesizer.LoadDefaultConfiguration();
	}
}
