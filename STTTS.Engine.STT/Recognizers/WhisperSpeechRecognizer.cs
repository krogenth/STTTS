using NAudio.CoreAudioApi;
using NAudio.Wave;
using STTTS.Common.Audio;
using STTTS.Common.Configuration;
using Vosk;
using Whisper.net;
using Whisper.net.Ggml;

namespace STTTS.Engine.STT.Recognizers;

public sealed class WhisperSpeechRecognizer : BaseSpeechRecognizer
{
	private WhisperProcessor? _recognizer;
	private IWaveIn? _waveInEvent;
	private DateTime _latestPoll = DateTime.Now;

	public WhisperSpeechRecognizer() : base()
	{
		AttachEventHandlers();
	}

	private void AttachEventHandlers()
	{
		ConfigurationState.Instance.Audio.InputDeviceID.ValueChanged += DeviceChanged;
		ConfigurationState.Instance.Audio.OutputDeviceID.ValueChanged += DeviceChanged;
		ConfigurationState.Instance.Whisper.Model.ValueChanged += DeviceChanged;
	}

	private void DetachEventHandlers()
	{
		ConfigurationState.Instance.Audio.InputDeviceID.ValueChanged -= DeviceChanged;
		ConfigurationState.Instance.Audio.OutputDeviceID.ValueChanged -= DeviceChanged;
		ConfigurationState.Instance.Whisper.Model.ValueChanged -= DeviceChanged;
	}

	public override bool Start()
	{
		if (!Paused)
		{
			try
			{
				Task.Run(LoadModel);
			}
			catch (Exception _)
			{
				return false;
			}

			var device = AudioDevices.GetDeviceByID(ConfigurationState.Instance.Audio.InputDeviceID.Value);
			_waveInEvent = new WasapiCapture(device)
			{
				ShareMode = AudioClientShareMode.Shared,
				WaveFormat = new WaveFormat(48000, 16, 1),
			};
			_waveInEvent.DataAvailable += OnDataAvailable;
		}

		// start the recording of device input
		_waveInEvent!.StartRecording();

		Paused = false;
		Stopped = false;
		OnStateChanged();
		return true;
	}

	private void OnDataAvailable(object? sender, WaveInEventArgs e)
	{
		// do nothing if we are paused
		if (Paused)
		{
			return;
		}

		if (e.BytesRecorded <= 0)
		{
			return;
		}

		Task.Run(() => ProcessAudioData(new MemoryStream(e.Buffer)));
	}

	private async Task ProcessAudioData(MemoryStream stream)
	{
		if (_recognizer == null)
		{
			return;
		}

		await foreach (var result in _recognizer!.ProcessAsync(stream))
		{
			OnRecognizedSpeech(result.Text);
		}
	}

	public override void Pause()
	{
		if (!Paused && !Stopped)
		{
			_waveInEvent!.StopRecording();
			Paused = true;
			OnStateChanged();
		}
	}

	public override void Stop()
	{
		if (!Stopped)
		{
			if (_waveInEvent != null)
			{
				_waveInEvent.StopRecording();
				_waveInEvent.DataAvailable -= OnDataAvailable;
				_waveInEvent.Dispose();
				_waveInEvent = null;
			}

			if (_recognizer != null)
			{
				_recognizer.Dispose();
				_recognizer = null;
			}

			Paused = false;
			Stopped = true;
			OnStateChanged();
		}
	}

	public override void Dispose()
	{
		if (!Stopped)
		{
			Stop();
		}

		DetachEventHandlers();
	}

	private async Task LoadModel()
	{
		string filename = WhisperModels.WhisperModelStringToFilename(
			ConfigurationState.Instance.Whisper.Model.Value
		);

		GgmlType ggmlModelType = WhisperModels.WhisperModelStringToGgmlType(
			ConfigurationState.Instance.Whisper.Model.Value
		);

		if (!File.Exists(filename))
		{
			using var modelStream = await WhisperGgmlDownloader.GetGgmlModelAsync(ggmlModelType);
			using var fileWriter = File.OpenWrite(filename);
			await modelStream.CopyToAsync(fileWriter);
		}

		using var whisperFactory = WhisperFactory.FromPath(filename);
		_recognizer = whisperFactory.CreateBuilder()
			.WithLanguage("auto")
			.Build();
	}
}
