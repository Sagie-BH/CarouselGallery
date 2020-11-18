using CarouselHomework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarouselHomework.Data
{
    public class CarouselRepository : IRepository<ImageModel>
    {
        private readonly CarouselContext context;

        public CarouselRepository(CarouselContext _context)
        {
            context = _context;
        }
        public void Create(ImageModel entity)
        {
            context.Gallery.Add(entity);
        }

        public IEnumerable<ImageModel> GetAll()
        {
            return context.Gallery;
        }

        public ImageModel GetById(int id)
        {
            return context.Gallery.FirstOrDefault(x => x.Id == id);
        }

        public void Remove(int id)
        {
            context.Gallery.Remove(GetById(id));
        }

        public async Task<bool> SaveChanges()
        {
            var taskResult = await Task.Run(() =>
            {
                bool changesSaved;
                try { context.SaveChanges(); changesSaved = true; }
                catch { changesSaved = false; }
                return changesSaved;
            });
            return taskResult;
        }

        public void Update(ImageModel entity)
        {
            context.Gallery.Update(entity);
        }
    }
}
