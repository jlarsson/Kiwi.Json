properties {
	# Update this manually for each new version
	$pack_version = "2.1.1.0"

	$base_dir = resolve-path .
	$lib_dir = join-path $base_dir "pack\lib"
	$nuget_tool = join-path $base_dir ".nuget\nuget.exe"

	$pack_author = "Joakim Larsson"
	$pack_copyright = "Copyright © Joakim Larsson 2012"

	# specify projects 
	$json = @{
		id="Kiwi.Json"; 
		title="Kiwi.Json"; 
		description = "Json library for .NET";
		project_dir = (join-path $base_dir Kiwi.Json);
		project = (join-path $base_dir Kiwi.Json\Kiwi.Json.csproj);
		nuspec = (join-path $base_dir Kiwi.Json\Kiwi.Json.Nuspec);
	}
	$projects = ($json)

	# specify solutions to build
	$net40 = @{ sln = (join-path $base_dir "Kiwi.Json.sln"); out = (join-path $base_dir "pack\lib\net40") }
	$solutions = ($net40)
}

Framework("4.0")


task default -depends pack

task clean {
	remove-item -force -recurse $lib_dir -ErrorAction SilentlyContinue
}

task update-assembly-info -depends clean {
	$projects | foreach { (Generate-Assembly-Info $_) }
}

task build -depends update-assembly-info {
	$solutions | foreach { msbuild $_.sln /target:"Build" /verbosity:quiet /nologo /p:Platform="Any CPU" /p:Configuration="Release" /p:OutDir=$($_.out)\ }
}

task pack -depends build {
	$projects | foreach { `
		& $nuget_tool pack $_.nuspec -p "id=$($_.id);version=$pack_version;title=$($_.title);author=$pack_author;description=$($_.description);copyright=$pack_copyright;libdir=..\pack\lib"
	}
}


function Generate-Assembly-Info
{
	param($project)

	$asmInfo = "using System;
using System.Reflection;
using System.Runtime.InteropServices;
[assembly: CLSCompliantAttribute(true)]
[assembly: ComVisibleAttribute(false)]
[assembly: AssemblyTitleAttribute(""$($project.title)"")]
[assembly: AssemblyDescriptionAttribute(""$($project.description)"")]
[assembly: AssemblyCompanyAttribute(""$pack_author"")]
[assembly: AssemblyProductAttribute(""$($project.id) $version"")]
[assembly: AssemblyCopyrightAttribute(""$pack_copyright"")]
[assembly: AssemblyVersionAttribute(""$pack_version"")]
[assembly: AssemblyInformationalVersionAttribute(""$pack_version"")]
[assembly: AssemblyFileVersionAttribute(""$pack_version"")]
[assembly: AssemblyDelaySignAttribute(false)]
"

	$file = join-path $project.project_dir "Properties\assemblyinfo.cs"
	Write-Host "Generating assembly info file: $file"
	Write-Output $asmInfo > $file
}
