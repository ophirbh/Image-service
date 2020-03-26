using ImageService.Communication.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    /// <summary>
    /// Represents a main window model.
    /// </summary>
    public class MainModel
    {
        public IClientCommunicationChannel Client { get; set; }

        /// <summary>
        /// Gets a client Instance and recieved the IsConnected status.
        /// </summary>
        public MainModel()
        {
            Client = TcpClientChannel.Instance;
            Connected = Client.IsConnected;
            Students = new List<Student>();
            MakeStudentsList();
        }

        [Required]
        [Display(Name = "Connected")]
        public bool Connected { get; set; }

        [Required]
        [Display(Name = "PhotoNumber")]
        public int PhotoNumber { get; set; }

        [Required]
        [Display(Name = "Students")]
        public List<Student> Students { get; set; }

        /// <summary>
        /// Reads the Details of the students, create Student object for 
        /// each one and adds it to the Students list.  
        /// </summary>
        private void MakeStudentsList()
        {
            try
            {
                StreamReader file = new StreamReader(HttpContext.Current.Server.MapPath("~/App_Data/Details.txt"));
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] student = line.Split(' ');
                    Student newStudent = new Student(student[0], student[1], student[2]);
                    Students.Add(newStudent);
                }
                file.Close();
            }
            catch(Exception)
            {

            }
        }
    }
}