using System;
using STTTS.Common.Events;
using STTTS.Engine.STT.Recognizers;
using STTTS.Engine.TTS.Synthesizers;
using STTTS.Integrations;

namespace STTTS.UI.ViewModels;

public class MainModelViewModel : BaseViewModel
{
	private string _recognizedText = string.Empty;

	public BaseSpeechRecognizer BaseSpeechRecognizer { get; set; }
	public BaseSpeechSynthesizer BaseSpeechSynthesizer { get; set; }

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
		SelectRecognizer();
		SelectSynthesizer();
	}

	private void SelectRecognizer()
	{
		BaseSpeechRecognizer = new VoskSpeechRecognizer();
		BaseSpeechRecognizer.RecognizedSpeech += OnRecognizedSpeech;
		BaseSpeechRecognizer.StateChanged += OnRecognizerStateChanged;
	}

	private void SelectSynthesizer()
	{
		BaseSpeechSynthesizer = new SystemSpeechSynthesizer();
		BaseSpeechSynthesizer.StateChanged += OnSynthesizerStateChanged;
	}

	private void OnRecognizedSpeech(object? sender, RecognizedSpeechEventArgs e)
	{
		RecognizedText = e.Text;
		BaseSpeechSynthesizer.AddSpeechMessage(e.Text);
		OSC.Instance!.SendText(e.Text);
	}

	// Speech Recognizer methods & properties
	public bool CanStartRecognizer => BaseSpeechRecognizer.CanStart;
	public bool StartRecognizer() => BaseSpeechRecognizer.Start();

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
	public void StartSynthesizer()
	{
		BaseSpeechSynthesizer.Start();
		OSC.Initialize();
	}

	public bool CanPauseSynthesizer => BaseSpeechSynthesizer.CanPause;
	public void PauseSynthesizer(object _) => BaseSpeechSynthesizer.Pause();

	public bool CanStopSynthesizer => BaseSpeechSynthesizer.CanStop;
	public void StopSynthesizer(object _)
	{
		BaseSpeechSynthesizer.Stop();
		OSC.Deinitialize();
	}

	private void NotifySynthesizerChanged()
	{
		OnPropertyChanged(nameof(CanStartSynthesizer));
		OnPropertyChanged(nameof(CanPauseSynthesizer));
		OnPropertyChanged(nameof(CanStopSynthesizer));
	}

	protected void OnSynthesizerStateChanged(object? sender, EventArgs e) =>
		NotifySynthesizerChanged();
}
