using System.Net.WebSockets;
using System.Text;

namespace iot.ws
{
  public class WS
  {
    private WebSocket sock;
    private byte[] lastMessageBuffer = new byte[1024 * 4];

    public WS(WebSocket sock)
    {
      this.sock = sock;
    }

    public byte[] getBufferValue()
    {
      return lastMessageBuffer;
    }

    public void clearBuffer()
    {
      lastMessageBuffer = new byte[2024 * 4];
    }

    public async Task SendAsync(string message)
    {
      try{
        await sock.SendAsync(
        new ArraySegment<byte>(Encoding.UTF8.GetBytes(message), 0, Encoding.UTF8.GetBytes(message).Length),
        messageType: WebSocketMessageType.Text,
        endOfMessage: true,
        CancellationToken.None
        );
      }catch(Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }

    public async Task<WebSocketReceiveResult> ReceiveAsync()
    {
        return await sock.ReceiveAsync(new ArraySegment<byte>(lastMessageBuffer), CancellationToken.None);
    }
    
    public async Task CloseAsync(string? message)
    {
            if(message == "" || message == null)
            {
              message = "No message";
            }
            await sock.CloseAsync(
            WebSocketCloseStatus.NormalClosure,
            message,
            CancellationToken.None);
    }
  }
}
