#pragma once
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// libEdge.h
//
// Start the Edge thread
// userDataFolderPath must exist and be writable
//
int StartEdge(const char* userDataFolderPath = R"(C:\Temp\)", const int xPos = 100, const int yPos = 0, const int width = 300, const int height = 400, const int timeOut = 5000);
//
// Stop the Edge thread
//
int StopEdge(const bool deleteData = false, const int timeOut = 2000);
//
// The two main functions: Navigate to the page and then run javascript on it
// ANSI (= Windows-1252 character set) functions
//
int Navigate(const char* url, const int timeOut = 5000);
int RunJavascript(const char* javascript, string& jsResult, const int timeOut = 2000);
int MsgBox(const char* message);
HWND Get_hWnd();
//
// Wide char (Unicode, 16-bits) equivalent functions
//
int NavigateW(const wchar_t* wUrl, const int timeOut = 5000);
int RunJavascriptW(const wchar_t* wJavascript, wstring& wJsResult, const int timeOut = 2000);
int MsgBoxW(const wchar_t* message);
