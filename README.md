# STTTS

## Setup

1. Setup a Virtual Audio Cable like [VB-Cable](https://vb-audio.com/Cable/).
2. For Vosk Recognition, you will need a model, many can be found [here](https://alphacephei.com/vosk/models), I would recommend the [vosk-model-small-en-us](https://alphacephei.com/vosk/models/vosk-model-small-en-us-0.15.zip) for those with lower tier GPUs, or the [vosk-model-en-us-daanzu-20200905-lgraph](https://alphacephei.com/vosk/models/vosk-model-en-us-daanzu-20200905-lgraph.zip) for those with a slightly above average GPU. Overall, I would recommend the l-graph over the small due to better dictation, but try different models and see what works best for you.
3. Extract the downloaded Vosk model (preferably to the same directory as `STTTS`).
3. Open whatever application you wish to use `STTTS` with, and set the Input device/Microphone to the `CABLE Output` device (for CB-Cable).
4. Download and extract the latest version from [here](https://github.com/krogenth/STTTS/releases).
5. Run `STTTS`, open the Option Settings.
6. In Audio, configure the Output device to the `CABLE Input` device (for VB-Cable).
6. In Vosk, click the `Change` button to select the directory containing the Vosk model to be used (i.e. - `vosk-model-small-en-us` for the small english Vosk model).
6. Click the Start buttons for both `Recognizer` and `Synthesizer` to begin recognizing and outputting speech.

## How to use

The application is fairly simplistic in its current state. Settings are currently not saved and thus need to be reconfigured each time the application is used.
The Voice setting can be changed at any time, even while the Synthesizer is running. However, if input/output/playback devices are changed, the respective recognizer/synthesizer will stop running.
The Synthesizer playback checkbox will enable or disable playing the synthesized audio to the selected Playback device in the Audio settings page.

## Future Development

I am unsure how often I will actually work on this, but I would like to expand the number of recognizers and synthesizers possible, as the C# System Speech Synthesizer is somewhat limited.
I would also like to allow this to work with OSC to enable input comments and textbox typing in VRChat.
