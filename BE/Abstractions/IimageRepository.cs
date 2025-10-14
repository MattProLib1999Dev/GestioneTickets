namespace GestioneTickets.Model
{
    public interface IImageRepository
    {
        Task<ImageUploadRequest> UploadImageAsync(ImageUploadRequest request);
    }
}