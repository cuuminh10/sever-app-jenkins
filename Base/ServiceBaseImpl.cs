using AutoMapper;
using gmc_api.Base.dto;
using gmc_api.Base.InterFace;
using System.Collections.Generic;
using System.Linq;

namespace gmc_api.Base
{
    public class ServiceBaseImpl<T, U, V, P> : IServiceGMCBase<T, U, V, P>
    {
        private readonly IRepositoriesBase<V, P> _repository;
        private readonly IMapper _mapper;

        public ServiceBaseImpl(IRepositoriesBase<V, P> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public T CreateObject(U model, UserLoginInfo info)
        {
            V dbObject = _mapper.Map<V>(model);
            if (dbObject.GetType().IsSubclassOf(typeof(FixFiveProps)))
            {
                dbObject.GetType().GetProperty("AACreatedUser").SetValue(dbObject, info.UserName);
                dbObject.GetType().GetProperty("AAUpdatedUser").SetValue(dbObject, info.UserName);
            }
            return _mapper.Map<T>(_repository.AddObject(dbObject));
        }

        public int CreateObjectNotReponse(U model, UserLoginInfo info)
        {
            V dbObject = _mapper.Map<V>(model);
            if (dbObject.GetType().IsSubclassOf(typeof(FixFiveProps)))
            {
                dbObject.GetType().GetProperty("AACreatedUser").SetValue(dbObject, info.UserName);
                dbObject.GetType().GetProperty("AAUpdatedUser").SetValue(dbObject, info.UserName);
            }
            _ = _repository.AddObject(dbObject);
            return 1;
        }

        public int DeleteObject(int id)
        {
            return _repository.DeleteObject(id);
        }

        public IEnumerable<T> GetAll(string conditionAppend = "")
        {
            return _repository.GetAll(conditionAppend).Select(a => _mapper.Map<T>(a)).ToList();
        }
        public IEnumerable<U> GetAllCreateMapper(string conditionAppend = "")
        {
            return _repository.GetAll(conditionAppend).Select(a => _mapper.Map<U>(a)).ToList();
        }
        public T GetObject(int Id)
        {
            V user = _repository.GetObjectById(Id);
            return _mapper.Map<T>(user);
        }

        public U GetObjectCreateMapper(int Id)
        {
            V user = _repository.GetObjectById(Id);
            return _mapper.Map<U>(user);
        }

        public V GetObjectEntityMapper(int Id)
        {
            V user = _repository.GetObjectById(Id);
            return user;
        }

        public IEnumerable<V> GetAllObjectEntityMapper(string conditionAppend = "")
        {
            return _repository.GetAll(conditionAppend).ToList();
        }

        public IEnumerable<T> GetPagingData(string conditionAppend, string orderByCondiion, Paging paging)
        {
            return _repository.GetPagingData(conditionAppend, orderByCondiion, paging).Select(a => _mapper.Map<T>(a)).ToList();
        }

        public int UpdateObject(SortedDictionary<string, object> mapProp)
        {
            return _repository.UpdateObject(mapProp);
        }
    }
}
