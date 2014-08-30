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
 
C#FBP Syntax and Component API:
* http://www.jpaulmorrison.com/fbp/csyntax.htm

Prerequisites
---

Install Visual C# Express

Build FBP Project
---

Create empty `csharpfbp` directory in your local GitHub directory

Do `git clone https://github.com/jpaulm/csharpfbp`

Now go into Visual C#, and `Open Project` `FBP.sln` (in the just cloned directory)

There will be a "solution" line, followed by a number of "projects" - two of which are `FBPLib` and `FBPVerbs`.

Right click on the "solution" line, and do `Build Solution`

If you only get warnings, you can proceed

Testing "Concord" (forms-based application)
---

Right click on `Concord`; Debug/Start new instance

When you see the screen with two panels, click on `Go`.  You should see a concordance being built using "artificial" Russian text (a so-called "lorem ipsum").  Source text will be on the left, and the concordance is on the right.

When you are finished, hit Exit.

Here is the output of Concord:

![ConcordOutput](https://github.com/jpaulm/csharpfbp/blob/master/docs/ConcordOutput.png "Output of Concordance")


Testing "MergeAndSort" (console application)
---

Right click on `MergeAndSort` in Solution Explorer; Debug/Start new instance

This network is an implementation in C#FBP of the one illustrated in https://github.com/jpaulm/javafbp/blob/master/README.md



