using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Model.BaseData
{
    public class MESUser
    {
        public Guid USER_ID { get; set; }
        public Guid PERSON_ID { get; set; }
        public string USER_NAME { get; set; }
        public string PERSON_NAME { get; set; }
        public string PASSWORD { get; set; }
        public bool CHANGE_PASSWORD { get; set; }

        public List<MESRole> MESRole { get; set; }
    }
}
