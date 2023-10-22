using STTTS.Common.Configuration;
using STTTS.Common.Events;

namespace STTTS.Engine.TTS.Synthesizers;

public abstract class BaseSpeechSynthesizer
{
	public event EventHandler<ErrorEventArgs>? ErrorTriggered;
	public event EventHandler<EventArgs>? StateChanged;

	/// <summary>
	/// Specifies if the Recognizer is currently listening
	/// to the Input device.
	/// </summary>
	public bool Paused { get; protected set; }

	/// <summary>
	/// Specifies if the Recognizer is currently initialized
	/// and can begin listening to the current Input device.
	/// </summary>
	public bool Stopped { get; protected set; }

	/// <summary>
	/// Specifies if the audio generated should be played back
	/// to the configured Playback device.
	/// </summary>
	public bool DoPlayback { get; set; }

	/// <summary>
	/// A queue of all recognized speech to synthesize.
	/// </summary>
	protected Queue<string> SpeechQueue { get; }

	public BaseSpeechSynthesizer()
	{
		Paused = false;
		Stopped = true;
		DoPlayback = true;
		SpeechQueue = new();
		ConfigurationState.Instance.Audio.OutputDeviceID.ValueChanged += DeviceChanged;
		ConfigurationState.Instance.Audio.PlaybackDeviceID.ValueChanged += DeviceChanged;
	}

	/// <summary>
	/// Creates the model and begins listening to the selected
	/// Input device for audio samples to be translated.
	/// </summary>
	/// <returns>If the recogizer has started or not</returns>
	public abstract bool Start();
	public bool CanStart => Paused || Stopped;

	/// <summary>
	/// Stops listening to the Input device for audio samples
	/// without removing the underlying model.
	/// </summary>
	public abstract void Pause();
	public bool CanPause => !Paused && !Stopped;

	/// <summary>
	/// Stops listening to the Input device for audio samples
	/// and removes the underlying model.
	/// </summary>
	public abstract void Stop();
	public bool CanStop => !Stopped;

	/// <summary>
	/// Adds a speech message to be synthesized
	/// so long as the synthesizer is not paused
	/// or stopped.
	/// </summary>
	/// <param name="text"></param>
	public void AddSpeechMessage(string text)
	{
		if (!Paused && !Stopped)
		{
			SpeechQueue.Enqueue(text);
		}
	}

	protected void OnStateChanged() =>
		StateChanged?.Invoke(this, EventArgs.Empty);

	/// <summary>
	/// Called when an error occurs within the Recognizer,
	/// calls the ErrorTriggered event for listeners to capture.
	/// </summary>
	protected void OnError(Exception e) =>
		ErrorTriggered?.Invoke(this, new ErrorEventArgs(e));

	/// <summary>
	/// Method to attach to ReactiveObjects to detect when a device has changed
	/// from the Configuration State.
	/// We want to avoid having the used Device change mid conversion,
	/// so detect when it changes to stop the Recognizer.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void DeviceChanged(object? sender, ValueChangedEventArgs<string> e) =>
		Stop();
}
