using STTTS.Common.Audio;
using STTTS.Common.Utility;

namespace STTTS.Common.Configuration;

public class AudioConfigurationState
{
	public ReactiveObject<string> InputDeviceID { get; private set; }
	public ReactiveObject<string> OutputDeviceID { get; private set; }
	public ReactiveObject<string> PlaybackDeviceID { get; private set; }

	public AudioConfigurationState()
	{
		InputDeviceID = new(string.Empty);
		OutputDeviceID = new(string.Empty);
		PlaybackDeviceID = new(string.Empty);
	}

	public void LoadDefaultConfiguration()
	{
		InputDeviceID = new(AudioDevices.GetInputAudioDevices().First().ID);
		OutputDeviceID = new(AudioDevices.GetInputAudioDevices().First().ID);
		PlaybackDeviceID = new(AudioDevices.GetOutputAudioDevices().First().ID);
	}
}
