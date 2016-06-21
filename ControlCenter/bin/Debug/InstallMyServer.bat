%SystemRoot%\Microsoft.NET\Framework\v2.0.50727\Installutil.exe AutoMonitor.exe
 Net Start MyWindowsServer
 sc config MyWindowsServer start= auto