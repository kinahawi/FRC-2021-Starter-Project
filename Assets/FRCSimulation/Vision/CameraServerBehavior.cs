using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Threading;

using System.IO;
using UnityEngine.UI;
using System;
using System.Text;
using System.Linq;


public class CameraServerBehavior : MonoBehaviour {

    public Camera theCamera;
    public int portNumber = 8080;

    public int imageWidth = 320;
    public int imageHeight = 240;
    public int imageQuality = 75;

    private RenderTexture rt;

    private int frameNum = 0;

    static readonly object lockObject = new object();

    private byte[] frameBytes;

    private bool newFrame = false;

    private Thread serverThread;

    private Texture2D tex;


    // Take a shot immediately
    IEnumerator Start() {

        Debug.Log("Started camera server");
        rt = new RenderTexture(imageWidth, imageHeight, 24);
        theCamera.targetTexture = rt;

        tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);

        serverThread = new Thread(new ThreadStart(ServerMain));
        serverThread.Start();

        StartCoroutine("SaveCameraFrame");
        yield return null;
    }


    void OnDestroy() {
        serverThread.Abort();
    }


    IEnumerator SaveCameraFrame() {
        // Create a texture the size of the screen, RGB24 format

        while (true) {
            // We should only read the screen buffer after rendering is complete
            yield return new WaitForEndOfFrame();

            if (!newFrame) {

                // Backup the currently set RenderTexture
                RenderTexture prt = RenderTexture.active;

                // Set the current RenderTexture to the temporary one we created
                RenderTexture.active = rt;

                tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                tex.Apply();

                lock (lockObject) {
                    frameBytes = tex.EncodeToJPG(imageQuality);
                    newFrame = true;
                }
                //RenderTexture.active = prt;

                frameNum++;
            }
            yield return null;
        }
    }





    private void ServerMain() {

        Socket Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Server.Bind(new IPEndPoint(IPAddress.Any, portNumber));
        Server.Listen(1);

        while (true) {
            Socket socket = Server.Accept();

            using (MjpegWriter wr = new MjpegWriter(new NetworkStream(socket, true))) {
                wr.WriteHeader();
                while (socket.Connected) {
                    if (newFrame) {
                        //Debug.Log($"   new frame read");
                        byte[] bytes;
                        bool gotFrame = false;
                        lock (lockObject) {
                            bytes = frameBytes.ToArray();
                            newFrame = false;
                            gotFrame = true;
                            //Debug.Log($"   got frame");
                        }

                        if (gotFrame) {
                            try {
                                wr.Write(bytes);
                                //Debug.Log($"   frame written");
                            }
                            catch {

                            }
                        }
                    }
                }
            }
            Debug.Log($"socket no longer connected");
        }
    }
}




/// <summary>
/// Provides a stream writer that can be used to write images as MJPEG 
/// or (Motion JPEG) to any stream.
/// </summary>
public class MjpegWriter : IDisposable {

    private static byte[] CRLF = new byte[] { 13, 10 };
    private static byte[] EmptyLine = new byte[] { 13, 10, 13, 10 };

    private string _Boundary;

    public MjpegWriter(Stream stream)
        : this(stream, "--boundary") {

    }

    public MjpegWriter(Stream stream, string boundary) {

        this.Stream = stream;
        this.Boundary = boundary;
    }

    public string Boundary { get; private set; }
    public Stream Stream { get; private set; }

    public void WriteHeader() {

        Write(
                "HTTP/1.1 200 OK\r\n" +
                "Content-Type: multipart/x-mixed-replace; boundary=" +
                this.Boundary +
                "\r\n"
             );

        this.Stream.Flush();
    }

    public void Write(byte[] bytes) {
        MemoryStream ms = new MemoryStream(bytes);
        this.Write(ms);
    }

    public void Write(MemoryStream imageStream) {

        StringBuilder sb = new StringBuilder();
        string eol = "\r\n";

        sb.Append(eol);
        sb.Append(this.Boundary + eol);
        sb.Append("Content-Type: image/jpeg" + eol);
        sb.Append("Content-Length: " + imageStream.Length.ToString() + eol);
        sb.Append(eol);

        Write(sb.ToString());
        imageStream.WriteTo(this.Stream);
        Write("\r\n");

        this.Stream.Flush();

    }

    private void Write(string text) {
        byte[] data = BytesOf(text);
        this.Stream.Write(data, 0, data.Length);
    }

    private static byte[] BytesOf(string text) {
        return Encoding.ASCII.GetBytes(text);
    }


    public string ReadRequest(int length) {

        byte[] data = new byte[length];
        int count = this.Stream.Read(data, 0, data.Length);

        if (count != 0)
            return Encoding.ASCII.GetString(data, 0, count);

        return null;
    }

    #region IDisposable Members

    public void Dispose() {

        try {

            if (this.Stream != null)
                this.Stream.Dispose();

        }
        finally {
            this.Stream = null;
        }
    }

    #endregion
}

