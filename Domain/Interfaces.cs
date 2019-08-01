using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain
{
    public delegate void VoidDelegate();
    public delegate void ObjectDelegate(Object obj);
    public delegate void StringDelegate(String s);
    public delegate void GameStateObjectDelegate(Object obj);

    public delegate void ClientResultCallback(IList<Result> results);
    public delegate void ClientStringCallback(string result, string uri, Exception e
        , ClientResultCallback resultCallback);


    public interface IDataObject
    {
        object Data { get; set; }
        string _DataString { get; set; }
        string _DataType { get; set; }
    }

    public interface IId
    {
        int idkey { get; set; }
    }

    public interface IName
    {
        string name { get; set; }
    }

    public interface IIndexedGameItem : IId, IName
    {
    }

}
