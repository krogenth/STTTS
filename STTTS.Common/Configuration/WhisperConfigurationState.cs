using STTTS.Common.Utility;
using STTTS.Common.Types;

namespace STTTS.Common.Configuration;

public class WhisperConfigurationState
{
	public ReactiveObject<string> Model { get; private set; }

	public WhisperConfigurationState()
	{
		Model = new(string.Empty);
	}

	public void LoadDefaultConfiguration()
	{
		Model = new(WhisperModel.Tiny.ToString());
	}
}
