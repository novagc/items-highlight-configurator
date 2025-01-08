$configuration = $args[0]
if ($configuration -eq $null) {
    $configuration = "Release"
}

$manifest = (cat "../Thunderstore/manifest.json")
$manifestObject = $manifest | ConvertFrom-Json

if(![System.IO.Directory]::Exists("../$($manifestObject.name)/bin/$configuration/netstandard2.1")){
    return;
}

if(![System.IO.Directory]::Exists("../public")){
    mkdir "../public" > $null;
}

rm -r -Force "../public/*"
mkdir "../public/plugins/$($manifestObject.author)-$($manifestObject.name)" > $null

cp ../Thunderstore/* ../public
cp ../README.md ../public
cp ../CHANGELOG.md ../public
cp -Recurse "../$($manifestObject.name)/bin/$configuration/netstandard2.1/*" "../public/plugins/$($manifestObject.author)-$($manifestObject.name)"

rm "../public/plugins/$($manifestObject.author)-$($manifestObject.name)/*.deps.json"
$compressConfig = @{
    Path = "../public/*"
    CompressionLevel = "Fastest"
    DestinationPath = "../public/$($manifestObject.author)-$($manifestObject.name)-$($manifestObject.version_number).zip"
}
Compress-Archive @compressConfig