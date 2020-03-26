using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    /// <summary>
    /// Represents an image service modal.
    /// </summary>
    public class ImageServiceModal : IImageServiceModal
    {
       
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size

        /// <summary>
        /// Constructor. Creates an image service modal instance.
        /// </summary>
        /// <param name="output">The output folder.</param> 
        /// <param name="thumbnailSize">The thumbnail size.</param> 
        public ImageServiceModal(string output, int thumbnailSize)
        {
            m_OutputFolder = output;
            m_thumbnailSize = thumbnailSize;
        }

        /// <summary>
        /// adds the file to the output directory in the year and month of the date the picture was taken.
        /// also creates a thumbnail in the thumbnails directory in the same year and month.
        /// if the picture doesn't have a taken date we work on creation date.
        /// if a picture in the same name exist in the output directory we add a number and check again.
        /// </summary>
        /// <param name="path">the path of the picture to be added</param>
        /// <param name="result">success or fail with a message</param>
        /// <returns></returns>
        public string AddFile(string path, out bool result)
        {
            try
            {
                if (File.Exists(path)) 
                {

                    DateTime timePicTaken = GetDateTakenFromImage(path);
                    // Get the month and year from the picture's taken date
                    string year = timePicTaken.Year.ToString();
                    string month = timePicTaken.Month.ToString();                   

                    // create target directory and make it hidden
                    DirectoryInfo directoryInfo = Directory.CreateDirectory(m_OutputFolder);
                    //new FileInfo(m_OutputFolder).Attributes = new FileInfo(m_OutputFolder).Attributes | FileAttributes.Hidden;

                    //create the path strings for the output folder and thumbnail
                    string outputFolder = m_OutputFolder + "\\" + year + "\\" + month;
                    string outputThumbnail = m_OutputFolder + "\\Thumbnails\\" + year + "\\" + month;

                    //create the directories
                    Directory.CreateDirectory(outputFolder);
                    Directory.CreateDirectory(outputThumbnail);
                    
                    string finalPath = outputFolder + "\\" + Path.GetFileName(path);
                    string finalThumbnail = outputThumbnail + "\\" + Path.GetFileName(path);
                    //if a picture with the same name already exists we change the name
                    /*if (File.Exists(finalPath))
                    {
                        Tuple<string, int> tuple = ChangePicName(finalPath, outputFolder);
                        finalPath = tuple.Item1;
                        //change thumbnail accordingly to match
                        finalThumbnail= outputThumbnail + "\\" + Path.GetFileNameWithoutExtension(path) + " (" + tuple.Item2.ToString() + ")" +Path.GetExtension(path);
                    }*/
                    
                    if (File.Exists(finalPath))
                        File.Delete(finalPath);
                    File.Move(path, finalPath);
                    
                    Image image = Image.FromFile(finalPath);
                    Image thumb = new Bitmap(image, new Size(m_thumbnailSize, m_thumbnailSize));
                    thumb.Save(finalThumbnail);
                 
                    result = true;
                    string message = "Picture: " + Path.GetFileName(path) + " was successfully added in path "+ finalPath;

                    //disposing the pic and thumb for further use
                    image.Dispose();
                    thumb.Dispose();
                    
                    return message;
                }
                else // file doesn't exist
                {
                    throw new Exception("File in path " + path + " doesn't exist");
                }
            }
            catch(Exception exc)
            {
                result = false;
                return exc.ToString();
            }

        }

        private static Regex r = new Regex(":");
        /// <summary>
        /// retrieves the datetime of the time taken ,or the creation time if it can't, WITHOUT loading the whole image
        /// </summary>
        /// <param name="path">the path of the picture</param>
        /// <returns>the date time</returns>
        public static DateTime GetDateTakenFromImage(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (Image myImage = Image.FromStream(fs, false, false))
                {
                    PropertyItem propItem = myImage.GetPropertyItem(36867);
                    string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    return DateTime.Parse(dateTaken);
                }
            }
            catch 
            {
                //unable to get time picture taken, continue with creation time.
                return File.GetCreationTime(path);
            }
        }

        /// <summary>
        /// finds an available path for the picture by adding a number to its name
        /// </summary>
        /// <param name="finalPath">the path of the picture</param>
        /// <param name="outputFolder">the output folder</param>
        /// <returns>a tuple of the new final path and the number to be used for the thumbnail as well</returns>
        private Tuple<string,int> ChangePicName(string finalPath, string outputFolder)
        {
            int count = 1;
            string fileName = Path.GetFileNameWithoutExtension(finalPath);
            string extension = Path.GetExtension(finalPath);
            while (File.Exists((finalPath = outputFolder + "\\" + fileName + " (" + count.ToString() + ")" + extension)))
            {
               count++;
            }
            return Tuple.Create(finalPath,count);
        }
    }
}
