using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace GestioneAccounts.BE.Domain.Models
{
    public class ImageUploadRequest
    {
        /// <summary>
        /// Gets or sets the base64 encoded image data.
        /// </summary>
        public string Base64Image { get; set; }

        /// <summary>
        /// Gets or sets the file name of the image.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the content type of the image.
        /// </summary>
        public string ContentType { get; set; }
    }
}
