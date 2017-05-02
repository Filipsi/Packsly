### **This is subject to change at any point in time**

Few things is required in order to set up modpack hosting:

1. Hosting service like any [Website hosting](https://www.google.cz/?gfe_rd=cr&ei=tiDaVKviDYmMOtGfgOAO&gws_rd=ssl#q=website+hosting) or [Dropbox](https://www.dropbox.com/)
2. Understanding of JSON file format
3. Understanding of text file formats

## How to make -seek command work?
Seek command is used to list modpacks that can be installed from specified url adress.

Choose one of these solutions:

**Manual** _(If hosting doesn't support PHP)_

1. You need to create ```collection.json``` file at root folder of your hosting
2. Set text file to UTF-8
3. Add JSON array with modpack names surrounded by ""
```json
["testpack", "myawesomemodpack", "vanility"]
```

**Automatic** _(If hosting supports PHP)_ ***Not implemented in current version***

1. You need to create ```collection.php``` file at root folder of your hosting
2. Set text file to UTF-8
2. Add folowing code that will search folder for all JSON files
```php
<?php
  echo json_encode(glob("*.json", GLOB_BRACE));
?>
```

## How to make -install command work?
Seek command is used to install modpack from specified url adress by specified name.

1. Create ```[modpackname].json``` file at root folder of your hosting, replace ```[modpackname]``` with name of your modpack
2. Set text file to UTF-8

### Metadata fields
These fields specify modpack properties.

| Field         | Value type   | Description                                                       | Update strategy |      
| ------------- |--------------| ------------------------------------------------------------------|-----------------|
| revision      | String       | Modpack version, forces clients to update when changed            | Value change    |   
| icon          | String / Url | Modpack icon, can be relative or absolute url to ```*.png``` file | Value change    | 
| minecraft     | String       | Version of Minecraft that MultiMC should use                      | Installation    |
| forge         | String       | Version of Forge that MultiMC should use                          | Value change    |
| mods          | JSON Array   | Mods distributed in the modpack                                   | Revision change |

These fields might change, especially update strategy when I will have some time to do refactoring.

### Mods entries fields
Field ```mods``` is part of modpack metadata that specifies what mods are distributed with modpack. This filed is JSON Array that contains JSON Objects. This is mod entry object structure:

| Field         | Value type   | Is required | Description                                                          |   
| ------------- |--------------| ------------| ---------------------------------------------------------------------|
| url           | String / Url | true        | Absolute url for download (relative path soon)                       |          
| file          | String       | true        | Client filename with ```*.jar``` extension; Filename after download  |
| config        | JSON Array   | false       | Download urls for associated configuration files                     |          

### Mods entries config field
Field ```config``` is optional part of ```mods``` entries This filed is JSON Array that contains Strings. These strings are absolute URLs for downloading configuration files. The url has to contain 'config' in it's path and everything after this keyworld will be considered as installation path, example: file from url ```https://dl.dropboxusercontent.com/u/<id>/config/opencomputers/settings.conf``` will be downloaded to ```config/opencomputers``` directory at client.
```json
"config": [
  "https://dl.dropboxusercontent.com/u/<id>/config/InvTweaks.cfg",
  "https://dl.dropboxusercontent.com/u/<id>/config/InvTweaksRules.txt",
  "https://dl.dropboxusercontent.com/u/<id>/config/InvTweaksTree.txt"
]
```

### Format example
Example of modpack for Minecraft 1.10.2 and Forge 12.18.3.2185, with mods: StorageDrawers, Chameleon, JourneyMap, Waila and InventoryTweaks download directly from CurseForge.
```json
{
  "revision": "10",
  "icon": "/vials.png",
  "minecraft": "1.10.2",
  "forge": "1.10.2-12.18.3.2185",
  "mods":
    [{
      "url": "https://minecraft.curseforge.com/projects/storage-drawers/files/2367220/download",
      "file": "StorageDrawers-1.10.2-3.5.17.jar",
      "config": [
        "https://dl.dropboxusercontent.com/u/<id>/config/StorageDrawers.cfg"
      ]
    },
    {
      "url": "https://minecraft.curseforge.com/projects/chameleon/files/2355632/download",
      "file": "Chameleon-1.10-2.2.2.jar"
    },
    {
      "url": "https://minecraft.curseforge.com/projects/journeymap-32274/files/2370304/download",
      "file": "journeymap-1.10.2-5.4.4.jar"
    },
    {
      "url": "https://minecraft.curseforge.com/projects/waila/files/2301859/download",
      "file": "Waila-1.7.0-B3_1.9.4.jar"
    },
    {
      "url": "https://minecraft.curseforge.com/projects/inventory-tweaks/files/2338989/download",
      "file": "InventoryTweaks-1.62-dev-66.jar",
      "config": [
        "https://dl.dropboxusercontent.com/u/<id>/config/InvTweaks.cfg",
        "https://dl.dropboxusercontent.com/u/<id>/config/InvTweaksRules.txt",
        "https://dl.dropboxusercontent.com/u/<id>/config/InvTweaksTree.txt"
      ]
    }]
}
```