using System;
using System.IO;
using System.Text;                      // StringBuilder
using System.Runtime.InteropServices;   // DllImport

namespace CS_libEdge
{
    class CS_libEdge
    {
        //
        // NOTE: libEdge.dll and WebView2Loader.dll must be in the same directory as the executable
        //
        [DllImport("libEdge.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //
        // NOTE: userDataFolder must exist and be writable. 
        //
        public static extern unsafe int StartEdge([MarshalAs(UnmanagedType.LPStr)] string userDataFolder = @"C:\Temp\", int xPos = 100, int yPos = 10, int width = 400, int height = 300, int timeOut = 5000);
        [DllImport("libEdge.dll")] public static extern int StopEdge(bool deleteData = false, int timeOut = 2000);
        [DllImport("libEdge.dll")] public static extern int Navigate([MarshalAs(UnmanagedType.LPStr)] string url, int timeOut = 5000);

        public delegate void CallBack(string s, ref StringBuilder sb);  // Callback from C++
        [DllImport("libEdge.dll")] public static extern int CS_RunJavascript([MarshalAs(UnmanagedType.LPStr)] string javascript, ref StringBuilder jsResult, CallBack MyCallBack, int timeOut = 2000);

        [DllImport("libEdge.dll")] public static extern int MsgBox([MarshalAs(UnmanagedType.LPStr)] string message);
        [DllImport("libEdge.dll")] public static extern int Get_hWnd();
        //
        // Javascript
        //
        static string JS_SETVAR = "var myClassElements = document.getElementsByClassName('myClass');"; // Access elements with myClassElements.myClass[0], myClass[1] ... 
        static string JS_GETCOUNT = "myClassElements.length;";                                         // Number of elements
        static string JS_GETELEMENT = "myClassElements[{0}].innerHTML;";							   // Content

        const string DEMOPAGE = @"\DemoPage.html";
        static int Main(string[] args)
        {
            int result, count, elementIndex;
            string js;
            string allElements;
            string binDir, projDir;
            StringBuilder jsResult = new StringBuilder(1);  // Result buffer
            CallBack Resize = SizeUp;                       // Callback pointer

            binDir = Directory.GetCurrentDirectory();                               // Executable
            projDir = Directory.GetParent(binDir).Parent.Parent.Parent.FullName;    // Project directory with DemoPage.html

            unsafe
            {
                result = StartEdge();   // Returns error code if error 
            }
            if (result != 0)        
            {
                MsgBox("Error starting Edge " + result);
                return result;
            }
            string demoPage = projDir + DEMOPAGE;

            result = Navigate(demoPage); // Desired URL, Navigate("https://example.com");
            if (result != 0)        
            {
                MsgBox("Error " + result + ", navigating to page " + demoPage);
                return result;
            }
            //
            // Access the DOM by applying javascript
            //
            js = JS_SETVAR;                                         // Create myClassElements
            result = CS_RunJavascript(js, ref jsResult, Resize);    // Set var myClassElements in the DOM

            js = JS_GETCOUNT;                                       // Get number of elements
            result = CS_RunJavascript(js, ref jsResult, Resize);    // Count in jsResult
            count = Int32.Parse(jsResult.ToString());

            allElements = "";
            for (elementIndex = 0; elementIndex < count; elementIndex++)
            {
                js = string.Format(JS_GETELEMENT, elementIndex);
                result = CS_RunJavascript(js, ref jsResult, Resize); // Get this element
                allElements += jsResult + "\n";
            }
            MsgBox(allElements + "\n" + "Click to quit program");
            return StopEdge(true);          // Stop Edge browswer and delete userDataFolder (cookies)
        }
        /////////////////////////////////////////////////////////////////////////////////
        // Callback from C++ to allocate room for javascript result in StringBuilder
        //
        public static void SizeUp(string js, ref StringBuilder jsResult)
        {
            jsResult.Clear();
            jsResult.Append(js);
        }
    }
}
