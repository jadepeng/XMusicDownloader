// Jade.Http.WebClient
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace XMusicDownloader.Http
{
    public class WebClient
    {
        private class CertPolicy : ICertificatePolicy
        {
            public bool CheckValidationResult(ServicePoint srvpt, X509Certificate cert, WebRequest req, int certprb)
            {
                return true;
            }
        }

        private const string BOUNDARY = "--HEDAODE--";

        private const int SEND_BUFFER_SIZE = 10245;

        private const int RECEIVE_BUFFER_SIZE = 10245;

        private WebHeaderCollection requestHeaders;

        private WebHeaderCollection responseHeaders;

        private TcpClient clientSocket;

        private MemoryStream postStream;

        private Encoding encoding = Encoding.Default;

        private string cookie = "";

        private string respHtml = "";

        private string strRequestHeaders = "";

        private string strResponseHeaders = "";

        private int statusCode;

        private bool isCanceled;

        public WebHeaderCollection RequestHeaders
        {
            get
            {
                return requestHeaders;
            }
            set
            {
                requestHeaders = value;
            }
        }

        public WebHeaderCollection ResponseHeaders
        {
            get
            {
                return responseHeaders;
            }
        }

        public string StrRequestHeaders
        {
            get
            {
                return strRequestHeaders;
            }
        }

        public string StrResponseHeaders
        {
            get
            {
                return strResponseHeaders;
            }
        }

        public string Cookie
        {
            get
            {
                return cookie;
            }
            set
            {
                cookie = value;
            }
        }

        public Encoding Encoding
        {
            get
            {
                return encoding;
            }
            set
            {
                encoding = value;
            }
        }

        public string RespHtml
        {
            get
            {
                return respHtml;
            }
        }

        public int StatusCode
        {
            get
            {
                return statusCode;
            }
        }

        public event WebClientUploadEvent UploadProgressChanged;

        public event WebClientDownloadEvent DownloadProgressChanged;

        public WebClient()
        {
            responseHeaders = new WebHeaderCollection();
            requestHeaders = new WebHeaderCollection();
        }

        public string OpenRead(string URL)
        {
            requestHeaders.Add("Connection", "close");
            SendRequestData(URL, "GET");
            return GetHtml();
        }

        public string OpenReadWithHttps(string URL, string strPostdata)
        {
            ServicePointManager.CertificatePolicy = new CertPolicy();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.CookieContainer = new CookieContainer();
            request.Method = "POST";
            request.Accept = "*/*";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] buffer = encoding.GetBytes(strPostdata);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), encoding);
            respHtml = reader.ReadToEnd();
            foreach (Cookie ck in response.Cookies)
            {
                string text = cookie;
                cookie = text + ck.Name + "=" + ck.Value + ";";
            }
            reader.Close();
            return respHtml;
        }

        public string OpenRead(string URL, string postData)
        {
            byte[] sendBytes = encoding.GetBytes(postData);
            postStream = new MemoryStream();
            postStream.Write(sendBytes, 0, sendBytes.Length);
            requestHeaders.Add("Content-Length", postStream.Length.ToString());
            requestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            requestHeaders.Add("Connection", "close");
            SendRequestData(URL, "POST");
            return GetHtml();
        }

        public Stream GetStream(string URL, string postData)
        {
            byte[] sendBytes = encoding.GetBytes(postData);
            postStream = new MemoryStream();
            postStream.Write(sendBytes, 0, sendBytes.Length);
            requestHeaders.Add("Content-Length", postStream.Length.ToString());
            requestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            requestHeaders.Add("Connection", "close");
            SendRequestData(URL, "POST");
            MemoryStream ms = new MemoryStream();
            SaveNetworkStream(ms);
            return ms;
        }

        public string UploadFile(string URL, string fileField)
        {
            return UploadFile(URL, "", fileField);
        }

        public string UploadFile(string URL, string textField, string fileField)
        {
            postStream = new MemoryStream();
            if (textField != "" && fileField != "")
            {
                WriteTextField(textField);
                WriteFileField(fileField);
            }
            else if (fileField != "")
            {
                WriteFileField(fileField);
            }
            else
            {
                if (!(textField != ""))
                {
                    throw new Exception("文本域和文件域不能同时为空。");
                }
                WriteTextField(textField);
            }
            byte[] buffer = encoding.GetBytes("----HEDAODE----\r\n");
            postStream.Write(buffer, 0, buffer.Length);
            requestHeaders.Add("Content-Length", postStream.Length.ToString());
            requestHeaders.Add("Content-Type", "multipart/form-data; boundary=--HEDAODE--");
            requestHeaders.Add("Connection", "Keep-Alive");
            SendRequestData(URL, "POST", true);
            return GetHtml();
        }

        private void WriteTextField(string textField)
        {
            string[] strArr = Regex.Split(textField, "&");
            textField = "";
            string[] array = strArr;
            foreach (string var in array)
            {
                Match M = Regex.Match(var, "([^=]+)=(.+)");
                textField += "----HEDAODE--\r\n";
                string text = textField;
                textField = text + "Content-Disposition: form-data; name=\"" + M.Groups[1].Value + "\"\r\n\r\n" + M.Groups[2].Value + "\r\n";
            }
            byte[] buffer = encoding.GetBytes(textField);
            postStream.Write(buffer, 0, buffer.Length);
        }

        private void WriteFileField(string fileField)
        {
            string filePath2 = "";
            int count2 = 0;
            string[] strArr = Regex.Split(fileField, "&");
            string[] array = strArr;
            foreach (string var in array)
            {
                Match M = Regex.Match(var, "([^=]+)=(.+)");
                filePath2 = M.Groups[2].Value;
                fileField = "----HEDAODE--\r\n";
                string text = fileField;
                fileField = text + "Content-Disposition: form-data; name=\"" + M.Groups[1].Value + "\"; filename=\"" + Path.GetFileName(filePath2) + "\"\r\n";
                fileField += "Content-Type: image/jpeg\r\n\r\n";
                byte[] buffer3 = encoding.GetBytes(fileField);
                postStream.Write(buffer3, 0, buffer3.Length);
                FileStream fs2 = new FileStream(filePath2, FileMode.Open, FileAccess.Read);
                buffer3 = new byte[50000];
                do
                {
                    count2 = fs2.Read(buffer3, 0, buffer3.Length);
                    postStream.Write(buffer3, 0, count2);
                }
                while (count2 > 0);
                fs2.Close();
                fs2.Dispose();
                fs2 = null;
                buffer3 = encoding.GetBytes("\r\n");
                postStream.Write(buffer3, 0, buffer3.Length);
            }
        }

        public Stream DownloadData(string URL)
        {
            requestHeaders.Add("Connection", "close");
            SendRequestData(URL, "GET");
            MemoryStream ms = new MemoryStream();
            SaveNetworkStream(ms, true);
            return ms;
        }

        public void DownloadFile(string URL, string fileName)
        {
            requestHeaders.Add("Connection", "close");
            SendRequestData(URL, "GET");
            if (URL.ToLower().Contains(".flac"))
            {
                fileName = fileName.Replace(".mp3", ".flac");
            }
            FileStream fs2 = new FileStream(fileName, FileMode.Create);
            SaveNetworkStream(fs2, true);
            fs2.Close();
            fs2 = null;
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate,
X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        bool isSSL = false;
        Stream workStream;

        private void SendRequestData(string URL, string method, bool showProgress)
        {
            isSSL = URL.StartsWith("https");
            clientSocket = new TcpClient();
            Uri URI = new Uri(URL);
            clientSocket.Connect(URI.Host, isSSL ? 443 : URI.Port);
            requestHeaders.Add("Host", URI.Host);

            Stream networkStream = clientSocket.GetStream();
            workStream = isSSL ? new SslStream(networkStream, false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null) : networkStream;
            byte[] request = GetRequestHeaders(method + " " + URI.PathAndQuery + " HTTP/1.1");

            if (!isSSL) {
                clientSocket.Client.Send(request);
            }
            else
            {
                ((SslStream)workStream).AuthenticateAsClient(URI.Host);
                workStream.Write(request, 0, request.Length);
                workStream.Flush();
            }

            if (postStream == null)
            {
                return;
            }
            byte[] buffer = new byte[10245];
            int count2 = 0;

            postStream.Position = 0L;
            UploadEventArgs e = default(UploadEventArgs);
            e.totalBytes = postStream.Length;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            while (!isCanceled)
            {
                count2 = postStream.Read(buffer, 0, buffer.Length);
                workStream.Write(buffer, 0, count2);
                if (showProgress)
                {
                    e.bytesSent += count2;
                    e.sendProgress = (double)e.bytesSent / (double)e.totalBytes;
                    double t2 = timer.ElapsedMilliseconds / 1000;
                    t2 = ((t2 <= 0.0) ? 1.0 : t2);
                    e.sendSpeed = (double)e.bytesSent / t2;
                    this.UploadProgressChanged?.Invoke(this, e);
                }
                if (count2 <= 0)
                {
                    break;
                }
            }
            timer.Stop();
            postStream.Close();
            postStream = null;
        }

        private void SendRequestData(string URL, string method)
        {
            SendRequestData(URL, method, false);
        }

        private byte[] GetRequestHeaders(string request)
        {
            requestHeaders.Add("Accept", "*/*");
            requestHeaders.Add("Accept-Language", "zh-cn");
            requestHeaders.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)");
            string headers2 = request + "\r\n";
            foreach (string key in requestHeaders)
            {
                string text = headers2;
                headers2 = text + key + ":" + requestHeaders[key] + "\r\n";
            }
            if (cookie != "")
            {
                headers2 = headers2 + "Cookie:" + cookie + "\r\n";
            }
            headers2 = (strRequestHeaders = headers2 + "\r\n");
            requestHeaders.Clear();
            return encoding.GetBytes(headers2);
        }

        private string GetHtml()
        {
            MemoryStream ms = new MemoryStream();
            SaveNetworkStream(ms);
            StreamReader sr = new StreamReader(ms, encoding);
            respHtml = sr.ReadToEnd();
            sr.Close();
            ms.Close();
            return respHtml;
        }

        private void SaveNetworkStream(Stream toStream, bool showProgress)
        {

            byte[] buffer2 = new byte[10240];
            int count4 = 0;
            int startIndex = 0;
            MemoryStream ms = new MemoryStream();
            for (int i = 0; i < 3; i++)
            {
                count4 = workStream.Read(buffer2, 0, 500);
                ms.Write(buffer2, 0, count4);
            }
            if (ms.Length == 0)
            {
                workStream.Close();
                throw new Exception("远程服务器没有响应");
            }
            buffer2 = ms.GetBuffer();
            count4 = (int)ms.Length;
            GetResponseHeader(buffer2, out startIndex);
            count4 -= startIndex;
            toStream.Write(buffer2, startIndex, count4);
            DownloadEventArgs e = default(DownloadEventArgs);
            if (responseHeaders["Content-Length"] != null)
            {
                e.totalBytes = long.Parse(responseHeaders["Content-Length"]);
            }
            else
            {
                e.totalBytes = -1L;
            }
            Stopwatch timer = new Stopwatch();
            timer.Start();
            while (!isCanceled)
            {
                if (showProgress)
                {
                    e.bytesReceived += count4;
                    e.ReceiveProgress = (double)e.bytesReceived / (double)e.totalBytes;
                    byte[] tempBuffer = new byte[count4];
                    Array.Copy(buffer2, startIndex, tempBuffer, 0, count4);
                    e.receivedBuffer = tempBuffer;
                    double t = ((double)timer.ElapsedMilliseconds + 0.1) / 1000.0;
                    e.receiveSpeed = (double)e.bytesReceived / t;
                    startIndex = 0;
                    if (this.DownloadProgressChanged != null)
                    {
                        this.DownloadProgressChanged(this, e);
                    }
                }
                count4 = workStream.Read(buffer2, 0, buffer2.Length);
                toStream.Write(buffer2, 0, count4);
                if (count4 <= 0)
                {
                    break;
                }
            }
            timer.Stop();
            if (responseHeaders["Content-Length"] != null)
            {
                toStream.SetLength(long.Parse(responseHeaders["Content-Length"]));
            }
            toStream.Position = 0L;
            workStream.Close();
            clientSocket.Close();
        }

        private void SaveNetworkStream(Stream toStream)
        {
            SaveNetworkStream(toStream, false);
        }

        private void GetResponseHeader(byte[] buffer, out int startIndex)
        {
            responseHeaders.Clear();
            string html = encoding.GetString(buffer);
            StringReader sr = new StringReader(html);
            int start = html.IndexOf("\r\n\r\n") + 4;
            strResponseHeaders = html.Substring(0, start);
            if (sr.Peek() > -1)
            {
                string line3 = sr.ReadLine();
                Match M4 = Regex.Match(line3, "\\d\\d\\d");
                if (M4.Success)
                {
                    statusCode = int.Parse(M4.Value);
                }
            }
            while (sr.Peek() > -1)
            {
                string line2 = sr.ReadLine();
                if (line2 != "")
                {
                    Match M3 = Regex.Match(line2, "([^:]+):(.+)");
                    if (M3.Success)
                    {
                        try
                        {
                            responseHeaders.Add(M3.Groups[1].Value.Trim(), M3.Groups[2].Value.Trim());
                        }
                        catch
                        {
                        }
                        if (M3.Groups[1].Value == "Set-Cookie")
                        {
                            M3 = Regex.Match(M3.Groups[2].Value, "[^=]+=[^;]+");
                            cookie = cookie + M3.Value.Trim() + ";";
                        }
                    }
                    continue;
                }
                if (responseHeaders["Content-Length"] == null && sr.Peek() > -1)
                {
                    line2 = sr.ReadLine();
                    Match M = Regex.Match(line2, "~[0-9a-fA-F]{1,15}");
                    if (M.Success)
                    {
                        int length = int.Parse(M.Value, NumberStyles.AllowHexSpecifier);
                        responseHeaders.Add("Content-Length", length.ToString());
                        strResponseHeaders = strResponseHeaders + M.Value + "\r\n";
                    }
                }
                break;
            }
            sr.Close();
            startIndex = encoding.GetBytes(strResponseHeaders).Length;
        }

        public void Cancel()
        {
            isCanceled = true;
        }

        public void Start()
        {
            isCanceled = false;
        }
    }

    public delegate void WebClientDownloadEvent(object sender, DownloadEventArgs e);

    public delegate void WebClientUploadEvent(object sender, UploadEventArgs e);

    // Jade.Http.UploadEventArgs
    public struct UploadEventArgs
    {
        public long totalBytes;

        public long bytesSent;

        public double sendProgress;

        public double sendSpeed;
    }

    public struct DownloadEventArgs
    {
        public long totalBytes;

        public long bytesReceived;

        public double ReceiveProgress;

        public byte[] receivedBuffer;

        public double receiveSpeed;
    }

}