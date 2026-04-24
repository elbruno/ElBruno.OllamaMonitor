param(
    [Parameter(Mandatory = $true)]
    [string]$PackagePath,

    [Parameter(Mandatory = $true)]
    [string]$DesktopPublishDir,

    [Parameter(Mandatory = $true)]
    [string]$ToolTargetFramework
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

if (-not (Test-Path -LiteralPath $PackagePath)) {
    throw "Package path not found: $PackagePath"
}

if (-not (Test-Path -LiteralPath $DesktopPublishDir)) {
    throw "Desktop publish directory not found: $DesktopPublishDir"
}

Add-Type -AssemblyName System.IO.Compression
Add-Type -AssemblyName System.IO.Compression.FileSystem

$desktopRoot = "tools/$ToolTargetFramework/any/desktop/"
$archive = $null

for ($attempt = 1; $attempt -le 10 -and $null -eq $archive; $attempt++) {
    try {
        $archive = [System.IO.Compression.ZipFile]::Open($PackagePath, [System.IO.Compression.ZipArchiveMode]::Update)
    }
    catch [System.IO.IOException] {
        if ($attempt -eq 10) {
            throw
        }

        Start-Sleep -Milliseconds 500
    }
}

try {
    $existingEntries = @($archive.Entries | Where-Object { $_.FullName.StartsWith($desktopRoot, [System.StringComparison]::OrdinalIgnoreCase) })
    foreach ($entry in $existingEntries) {
        $entry.Delete()
    }

    $desktopRootPath = (Resolve-Path -LiteralPath $DesktopPublishDir).Path.TrimEnd('\')

    Get-ChildItem -LiteralPath $desktopRootPath -Recurse -File | ForEach-Object {
        $relativePath = $_.FullName.Substring($desktopRootPath.Length).TrimStart('\').Replace('\', '/')
        $entryPath = "$desktopRoot$relativePath"
        [System.IO.Compression.ZipFileExtensions]::CreateEntryFromFile($archive, $_.FullName, $entryPath, [System.IO.Compression.CompressionLevel]::Optimal) | Out-Null
    }
}
finally {
    $archive.Dispose()
}
