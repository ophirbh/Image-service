using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    /// <summary>
    /// Represents a single photo.
    /// </summary>
    public class PhotosModel
    {

        private string name;
        private string relativePath;
        private string fullPath;
        private string thumbRelativePath;
        private string thumbFullPath;
        private string year;
        private string month;

        /// <summary>
        /// Ctor. gets arguments and creates a new photoModel object.
        /// </summary>
        /// <param name="photoName">The photo's name.</param>
        /// <param name="photoRelativePath">The photo's relative path (Relative to project directory).</param>
        /// <param name="thumbnailRelativePath">The thumbnail photo's relative path (Relative to project directory).</param>
        /// <param name="photofullPath">The photo's full path.</param>
        /// <param name="thumbnailFullPath">The thumbnail photo's full path.</param>
        /// <param name="photoYear">The year the photo was taken.</param>
        /// <param name="photoMonth">The month the photo was taken.</param>
        public PhotosModel(string photoName, string photoRelativePath, string thumbnailRelativePath,
            string photofullPath, string thumbnailFullPath, string photoYear, string photoMonth)
        {
            name = photoName;
            relativePath = photoRelativePath;
            thumbRelativePath = thumbnailRelativePath;
            fullPath = photofullPath; 
            thumbFullPath = thumbnailFullPath;
            year = photoYear;
            month = photoMonth;
        }

        //Properties.
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get => name; set => name = value; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "RelativePath")]
        public string RelativePath { get => relativePath; set => relativePath = value; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "RelativeThumbnailPath")]
        public string RelativeThumbnailPath { get => thumbRelativePath; set => thumbRelativePath = value; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "FullPath")]
        public string FullPath { get => fullPath; set => fullPath = value; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ThumbFullPath")]
        public string ThumbFullPath { get => thumbFullPath; set => thumbFullPath = value; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Year")]
        public string Year { get => year; set => year = value; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Month")]
        public string Month { get => month; set => month = value; }
    }
}