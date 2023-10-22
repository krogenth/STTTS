using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Speech.Synthesis;

namespace STTTS.UI.ViewModels;

public class VoiceSelectorViewModel : BaseViewModel
{
	private int _selectedVoiceIndex;

	public event EventHandler<VoiceChangedEventArgs> VoiceChanged;

	public VoiceSelectorViewModel(IEnumerable<InstalledVoice> voices, string defaultVoiceID)
	{
		Voices = new ObservableCollection<InstalledVoice>(voices);
		SelectedVoiceIndex = defaultVoiceID != null ?
			Voices.IndexOf(Voices.FirstOrDefault(voice => voice.VoiceInfo.Id == defaultVoiceID)) :
			0;
	}

	public ObservableCollection<InstalledVoice> Voices { get; }

	public int SelectedVoiceIndex
	{
		get => _selectedVoiceIndex;
		set
		{
			if (value < 0)
			{
				value = 0;
			}

			if (value >= Voices.Count)
			{
				value = Voices.Count - 1;
			}

			_selectedVoiceIndex = value;
			OnPropertyChanged(nameof(SelectedVoiceIndex));
			VoiceChanged?.Invoke(this, new VoiceChangedEventArgs(Voices.ElementAt(SelectedVoiceIndex).VoiceInfo.Id));
		}
	}
}

public class VoiceChangedEventArgs
{
	public string VoiceID { get; set; }

	public VoiceChangedEventArgs(string id)
	{
		VoiceID = id;
	}
}
