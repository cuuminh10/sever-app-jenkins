using gmc_api.Base;
using gmc_api.Base.InterFace;
using gmc_api.Entities;

namespace gmc_api.Repositories
{
    public interface IADAttachmentReponsitory : IRepositoriesBase<ADAttachments, ADAttachments>
    {
    }

    public class ADAttachmentReponsitory : RepositoriesBaseImpl<ADAttachments, ADAttachments>, IADAttachmentReponsitory
    {
        private readonly GMCContext _context;
        public ADAttachmentReponsitory(GMCContext context) : base(context, "ADAttachments", "ADAttachmentID")
        {
            _context = context;
        }
    }
}
