using System;
using STTTS.Common.Configuration;

namespace STTTS.UI.ViewModels.Settings;

public class SettingsWindowViewModel : BaseViewModel
{
	public event Action CloseWindow;

	public SettingsAudioViewModel AudioVM { get; }
	public SettingsRecognizerViewModel RecognizerVM { get; }
	public SettingsVoskModelViewModel VoskVM { get; }
	public SettingsWhisperModelViewModel WhisperVM { get; }
	public SettingsVoiceViewModel VoiceVM { get; }

	public SettingsWindowViewModel()
	{
		AudioVM = new();
		RecognizerVM = new();
		VoskVM = new();
		WhisperVM = new();
		VoiceVM = new();
	}

	public void SaveSettings()
	{
		ConfigurationState.Instance.SaveConfigurationStateToFile();
		CloseWindow?.Invoke();
	}
}
