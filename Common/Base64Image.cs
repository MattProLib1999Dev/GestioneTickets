public class Base64Image
{
    public string ContentType { get; set; }
    public byte[] FileContents { get; set; }

    public static Base64Image Parse(string base64Content)
    {
        if (string.IsNullOrWhiteSpace(base64Content))
            throw new ArgumentNullException(nameof(base64Content));

        int indexOfSemiColon = base64Content.IndexOf(';');
        if (indexOfSemiColon < 0)
            throw new FormatException("Invalid data URI format: missing ';'");

        string dataLabel = base64Content.Substring(0, indexOfSemiColon);
        string contentType = dataLabel.Split(':').Last();

        int base64StartIndex = base64Content.IndexOf("base64,", StringComparison.OrdinalIgnoreCase);
        if (base64StartIndex < 0)
            throw new FormatException("Invalid data URI format: missing 'base64,'");

        string fileContents = base64Content.Substring(base64StartIndex + 7);
        byte[] bytes = Convert.FromBase64String(fileContents);

        return new Base64Image
        {
            ContentType = contentType,
            FileContents = bytes
        };
    }

    public override string ToString()
    {
        return $"data:{ContentType};base64,{Convert.ToBase64String(FileContents)}";
    }

	/*
		1. FromFile(string path) ➡️ Converte un file immagine in Base64Image
		Passaggi:
		Legge il file immagine dal percorso specificato.

		Riconosce il tipo di immagine (JPEG, PNG, ecc.) in base all’estensione del file.

		Crea un oggetto Base64Image che contiene:

		Il tipo MIME (es: image/png)

		I byte dell’immagine (cioè i dati del file)
	*/
    public static Base64Image FromFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException("Image file not found.", filePath);

        byte[] fileBytes = File.ReadAllBytes(filePath);

        string contentType = GetMimeType(filePath);

        return new Base64Image
        {
            FileContents = fileBytes,
            ContentType = contentType
        };
    }

    private static string GetMimeType(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLowerInvariant();

        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".svg" => "image/svg+xml",
            ".webp" => "image/webp",
            _ => "application/octet-stream" // fallback
        };
    }
}
