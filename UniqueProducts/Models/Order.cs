using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniqueProducts.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        [Required(ErrorMessage = "Поле 'Дата заказа' обязательно для заполнения.")]
        [Display(Name = "Дата заказа")]
        public DateTime? OrderDate { get; set; }

        [Required(ErrorMessage = "Поле 'Компания' обязательно для заполнения.")]
        [Display(Name = "Компания")]
        public int? ClientId { get; set; }

        [Required(ErrorMessage = "Поле 'Изделие' обязательно для заполнения.")]
        [Display(Name = "Изделие")]
        public int? ProductId { get; set; }

        [Required(ErrorMessage = "Поле 'Кол-во изделий' обязательно для заполнения.")]
        [Display(Name = "Кол-во изделий")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Поле 'Кол-во изделий' должно содержать только целое число.")]
        public int? OrderAmount { get; set; }

        [Required(ErrorMessage = "Поле 'Полная стоимость' обязательно для заполнения.")]
        [Display(Name = "Полная стоимость")]
        [RegularExpression(@"^\d+(\.\d{2})?$", ErrorMessage = "Поле 'Полная стоимость' может содержать лишь число в формате x.xx")]
        public decimal? TotalPrice { get; set; }

        [Required(ErrorMessage = "Поле 'Отметка о выполнении' обязательно для заполнения.")]
        [Display(Name = "Отметка о выполнении")]
        public bool? IsCompleted { get; set; }

        [Required(ErrorMessage = "Поле 'Ответственный сотрудник' обязательно для заполнения.")]
        [Display(Name = "Ответственный сотрудник")]
        public int? EmployeeId { get; set; }

        public virtual Client? Client { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual Product? Product { get; set; }
    }
}
