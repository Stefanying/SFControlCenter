%SystemRoot%\Microsoft.NET\Framework\v2.0.50727\Installutil.exe AutoMonitor.exe  
ping 127.0.0.1  -n 3 >nul
Net Start MyWindowService
ping 127.0.0.1  -n 3 >nul 
sc config MyWindowService start= auto