using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Project.Domain
{
    public class ShowJobTypes
    {
        static ShowJobTypes()
        {

            string json = File.ReadAllText(@"data/JobTypes.json");
            JobCollection JobList = JsonConvert.DeserializeObject<JobCollection>(json);
            AllJobs = JobList.Jobtypes;
        }
        private static List<JobType> allJobs;

        public static List<JobType> AllJobs
        {
            get
            {
                return allJobs;
            }

            set
            {
                allJobs = value;
            }
        }

        public static List<JobType> GetJobs()
        {
            return AllJobs;
        }
    }
}
