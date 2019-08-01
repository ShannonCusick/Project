using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain
{
    [Serializable]
    public class Result : IDataObject
    {
        public string Name { get; set; }
        public bool Ok { get; set; }
        public object Data { get; set; }
        public string Msg { get; set; }
        public int Id { get; set; }
        public string _DataString { get; set; }
        public string _DataType { get; set; }

        public Result(String s, bool ok = false)
        {
            Ok = ok;
            if (!string.IsNullOrEmpty(s) && ok == false)
                Name = "OnError";
            Msg = s;
        }

        virtual public object GetExtraData()
        {
            return null;
        }
        virtual public void SetExtraData(object obj)
        {
        }

        public Result()
        {
            Msg = "";
        }

    }
}
