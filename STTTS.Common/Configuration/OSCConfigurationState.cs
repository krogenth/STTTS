using STTTS.Common.Utility;

namespace STTTS.Common.Configuration;

public class OSCConfigurationState
{
	public ReactiveObject<int> SendPort { get; private set; }

	public OSCConfigurationState()
	{
		SendPort = new(9000);
	}

	public void LoadFileConfiguration(ConfigurationFileFormat configurationFileFormat)
	{
		SendPort.Value = configurationFileFormat.SendPort;
	}

	public void LoadDefaultConfiguration()
	{
		SendPort = new(9000);
	}
}
