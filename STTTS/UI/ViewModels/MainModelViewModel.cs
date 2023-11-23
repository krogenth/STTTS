using System;
using STTTS.Common.Configuration;
using STTTS.Common.Events;
using STTTS.Common.Extensions;
using STTTS.Common.Types;
using STTTS.Engine.STT.Recognizers;
using STTTS.Engine.TTS.Synthesizers;
using STTTS.Integrations;

namespace STTTS.UI.ViewModels;

public class MainModelViewModel : BaseViewModel
{
	private string _recognizedText = string.Empty;

	public BaseSpeechRecognizer BaseSpeechRecognizer { get; set; }
	public BaseSpeechSynthesizer BaseSpeechSynthesizer { get; set; }

	protected OSC OSC { get; set; }

	public string RecognizedText
	{
		get => _recognizedText;
		set
		{
			_recognizedText = value ?? string.Empty;
			OnPropertyChanged(nameof(RecognizedText));
		}
	}

	public MainModelViewModel()
	{
		SelectRecognizer(ConfigurationState.Instance.Recognizer.Recognizer.Value.ToEnum<RecognizerType>());
		SelectSynthesizer();
		OSC = new();
		ConfigurationState.Instance.Recognizer.Recognizer.ValueChanged += OnRecognizerChanged;
	}

	private void SelectRecognizer(RecognizerType recognizer)
	{
		switch (recognizer)
		{
			case RecognizerType.Vosk:
				BaseSpeechRecognizer = new VoskSpeechRecognizer();
				break;
			case RecognizerType.Whisper:
				BaseSpeechRecognizer = new WhisperSpeechRecognizer();
				break;
		}

		if (BaseSpeechRecognizer != null)
		{
			BaseSpeechRecognizer.RecognizedSpeech += OnRecognizedSpeech;
			BaseSpeechRecognizer.StateChanged += OnRecognizerStateChanged;
		}

		OnPropertyChanged(nameof(BaseSpeechRecognizer));
	}

	private void SelectSynthesizer()
	{
		BaseSpeechSynthesizer = new SystemSpeechSynthesizer();
		BaseSpeechSynthesizer.StateChanged += OnSynthesizerStateChanged;
	}

	private void OnRecognizerChanged(object? sender, ValueChangedEventArgs<string> e) =>
		SelectRecognizer(e.NewValue.ToEnum<RecognizerType>());

	private void OnRecognizedSpeech(object? sender, RecognizedSpeechEventArgs e)
	{
		RecognizedText = e.Text;
		BaseSpeechSynthesizer.AddSpeechMessage(e.Text);
		OSC.SendText(e.Text);
	}

	// Speech Recognizer methods & properties
	public bool CanStartRecognizer => BaseSpeechRecognizer.CanStart;
	public void StartRecognizer() => BaseSpeechRecognizer.Start();

	public bool CanPauseRecognizer => BaseSpeechRecognizer.CanPause;
	public void PauseRecognizer(object _) => BaseSpeechRecognizer.Pause();

	public bool CanStopRecognizer => BaseSpeechRecognizer.CanStop;
	public void StopRecognizer(object _) => BaseSpeechRecognizer.Stop();

	private void NotifyRecognizerChanged()
	{
		OnPropertyChanged(nameof(CanStartRecognizer));
		OnPropertyChanged(nameof(CanPauseRecognizer));
		OnPropertyChanged(nameof(CanStopRecognizer));
	}

	protected void OnRecognizerStateChanged(object? sender, EventArgs _) =>
		NotifyRecognizerChanged();

	// Speech Synthesizer methods & properties
	public bool CanStartSynthesizer => BaseSpeechSynthesizer.CanStart;
	public void StartSynthesizer() => BaseSpeechSynthesizer.Start();

	public bool CanPauseSynthesizer => BaseSpeechSynthesizer.CanPause;
	public void PauseSynthesizer(object _) => BaseSpeechSynthesizer.Pause();

	public bool CanStopSynthesizer => BaseSpeechSynthesizer.CanStop;
	public void StopSynthesizer(object _) => BaseSpeechSynthesizer.Stop();

	private void NotifySynthesizerChanged()
	{
		OnPropertyChanged(nameof(CanStartSynthesizer));
		OnPropertyChanged(nameof(CanPauseSynthesizer));
		OnPropertyChanged(nameof(CanStopSynthesizer));
	}

	protected void OnSynthesizerStateChanged(object? sender, EventArgs e) =>
		NotifySynthesizerChanged();
}
