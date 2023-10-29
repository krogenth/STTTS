using BuildSoft.OscCore;
using System.Net.Sockets;

namespace STTTS.Integrations;
public class OSC
{
	public event EventHandler<ErrorEventArgs>? ErrorTriggered;
	public event EventHandler<EventArgs>? StateChanged;

	public bool OpenChatWindow { get; set; }

	protected OscClient _client;
	//protected OscServer _server;

	public OSC()
	{
		_client = new OscClient("127.0.0.1", 9000);
		//_server = new OscServer(9001);
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
