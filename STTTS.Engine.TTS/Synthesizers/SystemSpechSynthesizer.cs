using System.Diagnostics;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using STTTS.Common.Audio;
using STTTS.Common.Configuration;

namespace STTTS.Engine.TTS.Synthesizers;

public class SystemSpechSynthesizer : BaseSpeechSynthesizer
{
	private SpeechSynthesizer? _synthesizer = null;
	private CancellationTokenSource? _cancellationTokenSource = null;
	private CancellationToken? _cancellationToken = null;
	private Task? _speechTask = null;

	~SystemSpechSynthesizer()
	{
		if (_synthesizer != null)
		{
			_synthesizer.Dispose();
		}
	}

	public override bool Start()
	{
		if (!Paused)
		{
			try
			{
				_synthesizer = new SpeechSynthesizer();
				_cancellationTokenSource = new CancellationTokenSource();
				_cancellationToken = _cancellationTokenSource.Token;
				_speechTask = Task.Run(AsyncSpeechTask, _cancellationToken.Value);
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		Paused = false;
		Stopped = false;
		OnStateChanged();
		return true;
	}

	protected async Task AsyncSpeechTask()
	{
		while (!_cancellationToken!.Value.IsCancellationRequested)
		{
			if (SpeechQueue.TryDequeue(out string? text))
			{
				try
				{
					using MemoryStream micMemStream = new();
					using MemoryStream playbackMemStream = new();
					_synthesizer.SelectVoice(InstalledVoices.GetVoiceByID(ConfigurationState.Instance.SystemSpeechSynthesizer.VoiceID.Value)!.VoiceInfo.Name);
					_synthesizer.SetOutputToAudioStream(micMemStream, new SpeechAudioFormatInfo(44100, AudioBitsPerSample.Sixteen, AudioChannel.Mono));
					_synthesizer.Speak(text);
					micMemStream.Flush();
					micMemStream.Seek(0, SeekOrigin.Begin);

					if (DoPlayback)
					{
						micMemStream.CopyTo(playbackMemStream);
						micMemStream.Seek(0, SeekOrigin.Begin);
						playbackMemStream.Seek(0, SeekOrigin.Begin);
					}

					var micDevice = AudioDevices.GetDeviceByID(ConfigurationState.Instance.Audio.OutputDeviceID.Value)!;
					var playbackDevice = AudioDevices.GetDeviceByID(ConfigurationState.Instance.Audio.PlaybackDeviceID.Value)!;

					using RawSourceWaveStream micProvider = new(micMemStream, new WaveFormat(44100, 16, 1));
					using WasapiOut micWasapiOut = new(micDevice, AudioClientShareMode.Shared, false, 10);
					using WasapiOut playbackWasapiOut = new(playbackDevice, AudioClientShareMode.Shared, false, 10);

					micWasapiOut.Init(micProvider);
					micWasapiOut.Play();

					if (DoPlayback)
					{
						using RawSourceWaveStream playbackProvider = new(playbackMemStream, new WaveFormat(44100, 16, 1));
						playbackWasapiOut.Init(playbackProvider);
						playbackWasapiOut.Play();
					}

					while (micWasapiOut.PlaybackState == PlaybackState.Playing || playbackWasapiOut.PlaybackState == PlaybackState.Playing)
					{
						Thread.Sleep(100);
					}
				}
				catch (Exception _)
				{
					return;
				}
			}

			Thread.Sleep(100);
		}
	}

	public override void Pause()
	{
		if (!Paused && !Stopped)
		{
			Paused = true;
			OnStateChanged();
		}
	}

	public override async void Stop()
	{
		if (!Stopped)
		{
			_cancellationTokenSource?.Cancel();
			try
			{
				await _speechTask!.ConfigureAwait(true);
			}
			catch (OperationCanceledException e)
			{
				Debug.WriteLine(e);
			}
			finally
			{
				_cancellationTokenSource?.Dispose();
				_cancellationTokenSource = null;
				_cancellationToken = null;
			}

			Paused = false;
			Stopped = true;
			OnStateChanged();
		}
	}
}
