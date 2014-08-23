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


The project requires Gradle for building (tested with version 2.0). You can download the corresponding package from the following URL: 
http://www.gradle.org

Windows and Linux users should follow the installation instructions on the Maven website (URL provided above).

OSX users (using Brew, http://brew.sh) can install Maven by executing the following command:

    brew install gradle


Building from command line
---

For building the project simply run the following command:

    gradle build

As a result a `csharpfbp-x.x.jar` file will be created in the `build/libs` directory. It will include the CsharpFBP core (runtime) and all the components from the source code. 

Testing your download
---

Run `Concord`.  When you see the screen with two panels, click on `Go`.  You should see a concordance being built using "artificial" Russian text.

