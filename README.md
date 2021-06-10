# BMC-Scheduler-Task-Service
BMC Home Assignment - Scheduler Task Service

Mini Scheduler

In the following exercise you are asked to write SchedulerTaskService which read tasks from file and run them according to their details and prerequisites.
A SchedulerTask contains the following data:
a.	*ID: [unique number]
b.	*Name: [task name]
c.	*Type: [task type] - Task Type can be one of: [Command, Script, REST Call]. 
d.	Details – [if task type is Script then the script path should be provided. If the type is Command the command text should be provided]
e.	Time – [format time: hh:mm  execution time – its optional and can be empty only if there is a value in BasedOn field]
f.	BasedOn – [A condition, holds task ID which is a prerequisite. Its optional and can be empty only if there is a value in Time field]
* for required attributes
The attached ‘tasks.xml’ file contains definitions for tasks. Please write a Scheduler Service in Java which executes the following:
1.	Reads the SchedulerTask elements from XML\JSON file.
[ Input data Example in XML:
  <SchedulerTask id=”1” Name=”Print Command” Type=”Command” Details=”echo hi there” Time=”14:30”    
  BasedOn=””/>
  <SchedulerTask id=”2” Name=”Run Script” Type=”Script” Details=”c:\example.bat”, BasedOn=”1”/> ]

2.	Runs all the tasks with the following limitations:
a.	Tasks can run in parallel.
For example, if Task A and Task B are both ready to run on 13:30, the should run in parallel and NOT one after the other.
b.	Every task is ready to run only if his prerequisite is done (if exists) and the task time has arrived.
For Example:
If the time now is 13:07 and the task time is 13:00 then the task should be run if there is no prerequisite task OR his prerequisite task is done.
If the prerequisite task is not done, the task should be waiting and once his prerequisite task is done, the Task should be run.
c.	On the first running, the scheduler should also execute every task that his time has already passed today (if his prerequisite is already done). 
d.	Please write message to console/log when task is waiting, running or finished.
For example: “Task 1 started”, “Task 4 is waiting for Task 3 to be done”…

Good luck!!
