How to install modpack using Packsly:

1. First thing you will need is Packsly executable which you can download from [Releases](https://github.com/Filipsi/Packsly/releases). Make sure you download latest version.
2. Extract archive anywhere you want
3. Open command-line at extracted folder location.
  - 'Shift + Right click' in the folder and select 'Open command window here'
  - Or type 'cmd', into navigation bar in Windows Explorer
4. You have to specify where is your MultiMC installed. This step is required only **once**. Type 'Packsly.exe -set multimc [path]' where [paath] is replaced by absolute path to your MultiMC folder.
```
Packsly.exe -set multimc C:\Users\<USER>\Desktop\MultiMC
```
5. Type 'Packsly.exe -seek [url]' where [url] is replaced by link from where you want to get modpacks. This command will show all available modpacks at specified url.
```
Packsly.exe -seek packsly.filipsi.net
```
6. Choose modpack you wish to install from the list
7. Type 'Packsly.exe -install seek [packname]' where [packname] is replaced by name of the modpack. This command will try to install modpack from last url used by seek command. You can also use 'Packsly.exe -install [url] [packname]' and skip seek step.
```
Packsly.exe -install seek testpack
```
8. That's all. After successful installation there should be new instace in your MultiMC!