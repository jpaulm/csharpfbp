CsharpFBP
===

### C# Implementation of Flow-Based Programming (FBP)

General web site on Flow-Based Programming: https://jpaulm.github.io/fbp/ .


General
---

In computer programming, flow-based programming (FBP) is a programming paradigm that defines applications as networks of "black box" processes, which exchange data across predefined connections by message passing, where the connections are specified externally to the processes. These black box processes can be reconnected endlessly to form different applications without having to be changed internally. FBP is thus naturally component-oriented.

FBP is a particular form of dataflow programming based on bounded buffers, information packets with defined lifetimes, named ports, and separate definition of connections.

Other web sites for FBP: 
* https://github.com/flowbased/flowbased.org/wiki
 
C#FBP Syntax and Component API:
* http://www.jpaulmorrison.com/fbp/csyntax.htm

Prerequisites
---

Install Microsoft Visual Studio 

Build FBP Project
---

Create empty `csharpfbp` directory in your local GitHub directory

Do `git clone https://github.com/jpaulm/csharpfbp`

Now go into Visual C#, and `Open Project` `CsharpFBP.sln` (in the just cloned directory)

Go into Solution Explorer, where you will see a number of "projects" - two of which are `FBPLib` and `FBPVerbs`.  The .dlls for these will be needed when testing your application, and these two projects should be added as references.

Compile Commands
---

You will find a number of C# compile commands in [Commands](commands.md)

"Concord" (forms-based Windows application)
---

Concord is a network that creates a "concordance" from arbitrary text: a list of each word, surrounded by a number of the actual characters surrounding it - then with the words sorted in alphabetic sequence.  Here is the diagram for Concord, created using DrawFBP (https://github.com/jpaulm/drawfbp ):

![ConcordDiagram](https://github.com/jpaulm/csharpfbp/blob/master/docs/ConcordVS.png "Diagram for Concord")

Testing "Concord"
-----

Right click on `Concord`; Debug/Start new instance

When you see the screen with two panels, click on `Go`.  You should see a concordance being built using "artificial" Russian text (a so-called "lorem ipsum").  Source text will be on the left, and the concordance is on the right.

When you are finished, hit Exit.

Here is the output of Concord:

![ConcordOutput](https://github.com/jpaulm/csharpfbp/blob/master/docs/ConcordOutput.png "Output of Concordance")


Testing "MergeAndSort" (console application)
---

Right click on `MergeAndSort` in Solution Explorer; Debug/Start new instance

This network is an implementation in C#FBP of the one illustrated in https://github.com/jpaulm/javafbp/blob/master/README.md

Other Tests
------

"FBPTest" (forms-based Windows application)

A number of console application networks have been grouped under "FBPTest". Right-clicking on `FBPTest` and selecting Debug will bring up a panel containing over a dozen buttons, each of which will trigger a C#FBP network.

At the end of each run, you should see something like:

    Run complete.  Time: x.xxx seconds
    Counts: C: xx, D: xx, S: xx, R (non-null): xx, DO: xx
    
where the counts (`xx`) are respectively: creates, normal drops, sends, non-null receives, and drops done by "drop oldest".   

Warning!
-----
Care must be taken if combining `LoadBalance` (with substreams) and `SubstreamSensitiveMerge` in a divergent-convergent pattern - this pattern is one of the warning signals for deadlocks anyway. The network `TestLoadBalanceWithSubstreams` is an example. The problem is described in more detail under https://github.com/jpaulm/javafbp/issues/8.

Tracing
-------

To enable tracing, right click on the `FBPLib` project, then go into Settings - you will see two boolean variables: `Tracing` and `DeadlockTestEnabled` - set them as desired.

If enabled, the trace will be found in `C:\Temp\xxxx-fulltrace.txt`, where `xxxx` is the name of the test being run.  Subnets have their own files, with a qualified name, e.g. `TestInfQueue.InfiniteQueue-fulltrace.txt`.

