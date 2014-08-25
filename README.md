CsharpFBP
===

C# Implementation of Flow-Based Programming (FBP)


General
---

In computer programming, flow-based programming (FBP) is a programming paradigm that defines applications as networks of "black box" processes, which exchange data across predefined connections by message passing, where the connections are specified externally to the processes. These black box processes can be reconnected endlessly to form different applications without having to be changed internally. FBP is thus naturally component-oriented.

FBP is a particular form of dataflow programming based on bounded buffers, information packets with defined lifetimes, named ports, and separate definition of connections.

Web sites for FBP: 
* http://www.jpaulmorrison.com/fbp/
* https://github.com/flowbased/flowbased.org/wiki

Prerequisites
---

Install Visual C# Express

Build FBP Project
---

Create empty `csharpfbp` directory in your local GitHub directory

Go up one level, and do a `git clone https://github.com/jpaulm/csharpfbp`

Now go into Visual C#, and `Open Project` `FBPLib/FBPLib.csproj` (in the just cloned directory)
This will create a "solution" called `FBPLib` and one "project" also called `FBPLib`

Go to the "solution" line, and rename to `FBP`.

Still on "solution" line, in this exact sequence,

Add/Existing Project `FBPVerbs/FBPVerbs.csproj`

Add/Existing Project `Concord/Concord.csproj`

The "solution" line should now say 3 projects

Now close Visual C#, and it will ask you if you want to save `FBP.sln`

Say `Yes`, and it will prompt you with a sugested location.  Remove `FBPLibs\` from the suggested file name, and hit `Save`.

Reopen Visual C#

Do `Build Solution`

If you only get warnings, you can proceed

Testing your Solution
---

Right click on `Concord`; Debug/Start new instance

When you see the screen with two panels, click on `Go`.  You should see a concordance being built using "artificial" Russian text (a so-called "lorem ipsum").  Source text will be on the left, and the concordance is on the right.

When you are finished, hit Exit.

Here is the output of Concord:

![ConcordOutput](https://github.com/jpaulm/csharpfbp/blob/master/docs/ConcordOutput.png "Output of Concordance")

Adding some batch examples
---

Open Visual C#

Go to "solution" line

Add/Existing Project `TestNetworks/MergeAndSort/MergeAndSort.csproj` 

Solution should now show "4 projects"

Right click on `MergeAndSort` in Solution Explorer; Debug/Start new instance



