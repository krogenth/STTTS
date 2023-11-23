using System;
using STTTS.Common.Configuration;
using STTTS.Common.Types;

namespace STTTS.UI.ViewModels.Settings;

public class SettingsRecognizerViewModel : BaseViewModel
{
	private string _selectedRecognizer = string.Empty;

	public SettingsRecognizerViewModel()
	{
		_selectedRecognizer = ConfigurationState.Instance.Recognizer.Recognizer.Value;
	}

	public string[] AvailableRecognizers { get; set; } = Enum.GetNames(typeof(RecognizerType));

	public string SelectedRecognizer
	{
		get => _selectedRecognizer;
		set
		{
			_selectedRecognizer = value;
			ConfigurationState.Instance.Recognizer.Recognizer.Value = _selectedRecognizer;
			OnPropertyChanged(nameof(SelectedRecognizer));
		}
	}
}
