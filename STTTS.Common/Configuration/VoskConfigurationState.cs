using STTTS.Common.Utility;

namespace STTTS.Common.Configuration;
public class VoskConfigurationState
{
	public ReactiveObject<string> ModelDirectory { get; private set; }

	public VoskConfigurationState()
	{
		ModelDirectory = new(string.Empty);
	}

	public void LoadDefaultConfiguration()
	{
		ModelDirectory = new(AppDomain.CurrentDomain.BaseDirectory);
	}
}
