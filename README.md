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

Install Visual C# 2010 Express
Create a "solution" in your project, e.g. `FBP`.
Open Visual C# 2010 Express
Add Existing Item `Concord.csproj`
Add Existing Item `FBPlib.csproj`
Add Existing Item `FBPVerbs.csproj`
Build 

Testing your download
---

Run `Concord`.  When you see the screen with two panels, click on `Go`.  You should see a concordance being built using "artificial" Russian text.

