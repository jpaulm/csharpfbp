To compile FBPLib, position current directory to `GitHub\csharpfbp\FBPLib`:

`csc -t:library -out:bin/Debug/FBPLib.dll -recurse:*.cs`

To compile FBPVerbs, position current directory to `GitHub\csharpfbp\FBPVerbs`:

`csc -t:library -r:c:\users\paul\documents\Github\csharpfbp\FBPLib/bin/Debug/FBPLib.dll -out:bin/Debug/FBPVerbs.dll Properties/*.cs FBPComponents/*.cs`
 
To compile Concord, position current directory to `GitHub\csharpfbp`:

`csc -t:exe -r:FBPLib\bin\Debug/FBPLib.dll -r:FBPVerbs\bin\Debug/FBPVerbs.dll -out:TestCases\Concord/bin/Debug/Concord.exe TestCases\Concord\Properties\*.cs TestCases\Concord\*.cs`
