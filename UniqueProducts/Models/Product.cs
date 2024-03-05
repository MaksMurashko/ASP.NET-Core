using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniqueProducts.Models
{
    public partial class Product
    {
        public Product()
        {
            Orders = new HashSet<Order>();
        }

        public int ProductId { get; set; }
        [Required(ErrorMessage = "Поле 'Название изделия' обязательно для заполнения.")]
        [Display(Name = "Название изделия")]
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Поле 'Описание изделия' обязательно для заполнения.")]
        [Display(Name = "Описание изделия")]
        public string? ProductDescript { get; set; }

        [Required(ErrorMessage = "Поле 'Вес изделия' обязательно для заполнения.")]
        [Display(Name = "Вес изделия")]
        [RegularExpression(@"^\d+(\.\d{2})?$", ErrorMessage = "Поле 'Вес изделия' может содержать лишь число в формате x.xx")]
        public float? ProductWeight { get; set; }

        [Required(ErrorMessage = "Поле 'Диаметр изделия' обязательно для заполнения.")]
        [Display(Name = "Диаметр изделия")]
        [RegularExpression(@"^\d+(\.\d{2})?$", ErrorMessage = "Поле 'Диаметр изделия' может содержать лишь число в формате x.xx")]
        public float? ProductDiameter { get; set; }

        [Required(ErrorMessage = "Поле 'Цвет изделия' обязательно для заполнения.")]
        [Display(Name = "Цвет изделия")]
        public string? ProductColor { get; set; }

        [Required(ErrorMessage = "Поле 'Материал изделия' обязательно для заполнения.")]
        [Display(Name = "Материал изделия")]
        public int? MaterialId { get; set; }

        [Required(ErrorMessage = "Поле 'Цена изделия' обязательно для заполнения.")]
        [Display(Name = "Цена изделия")]
        [RegularExpression(@"^\d+(\.\d{2})?$", ErrorMessage = "Поле 'Цена изделия' может содержать лишь число в формате x.xx")]
        public decimal? ProductPrice { get; set; }

        public virtual Material? Material { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
