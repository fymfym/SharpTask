# SharpTask
Yet another task scheduling system in c# for .net framwork 

This project is work in progress.

Intentions:
- Windows service
- Adding your own tasks as separate .DLL file in a directory (Inherited from SharpTaksTask)  
- The tasks is scheduled by implmenting the interface TaskTrigger list
- A number of working implementations of triggers supplied

Use case:
- Download and install the .MSI
  - This installs the service and gives you a SharpTaskTask.dll to include in your own project
- Make a VS Class librarey project, refference the the SharpTaskTask.dll
- Implement the SharpTaskInterface from SharpTaskTask.dll
- In the .Run method, implement your code to do whatever
- Make your own test console program or GUI test tools
- Compile the project
- Copy the .DLL file to the TaskLibrary of the service
- The service picks it up and runs the tasks at "trigger timer"
