namespace STTTS.UI.ViewModels.Settings;

public class SettingsWindowViewModel : BaseViewModel
{
	public SettingsAudioViewModel AudioVM { get; }
	public SettingsVoskModelViewModel VoskVM { get; }
	public SettingsVoiceViewModel VoiceVM { get; }

	public SettingsWindowViewModel()
	{
		AudioVM = new();
		VoskVM = new();
		VoiceVM = new();
	}
}
