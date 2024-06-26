using BuildSoft.OscCore;
using System.Net.Sockets;
using STTTS.Common.Configuration;

namespace STTTS.Integrations;
public class OSC
{
	public event EventHandler<ErrorEventArgs>? ErrorTriggered;
	public event EventHandler<EventArgs>? StateChanged;

	public bool OpenChatWindow { get; set; }

	protected OscClient _client;
	//protected OscServer _server;

	public static OSC? Instance { get; private set; } = null;

	private OSC()
	{
		_client = new OscClient("127.0.0.1", ConfigurationState.Instance.OSC.SendPort.Value);
		//_server = new OscServer(9001);
	}

	public static void Initialize()
	{
		Instance ??= new();
	}

	public static void Deinitialize()
	{
		Instance = null;
	}

	public void SendText(string text)
	{
		using var writer = new OscWriter();
		uint tags = (uint)',' + ((uint)TypeTag.String << 8) + ((uint)(OpenChatWindow ? TypeTag.True: TypeTag.False) << 16);
		writer.WriteAddressAndTags("/chatbox/input", tags);
		writer.Write(text);
		_client.Socket.Send(writer.Buffer, writer.Length, SocketFlags.None);
	}
}
