using gmc_api.Base.Helpers;
using gmc_api.Base.InterFace;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using static gmc_api.Base.Helpers.Constants;

namespace gmc_api.Base.Files
{
    public class StorageLocalService : IStorageService
    {
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _hostEnv;
        public StorageLocalService(IOptions<AppSettings> appSettings, IWebHostEnvironment hostEnv)
        {
            this._appSettings = appSettings.Value;
            this._hostEnv = hostEnv;
        }

        public string GetPathByName(string type, string fileName)
        {
            var pathFolder = "/" + _appSettings.UpdloadFolder
                    + "/" + FileUploadType.folderUpload[type] + "/";
            return _hostEnv.WebRootPath + pathFolder + fileName;
        }

        public string Upload(string type, IFormFile formFile)
        {
            try
            {
                var pathFolder = "/" + _appSettings.UpdloadFolder
                        + "/" + FileUploadType.folderUpload[type] + "/";
                var folderName = _hostEnv.WebRootPath + pathFolder;
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }
                var fileLocalName = DateTime.Now.ToString("yyyyMMddhhmmss") + "_" + formFile.FileName;
                using (FileStream fs = File.Create(folderName + fileLocalName))
                {
                    formFile.CopyTo(fs);
                    fs.Flush();
                    return fileLocalName;
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

    }
}
