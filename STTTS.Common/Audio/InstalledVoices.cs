using System.Speech.Synthesis;

namespace STTTS.Common.Audio;

public static class InstalledVoices
{
	public static IEnumerable<InstalledVoice> GetVoices()
	{
		using var synthesizer = new SpeechSynthesizer();
		return synthesizer.GetInstalledVoices();
	}

	public static InstalledVoice? GetVoiceByID(string id)
	{
		using var synthesizer = new SpeechSynthesizer();
		return synthesizer.GetInstalledVoices().Single(voice => voice.VoiceInfo.Id == id);
	}
}
