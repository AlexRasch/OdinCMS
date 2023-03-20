using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdinCMS.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }

        // Product
        public int ProductId { get; set; }
        [ForeignKey("ProductId"), ValidateNever]
        public Product Product { get; set; }

        [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int Count { get; set; }

        // User details
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
