namespace GestioneTickets.Model
{
    public class ImageUploadRequest
    {
        public string Base64Image { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
    }
}
