using System.Collections.Generic;

namespace gmc_api.Base.InterFace
{
    public interface IReponsitoriesFireBase<T>
    {
        T Get(T record);
        List<T> GetAll();
        T Add(T record);
        bool Update(T record);
        bool Delete(T record);
    }
}
