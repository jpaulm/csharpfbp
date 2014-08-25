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

Now go into Visual C#, and `Open Project` `Concord.csproj` (in the just cloned directory)
This will create a "solution" called `Concord` and one "project" also called `Concord`
Go to the "solution" line, and rename to `FBP`.

Still on "solution" line,
Add/Existing Project `FBPlib.csproj`
Add/Existing Project `FBPVerbs.csproj`

The "solution" line should now say 3 projects
Now close Visual C#, and it will ask you if you want to save `FBP.sln`
Say `Yes`, and it will prompt you with a sugested location.  Remove `Concord` from the suggested file name, and hit `Save`.

Go back into Visual C# 
Right click on `Concord`, click on Add Reference..., select both FBPLib and FBPVerbs, hit OK
Right click on `FBPVerbs`, click on Add Reference..., select FBPLib, hit OK

Do `Build Solution`

Testing your Solution
---

Click on `Concord`; Debug/Start new instance

When you see the screen with two panels, click on `Go`.  You should see a concordance being built using "artificial" Russian text (a so-called "lorem ipsum").

