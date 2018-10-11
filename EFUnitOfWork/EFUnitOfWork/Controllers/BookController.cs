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

            HandleUploadFiles(Request.Files, id);
            return Json(new { status = true });
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

                    //Merge file
                    ut.MergeFile(path, out result, out storeFileName);

                    if(result)
                    {
                        var model = bookRepository.GetById(id);
                        model.Url = storeFileName;
                        bookRepository.Update(model);
                        unitOfWork.Commit();
                    }

                });
            }
        }


        [HttpGet, ActionName("Download")]
        public ActionResult Download(Int64? id)
        {
            if (!id.HasValue) { return View("Index"); }
            var book = bookRepository.GetById(id.Value);
            if (ReferenceEquals(book, null))
            {
                return RedirectToAction("Index");
            }
            var fileName = book.Url;
            if (string.IsNullOrEmpty(fileName))
            {
                return RedirectToAction("Index");
            }

            var uploadPath = Server.MapPath("~/APP_Data/uploads");
            var fullPath = uploadPath + Path.DirectorySeparatorChar + fileName;
            if (!FileHelper.Exist(fullPath))
            {
                return Content("<script type='text/javaScript'>alert('未上传或已删除');location.href='/';</script>");
            }

            return File(new FileStream(fullPath, FileMode.Open, FileAccess.Read), "text/plain", fileName);
        }

        /// <summary>
        /// 获取删除书籍信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteBook(int id)
        {
            var entity = bookRepository.GetById(id);
            if (ReferenceEquals(entity, null))
            {
                return RedirectToAction("Index");
            }

            var model = Mapper.Map<Book, BookDTO>(entity);

            return View(model);
        }


        /// <summary>
        /// 删除书籍
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("DeleteBook")]
        public ActionResult ConfirmDeleteBook(int id)
        {
            var model = bookRepository.GetById(id);
            bookRepository.Delete(model);
            unitOfWork.Commit();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 书籍概述
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DetailBook(int id)
        {
            var model = bookRepository.GetById(id);

            var bookDTO = Mapper.Map<Book, BookDTO>(model);

            return View(bookDTO);
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }

    }
}