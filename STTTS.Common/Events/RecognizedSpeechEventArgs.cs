namespace STTTS.Common.Events;

public class RecognizedSpeechEventArgs
{
	public string Text { get; }

	public RecognizedSpeechEventArgs(string text)	{
		Text = text;
	}
}
