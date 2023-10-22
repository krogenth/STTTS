using STTTS.Common.Audio;
using STTTS.Common.Utility;

namespace STTTS.Common.Configuration;
public class SystemSpeechSynthesizerConfigurationState
{
	public ReactiveObject<string> VoiceID { get; private set; }

	public SystemSpeechSynthesizerConfigurationState()
	{
		VoiceID = new(string.Empty);
	}

	public void LoadDefaultConfiguration()
	{
		VoiceID = new(InstalledVoices.GetVoices().First().VoiceInfo.Id);
	}
}
