using System.Text.Json;
using System.Text.Json.Serialization;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using STTTS.Common.Audio;
using STTTS.Common.Configuration;
using Vosk;

namespace STTTS.Engine.STT.Recognizers;

public class VoskSpeechRecognizer : BaseSpeechRecognizer
{
	private Model? _model;
	private VoskRecognizer? _recognizer;
	private IWaveIn? _waveInEvent;

	public VoskSpeechRecognizer() : base()
	{
		AttachEventHandlers();
	}

	private void AttachEventHandlers()
	{
		ConfigurationState.Instance.Audio.InputDeviceID.ValueChanged += DeviceChanged;
		ConfigurationState.Instance.Audio.OutputDeviceID.ValueChanged += DeviceChanged;
	}

	public override bool Start()
	{
		string path = ConfigurationState.Instance.Vosk.ModelDirectory.Value;
		if (string.IsNullOrEmpty(path))
		{
			return false;
		}

		// only if we are not paused do we need to initialize everything
		if (!Paused)
		{
			try
			{
				_model = new Model(path);
				_recognizer = new VoskRecognizer(_model, 48000f);
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

		if (_recognizer!.AcceptWaveform(e.Buffer, e.BytesRecorded))
		{
			string json = _recognizer.Result();
			var result = JsonSerializer.Deserialize<VoskResult>(json);
			if (result != null && !string.IsNullOrEmpty(result.Text))
			{
				OnRecognizedSpeech(result.Text);
			}
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

			if (_model != null)
			{
				_model.Dispose();
				_model = null;
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
	}
}

public class VoskResultWord
{
	[JsonPropertyName("conf")]
	public float Conf { get; set; }
	[JsonPropertyName("end")]
	public float End { get; set; }
	[JsonPropertyName("start")]
	public float Start { get; set; }
	[JsonPropertyName("word")]
	public string Word { get; set; } = string.Empty;
}

public class VoskResult
{
	[JsonPropertyName("result")]
	public List<VoskResultWord> Result { get; set; } = new List<VoskResultWord>();
	[JsonPropertyName("text")]
	public string Text { get; set; } = string.Empty;
}
