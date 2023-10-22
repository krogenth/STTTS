using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NAudio.CoreAudioApi;
using STTTS.Common.Audio;

namespace STTTS.UI.ViewModels;

public class AudioDeviceSelectorViewModel : BaseViewModel
{
	private int _selectedDeviceIndex;

	public event EventHandler<AudioDeviceChangedEventArgs>? DeviceChanged;

	public AudioDeviceSelectorViewModel(AudioDeviceType type, IEnumerable<MMDevice> devices, string defaultDeviceID)
	{
		AudioDeviceType = type;
		Devices = new ObservableCollection<MMDevice>(devices);

		SelectedDeviceIndex = defaultDeviceID != null ?
			Devices.IndexOf(Devices.FirstOrDefault(device => device.ID == defaultDeviceID)!) :
			0;
	}

	public AudioDeviceType AudioDeviceType { get; }
	public ObservableCollection<MMDevice> Devices { get; }

	public int SelectedDeviceIndex
	{
		get => _selectedDeviceIndex;
		set
		{
			if (value < 0)
			{
				value = 0;
			}

			if (value >= Devices.Count)
			{
				value = Devices.Count - 1;
			}

			_selectedDeviceIndex = value;
			OnPropertyChanged(nameof(SelectedDeviceIndex));
			DeviceChanged?.Invoke(this, new AudioDeviceChangedEventArgs(AudioDeviceType, Devices.ElementAt(SelectedDeviceIndex).ID));
		}
	}
}

public class AudioDeviceChangedEventArgs
{
	public string DeviceID { get; set; }
	public AudioDeviceType AudioDeviceType { get; set; }

	public AudioDeviceChangedEventArgs(AudioDeviceType type, string id)
	{
		AudioDeviceType = type;
		DeviceID = id;
	}
}
