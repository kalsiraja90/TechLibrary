using AutoMapper;
using System;
using TechLibrary.Domain;
using TechLibrary.Models;

namespace TechLibrary.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Book, BookResponse>().ForMember(x => x.Descr, opt => opt.MapFrom(src => src.ShortDescr))
                                           .ForMember(x => x.PublishedDate, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.PublishedDate) ? Convert.ToDateTime(src.PublishedDate).ToString("yyyy-MM-dd") : string.Empty));
            CreateMap<BookRequest, Book>().ForMember(x => x.ShortDescr, opt => opt.MapFrom(src => src.Descr));
            CreateMap<BookResult, BookResultResponse>().ForMember(x => x.Items, opt => opt.MapFrom(src => src.Items));
        }
    }
}