# SharpTask
Yet another task scheduling system in c# for .net framwork 

All in this list ois not yet finished, this work is not even beta ready

Intentions:
- Windows service running in the background
- Adding your own tasks as separate .DLL via filecopy (Inherited from SharpTaksTask)  
- The tasks are scheduled by implmenting the interface TaskTrigger list
- A number of work implementations of triggers supplied

Use case:
- Download and install the .MSI
  - This installs the service and gives you a SharpTaskTask.dll to include in your own project
- Make a VS Class librarey project,refference the the SharpTaskTask.dll
- Implement the SharpTaskInterface from SharpTaskTask.dll
- In the .Run method, implement your code to do whater
- Make your own test console program or GUI test, whatever
- Compile the project
- Copy the .DLL file to the TaskLibrary of the service
- The service picks it up and runs the tasks at "trigger timer"
