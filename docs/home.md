Welcome to the Packsly wiki!

Packsly is command-line tool that can install [MultiMC](https://github.com/MultiMC/MultiMC5) instances from remote location and keep them up to date with embeded updater using pre-launch commands.

Learn how to [Install modpack](https://github.com/Filipsi/Packsly/wiki/Installing-modpack) or [Setup modpack hosting](https://github.com/Filipsi/Packsly/wiki/Setting-up-modpack-hosting)!

## How to use Packsly?
Check out [Installing modpack](https://github.com/Filipsi/Packsly/wiki/Installing-modpack) page or run Packsly with ```-help``` argument.

## How updating works?
Updating is fully automatic using MultiMC pre-launch commands. When you start modpack installed using Packsly, these tasks are performed in listed order:

- If modpack icon was changed download new one
- If Forge version changed  generate new pach
- If modpack revision changed
  + Download **all** configuration files
  + Remove mods that are not listed
  + Download mods that are missing
- If anything changed update instance.packsly file