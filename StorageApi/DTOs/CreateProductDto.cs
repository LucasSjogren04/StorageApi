using System.ComponentModel.DataAnnotations;

namespace StorageApi.DTOs
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Namn måste anges.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Namnet måste vara mellan 2 och 100 tecken.")]
        public string Name { get; set; } = string.Empty;

        [Range(1, 1_000_000, ErrorMessage = "Priset måste vara mellan 1 och 1 000 000.")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Kategori måste anges.")]
        [StringLength(50, ErrorMessage = "Kategorin får vara högst 50 tecken.")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hyllplats måste anges.")]
        [StringLength(20, ErrorMessage = "Hyllplatsen får vara högst 20 tecken.")]
        public string Shelf { get; set; } = string.Empty;

        [Range(0, 100_000, ErrorMessage = "Antalet måste vara mellan 0 och 100 000.")]
        public int Count { get; set; }

        [Required(ErrorMessage = "Beskrivning måste anges.")]
        [StringLength(500, ErrorMessage = "Beskrivningen får vara högst 500 tecken.")]
        public string Description { get; set; } = string.Empty;
    }
}
