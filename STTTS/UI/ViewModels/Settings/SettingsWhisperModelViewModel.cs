using System;
using STTTS.Common.Configuration;
using STTTS.Common.Types;

namespace STTTS.UI.ViewModels.Settings;

public class SettingsWhisperModelViewModel : BaseViewModel
{
	private string _selectedModelModel = string.Empty;

	public SettingsWhisperModelViewModel()
	{
		_selectedModelModel = ConfigurationState.Instance.Whisper.Model.Value;
	}

	public string[] AvailableModels { get; set; } = Enum.GetNames(typeof(WhisperModel));

	public string SelectedModelModel
	{
		get => _selectedModelModel;
		set
		{
			_selectedModelModel = value;
			ConfigurationState.Instance.Whisper.Model.Value = value;
			OnPropertyChanged(nameof(SelectedModelModel));
		}
	}
}
