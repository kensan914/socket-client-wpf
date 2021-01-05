using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;

public class ReceiverClient
{
    public IPEndPoint ServerIPEndPoint { get; set; }
    private Socket Socket { get; set; }
    public const int BufferSize = 1024;
    public byte[] Buffer { get; } = new byte[BufferSize];

    const string HOST = "127.0.0.1";
    const int PORT = 8888;
    const string CHANNEL = "abc";
    byte SENDER = Byte.Parse("1");
    private Boolean isSuccessConn = false;
    const string OK_MESSAGE = "OK";

    public ReceiverClient()
    {
        this.ServerIPEndPoint = new IPEndPoint(Dns.GetHostAddresses(HOST)[0], PORT);
    }

    byte[] createHeader()
    {
        string headerChannel = CHANNEL.Length + CHANNEL;
        byte[] headerChannelBytes = new ASCIIEncoding().GetBytes(headerChannel);

        byte[] headerBytes = new byte[headerChannelBytes.Length + 1];
        headerChannelBytes.CopyTo(headerBytes, 1);
        headerBytes[0] = SENDER;
        return (headerBytes);
    }

    // ソケット通信の接続
    public void connect()
    {
        this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        this.Socket.Connect(this.ServerIPEndPoint);

        // 非同期で受信を待機
        this.Socket.BeginReceive(this.Buffer, 0, BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), this.Socket);

        // header送信
        byte[] headerBytes = createHeader();
        this.Socket.Send(headerBytes);
    }

    // ソケット通信接続の切断
    public void disConnect()
    {
        this.Socket?.Disconnect(false);
        this.Socket?.Dispose();
    }

    // メッセージの送信(同期処理)
    public void send(string message)
    {
        var sendBytes = new ASCIIEncoding().GetBytes(message);
        this.Socket.Send(sendBytes);
    }


    public delegate void ShowMessage(string message);
    ShowMessage showMessage;
    public void setShowMessage(ShowMessage _showMessage)
    {
        this.showMessage = _showMessage;
    }

    // 非同期受信のコールバックメソッド(別スレッドで実行される)
    private void ReceiveCallback(IAsyncResult asyncResult)
    {
        var socket = asyncResult.AsyncState as Socket;

        var byteSize = -1;
        try
        {
            // 受信を待機
            byteSize = socket.EndReceive(asyncResult);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }

        // 受信したデータがある場合、その内容を表示する
        // 再度非同期での受信を開始する
        if (byteSize > 0)
        {
            string message = new ASCIIEncoding().GetString(this.Buffer, 0, byteSize);
            MessageBox.Show(message);
            if (isSuccessConn)
            {
                showMessage(message);
            }
            else
            {
                if (message == OK_MESSAGE)
                {
                    isSuccessConn = true;
                }
                else
                {
                    disConnect();
                }
            }
            socket.BeginReceive(this.Buffer, 0, this.Buffer.Length, SocketFlags.None, ReceiveCallback, socket);
        }
    }
}