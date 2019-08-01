using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain
{
    //job types
    public class JobType
    {
        public int idkey { get; set; }
        public int building_id { get; set; }
        public int job_id { get; set; }
        public int qty { get; set; }
        public string name { get; set; }

        public JobType GetJobType(int jobId)
        {
            JobType job = new JobType();
            foreach (var j in ShowJobTypes.GetJobs())
            {
                if(j.idkey==jobId){
                    return j;
                }
            }
            return job;
        }
        public BuildingType GetBuilding(int buildingId)
        {
            BuildingType building = new BuildingType();
            List <BuildingType> BuildingList = ShowBuildingTypes.GetBuildings();
            if (BuildingList != null)
            {
                for (int s = 0; s < BuildingList.Count; s++)
                {
                    if(buildingId==BuildingList[s].idkey)
                    {
                        building = BuildingList[s];
                    }
                }
            }
            return building;
        }

    }
}
