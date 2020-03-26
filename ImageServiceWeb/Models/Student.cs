using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class Student
    {
        public Student(string first,string second,string IDnum)
        {
            First = first;
            Second = second;
            ID = IDnum;
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First name")]
        public string First { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Second name")]
        public string Second { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ID")]
        public string ID { get; set; }
    }
}