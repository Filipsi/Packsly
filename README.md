<h1 align="center">
	<br>
	Packsly3
	<br>
</h1>

<h4 align="center">A modular modpack installer and embedded updater for Minecraft launchers like MultiMC </h4>

<p align="center">
	<a href="https://ci.appveyor.com/project/Filipsi/packsly/branch/master">
		<img src="https://ci.appveyor.com/api/projects/status/di1a2rpglgywnkov/branch/master?svg=true&passingText=master%20-%20passing&failingText=master%20-%20failing&pendingText=master%20-%20pending">
	</a>
		<a href="https://ci.appveyor.com/project/Filipsi/packsly">
		<img src="https://ci.appveyor.com/api/projects/status/di1a2rpglgywnkov?svg=true&passingText=bleeding%20-%20passing&failingText=bleeding%20-%20failing&pendingText=bleeding%20-%20pending">
	</a>
	<a href="https://paypal.me/Filipsi">
		<img src="https://img.shields.io/badge/$-donate-ff69b4.svg?maxAge=2592000&amp;style=flat">
	</a>
</p>

<p align="center">
	<a href="#key-features">Key Features</a> •
	<a href="#how-to-use">How To Use</a> •
	<a href="#download">Download</a> •
	<a href="#license">License</a>
</p>

## Key Features

### Launchers
* Compatible with multiple Minecraft Launchers.
* Default [MultiMC](https://multimc.org/) launcher support via [MmcLauncherEnvironment](https://github.com/Filipsi/Packsly/blob/master/source/modules/Packsly3.MultiMC/Launcher/MmcLauncherEnvironment.cs).
* Default [server](https://minecraft.gamepedia.com/Server) environment support via [ServerLauncherEnvironment](https://github.com/Filipsi/Packsly/blob/master/source/modules/Packsly3.Server/Launcher/ServerLauncherEnvironment.cs).
* Extensibility provided by [ILauncherEnvironment](https://github.com/Filipsi/Packsly/blob/master/source/modules/Packsly3.Core/Launcher/ILauncherEnvironment.cs) implementations.

### ModLoaders
* MultiMC launcher supports Forge, LittleLoader and Fabric modloaders.
* Server environment supports only Forge modloader for now.
* Extensibility provided by [IModLoaderHandler](https://github.com/Filipsi/Packsly/blob/master/source/modules/Packsly3.Core/Launcher/Modloader/IModLoaderHandler.cs) implementations.
  * [BasicModLoaderHandler](https://github.com/Filipsi/Packsly/blob/master/source/modules/Packsly3.Core/Launcher/Modloader/Impl/BasicModLoaderHandler.cs) is base class tied to particular Minecraft instance type.
  * [MultiModLoaderHandler](https://github.com/Filipsi/Packsly/blob/master/source/modules/Packsly3.Core/Launcher/Modloader/Impl/MultiModLoaderHandler.cs) is a helper providing multiple installation strategies via [IModLoaderInstallationStrategy](https://github.com/Filipsi/Packsly/blob/master/source/modules/Packsly3.Core/Launcher/Modloader/IModLoaderInstallationStrategy.cs).

### Modpacks
* Download of individual mods and configuration files.
* Modpack content can be specified in [modpack definition](https://github.com/Filipsi/Packsly/blob/master/resources/modpack-definition-example.json#L26) file that can be ether on disk or obtained from URL address.
* Download locations are abstracted by [environmental variables](https://github.com/Filipsi/Packsly/blob/master/source/modules/Packsly3.Core/Launcher/Instance/EnvironmentVariables.cs) that are specified by [instances](https://github.com/Filipsi/Packsly/blob/master/source/modules/Packsly3.MultiMC/Launcher/MmcMinecraftInstance.cs#L46).
* Mods can be white-listed or black-listed [depending](https://github.com/Filipsi/Packsly/blob/master/resources/modpack-definition-example.json#L54) on current environment.
  * For example MiniMap mod only on client-side can be black-listed in server environments.
  * Or server-side mod like [Morpheus](https://www.curseforge.com/minecraft/mc-mods/morpheus) can be white-listed only in server environments.

### Minecraft instances
* Ability to create and configure launcher instances.
* Instances can be configured in modpack [definition](definition) file.
* Configuration is controlled by modpack [instance](https://github.com/Filipsi/Packsly/blob/master/source/modules/Packsly3.MultiMC/Launcher/MmcMinecraftInstance.cs#L66) which is providing it's own handling logic - for example a [file wrapper](https://github.com/Filipsi/Packsly/blob/master/source/modules/Packsly3.MultiMC/FileSystem/MmcConfigFile.cs).

### Updating
* Following behavior is default implementation and can be changed by using your own adapter.
* Modpack has a numerical version and on each change, the updater compares names of all mod files.
* Mods that are not in newest revision are downloaded and mods that are missing are deleted.
* Configuration files are downloaded on every revision change.
* Update adapter is enabled and configured in [modpack definition](https://github.com/Filipsi/Packsly/blob/master/resources/modpack-definition-example.json#L6).
* Default updating strategy is provided by [RevisionUpdateAdapter](https://github.com/Filipsi/Packsly/blob/master/source/modules/Packsly3.Core/Launcher/Adapter/Impl/RevisionUpdateAdapter.cs).

### Custom behavior
* [Adapters](https://github.com/Filipsi/Packsly/blob/master/source/modules/Packsly3.Core/Launcher/Adapter/IAdapter.cs) can be used to execute custom logic.
* Execution is tied to Minecraft instance [lifecycle](https://github.com/Filipsi/Packsly/blob/master/source/modules/Packsly3.Core/Launcher/Instance/Lifecycle.cs) events.
* Adapter has to be enabled in [modpack definition](https://github.com/Filipsi/Packsly/blob/master/resources/modpack-definition-example.json#L5) in order to be used.

### Modularity
* Support for loading module DLLs that can provide their own implementations.
* Automatic registration by annotating classes with [Register](https://github.com/Filipsi/Packsly/blob/master/source/modules/Packsly3.Core/Common/Register/RegisterAttribute.cs) attribute.

## How To Use

Some example files are available [here](https://github.com/Filipsi/Packsly/tree/master/resources).

To show help while running the app in CLI mode run `Packsly3.Cli` executable with `--help` argument.

More coming soon...

## Download

You can [download](https://github.com/Filipsi/Packsly/releases) the latest stable version of Packsly for Windows, Linux (mono).

Bleeding edge builds are available for [download](https://ci.appveyor.com/project/Filipsi/packsly) as atrifacts from build server.

## License

GNU General Public License v3.0
