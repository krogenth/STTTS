using System;
using STTTS.Common.Configuration;

namespace STTTS.UI.ViewModels.Settings;

public class SettingsWindowViewModel : BaseViewModel
{
	public event Action CloseWindow;

	public SettingsAudioViewModel AudioVM { get; }
	public SettingsVoskModelViewModel VoskVM { get; }
	public SettingsVoiceViewModel VoiceVM { get; }

	public SettingsWindowViewModel()
	{
		AudioVM = new();
		VoskVM = new();
		VoiceVM = new();
	}

	public void SaveSettings()
	{
		ConfigurationState.Instance.SaveConfigurationStateToFile();
		CloseWindow?.Invoke();
	}
}
