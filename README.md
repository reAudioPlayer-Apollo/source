# reAudioPlayer Apollo

.NET 5 Standard version not fully tested for stability yet.

This programme has been migrated from .NET Framework 4.7.2 to .NET 5 as .NET Framework will be the last .NET Framework version.
.NET 5 also adds new possibilities (eg. TaskDialog) and further improves performance and stability.

Due to the fact that our first web server library does not support .NET Core and therefore .NET 5 Standard, we also decided to use EmbedIO as our new web server library.
EmbedIO also provides an easy way to add websocket servers, which we did in one of our recent updates. Note that the http endpoints will still be available as a description in the wiki. The websocket servers only serve as a more convenient, reliable and smooth wrapper of our API.

## Dependencies

- Webserver - [EmbedIO](https://github.com/unosquare/embedio)
- Youtube API - [Google API](https://github.com/googleapis/google-api-dotnet-client)
- Updater - [LibGit2Sharp](https://github.com/libgit2/libgit2sharp)
- Youtube Download - [NYoutubeDL](http://gitlab.com/rgunti/nyoutubedl)
- Hotkeys - [NHotkey](https://github.com/thomaslevesque/NHotkey)
- QRCode Generator - [QRCoder](https://github.com/codebude/QRCoder)
- Spotify API - [SpotifyAPI-NET](https://github.com/JohnnyCrazy/SpotifyAPI-NET)
- IDv3 Tag R/W - [Taglib-Sharp](https://github.com/mono/taglib-sharp)
- GameSharing API - [Created & Maintaned by Claythve](https://github.com/Claythve)

## Installation

Download the "Source Code" and execute Updater.exe. https://github.com/reAudioPlayer-Apollo/installer

Alternatively, clone the project:

```
git clone https://github.com/reAudioPlayer-Apollo/installer.git
```

## Local Storage & Collected Data

reAudioPlayer collects and stores locally in "%localappdata%\reAudioPlayer":
- playlists (folders) you have played, in order to use them in "Apollo On Air" (PLMLib.list)
- played songs as a look up table for the csv (described below) (SMLib.list)
- order of songs played for the recommendation algorithm (using SMLib.list) (SMLT.csv)
- songs played while playing a game, used for the playlist recommendation algorithm (featured in Apollo On Air) (using SMLib.list) (GMLT.csv)
- Move Actions, source / target for 'move' actions for the recommendation algorithm (MVMLT.csv)
- YT-Sync Jobs (job description + archive) (Syncs/*)

reAudioPlayer collects and stores anonymously on an external server, only accessible through the audio player:
- [install directory]/ressources/games.json is being synchronised to the cloud

reAudioPlayer collects and temporary stores anonymously on an external server, only accessible through the audio player with an automatically generated key:
- game library, used to share your games, generated key can be shared to friends, expires one day after creation
