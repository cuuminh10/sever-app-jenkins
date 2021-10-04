using gmc_api.Base.dto;
using System;
using System.Collections.Generic;

namespace gmc_api.Base.InterFace
{
    public interface IServiceGMCBase<T, U, V, P>
    {
        IEnumerable<T> GetAll(string conditions = "");
        IEnumerable<T> GetPagingData(string conditionAppend, string orderByCondiion, Paging paging);
        public IEnumerable<U> GetAllCreateMapper(string conditionAppend = "");
        public IEnumerable<V> GetAllObjectEntityMapper(string conditionAppend = "");
        T GetObject(int Id);
        public U GetObjectCreateMapper(int Id);
        public V GetObjectEntityMapper(int Id);
        T CreateObject(U model, UserLoginInfo info = null);
        int CreateObjectNotReponse(U model, UserLoginInfo info = null);
        int UpdateObject(SortedDictionary<string, Object> mapProp);
        int DeleteObject(int id);
    }
}
