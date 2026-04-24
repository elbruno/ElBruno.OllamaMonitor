param(
    [string]$Configuration = 'Release',
    [string]$Version = ''
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
$toolProject = Join-Path $repoRoot 'src\ElBruno.OllamaMonitor.Tool\ElBruno.OllamaMonitor.Tool.csproj'
$desktopProject = Join-Path $repoRoot 'src\ElBruno.OllamaMonitor\ElBruno.OllamaMonitor.csproj'
$desktopPublishDir = Join-Path $repoRoot 'src\ElBruno.OllamaMonitor\obj\desktop-publish\'
$packageDirectory = Join-Path $repoRoot 'artifacts\packages'
$versionArgs = @()

if (-not [string]::IsNullOrWhiteSpace($Version)) {
    $versionArgs += "-p:Version=$Version"
}

dotnet publish $desktopProject -c $Configuration -f net10.0-windows10.0.19041.0 -r win-x64 --self-contained false -p:PublishDir="$desktopPublishDir" @versionArgs
dotnet pack $toolProject -c $Configuration @versionArgs

$packagePath = Get-ChildItem -LiteralPath $packageDirectory -Filter 'ElBruno.OllamaMonitor.*.nupkg' |
    Sort-Object LastWriteTimeUtc -Descending |
    Select-Object -First 1 -ExpandProperty FullName

& (Join-Path $PSScriptRoot 'Inject-DesktopPayload.ps1') -PackagePath $packagePath -DesktopPublishDir $desktopPublishDir -ToolTargetFramework 'net10.0'

Write-Host "Packaged tool: $packagePath"
