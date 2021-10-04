using AutoMapper;
using gmc_api.DTO.PP;
using gmc_api.Entities;
using gmc_api.Repositories;
using gmc_api.Base.InterFace;
using gmc_api.Base;
using gmc_api.Base.dto;

namespace gmc_api.Services
{
    public interface IPPProductionOrdrEstFGService : IServiceGMCBase<DtoGMCBase, PPProductionOrdrEstFGCreate, PPProductionOrdrEstFGs, PPProductionOrdrEstFGs>
    {

    }

    public class PPProductionOrdrEstFGService : ServiceBaseImpl<DtoGMCBase, PPProductionOrdrEstFGCreate, PPProductionOrdrEstFGs, PPProductionOrdrEstFGs>, IPPProductionOrdrEstFGService
    {
        private readonly IPPProductionOrdrEstFGReponsitory _repository;
        private readonly IMapper _mapper;

        public PPProductionOrdrEstFGService(IPPProductionOrdrEstFGReponsitory repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


    }
}
