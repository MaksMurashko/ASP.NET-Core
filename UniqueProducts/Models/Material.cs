using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniqueProducts.Models
{
    public partial class Material
    {
        public Material()
        {
            Products = new HashSet<Product>();
        }

        public int MaterialId { get; set; }
        [Required(ErrorMessage = "Поле 'Название материала' обязательно для заполнения.")]
        [Display(Name = "Название материала")]
        public string? MaterialName { get; set; }

        [Required(ErrorMessage = "Поле 'Описание материала' обязательно для заполнения.")]
        [Display(Name = "Описание материала")]
        public string? MaterialDescript { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
