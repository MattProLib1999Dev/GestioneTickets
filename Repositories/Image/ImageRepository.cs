using GestioneTickets.DataAccess;
using GestioneTickets.Model;

namespace GestioneTickets.Repositories
{
   public class ImageRepository(ApplicationDbContext applicationDbContext) : IImageRepository
   {
      private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

        public async Task<ImageUploadRequest> UploadImageAsync(ImageUploadRequest img)
        {
            _applicationDbContext.Add(img);
            await _applicationDbContext.SaveChangesAsync();
            return img;
        }
   }
}