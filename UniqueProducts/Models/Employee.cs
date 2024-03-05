using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniqueProducts.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Orders = new HashSet<Order>();
        }

        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Поле 'Имя' обязательно для заполнения.")]
        [Display(Name = "Имя")]
        public string? EmployeeName { get; set; }

        [Required(ErrorMessage = "Поле 'Фамилия' обязательно для заполнения.")]
        [Display(Name = "Фамилия")]
        public string? EmployeeSurname { get; set; }

        [Required(ErrorMessage = "Поле 'Отчество' обязательно для заполнения.")]
        [Display(Name = "Отчество")]
        public string? EmployeeMidname { get; set; }

        [Required(ErrorMessage = "Поле 'Должность' обязательно для заполнения.")]
        [Display(Name = "Должность")]
        public string? EmployeePosition { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
