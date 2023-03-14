using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdinCMS.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ?Description { get; set; }

        [DisplayName("Image url")]
        [ValidateNever]
        public string ?ImageUrl { get; set; }

        [DisplayName("List price")]
        [Required, Range(1, 1000000)]
        public double ListPrice { get; set; }
        [Required, Range(1, 1000000)]
        public double Price { get; set; }

        [DisplayName("Category Id")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
        
        [DisplayName("Cover type Id")]
        public int CoverTypeId { get; set; }
        
        [ValidateNever, DisplayName("Cover type")]
        public CoverType CoverType { get; set; }

    }
}
