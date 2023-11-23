using STTTS.Common.Types;
using STTTS.Common.Utility;

namespace STTTS.Common.Configuration;

public class RecognizerConfigurationState
{
	public ReactiveObject<string> Recognizer { get; private set; }

	public RecognizerConfigurationState()
	{
		Recognizer = new(string.Empty);
	}

	public void LoadDefaultConfiguration()
	{
		Recognizer = new(RecognizerType.Vosk.ToString());
	}
}
