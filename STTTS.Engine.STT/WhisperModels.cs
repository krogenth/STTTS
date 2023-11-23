using STTTS.Common.Extensions;
using Whisper.net.Ggml;

namespace STTTS.Engine.STT;

public enum WhisperModel
{
	Tiny,
	TinyEN,
	Base,
	BaseEN,
	Small,
	SmallEN,
	Medium,
	MediumEN,
	LargeV1,
	Large,
};

public static class WhisperModels
{
	public static string WhisperModelStringToFilename(string model) =>
		WhisperModelToFilename(model.ToEnum<WhisperModel>());

	public static string WhisperModelToFilename(WhisperModel model)
	{
		return model switch
		{
			WhisperModel.Tiny => "ggml-tiny.bin",
			WhisperModel.TinyEN => "ggml-tiny.en.bin",
			WhisperModel.Base => "ggml-base.bin",
			WhisperModel.BaseEN => "ggml-base.en.bin",
			WhisperModel.Small => "ggml-small.bin",
			WhisperModel.SmallEN => "ggml-small.en.bin",
			WhisperModel.Medium => "ggml-medium.bin",
			WhisperModel.MediumEN => "ggml-medium.en.bin",
			WhisperModel.LargeV1 => "ggml-large-v1.bin",
			WhisperModel.Large => "ggml-large.bin",
			_ => throw new ArgumentOutOfRangeException(),
		};
	}

	public static GgmlType WhisperModelStringToGgmlType(string model) =>
		WhisperModelToGgmlType(model.ToEnum<WhisperModel>());

	public static GgmlType WhisperModelToGgmlType(WhisperModel model)
	{
		return model switch
		{
			WhisperModel.Tiny => GgmlType.Tiny,
			WhisperModel.TinyEN => GgmlType.TinyEn,
			WhisperModel.Base => GgmlType.Base,
			WhisperModel.BaseEN => GgmlType.BaseEn,
			WhisperModel.Small => GgmlType.Small,
			WhisperModel.SmallEN => GgmlType.SmallEn,
			WhisperModel.Medium => GgmlType.Medium,
			WhisperModel.MediumEN => GgmlType.MediumEn,
			WhisperModel.LargeV1 => GgmlType.LargeV1,
			WhisperModel.Large => GgmlType.Large,
			_ => throw new ArgumentOutOfRangeException(),
		};
	}
}
