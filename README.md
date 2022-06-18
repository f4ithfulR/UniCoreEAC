# UniCoreEAC

Setting the affinity of the EasyAntiCheat process to a single core is known to be a possible solution to at least reduce the frequency of crashes when playing _Tom Clancy's The Division 2™_. As there's no way to permanently configure the affinity inside Windows itself and it's a *PITA* to do it every time the game is launched this tool shall take care of it.

## Prerequisites
UniCoreEAC is build upon [.NET Core 3.1](https://dotnet.microsoft.com/en-us/download/dotnet/3.1), i.e. the corresponding framework/desktop runtime matching the chosen architecture (32/64 bit) has to be installed. In case it's missing the user will be prompted to install upon first launch of UniCoreEAC:

1. [.NET Core 3.1 Desktop Runtime (v3.1.26) - Windows x86 Installer](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-3.1.26-windows-x86-installer)
2. [.NET Core 3.1 Desktop Runtime (v3.1.26) - Windows x64 Installer](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-3.1.26-windows-x64-installer)

During runtime UniCoreEAC requires administor priviledges which are requested per manifest. The need for administrator priviledges arises from the fact that EasyAntiCheat is running as system service, i.e. otherwise UniCoreEAC won't be allowed the change EasyAntiCheat's affinity.

## Installation
As UniCoreEAC is compiled as monolithic executable its installation is quite straight-forward: 

 1. Copy `UniCoreEAC.exe` to a path of your likings.
 2. Run it.
 3. Open its context menu by right clicking the systray icon.
 4. Check `Start UniCoreEAC with Windows`

Once done UniCoreEAC will be started automatically during Windows startup. The autostart is implemented as a scheduled task which is bound to a single user. Using a scheduled task is the easiest approach as UniCoreEAC needs to be run with administrator priviledges for being able to change EasyAntiCheat's affinity.

> Note: please be aware that in case you move the UniCoreEAC.exe to another directory after enabling autostart the autostart won't work anymore as well as the detection whether it's enabled or not. To come around this problem please uncheck (removes the scheduled task pointing to the wrong executable) and recheck (creates a new scheduled task using the correct executable) `Start UniCoreEAC with Windows` once you started it from its new location.

## How it works
Once running UniCoreEAC checks every 5 seconds whether the EasyAntiCheat process - which is created by the game upon start - is running. In case it's recognized it's affinity is set to the first CPU core. Once affinity has been changed a system notification is shown and the status can also been seen inside the systray icon's context menu.

## Final words
The approach to limit the affinity of EasyAntiCheat to one core may or may not help in reducing crashes of _Tom Clancy's The Division 2™_. For myself I can state that it seems to help, even though I still experience crashes but less than before. Apart from the affinity there are a couple of further fixes which have been documented by [u/andro-bourne](https://www.reddit.com/user/andro-bourne/) at [Reddit](https://www.reddit.com/r/thedivision/comments/uwzv6n/div_2_crash_fix_stopped_interacting_with_windows/).
