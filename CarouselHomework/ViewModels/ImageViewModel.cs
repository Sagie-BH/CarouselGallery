using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarouselHomework.ViewModels
{
    public class ImageViewModel
    {
        public string Title { get; set; }
        public IFormFile UploadedImage { get; set; }
    }
}
