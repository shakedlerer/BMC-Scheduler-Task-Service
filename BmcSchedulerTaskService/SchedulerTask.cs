using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BmcSchedulerTaskService
{
    class SchedulerTask
    {
        private int taskID;
        private string taskName;
        private string taskType;
        private string taskDetails;
        private string taskTime;
        private int taskBasedOn;
        private bool taskDone;
        public SchedulerTask(int taskID, string taskName, string taskType, string taskDetails, string taskTime, int taskBasedOn)
        {
            this.taskID = taskID;
            this.taskName = taskName;
            this.taskType = taskType;
            this.taskDetails = taskDetails;
            this.taskTime = taskTime;
            this.taskBasedOn = taskBasedOn;
            this.taskDone = false;
        }
        public void tryToExecuteTask(SchedulerTask currentTask, Dictionary<int, SchedulerTask> taskDict)
        {
            if (!currentTask.isTaskDone())
            {
                bool noPrerequisite = false;
                bool prerequisiteDone = false;
                bool taskTimeArrived = false;
                bool taskHasNoTime = false;

                int currentTime = getCurrentTime();
                string taskTime = currentTask.getTime();
                int prerequisiteID = currentTask.getBasedOn();

                if (prerequisiteID == -1)
                    noPrerequisite = true;
                else if (checkIfPrerequisiteDone(prerequisiteID, taskDict))
                    prerequisiteDone = true;

                if (convertStringTimeToInt(taskTime) == -1)
                    taskHasNoTime = true;
                else if (checkIfTaskTimeArrived(currentTime, taskTime))
                    taskTimeArrived = true;

                if (taskTimeArrived)
                {
                    if (noPrerequisite || prerequisiteDone)
                        currentTask.executeTask(null);
                }
                else if (prerequisiteDone && taskHasNoTime)
                    currentTask.executeTask(taskDict);
            }
        }
        static public int convertStringTimeToInt(string time)
        {
            if (time == null)
                return -1;
            time = time.Replace(":", "");
            return Convert.ToInt32(time);
        }
        static bool checkIfPrerequisiteDone(int prerequisiteID, Dictionary<int, SchedulerTask> taskDict)
        {
            SchedulerTask prerequisiteTask = taskDict[prerequisiteID];
            return prerequisiteTask.isTaskDone();
        }
        static bool checkIfTaskTimeArrived(int currentTime, string taskTime)
        {
            return (convertStringTimeToInt(taskTime) <= currentTime);
        }

        static int getCurrentTime()
        {
            return ((DateTime.Now.Hour * 100) + DateTime.Now.Minute);
        }
        public void executeTask(Dictionary<int, SchedulerTask> taskDict)
        {
            Console.WriteLine("Task {0} started", this.getID());
            if (taskDict != null)
            {
                foreach (KeyValuePair<int, SchedulerTask> entry in taskDict)
                {
                    if (entry.Value.getBasedOn() == this.getID())
                        Console.WriteLine("Task {0} is waiting for Task {1} to be done", entry.Value.getID(), this.getID());
                }
            }
            Thread.Sleep(2000);
            Console.WriteLine(this.getName() + " " + this.getDetails());
            finishTask();
            Console.WriteLine("Task {0} finished", this.getID());
        }
        public bool isTaskDone()
        {
            return this.taskDone;
        }
        public void finishTask()
        {
            this.taskDone = true;
        }
        public int getID()
        {
            return this.taskID;
        }
        public string getName()
        {
            return this.taskName;
        }
        public string getType()
        {
            return this.taskType;
        }
        public string getDetails()
        {
            return this.taskDetails;
        }
        public string getTime()
        {
            return this.taskTime;
        }
        public int getBasedOn()
        {
            return this.taskBasedOn;
        }
    }
}
