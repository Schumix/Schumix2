#define MyAppName "Schumix"
#define MyAppVersion "3.x"
#define MyAppPublisher "Megax Productions"
#define MyAppURL "https://github.com/megax/Schumix2"
#define MyAppExeName "Schumix.exe"

[Setup]
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
LicenseFile=..\LICENSE
InfoBeforeFile=..\README.EN.md
OutputDir=.\
OutputBaseFilename=Setup
SetupIconFile=..\Applications\Schumix\icon.ico
Compression=lzma2/ultra64
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "basque"; MessagesFile: "compiler:Languages\Basque.isl"
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"
Name: "catalan"; MessagesFile: "compiler:Languages\Catalan.isl"
Name: "czech"; MessagesFile: "compiler:Languages\Czech.isl"
Name: "danish"; MessagesFile: "compiler:Languages\Danish.isl"
Name: "dutch"; MessagesFile: "compiler:Languages\Dutch.isl"
Name: "finnish"; MessagesFile: "compiler:Languages\Finnish.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"
Name: "hebrew"; MessagesFile: "compiler:Languages\Hebrew.isl"
Name: "hungarian"; MessagesFile: "compiler:Languages\Hungarian.isl"
Name: "italian"; MessagesFile: "compiler:Languages\Italian.isl"
Name: "japanese"; MessagesFile: "compiler:Languages\Japanese.isl"
Name: "norwegian"; MessagesFile: "compiler:Languages\Norwegian.isl"
Name: "polish"; MessagesFile: "compiler:Languages\Polish.isl"
Name: "portuguese"; MessagesFile: "compiler:Languages\Portuguese.isl"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"
Name: "serbiancyrillic"; MessagesFile: "compiler:Languages\SerbianCyrillic.isl"
Name: "serbianlatin"; MessagesFile: "compiler:Languages\SerbianLatin.isl"
Name: "slovak"; MessagesFile: "compiler:Languages\Slovak.isl"
Name: "slovenian"; MessagesFile: "compiler:Languages\Slovenian.isl"
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"
Name: "ukrainian"; MessagesFile: "compiler:Languages\Ukrainian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Dirs]
Name: "{localappdata}\Schumix\Logs"
Name: "{localappdata}\Schumix\Channels"

[Files]
Source: "..\Run\Release\ICSharpCode.SharpZipLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\KopiLua.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\LuaInterface.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Mono.Posix.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\MySql.Data.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Schumix.API.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Schumix.db3"; DestDir: "{localappdata}\Schumix\"; Flags: ignoreversion
Source: "..\Run\Release\Schumix.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Schumix.Framework.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Schumix.Irc.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Schumix.Libraries.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Schumix.LuaEngine.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Schumix.PythonEngine.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Schumix.Updater.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Server.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\sqlite3.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\System.Data.SQLite.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\YamlDotNet.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\YamlDotNet.RepresentationModel.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\IronPython.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\IronPython.Modules.dll"; DestDir: "{app}"; Flags: ignoreversion
;Addons
Source: "..\Run\Release\Addons\CalendarAddon.dll"; DestDir: "{localappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\ChatterBotAddon.dll"; DestDir: "{localappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\ChatterBotAPI.dll"; DestDir: "{localappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\CompilerAddon.dll"; DestDir: "{localappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\ExtraAddon.dll"; DestDir: "{localappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\GameAddon.dll"; DestDir: "{localappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\GitRssAddon.dll"; DestDir: "{localappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\HgRssAddon.dll"; DestDir: "{localappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\MantisBTRssAddon.dll"; DestDir: "{localappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\RevisionAddon.dll"; DestDir: "{localappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\SvnRssAddon.dll"; DestDir: "{localappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\TestAddon.dll"; DestDir: "{localappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\WolframAPI.dll"; DestDir: "{localappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\WordpressRssAddon.dll"; DestDir: "{localappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\YamlDotNet.Core.dll"; DestDir: "{localappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\YamlDotNet.RepresentationModel.dll"; DestDir: "{localappdata}\Schumix\Addons\"; Flags: ignoreversion
;Configs
Source: "..\Configs\CalendarAddon.yml"; DestDir: "{localappdata}\Schumix\Configs\"; Flags: ignoreversion
Source: "..\Configs\CompilerAddon.yml"; DestDir: "{localappdata}\Schumix\Configs\"; Flags: ignoreversion
Source: "..\Configs\ExtraAddon.yml"; DestDir: "{localappdata}\Schumix\Configs\"; Flags: ignoreversion
Source: "..\Configs\GitRssAddon.yml"; DestDir: "{localappdata}\Schumix\Configs\"; Flags: ignoreversion
Source: "..\Configs\HgRssAddon.yml"; DestDir: "{localappdata}\Schumix\Configs\"; Flags: ignoreversion
Source: "..\Configs\MantisBTRssAddon.yml"; DestDir: "{localappdata}\Schumix\Configs\"; Flags: ignoreversion
Source: "..\Configs\SvnRssAddon.yml"; DestDir: "{localappdata}\Schumix\Configs\"; Flags: ignoreversion
Source: "..\Configs\WordPressRssAddon.yml"; DestDir: "{localappdata}\Schumix\Configs\"; Flags: ignoreversion
Source: "Schumix.yml"; DestDir: "{localappdata}\Schumix\Configs\"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Parameters: "--config-dir=$localappdata\Schumix\Configs\ --config-file=Schumix.yml"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Parameters: "--config-dir=$localappdata\Schumix\Configs\ --config-file=Schumix.yml"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Schumix2 IRC Bot and Framework"; Flags: nowait postinstall skipifsilent

