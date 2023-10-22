using STTTS.Common.Audio;
using STTTS.Common.Configuration;

namespace STTTS.UI.ViewModels.Settings;

public class SettingsVoiceViewModel : BaseViewModel
{
	public VoiceSelectorViewModel VoiceSelectorVM { get; }

	public SettingsVoiceViewModel()
	{
		VoiceSelectorVM = new(
			InstalledVoices.GetVoices(),
			ConfigurationState.Instance.SystemSpeechSynthesizer.VoiceID.Value
		);

		AttachEventHandlers();
	}

	private void AttachEventHandlers() =>
		VoiceSelectorVM.VoiceChanged += VoiceChanged;

	private void VoiceChanged(object? sender, VoiceChangedEventArgs e) =>
		ConfigurationState.Instance.SystemSpeechSynthesizer.VoiceID.Value = e.VoiceID;
}
