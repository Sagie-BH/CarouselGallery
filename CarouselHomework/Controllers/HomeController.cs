using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CarouselHomework.Models;
using CarouselHomework.ViewModels;
using CarouselHomework.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace CarouselHomework.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<ImageModel> repository;
        private readonly IWebHostEnvironment environment;

        public HomeController(ILogger<HomeController> logger, IRepository<ImageModel> repository, IWebHostEnvironment environment)
        {
            _logger = logger;
            this.repository = repository;
            this.environment = environment;
        }

        public IActionResult Index()
        {
            return View(GetCurrentVM());
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private IEnumerable<string> GetLocalImages()
        {
            var imgRootDir = environment.WebRootPath + "/Images";
            var filters = new String[] { ".jpg", ".jpeg", ".png", ".gif", ".tiff", ".bmp", ".svg" };
            var baseUrl = "/";

            var imgUrls = Directory.EnumerateFiles(imgRootDir, "*.*", SearchOption.AllDirectories)
                .Where(fileName => filters.Any(filter => fileName.EndsWith(filter)))
                .Select(fileName => Path.GetRelativePath(imgRootDir, fileName)) // get relative path
                .Select(fileName => Path.Combine(baseUrl, fileName))          // prepend the baseUrl
                .Select(fileName => fileName.Replace("\\", "/")).ToList()             // replace "\" with "/"
                ;
            return imgUrls;
        }
        [HttpPost]
        public async Task<ActionResult> CreateImage(CarouselViewModel carouselViewModel)
        {
            var viewModel = carouselViewModel.ImageViewModel;
            var file = viewModel.UploadedImage;
            string path = Path.Combine(environment.WebRootPath, "Images/" + file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileSize = GetFileSize(file.Length);
            repository.Create(new ImageModel { Path = file.FileName, Size = fileSize, Title = viewModel.Title });

            carouselViewModel.ImagesUrls = GetLocalImages().Append(file.FileName);
            carouselViewModel.ImageModelList = repository.GetAll();
            if (await repository.SaveChanges())
            {
                return View("Index", carouselViewModel);
            }
            return NotFound();
        }
        private CarouselViewModel GetCurrentVM()
        {
            return new CarouselViewModel
            {
                ImagesUrls = GetLocalImages(),
                ImageModelList = repository.GetAll()
            };
        }

        private string GetFileSize(long bytes)
        {
            // Load all suffixes in an array  
            string[] suffixes =
           { "Bytes", "KB", "MB", "GB", "TB", "PB" };

            int counter = 0;
            decimal number = (decimal)bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            return string.Format("{0:n1}{1}", number, suffixes[counter]);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var imgModel = repository.GetById(id);
            if (imgModel == null)
            {
                return NotFound();
            }
            repository.Remove(id);
            if (await repository.SaveChanges())
            {
                return View("Index", GetCurrentVM());
            }
            return BadRequest("Remove Failed");
        }
    }
}
