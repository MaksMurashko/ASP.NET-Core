using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniqueProducts.Models
{
    public partial class Client
    {
        public Client()
        {
            Orders = new HashSet<Order>();
        }

        public int ClientId { get; set; }
        [Required(ErrorMessage = "Поле 'Компания' обязательно для заполнения.")]
        [Display(Name = "Компания")]
        public string? Company { get; set; }

        [Required(ErrorMessage = "Поле 'Представитель' обязательно для заполнения.")]
        [Display(Name = "Представитель")]
        public string? Representative { get; set; }

        [Required(ErrorMessage = "Поле 'Телефон' обязательно для заполнения.")]
        [RegularExpression(@"\+375 \d{9}", ErrorMessage = "Номер телефона должен быть в формате: +375 xxxxxxxxx.")]
        [Display(Name = "Телефон в формате: +375 xxxxxxxxx")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Поле 'Адрес' обязательно для заполнения.")]
        [Display(Name = "Адрес")]
        public string? CompanyAddress { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
