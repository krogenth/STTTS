﻿using STTTS.Common.Configuration;
using STTTS.Common.Events;

namespace STTTS.Engine.STT.Recognizers;

public abstract class BaseSpeechRecognizer : IDisposable
{
	public event EventHandler<ErrorEventArgs>? ErrorTriggered;
	public event EventHandler<EventArgs>? StateChanged;
	public event EventHandler<RecognizedSpeechEventArgs>? RecognizedSpeech;

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

	public BaseSpeechRecognizer()
	{
		Paused = false;
		Stopped = true;
		ConfigurationState.Instance.Audio.InputDeviceID.ValueChanged += DeviceChanged;
		ConfigurationState.Instance.Audio.OutputDeviceID.ValueChanged += DeviceChanged;
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

	protected void OnStateChanged() =>
		StateChanged?.Invoke(this, EventArgs.Empty);

	protected void OnRecognizedSpeech(string text) =>
		RecognizedSpeech?.Invoke(this, new RecognizedSpeechEventArgs(text));

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

	public abstract void Dispose();
}
