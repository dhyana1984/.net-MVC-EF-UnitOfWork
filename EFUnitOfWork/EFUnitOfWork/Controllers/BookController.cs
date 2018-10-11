using EF.Core.Entity;
using EF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using EFUnitOfWork.Models;
using AutoMapper.QueryableExtensions;
using System.IO;
using EF.Core.Helper;
using EFUnitOfWork.UploadUtils;

namespace EFUnitOfWork.Controllers
{
    public class BookController : Controller
    {
        private static object _lock = new object();

        private UnitOfWork unitOfWork = new UnitOfWork();
        private Repository<Book> bookRepository;

        public BookController()
        {
            this.bookRepository = unitOfWork.Repository<Book>();
        }
        public ActionResult Index()
        {
            var books = bookRepository.Table.ProjectTo<BookDTO>().ToList();
            return View(books);
        }

        public ActionResult CreateEditBook(int? id)
        {
            var bookDTO = new BookDTO();
            if(id.HasValue)
            {
                var entity = bookRepository.GetById(id.Value);
                bookDTO = Mapper.Map<Book, BookDTO>(entity);
            }

            return View(bookDTO);
        }
        /// <summary>
        /// 添加和修改书籍 
        /// </summary>
        /// <param name="bookDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateEditBook(BookDTO bookDTO)
        {
            if(bookDTO.ID==0)
            {
                var model = Mapper.Map<BookDTO, Book>(bookDTO);

                model.ModifyTime = DateTime.Now;
                model.CreateTime = DateTime.Now;
                model.Url = string.Empty;
                model.IP = Request.UserHostAddress;

                bookRepository.Insert(model);
                unitOfWork.Commit();
            }
            else
            {
                var editModel = bookRepository.GetById(bookDTO.ID);
                bookDTO.ID = editModel.ID;
                editModel.Author = bookDTO.Author;
                editModel.ISBN = bookDTO.ISBN;
                editModel.Title = bookDTO.Title;
                editModel.Published = bookDTO.Published;
                editModel.ModifyTime = DateTime.Now;
                editModel.IP = Request.UserHostAddress;
                bookRepository.Update(editModel);
                unitOfWork.Commit();

            }

            if(bookDTO.ID>0)
            {
                return View("Upload", bookDTO);
            }
            return RedirectToAction("Index");
        }

        [HttpPost,ActionName("Upload")]
        public ActionResult Upload(Int64 id)
        {
            if(Request.Files.Count<=0)
            {
                return Json(new { status = false, msg = "请选择要上传的书籍！" });
            }

            HandleUploadFiles
        }

        public void HandleUploadFiles(HttpFileCollectionBase files, Int64 id)
        {
            foreach (string file in Request.Files)
            {
                var fileDataContent = Request.Files[file];
                var stream = fileDataContent.InputStream;
                var fileName = Path.GetFileName(fileDataContent.FileName);
                var uploadPath = Server.MapPath("~/App_Data/uploads");
                if(!FileHelper.ExistDirectory(uploadPath))
                {
                    FileHelper.CreateDirectory(uploadPath);
                }

                var path = Path.Combine(uploadPath, fileName);

                //使用瞬态故障处理库Polly处理异常，采用等待重试策略
                PollyHelper.WaitAndRetry<IOException>(() =>
                {
                    if (FileHelper.Exist(path))
                    {
                        FileHelper.Delete(path);
                    }

                    using (var fileStream = System.IO.File.Create(path))
                    {
                        stream.CopyTo(fileStream);
                    }

                    //当上传中断，已上传部分是否能合并
                    var ut = new Utils();
                    var storeFileName = string.Empty;
                    var result = false;

                    //merge
                    




                });
            }
        }
    }
}