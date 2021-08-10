using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Helper.ExtensionMethods.String;
using Elkood.Web.Service.BoundedContext.General;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.AdvertisementDto;
using SmartStart.Model.Setting;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Setting.AdvertisementService
{
    public class AdvertisementRepository : ElRepositoryGeneral<SmartStartDbContext, Guid, Advertisement, AdvertisementDto>, IAdvertisementRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public AdvertisementRepository(SmartStartDbContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public override async Task<OperationResult<AdvertisementDto>> DeleteAsync(Guid id)
            => await RepositoryHandler(_delete(id));

        public async Task<OperationResult<AdvertisementDto>> Upload(AdvertisementDto dto, IFormFile file)
            => await RepositoryHandler(_upload(dto, file));

        public async Task<OperationResult<IEnumerable<AdvertisementDto>>> GetAdvertisement()
            => await RepositoryHandler(_getAdvertisement());

        
        private Func<OperationResult<AdvertisementDto>, Task<OperationResult<AdvertisementDto>>> _delete(Guid id)
            => async operation =>
            {
                var advertisement = await FindAsync(id);
                if (advertisement is null)
                    return (OperationResultTypes.NotExist);
                TryDeleteFile(advertisement.ImagePath);
                return await base.DeleteAsync(id);
            };

        private Func<OperationResult<AdvertisementDto>, Task<OperationResult<AdvertisementDto>>> _upload(AdvertisementDto dto, IFormFile file)
            => async operation =>
            {
                var advertisement = await FindAsync(dto.Id);

                if (advertisement is null)
                {
                    var myImage = TryUploadFile(file,out string path);
                    if(myImage.IsSuccess)
                    {
                        dto.ImagePath = path;
                        var newAdvertisement = await base.AddAsync(dto);
                        if(newAdvertisement.IsSuccess)
                        {
                            dto.Id = newAdvertisement.Result.Id;
                            return operation.SetSuccess(dto);
                        }
                        return operation.SetFailed(newAdvertisement.Message + " " + newAdvertisement.FullExceptionMessage);
                    }
                    return operation.SetFailed("Failed upload, Message:" + " " + myImage.FullExceptionMessage);
                }
                if(advertisement is not null)
                {
                    if(advertisement.ImagePath != dto.ImagePath || file != null)
                        TryDeleteFile(advertisement.ImagePath);
                }
                var newFile = TryUploadFile(file, out string newPath);
                if (newFile.IsSuccess)
                {
                    dto.ImagePath = newPath;
                    Context.Entry(advertisement).State = EntityState.Detached;
                    var newAdvertisement = await base.UpdateAsync(dto);
                    if (newAdvertisement.IsSuccess)
                    {
                        return operation.SetSuccess(dto);
                    }
                    return operation.SetFailed(newAdvertisement.Message + " " + newAdvertisement.FullExceptionMessage);
                }
                return operation.SetFailed("Failed upload, Message:" + " " + newFile.FullExceptionMessage);
            };

        private Func<OperationResult<IEnumerable<AdvertisementDto>>, Task<OperationResult<IEnumerable<AdvertisementDto>>>> _getAdvertisement()
            => async operation =>
            {
                var advertisements = await Query.Where(ads => ads.EndDate >= DateTime.Now)
                                                 .Select(ads => new AdvertisementDto()
                                                 {
                                                     Id = ads.Id,
                                                     Title = ads.Title,
                                                     ImagePath = ads.ImagePath,
                                                     StartDate = ads.StartDate,
                                                     EndDate = ads.EndDate
                                                 }).ToListAsync();

                return operation.SetSuccess(advertisements);
            };



        #region Helper Methods

        private OperationResult<bool> TryDeleteFile(string path)
        {
            if(!path.IsNullOrEmpty())
            {
                var pathFile = Path.Combine(webHostEnvironment.WebRootPath, path);
                try
                {
                    File.Delete(pathFile);
                }
                catch (Exception ex)
                {
                    return new OperationResult<bool>().SetException(ex);
                }
            }
            return new OperationResult<bool>().SetSuccess(true);
        }

        private OperationResult<bool> TryUploadFile(IFormFile file, out string path)
        {
            path = null;
            try
            {
                if(file != null)
                {
                    var documentsDirectory = Path.Combine("wwwroot", "Documents", "Advertiment_Images");
                    if (!Directory.Exists(documentsDirectory))
                    {
                        Directory.CreateDirectory(documentsDirectory);
                    }
                    path = Path.Combine("Documents", "Advertisement_Image", Guid.NewGuid().ToString(), "_", file.FileName);
                    string fileName = Path.Combine(webHostEnvironment.WebRootPath, path);
                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
                return new OperationResult<bool>().SetSuccess(true);
            }
            catch (Exception ex)
            {
                return new OperationResult<bool>().SetException(ex);
            }
        }

        #endregion
    }
}
