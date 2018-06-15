@echo off
set sdkDir="C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A"

%sdkDir%\bin\xsd.exe PluginConfiguration.xsd /classes /l:cs /n:Syncfusion.Addin.Core.Metadata /o:..\Metadata


pause
@echo on
