using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace BmcSchedulerTaskService
{
    class Program
    {
        static public void parseTasksAndInsertToDB(XmlNodeList taskList, Dictionary<int, SchedulerTask> taskDict)
        {
            for (int i = 0; i < taskList.Count; i++)
            {
                int taskID = -1, taskBasedOn = -1; //base values - if no task based on then put a null value
                string taskName = null, taskType = null, taskDetails = null, taskTime = null; //same as above but for task time
                string currentAttribute;
                for (int j = 0; j < taskList[i].Attributes.Count; j++)
                {
                    currentAttribute = taskList[i].Attributes[j].Name;

                    if (currentAttribute == "id")
                        taskID = Convert.ToInt32(taskList[i].Attributes[j].InnerText);
                    else if (currentAttribute == "Name")
                        taskName = taskList[i].Attributes[j].InnerText.ToString();
                    else if (currentAttribute == "Type")
                        taskType = taskList[i].Attributes[j].InnerText.ToString();
                    else if (currentAttribute == "Details")
                        taskDetails = taskList[i].Attributes[j].InnerText.ToString();
                    else if (currentAttribute == "Time")
                        taskTime = taskList[i].Attributes[j].InnerText.ToString();
                    else if (currentAttribute == "BasedOn" && taskList[i].Attributes[j].InnerText != "")
                        taskBasedOn = Convert.ToInt32(taskList[i].Attributes[j].InnerText);
                }
                SchedulerTask newTask = new SchedulerTask(taskID, taskName, taskType, taskDetails, taskTime, taskBasedOn);
                taskDict.Add(taskID, newTask);
            }
        }

        static public XmlNodeList extractDataFromXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("C:\\tasks.xml");
            return doc.SelectNodes("/catalog/SchedulerTask");
        }

        static void runTaskScheduler(Dictionary<int, SchedulerTask> taskDict)
        {
            while (true)
            {
                Parallel.ForEach(taskDict, entry =>
                {
                    entry.Value.tryToExecuteTask(entry.Value, taskDict);
                });
            }
        }

        static void Main(string[] args)
        {
            Dictionary<int, SchedulerTask> taskDict = new Dictionary<int, SchedulerTask>();
            XmlNodeList taskList = extractDataFromXML();
            parseTasksAndInsertToDB(taskList, taskDict);
            runTaskScheduler(taskDict);
        }
    }
}
