using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Store.Web.Data.Entities
{
    public partial class Employees
    {
        public Employees()
        {
            EmployeeTerritories = new HashSet<EmployeeTerritories>();
            InverseReportsToNavigation = new HashSet<Employees>();
            Orders = new HashSet<Orders>();
        }

        public int EmployeeId { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "You must enter {0}")]
        [StringLength(20, ErrorMessage = "The field {0} must be betwen {1} and {2} characters", MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(10, ErrorMessage = "The field {0} must be betwen {1} and {2} characters", MinimumLength = 3)]
        public string FirstName { get; set; }

        [JsonIgnore]
        [NotMapped]
        [Display(Name = "Name")]
        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        [StringLength(30, ErrorMessage = "The field {0} must be betwen {1} and {2} characters", MinimumLength = 2)]
        public string Title { get; set; }

        [StringLength(25, ErrorMessage = "The field {0} must be betwen {1} and {2} characters", MinimumLength = 2)]
        public string TitleOfCourtesy { get; set; }

        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Hire Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? HireDate { get; set; }

        [StringLength(60, ErrorMessage = "The field {0} must be betwen {1} and {2} characters", MinimumLength = 5)]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [StringLength(15, ErrorMessage = "The field {0} must be betwen {1} and {2} characters", MinimumLength = 2)]
        public string City { get; set; }

        [StringLength(15, ErrorMessage = "The field {0} must be betwen {1} and {2} characters", MinimumLength = 2)]
        public string Region { get; set; }

        [StringLength(15)]
        public string PostalCode { get; set; }

        [StringLength(15, ErrorMessage = "The field {0} must be betwen {1} and {2} characters", MinimumLength = 2)]
        public string Country { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(24, ErrorMessage = "The field {0} must be betwen {1} and {2} characters", MinimumLength = 4)]
        public string HomePhone { get; set; }

        [StringLength(4)]
        public string Extension { get; set; }

        [JsonIgnore]
        public byte[] Photo { get; set; }

        [JsonIgnore]
        [NotMapped]
        public string PhotoBase64
        {
            get
            {
                return (Photo == null ? string.Empty : String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(Photo)));
            }
        }

        public string Notes { get; set; }

        public int? ReportsTo { get; set; }

        [JsonIgnore]
        public string PhotoPath { get; set; }

        public string ImageFullPath => string.IsNullOrEmpty(PhotoPath)
           //? null : $"http://warlinsano.somee.com{PhotoPath.Substring(1)}";
           ? null : $"https://localhost:44396{PhotoPath.Substring(1)}";


        //[Display(Name = "Birth Date")]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        //public DateTime BirthDateLocal => BirthDate.ToLocalTime();

        //[DataType(DataType.Currency)]
        //[Required(ErrorMessage = "You must enter {0}")]
        //[DisplayFormat(DataFormatString = "{0:2}", ApplyFormatInEditMode = false)]
        //public decimal Salary { get; set; }

        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; }

        //url 
        //bonus
        [JsonIgnore]
        public virtual Employees ReportsToNavigation { get; set; }
        [JsonIgnore]
        public virtual ICollection<EmployeeTerritories> EmployeeTerritories { get; set; }
        [JsonIgnore]
        public virtual ICollection<Employees> InverseReportsToNavigation { get; set; }
        [JsonIgnore]
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
