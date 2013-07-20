#define MyAppName "Schumix"
#define MyAppVersion "4.1.x"
#define MyAppPublisher "Schumix Productions"
#define MyAppURL "https://github.com/Schumix/Schumix2"
#define MyAppExeName "Schumix.exe"

[Setup]
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}/issues
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
LicenseFile=..\License
InfoBeforeFile=..\Readme.en.md
OutputDir=.\
OutputBaseFilename=Setup
SetupIconFile=..\Applications\Schumix\icon.ico
Compression=lzma2/ultra64
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"
Name: "catalan"; MessagesFile: "compiler:Languages\Catalan.isl"
Name: "corsican"; MessagesFile: "compiler:Languages\Corsican.isl"
Name: "czech"; MessagesFile: "compiler:Languages\Czech.isl"
Name: "danish"; MessagesFile: "compiler:Languages\Danish.isl"
Name: "dutch"; MessagesFile: "compiler:Languages\Dutch.isl"
Name: "finnish"; MessagesFile: "compiler:Languages\Finnish.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"
Name: "greek"; MessagesFile: "compiler:Languages\Greek.isl"
Name: "hebrew"; MessagesFile: "compiler:Languages\Hebrew.isl"
Name: "hungarian"; MessagesFile: "compiler:Languages\Hungarian.isl"; InfoBeforeFile: ..\Readme.markdown    
Name: "italian"; MessagesFile: "compiler:Languages\Italian.isl"
Name: "japanese"; MessagesFile: "compiler:Languages\Japanese.isl"
Name: "norwegian"; MessagesFile: "compiler:Languages\Norwegian.isl"
Name: "polish"; MessagesFile: "compiler:Languages\Polish.isl"
Name: "portuguese"; MessagesFile: "compiler:Languages\Portuguese.isl"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"
Name: "serbiancyrillic"; MessagesFile: "compiler:Languages\SerbianCyrillic.isl"
Name: "serbianlatin"; MessagesFile: "compiler:Languages\SerbianLatin.isl"
Name: "slovenian"; MessagesFile: "compiler:Languages\Slovenian.isl"
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"
Name: "ukrainian"; MessagesFile: "compiler:Languages\Ukrainian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Dirs]
Name: "{userappdata}\Schumix\Logs"
Name: "{userappdata}\Schumix\Channels"

[Files]
Source: "..\Run\Release\ICSharpCode.SharpZipLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\NGit.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\NSch.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Sharpen.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Sharpen.Unix.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\KeraLua.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\KopiLua.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\NLua.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\NGettext.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Mono.Posix.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Mono.Security.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\MySql.Data.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Schumix.db3"; DestDir: "{userappdata}\Schumix\"; Flags: ignoreversion
Source: "..\Run\Release\Schumix.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Schumix.Framework.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Schumix.Irc.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Schumix.Libraries.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Schumix.Components.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Server.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\sqlite3.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\System.Data.SQLite.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\YamlDotNet.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\YamlDotNet.RepresentationModel.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\IronPython.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\IronPython.Modules.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Microsoft.Dynamic.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Run\Release\Microsoft.Scripting.dll"; DestDir: "{app}"; Flags: ignoreversion
;Addons
Source: "..\Run\Release\Addons\CalendarAddon.dll"; DestDir: "{userappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\ChatterBotAddon.dll"; DestDir: "{userappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\ChatterBotAPI.dll"; DestDir: "{userappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\CompilerAddon.dll"; DestDir: "{userappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\ExtraAddon.dll"; DestDir: "{userappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\GameAddon.dll"; DestDir: "{userappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\GitRssAddon.dll"; DestDir: "{userappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\HgRssAddon.dll"; DestDir: "{userappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\MantisBTRssAddon.dll"; DestDir: "{userappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\RevisionAddon.dll"; DestDir: "{userappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\SvnRssAddon.dll"; DestDir: "{userappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\TestAddon.dll"; DestDir: "{userappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\WolframAPI.dll"; DestDir: "{userappdata}\Schumix\Addons\"; Flags: ignoreversion
Source: "..\Run\Release\Addons\WordpressRssAddon.dll"; DestDir: "{userappdata}\Schumix\Addons\"; Flags: ignoreversion
;Locale
Source: "..\Run\Release\locale\*"; DestDir: "{app}\locale\"; Flags: ignoreversion recursesubdirs createallsubdirs
;Configs
Source: "..\Configs\CalendarAddon.yml"; DestDir: "{userappdata}\Schumix\Configs\"; Flags: ignoreversion
Source: "..\Configs\CompilerAddon.yml"; DestDir: "{userappdata}\Schumix\Configs\"; Flags: ignoreversion
Source: "..\Configs\ExtraAddon.yml"; DestDir: "{userappdata}\Schumix\Configs\"; Flags: ignoreversion
Source: "..\Configs\GitRssAddon.yml"; DestDir: "{userappdata}\Schumix\Configs\"; Flags: ignoreversion
Source: "..\Configs\HgRssAddon.yml"; DestDir: "{userappdata}\Schumix\Configs\"; Flags: ignoreversion
Source: "..\Configs\MantisBTRssAddon.yml"; DestDir: "{userappdata}\Schumix\Configs\"; Flags: ignoreversion
Source: "..\Configs\SvnRssAddon.yml"; DestDir: "{userappdata}\Schumix\Configs\"; Flags: ignoreversion
Source: "..\Configs\WordPressRssAddon.yml"; DestDir: "{userappdata}\Schumix\Configs\"; Flags: ignoreversion
Source: "Schumix.yml"; DestDir: "{userappdata}\Schumix\Configs\"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Parameters: "--config-dir=$userappdata\Schumix\Configs\ --config-file=Schumix.yml"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Parameters: "--config-dir=$userappdata\Schumix\Configs\ --config-file=Schumix.yml"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}";  Parameters: "--config-dir=$userappdata\Schumix\Configs\ --config-file=Schumix.yml"; Description: "Schumix2 IRC Bot and Framework"; Flags: nowait postinstall skipifsilent
