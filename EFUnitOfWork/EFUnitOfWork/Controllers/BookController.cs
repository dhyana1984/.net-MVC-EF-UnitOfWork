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
    }
}