using System;
using System.Collections.Generic;

namespace Project.Domain
{
    public class GameState
    {
        public Ship ship { get; set; }
        public List<BuildingType> BuildingTypes { get; set; }
        public List<JobType> JobTypes { get; set; }
        public List<PersonalityType> PersonalityTypes { get; set; }
        public List<SocialQueue> SocialQueue { get; set; }

    }




}