using Microsoft.AspNetCore.Identity;
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
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string? StreetAddress { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? Region { get; set; }
        [Required]
        public string? PostalCode { get; set; }
        
        // If user belong to a company
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId"), ValidateNever]
        public Company Company { get; set; }

    }
}
