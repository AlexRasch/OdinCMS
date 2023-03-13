﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
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
        [ValidateNever]
        public string ImageUrl { get; set; }

        [Required, Range(1, 1000000)]
        public double ListPrice { get; set; }
        [Required, Range(1, 1000000)]
        public double Price { get; set; }


        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }

        public int CoverTypeId { get; set; }
        [ValidateNever]
        public CoverType CoverType { get; set; }

    }
}
