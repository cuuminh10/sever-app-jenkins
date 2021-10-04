using AutoMapper;
using gmc_api.Base;
using gmc_api.Base.InterFace;
using gmc_api.DTO.FC;
using gmc_api.Entities;
using gmc_api.Repositories;

namespace gmc_api.Services
{
    public interface IADAttachmentService : IServiceGMCBase<ADAttachmentReponse, ADAttachmentCreateRequest, ADAttachments, ADAttachments>
    {
    }

    public class ADAttachmentService : ServiceBaseImpl<ADAttachmentReponse, ADAttachmentCreateRequest, ADAttachments, ADAttachments>, IADAttachmentService
    {
        private readonly IADAttachmentReponsitory _repository;
        private readonly IMapper _mapper;

        public ADAttachmentService(IADAttachmentReponsitory repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

    }
}
