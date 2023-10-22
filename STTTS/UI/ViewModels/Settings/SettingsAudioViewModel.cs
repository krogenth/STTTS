using STTTS.Common.Audio;
using STTTS.Common.Configuration;

namespace STTTS.UI.ViewModels.Settings;

public class SettingsAudioViewModel : BaseViewModel
{
	public AudioDeviceSelectorViewModel OutputDeviceSelectorVM { get; }
	public AudioDeviceSelectorViewModel InputDeviceSelectorVM { get; }
	public AudioDeviceSelectorViewModel PlaybackDeviceSelectorVM { get; }

	public SettingsAudioViewModel()
	{
		OutputDeviceSelectorVM = new(
			AudioDeviceType.Output,
			AudioDevices.GetOutputAudioDevices(),
			ConfigurationState.Instance.Audio.OutputDeviceID.Value
		);
		InputDeviceSelectorVM = new(
			AudioDeviceType.Input,
			AudioDevices.GetInputAudioDevices(),
			ConfigurationState.Instance.Audio.InputDeviceID.Value
		);
		PlaybackDeviceSelectorVM = new(
			AudioDeviceType.Playback,
			AudioDevices.GetOutputAudioDevices(),
			ConfigurationState.Instance.Audio.PlaybackDeviceID.Value
		);

		AttachEventHandlers();
	}

	private void AttachEventHandlers()
	{
		OutputDeviceSelectorVM.DeviceChanged += AudioDeviceChanged;
		InputDeviceSelectorVM.DeviceChanged += AudioDeviceChanged;
		PlaybackDeviceSelectorVM.DeviceChanged += AudioDeviceChanged;
	}

	private void AudioDeviceChanged(object? sender, AudioDeviceChangedEventArgs e)
	{
		switch (e.AudioDeviceType)
		{
			case AudioDeviceType.Input:
				ConfigurationState.Instance.Audio.InputDeviceID.Value = e.DeviceID;
				break;
			case AudioDeviceType.Playback:
				ConfigurationState.Instance.Audio.PlaybackDeviceID.Value = e.DeviceID;
				break;
			case AudioDeviceType.Output:
				ConfigurationState.Instance.Audio.OutputDeviceID.Value = e.DeviceID;
				break;
		}
	}
}
