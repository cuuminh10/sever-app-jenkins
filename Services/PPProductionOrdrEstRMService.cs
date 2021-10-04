using AutoMapper;
using gmc_api.DTO.PP;
using gmc_api.Entities;
using gmc_api.Repositories;
using gmc_api.Base.InterFace;
using gmc_api.Base;
using gmc_api.Base.dto;

namespace gmc_api.Services
{
    public interface IPPProductionOrdrEstRMService : IServiceGMCBase<DtoGMCBase, PPProductionOrdrEstRMCreate, PPProductionOrdrEstRMs, PPProductionOrdrEstRMs>
    {

    }

    public class PPProductionOrdrEstRMService : ServiceBaseImpl<DtoGMCBase, PPProductionOrdrEstRMCreate, PPProductionOrdrEstRMs, PPProductionOrdrEstRMs>, IPPProductionOrdrEstRMService
    {
        private readonly IPPProductionOrdrEstRMReponsitory _repository;
        private readonly IMapper _mapper;

        public PPProductionOrdrEstRMService(IPPProductionOrdrEstRMReponsitory repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}
