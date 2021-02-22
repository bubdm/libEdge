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
        [DllImport("libEdge.dll")] public static extern int StopEdge(int deleteData = 0, int timeOut = 2000);
        [DllImport("libEdge.dll")] public static extern int Navigate([MarshalAs(UnmanagedType.LPStr)] string url, int timeOut = 5000);
        [DllImport("libEdge.dll")] public static extern int CS_RunJavascript([MarshalAs(UnmanagedType.LPStr)] string javascript, StringBuilder jsResult, int maxLen, int timeOut = 2000);
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
            int result, size, count, elementIndex;
            string js;
            string allElements;
            string binDir, projDir;
            StringBuilder jsResult = new StringBuilder(1); // Result buffer

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

            result = Navigate(demoPage); // Change to desired URL, Navigate("https://example.com");
            if (result != 0)        
            {
                MsgBox("Error " + result + ", navigating to page " + demoPage);
                return result;
            }
            //
            // Access the DOM by applying javascript
            //
            js = JS_SETVAR;                                 // Create myClassElements
            size = CS_RunJavascript(js, null, 0);           // Get required buffer size
            SizeUp(size, ref jsResult);                     // Increase if needed
            result = CS_RunJavascript(js, jsResult, size);  // Set var myClassElements in the DOM

            js = JS_GETCOUNT;                               // Get number of elements
            size = CS_RunJavascript(js, null, 0);           // Get required buffer size
            SizeUp(size, ref jsResult);
            result = CS_RunJavascript(js, jsResult, size);  // Count in jsResult
            count = Int32.Parse(jsResult.ToString());

            allElements = "";
            for (elementIndex = 0; elementIndex < count; elementIndex++)
            {
                js = string.Format(JS_GETELEMENT, elementIndex);
                size = CS_RunJavascript(js, null, 0);         // Size request
                SizeUp(size, ref jsResult); 
                result = CS_RunJavascript(js, jsResult, size); // Get this element
                allElements += jsResult + "\n";
            }
            MsgBox(allElements + "\n" + "Click to quit program");
            return StopEdge();          // Stop Edge browswer and delete userDataFolder
        }
        ////////////////////////////////////////////////////////////////////////
        // Increase the size of our buffer if needed
        //
        static void SizeUp(int reqSize, ref StringBuilder jsResult)
        {
            if (reqSize > jsResult.Capacity)
                jsResult.Capacity = reqSize;
            jsResult.Length = reqSize;          // Exact length
        }
    }
}
