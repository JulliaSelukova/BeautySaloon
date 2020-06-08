using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyService.Models
{
    public class Registration
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }
        [Display(Name = "Время")]
        public string Time { get; set; }
        [Display(Name = "Клиент")]
        public string ClientName { get; set; }
        [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$", ErrorMessage = "Некорректный номер телефона")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Номер телефона обязателен для заполнения")]
        [Display(Name = "Контактный телефон")]
        public string ClientPhone { get; set; }
        public bool IsEngaged { get; set; }
        [Display(Name = "Услуга")]
        public int? ServiceTypeId { get; set; }
        [Display(Name = "Услуга")]
        public virtual ServiceType ServiceType { get; set; }
        [Display(Name = "Мастер")]
        public int ServiceCategoryId { get; set; }
        [Display(Name = "Мастер")]
        public virtual ServiceCategory ServiceCategory { get; set; }

        public Registration(int serviceCategoryId)
        {
            ServiceCategoryId = serviceCategoryId;
        }

        public Registration()
        {
            
        }
    }
}
