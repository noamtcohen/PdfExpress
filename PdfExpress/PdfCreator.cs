using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Web;

namespace PdfExpress
{
    public  class PdfCreator
    {
        private string _url;
        public string SavePdfPath { get; private set; }

        public bool WasCreated = false;
        private Orientation _orientation = Orientation.Portrait;
        private string _title;
        private readonly Dictionary<string,string> _postParams = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _cookies = new Dictionary<string, string>();

        private string _styleSheet;
        private string _js;
        private string _cookieJarFile;
        private int _delay = 3000;
        private int _dpi= 300;

        public static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return System.IO.Path.GetDirectoryName(path);
            }
        }

        private string GetArguments()
        {
            return
                GetOrienation() + GetTitle() + GetHeaders() +
                GetCookies() + GetPostParams() + GetJavascriptDelay() +
                GetMediaPrint() + GetStyleSheet() + GetExecuteJs() + GetCookieJarFile() + GetDpi();
        }


        public void Create()
        {
            var wkhtmltopdf = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = AssemblyDirectory + "\\wkhtmltopdf.exe",
                    Arguments = GetArguments() + _url + " " + SavePdfPath,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                }
            };
            wkhtmltopdf.Start();

            wkhtmltopdf.WaitForExit();
            WasCreated = true;
        }

        public void SetPaths(string url, string savePdfToPath)
        {
            SavePdfPath = savePdfToPath;
            _url = url;

            _title = url;
        }
        public void SetJsRunDelayInMsec(int delay)
        {
            _delay = delay;
        }

        public void SetOrientation(Orientation orientation)
        {
            _orientation = orientation;
        }

        public void SetTitle(string title)
        {
            _title = title;
        }

        public void SetExternalStyleSheetUrl(string url)
        {
            _styleSheet = url;
        }

        public void SetRunJsAfterLoad(string js)
        {
            _js = js;
        }

        public void AddPostParams(string name, string value)
        {
            _postParams.Add(name, value);
        }

        public void AddCookie(string name, string value)
        {
            _cookies.Add(name, value);
        }

        public void SetCookieJarFile(string cookieJarFile)
        {
            _cookieJarFile = cookieJarFile;
        }

        public void SetDpi(int dpi)
        {
            _dpi = dpi;
        }

        private string GetDpi()
        {
            return " --dpi " + _dpi + " ";
        }

        private string GetCookieJarFile()
        {
            if (string.IsNullOrEmpty(_cookieJarFile))
                return "";

            return " --cookie-jar " + _cookieJarFile + " ";
        }

        private string GetExecuteJs()
        {
            if (string.IsNullOrEmpty(_js))
                return "";

            return " --run-script " + _js + " ";
        }

        private string GetPostParams()
        {
            var argString = " ";

            foreach (var postParam in _postParams)
                argString += " --post "+postParam.Key+ " \"" +postParam.Value + "\" ";
            
            return argString;
        }

        private string GetJavascriptDelay()
        {
            return " --javascript-delay " + _delay + " ";
        }

        private string GetOrienation()
        {
            return " -O " +_orientation + " ";
        }

        private string GetTitle()
        {
            return " --title \"" + _title + "\" ";
        }

        private string GetHeaders()
        {
            return  " --custom-header-propagation ";
        }

        private string GetCookies()
        {
            if (_cookies.Count == 0)
                return "";

            var argString = " --custom-header Cookie ";
            
            foreach (var cookie in _cookies)
                argString += cookie.Key + "=" + HttpUtility.UrlEncode(cookie.Value) + ";";
            
            return argString.Trim(';') + " ";
        }

        private string GetMediaPrint()
        {
            return " --print-media-type ";
        }

        private string GetStyleSheet()
        {
            if (string.IsNullOrEmpty(_styleSheet))
                return "";

            return " --user-style-sheet " + _styleSheet + " ";
        }

        public enum Orientation
        {
            Landscape,
            Portrait
        }
    }
}
