#pragma once
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// dllEdge.h for C# & VBA 
//
#if defined _WINDLL
#define DLL __declspec(dllexport)
#else
#define DLL __declspec(dllimport)
#endif
//
// VBA only
//
// Start the Edge thread. The userDataFolderRootPath folder must be writable. 
// A subfolder, EBWebView, will be created in the path
//
DLL int __stdcall VBA_StartEdge(const char* userDataFolder = R"(C:\Temp\)", const int xPos = 100, const int yPos = 0, const int width = 1000, const int height = 1150, const int timeOut = 5000);
DLL int __stdcall VBA_StopEdge(const int timeOut = 2000);
DLL int __stdcall VBA_Navigate(const char* url, const int timeOut = 5000);
DLL int __stdcall VBA_RunJavascript(const char* javascript, char* jsResult, const int maxLen, const int timeOut);
DLL int __stdcall VBA_MsgBox(const char* message);
DLL HWND __stdcall VBA_Get_hWnd();
//
// C# only
//
DLL int CS_RunJavascript(const char* javascript, char* jsResult, const int maxLen, const int timeOut);
//
// C++, C#, VBA
//
DLL HWND Get_hWnd(); 
DLL int MsgBox(const char* message);
//
// C# (and C++ if using the DLL)
// Start the Edge thread
//
DLL int StartEdge(const char* userDataFolder = R"(C:\Temp\)", const int xPos = 100, const int yPos = 0, const int width = 1000, const int height = 1150, const int timeOut = 5000);
//
// Stop the Edge thread
//
DLL int StopEdge(const int timeOut = 2000);
//
// The two main functions: Navigate to the page and then run javascript on it
// ANSI (= Windows-1252 character set) functions. Codepage=1252
//
DLL int Navigate(const char* url, const int timeOut = 5000);
DLL int RunJavascript(const char* javascript, string& jsResult, const int timeOut = 2000);
//
// Wide char equivalent functions. CodePage = CP_UTF8
//
DLL int NavigateW(const wchar_t* wUrl, const int timeOut = 5000);
DLL int RunJavascriptW(const wchar_t* wJavascript, wstring& wJsResult, const int timeOut = 2000);
DLL int MsgBoxW(const wchar_t* message);
