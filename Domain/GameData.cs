using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain
{
    public class JobCollection
    {
        private List<JobType> jobtypes;

        public List<JobType> Jobtypes
        {
            get
            {
                return jobtypes;
            }

            set
            {
                jobtypes = value;
            }
        }
    }
    public class BuildingCollection
    {

        private List<BuildingType> buildingtypes;

        public List<BuildingType> Buildingtypes
        {
            get
            {
                return buildingtypes;
            }

            set
            {
                buildingtypes = value;
            }
        }
    }
    public class PersonalityCollection
    {

        private List<PersonalityType> personalitytypes;

        public List<PersonalityType> Personalitytypes
        {
            get
            {
                return personalitytypes;
            }

            set
            {
                personalitytypes = value;
            }
        }
    }
    public class CharacterNamesCollection
    {

        private List<CharacterNames> names;

        public List<CharacterNames> Names
        {
            get
            {
                return names;
            }

            set
            {
                names = value;
            }
        }
    }
}