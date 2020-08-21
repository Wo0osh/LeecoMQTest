using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;
using System.Security.Policy;

namespace LeecoMQTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //string path = @"vleecoreports.leecosteel.com:1801\GPin";
            string path = @"vleecoreports.leecosteel.com:\GPin";
            //string path = @"vleecoreports.leecosteel.com:1801\GPout";
            //string path = @"vleecoreports.leecosteel.com\GPin";
            //tim was here

            Console.WriteLine("Enter path or leave blank for default:");
            Console.WriteLine("<Default> " + path);
            Console.WriteLine(path);
            var str = Console.ReadLine();
            if (!string.IsNullOrEmpty(str))
                path = str;
            MessageQueueActor mqa = new MessageQueueActor();
            List<string> results = mqa.ReadQueue(path);
            foreach (string s in results)
            {
                Console.WriteLine(s);
            }

            List<string> privateQueues = mqa.GetPrivateQueues(path);
            foreach (string s in privateQueues)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("<Any key to end>");
            Console.ReadKey();
        }
    }

    class MessageQueueActor
    {
        private string url = "";
        private string queueName = "";
        public MessageQueueActor()
        {
            //url = @"vleecoreports.leecosteel.com\Private$\GP";
            //queueName = "GP";
        }

        public List<string> ReadQueue(string path)

        {

            List<string> lstMessages = new List<string>();

            using(MessageQueue messageQueue = new MessageQueue(path))
            {
                var t = MessageQueue.GetPublicQueues();
                Console.WriteLine("Found: " + t.Length.ToString() + " public queues.");
                //t = MessageQueue.GetPublicQueuesByMachine("vleecoreports.leecosteel.com");
                //var x = MessageQueue.GetMachineId("vleecoreports");
                //t = MessageQueue.GetPrivateQueuesByMachine("vleecoreports.leecosteel.com");
                if (MessageQueue.Exists(path))
                {
                    System.Messaging.Message[] messages = messageQueue.GetAllMessages();

                    foreach (System.Messaging.Message message in messages)

                    {

                        message.Formatter = new XmlMessageFormatter(

                        new String[] { "System.String, mscorlib" });

                        string msg = message.Body.ToString();

                        lstMessages.Add(msg);

                    }
                }
            }

            return lstMessages;

        }

        public List<string> GetPrivateQueues(string path)
        {
            // Get a list of queues with the specified category.
            MessageQueue[] QueueList =
                MessageQueue.GetPrivateQueuesByMachine(path);

            var results = new List<string>();

            // Display the paths of the queues in the list.
            foreach (MessageQueue queueItem in QueueList)
            {
                results.Add(queueItem.Path);
            }

            return results;
        }
    }
}
