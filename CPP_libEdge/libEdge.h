#pragma once
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// libEdge.h for C++ 
//
// Start the Edge thread. The userDataFolderRootPath folder must be writable. 
// A subfolder, EBWebView, will be created in the path
//
extern int StartEdge(const char* userDataFolderRootPath = R"(C:\Temp\)", const int xPos = 100, const int yPos = 0, const int width = 1000, const int height = 1150, const int timeOut = 5000);
//
// Stop the Edge thread
//
extern int StopEdge(int timeOut = 2000);
//
// The two main functions: Navigate to the page and then run javascript on it.
// ANSI (= Windows-1252 character set) functions. Codepage=1252
//
extern int Navigate(const char* url, const int timeOut = 5000);
extern int RunJavascript(const char* javascript, string& jsResult, const int timeOut = 2000);
extern int MsgBox(const char* message); // MessageBox 
//
extern HWND Get_hWnd();					// hWnd of Edge window
//
// Wide char equivalent functions. CodePage = CP_UTF8
//
extern int NavigateW(const wchar_t* wUrl, const int timeOut = 5000);
extern int RunJavascriptW(const wchar_t* wJavascript, wstring& wJsResult, const int timeOut = 2000);
extern int MsgBoxW(const wchar_t* message);
