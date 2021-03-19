////////////////////////////////////////////////////////////////////////////////////////////
// Example of how to use libEdge in C++
//
#include <windows.h>
#include <string>
using namespace std;
#include "libEdge.h"	

#ifdef _DEBUG
#ifdef _WIN64
#pragma comment(lib, R"(..\x64\Debug\libEdge.lib)")
#else // Win32
#pragma comment(lib, R"(..\x86\Debug\libEdge.lib)")
#endif
#else // Release
#ifdef _WIN64
#pragma comment(lib, R"(..\x64\Release\libEdge.lib)")
#else // Win32
#pragma comment(lib, R"(..\x86\Release\libEdge.lib)")
#endif
#endif
const char* CAPTION = "Edge is running";
//
// Javascript used in this demo
//
const char* JS_SETVAR = "var myClassElements = document.getElementsByClassName('myClass');"; // Access elements with myClassElements.myClass[0], myClass[1] ... 
const char* JS_GETCOUNT = "myClassElements.length;";										 // Number of elements
const char* JS_GETELEMENT = "myClassElements[%d].innerHTML;";								 // Content
//
// More examples:
//
// const char* JS_GETPAGE = "document.documentElement.outerHTML;";									// Javascript to get page HTML content
// const char* JS_GETBODY = "document.getElementsByTagName('body')[0].outerHTML;";					// Just the body tag content
// const char* JS_GETFIRSTCLASS = "document.getElementsByClassName('myClass')[0].firstElementChild;";	
// const char* JS_GETID = "document.getElementById('myId').innerHTML;";
//
int main()
{
	HWND hWnd;
	int result, elementCount, elementIndex;
	string jsResult, allResults;
	char txtBuf[_MAX_PATH];

	result = StartEdge();	// Start Edge browser
	if (result)
	{
		sprintf(txtBuf, "Error %d starting Edge", result);
		MessageBox(NULL, txtBuf, "Edge error", MB_SYSTEMMODAL);
		return result;
	}
	hWnd = Get_hWnd();	// Handle to Edge window
	MessageBox(NULL, "Click to hide window", CAPTION, MB_SYSTEMMODAL);
	result = ShowWindow(hWnd, SW_HIDE); 
	MessageBox(NULL, "Click to show window", CAPTION, MB_SYSTEMMODAL);
	result = ShowWindow(hWnd, SW_SHOW);
	//
	// Use local file for demo
	//
	GetCurrentDirectory(sizeof(txtBuf), txtBuf);
	strcat(txtBuf, "\\DemoPage.html");
	result = Navigate(txtBuf); // Desired URL -> Navigate("https://example.com");
	if (result)
		return result; // Error
	//
	// Access the DOM by applying javascript
	//
	result = RunJavascript(JS_SETVAR, jsResult); // Create myClassElements
	if (result)
		return result;
	result = RunJavascript(JS_GETCOUNT, jsResult); // Get number of elements
	elementCount = stoi(jsResult);
	for (elementIndex = 0; elementIndex < elementCount; elementIndex++)
	{
		sprintf(txtBuf, JS_GETELEMENT, elementIndex);
		result = RunJavascript(txtBuf, jsResult); // Get this element
		allResults += jsResult + "\n";
	}
	allResults += "\nClick to stop\n";
	//
	// Show result. Exit Edge 
	//
	MessageBox(NULL, allResults.c_str(), CAPTION, MB_SYSTEMMODAL);
	return StopEdge();
}
