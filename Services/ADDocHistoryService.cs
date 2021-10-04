using AutoMapper;
using gmc_api.Base;
using gmc_api.Base.InterFace;
using gmc_api.DTO.CommonData;
using gmc_api.Entities;
using gmc_api.Repositories;

namespace gmc_api.Services
{
    public interface IADDocHistoryService : IServiceGMCBase<ADDocHistorys, ADDocHistoryInfo, ADDocHistorys, ADDocHistorys>
    {
    }

    public class ADDocHistoryService : ServiceBaseImpl<ADDocHistorys, ADDocHistoryInfo, ADDocHistorys, ADDocHistorys>, IADDocHistoryService
    {
        private readonly IADDocHistoryReponsitory _repository;
        private readonly IMapper _mapper;

        public ADDocHistoryService(IADDocHistoryReponsitory repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

    }
}
