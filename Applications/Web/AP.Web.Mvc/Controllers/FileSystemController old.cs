//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using AP.IO;
//using System.Web.Mvc;
//using System.IO;
//using AP.Data;
//using System.Web;
//using AP.ComponentModel;

//namespace AP.Web.Mvc.Controllers
//{
//    // todo: modelbinder for IDirectoryData + IFileData

//    [Authorize, RequireHttps]
//    public class FileSystemController : ControllerBase
//    {
//        protected IRepository<IDirectoryData> DirectoryRepository
//        { 
//            get; 
//            set;         
//        }

//        protected IRepository<IFileData> FileRepository
//        {
//            get;
//            set;
//        }

//        protected override void Initialize()
//        {                
//            base.Initialize();
//            this.FileRepository = Application.Repositories.GetRepositoryProvider<IFileData>().CreateRepository<IFileData>();
//            this.DirectoryRepository = Application.Repositories.GetRepositoryProvider<IDirectoryData>().CreateRepository<IDirectoryData>();

//            ISharedContextUser fileScu = this.FileRepository as ISharedContextUser;
//            ISharedContextUser dirScu = this.DirectoryRepository as ISharedContextUser;

//            if (fileScu != null && dirScu != null && fileScu.IsCompatible(dirScu))
//                dirScu.Context = fileScu.Context;
//        }

//        protected virtual TFileSystemData CreateModel<TFileSystemData>(string path)            
//            where TFileSystemData : IFileSystemData
//        {
//            string serverPath = GetServerPath(path);
//            FileAttributes attributes = System.IO.File.GetAttributes(serverPath);
            

//            // it's a directory?
//            if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
//            {
//                DirectoryInfo di = new DirectoryInfo(serverPath);

//                var directory = this.Repository.CreateEntity<IDirectoryData>();

//                directory.Path = di.FullName.Substring(0, di.FullName.Length - di.Name.Length);
//                directory.Name = di.Name;

//                return (TFileSystemData)directory;
//            }
//            else
//            {
//                FileInfo fi = new FileInfo(serverPath);                
//            }
//        }

//        [HttpGet]
//        public virtual ActionResult Upload(IDirectoryData directory)
//        {
//            var dir = Repository.OfType<IDirectoryData>().Where(p => Path.Combine(p.Path, p.Name) == GetClientPath(path)).FirstOrDefault();
//            if (dir == null)
//            {
//                dir = Repository.CreateEntity<IDirectoryData>();
                
//                Repository.Insert(dir);
//            }            
//            return View();
//        }

//        [HttpPost]
//        public virtual ActionResult Upload(IDirectoryData directory, HttpFileCollectionBase files)
//        {            
//            return View();
//        }

//        public virtual ActionResult List(string path)
//        {
//            HttpPostedFileBase f;
//            // UserContent/{path}
//            return View();
//        }


//        [HttpPost]
//        public virtual ActionResult Delete(string path)
//        {
//            this.Repository.Delete(CreateModel(path));
            
//            return View();
//        }

//        /// <summary>
//        /// Builds the client path like
//        /// /UserContent/{path}
//        /// </summary>
//        /// <param name="path"></param>
//        /// <returns></returns>
//        protected virtual string GetClientPath(string path)
//        {
//            return Path.Combine(Application.Settings.UserContentDirectory, path);
//        }

//        /// <summary>
//        /// Builds the server path like
//        /// C:\inetpub\{siteName}\UserContent\{path}            
//        /// </summary>
//        /// <param name="path"></param>
//        /// <returns></returns>
//        protected virtual string GetServerPath(string path)
//        {
//            return Server.MapPath(this.GetClientPath(path));
//        }
//    }
//}
