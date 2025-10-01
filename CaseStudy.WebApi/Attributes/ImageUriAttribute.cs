using System.ComponentModel.DataAnnotations;

namespace CaseStudy.WebApi.Attributes
{
    /// <summary>
    /// A custom attribute to define valid URIs ending with a suffix for images.
    /// </summary>
    public class ImageUriAttribute : ValidationAttribute
    {
        /// <summary>
        /// Valid image file extensions.
        /// </summary>
        private static readonly string[] AllowedImgExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg" };

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            if (value is string strUri && Uri.TryCreate(strUri, UriKind.Absolute, out Uri? uri))
            {
                string ext = Path.GetExtension(uri.AbsolutePath).ToLowerInvariant();
                if (Array.Exists(AllowedImgExtensions, e => e == ext))
                    return ValidationResult.Success;

                return new ValidationResult($"{validationContext.DisplayName} must be an image URL. Supported extensions are {string.Join(", ", AllowedImgExtensions)}.");
            }

            return new ValidationResult($"{validationContext.DisplayName} is not a valid URI.");
        }
    }
}
