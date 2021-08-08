using Microsoft.AspNetCore.Http;
using SmartStart.DataTransferObject.AdvertisementDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.ViewModels.Settings
{
    public class UploadAdvertisementViewModel : AdvertisementDto
    {
        public IFormFile File { get; set; }
    }
}
