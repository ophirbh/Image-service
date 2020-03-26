using ImageServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class FirstController : Controller
    {
        static ConfigModel configModel = new ConfigModel();
        static LogsModel logsModel = new LogsModel();
        static PhotosCollectionModel photosCollectionModel = new PhotosCollectionModel();
        static MainModel mainModel = new MainModel();
        private static string handlerDelete;

        /// <summary>
        /// constructor. waits for the OutputDir then calls the GetPhotosCollection method. 
        /// </summary>
        public FirstController()
        {
            if (mainModel.Connected == true)
            {
                while (configModel.OutputDir == null)
                {
                    Thread.Sleep(1000);
                }
                string outputDirPath = configModel.OutputDir;
                photosCollectionModel.GetPhotosCollection(outputDirPath);
            }
        }
        // GET: First
        /// <summary>
        /// sets the property PhotoNumber to the number of photos in the Photos list.
        /// </summary>
        /// <returns>the Index View with mainModel.</returns>
        public ActionResult Index()
        {
            mainModel.PhotoNumber = photosCollectionModel.Photos.Count;
            return View(mainModel);
        }

        /// <summary>
        /// Config page constractor.
        /// </summary>
        /// <returns>the Config View with configModel.</returns>
        [HttpGet]
        public ActionResult Config()
        {
            return View(configModel);
        }

        /// <summary>
        /// DeleteHandler page constractor.
        /// </summary>
        /// <returns>the View.</returns>
        [HttpGet]
        public ActionResult DeleteHandler()
        {
            return View();
        }

        /// <summary>
        /// Logs page constractor.
        /// </summary>
        /// <returns>the Logs View with logsModel.</returns>
        [HttpGet]
        public ActionResult Logs()
        {
            return View(logsModel);
        }

        /// <summary>
        /// The method gets a handler(that was clicked on in the view) and sets the handlerDelete field to equal it.
        /// </summary>
        /// <param name="handlerToDelete"></param>
        /// <returns>redirects to DeleteHandler page</returns>
        public ActionResult SaveHandler(string handlerToDelete)
        {
            handlerDelete = handlerToDelete;
            return RedirectToAction("DeleteHandler");
        }

        /// <summary>
        /// The method activates the DeleteHandler method in configModel and waits until deletion.
        /// </summary>
        /// <returns>Redirects to Config page</returns>
        public ActionResult OKClicked()
        {
            configModel.DeleteHandler(handlerDelete);
            while (configModel.Handlers.Contains(handlerDelete))
            {
                Thread.Sleep(1000);
            }
            return RedirectToAction("Config");
        }

        /// <summary>
        /// Gets the photo collection and shows the photos.
        /// </summary>
        /// <returns>The view of the photos.</returns>
        public ActionResult Photos()
        {
            return View(photosCollectionModel);
        }

        /// <summary>
        /// Gets a relative path to a photo (Relative to project directory) and shows the photo.
        /// </summary>
        /// <param name="photoRelPath">The relative path to the photo. (Relative to project directory).</param>
        /// <returns>The view of the photo.</returns>
        public ActionResult PhotoPresenter(string photoRelPath)
        {
            foreach (PhotosModel photo in photosCollectionModel.Photos)
            {
                if (photo.RelativePath == photoRelPath)
                {
                    return View(photo);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets a The thumbnail relative path of a photo (Relative to project directory),
        /// and returns the deletion confirmation view of the photo.
        /// </summary>
        /// <param name="ThumbPhotoRelPath"></param>
        /// <returns>The deletion confirmation view of the photo.</returns>
        public ActionResult DeletePhoto(string ThumbPhotoRelPath)
        {
            foreach (PhotosModel photo in photosCollectionModel.Photos)
            {
                if (photo.RelativeThumbnailPath == ThumbPhotoRelPath)
                {
                    return View(photo);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets a The thumbnail relative path of a photo (Relative to project directory),
        /// deletes that photo from the photo collection, and returns to the photos view.
        /// </summary>
        /// <param name="thumbPhotoRelPath">The thumbnail relative path (Relative to project directory) of the photo
        /// to be deleted.</param>
        /// <returns>The photos view.</returns>
        public ActionResult PhotoDeletionConfirmed(string thumbPhotoRelPath)
        {
            foreach (PhotosModel photo in photosCollectionModel.Photos)
            {
                if (photo.RelativeThumbnailPath == thumbPhotoRelPath)
                {
                    System.IO.File.Delete(photo.FullPath);
                    System.IO.File.Delete(photo.ThumbFullPath);
                    return RedirectToAction("Photos");
                }
            }
            return null;
        }
    }

}