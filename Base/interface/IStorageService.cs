using Microsoft.AspNetCore.Http;

namespace gmc_api.Base.InterFace
{
    public interface IStorageService
    {
        string Upload(string type, IFormFile formFile);
        string GetPathByName(string type, string fileName);
    }
}
