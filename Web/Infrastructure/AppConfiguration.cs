using System.ComponentModel.DataAnnotations;

namespace Contemplation
{
    public class AppConfiguration
    {
        [Required(AllowEmptyStrings = false)]
        public string ImagesFolder { get; set; }

        [Required]
        public int PartsBlurRadius { get; set; }

        [Required]
        public float PartsSaturation { get; set; }

        [Required]
        public float PartsOpacity { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string PartsBackgroundColor { get; set; }
    }
}
