#### something is called a Dotnet invariant project
```
​<​ItemGroup​>
  <​RuntimeHostConfigurationOption​ ​Include​=​"​System.Globalization.Invariant​"​ ​Value​=​"​true​"​ />
</​ItemGroup​>
```

#### Ideas
- Make a Grid with cols and rows xaml generator
- Make a data transform framework
- Make some tutorials on youtube
- Write blogs



Links
- https://itnext.io/the-concept-of-workflow-engines-c14e8088283
- [RDO.net] (https://rdo.devzest.com/articles/tutorial/get_started.html?tabs=vs2017%2Ccs)
- [ScottPlot](https://swharden.com/scottplot/quickstart#console-quickstart)

```
        public static byte[] ZipFiles(Dictionary<string, byte[]> files)
        {
            using MemoryStream stream = new MemoryStream();
            using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Update))
            {
                foreach (var (key, value) in files)
                {
                    ZipArchiveEntry orderEntry = archive.CreateEntry(key);
                    using BinaryWriter writer = new BinaryWriter(orderEntry.Open());
                    writer.Write(value);
                }
            }
            return stream.ToArray();
        }
```

#### Windows Terminal Settings
```
{
    "$schema": "https://aka.ms/terminal-profiles-schema",
    "defaultProfile": "{61c54bbd-c2c6-5271-96e7-009a87ff44bf}",
    "copyOnSelect": false,
    "copyFormatting": false,
    "profiles":
    {
        "defaults":
        {
            "closeOnExit" : false,
            "colorScheme" : "Retro",
            "cursorColor" : "#FFFFFF",
            "cursorShape": "filledBox",
            "fontSize" : 16,
            "padding" : "5, 5, 5, 5",
            "fontFace": "PxPlus IBM VGA8",
            "experimental.retroTerminalEffect": true,
            "hidden": false,
            "startingDirectory": "C:\\Users\\frank\\"
        },
        "list":
        [
            {
                "guid": "{b453ae62-4e3d-5e58-b989-0a918ec651b8}",
                "name": "Anaconda",
                "icon": "C:\\Users\\frank\\anaconda3\\Menu\\anaconda-navigator.ico",
                "commandline": "powershell.exe -ExecutionPolicy ByPass -NoExit -Command \"& 'C:\\Users\\frank\\anaconda3\\shell\\condabin\\conda-hook.ps1'"
            },
            {
                "guid": "{61c54bbd-c2c6-5271-96e7-009a87ff44bf}",
                "name": "Windows PowerShell",
                "commandline": "powershell.exe"
            },
            {
                "guid": "{0caa0dad-35be-5f56-a8ff-afceeeaa6101}",
                "name": "Command Prompt",
                "commandline": "cmd.exe"
            },
            {
                "guid": "{b453ae62-4e3d-5e58-b989-0a998ec441b8}",
                "hidden": false,
                "name": "Azure Cloud Shell",
                "source": "Windows.Terminal.Azure"
            }
        ]
    },
    "schemes": [
        {
            "name": "Retro",
            "background": "#000000",
            "black": "#00ff00",
            "blue": "#00ff00",
            "brightBlack": "#00ff00",
            "brightBlue": "#00ff00",
            "brightCyan": "#00ff00",
            "brightGreen": "#00ff00",
            "brightPurple": "#00ff00",
            "brightRed": "#00ff00",
            "brightWhite": "#00ff00",
            "brightYellow": "#00ff00",
            "cyan": "#00ff00",
            "foreground": "#00ff00",
            "green": "#00ff00",
            "purple": "#00ff00",
            "red": "#00ff00",
            "white": "#00ff00",
            "yellow": "#00ff00"
        }
    ],
    "actions":
    [
        { "command": {"action": "copy", "singleLine": false }, "keys": "ctrl+c" },
        { "command": "paste", "keys": "ctrl+v" },
        { "command": "find", "keys": "ctrl+shift+f" },
        { "command": { "action": "splitPane", "split": "auto", "splitMode": "duplicate" }, "keys": "alt+shift+d" }
    ]
}

```
