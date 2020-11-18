using CarouselHomework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarouselHomework.ViewModels
{
    public class CarouselViewModel
    {
        public ImageViewModel ImageViewModel { get; set; }
        public IEnumerable<string> ImagesUrls { get; set; }
        public IEnumerable<ImageModel> ImageModelList { get; set; }
        public ImageModel ImageModel { get; set; }
    }
}
