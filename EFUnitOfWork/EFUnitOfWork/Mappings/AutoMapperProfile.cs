using AutoMapper;
using EF.Core.Entity;
using EFUnitOfWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace EFUnitOfWork.Mappings
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, BookDTO>();
            CreateMap<BookDTO, Book>();
        }
    }
}