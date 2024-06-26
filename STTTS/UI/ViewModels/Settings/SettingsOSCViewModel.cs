using STTTS.Common.Configuration;

namespace STTTS.UI.ViewModels.Settings;

public class SettingsOSCViewModel : BaseViewModel
{
	public NumberInputViewModel SendPortSelectorVM { get; }

	public SettingsOSCViewModel()
	{
		SendPortSelectorVM = new(
			"Send Port",
			ConfigurationState.Instance.OSC.SendPort.Value
		);

		AttachEventHandlers();
	}

	private void AttachEventHandlers()
	{
		SendPortSelectorVM.NumberValueChanged += SendPortChanged;
	}

	private void SendPortChanged(object? sender, NumberValueChangedEventArgs e)
	{
		ConfigurationState.Instance.OSC.SendPort.Value = e.Value;
	}
}
