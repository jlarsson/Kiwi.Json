properties {
	$base_dir = resolve-path .
	$lib_dir = join-path $base_dir pack\lib

	$nuget_tool = join-path $base_dir ".nuget\nuget.exe"

	# specify projects 
	$json = @{
		id="Kiwi.Json"; title="Kiwi.Json"; description = "Json library for .NET";
		project_dir = (join-path $base_dir Kiwi.Json)
	}
	$projects = ($json)

	# specify solutions to build
	$net40 = @{ sln = (join-path $base_dir "Kiwi.Json.sln"); out = (join-path $base_dir "pack\\lib\\net40") }
	$solutions = ($net40)
}

task default -depends pack

task clean {
	remove-item -force -recurse $lib_dir -ErrorAction SilentlyContinue
}

task update-assembly-info -depends clean {
	$projects | foreach { (Generate-Assembly-Info $_) }
}

task build -depends update-assembly-info {
	$solutions | foreach { `
		$configuration = "Release"
		$platform = "Any CPU"
		$properties = "OutDir=$out;Configuration=$configuration;Platform=$platform"
		msbuild $_.sln /target:Build /nologo /verbosity:quiet /p:$properties 
	}
}

task pack -depends build {
<#
	Ensure-Directory $pack_dir
	$projects | foreach { `
		$p = $_
		$copy_folders = ("tools","content")
		$copy_folders | foreach { Copy-Item  -path  (join-path $p.project_dir $_) -destination $build_dir -force  -recurse -ErrorAction SilentlyContinue }
		$props = "id=$($p.id);version=$($p.version);title=$($p.title);author=$($p.author);description=$($p.description);copyright=$($p.copyright);root=$($build_dir);"
		($nuget_tool pack $p.nuspec_file `
			-p $props `
			-basePath $build_dir `
			-o $pack_dir) `
	}
#>
}







function Generate-Assembly-Info
{
	param($project)
	$version = (git describe --tags --abbrev=0).Replace("v", "")
	$commit = (git log --oneline -1).Split(' ')[0]

	$asmInfo = "using System;
using System.Reflection;
using System.Runtime.InteropServices;
[assembly: CLSCompliantAttribute(true)]
[assembly: ComVisibleAttribute(false)]
[assembly: AssemblyTitleAttribute(""$($project.title)"")]
[assembly: AssemblyDescriptionAttribute(""$($project.description)"")]
[assembly: AssemblyCompanyAttribute(""Joakim Larsson"")]
[assembly: AssemblyProductAttribute(""$($project.id) $($project.version)"")]
[assembly: AssemblyCopyrightAttribute(""Copyright © Joakim Larsson 2012"")]
[assembly: AssemblyVersionAttribute(""$version"")]
[assembly: AssemblyInformationalVersionAttribute(""$version / $commit"")]
[assembly: AssemblyFileVersionAttribute(""$version"")]
[assembly: AssemblyDelaySignAttribute(false)]
"

	$file = join-path $project.project_dir "Properties\assemblyinfo.cs"
	Ensure-File-Directory $file
	Write-Host "Generating assembly info file: $file"
	#Write-Output $asmInfo > $file
}
