using gmc_api.Base.dto;
using System;
using System.Collections.Generic;

namespace gmc_api.Base.InterFace
{
    public interface IRepositoriesBase<T, P>
    {
        IEnumerable<T> GetAll(string condition);
        IEnumerable<T> GetObjectByCondition(SortedDictionary<string, Object> mapProp);
        IEnumerable<P> GetPagingData(string conditionAppend, string orderByCondiion, Paging paging);
        T GetObjectById(int id);
        T AddObject(T obj);
        int UpdateObject(SortedDictionary<string, Object> mapProp);
        int DeleteObject(int id);
    }
}
