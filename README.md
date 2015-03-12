To alter GMDA source code and run on your PC you must have Visual Studio installed on your computer.
You must also have version 4 of Microsoft.NET installed on your computer. On my machine .NET is installed at:
C:\Windows\Microsoft.NET\Framework\v4.0.30319

If your version differs you may need to alter the build events in order to sucessfully build the application in Visual Studio.

To do this go to Project --> MapDrawingAnalyzer Properties (or press ALT-F7).
Then go to build events. In the Post-build event command line section find this line:

"Map Drawing Analyzer\Ilmerge\ILMerge.exe" "Map Drawing Analyzer\Ilmerge\GMDA.exe" "Map Drawing Analyzer\Ilmerge\MessageBoxManager.dll" "Map Drawing Analyzer\Ilmerge\AppLimit.NetSparkle.dll" /targetplatform:v4,"C:\Windows\Microsoft.NET\Framework\v4.0.30319" /t:winexe /out:"Map Drawing Analyzer\Ilmerge\dest\GMDA.exe"

The important section is:

/targetplatform:v4,"C:\Windows\Microsoft.NET\Framework\v4.0.30319"

Change to the directory in quotes to point to where your .NET installation is located.