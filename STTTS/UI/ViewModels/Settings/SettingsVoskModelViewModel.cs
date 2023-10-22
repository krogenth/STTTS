using STTTS.Common.Configuration;

namespace STTTS.UI.ViewModels.Settings;

public class SettingsVoskModelViewModel : BaseViewModel
{
	private string _selectedModelDirectory = string.Empty;

	public SettingsVoskModelViewModel()
	{
		_selectedModelDirectory = ConfigurationState.Instance.Vosk.ModelDirectory.Value;
	}

	public string SelectedModelDirectory
	{
		get => _selectedModelDirectory;
		set
		{
			_selectedModelDirectory = value;
			ConfigurationState.Instance.Vosk.ModelDirectory.Value = value;
			OnPropertyChanged(nameof(SelectedModelDirectory));
		}
	}
}
