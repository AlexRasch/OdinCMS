using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OdinCMS.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [DisplayName("Display order")]
        public int DisplayOrder { get; set; }
        
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;


    }
}
