using Application.ViewModels;
using AutoMapper;
using Domain.Entities;

namespace Application.AutoMapper
{
    public class ModelToViewModel : Profile
    {
        public ModelToViewModel()
        {
            #region AutoMapper


            #region Client
            CreateMap<Cliente, ClienteViewModel>().ReverseMap();
            #endregion

            #endregion AutoMapper
        }
    }
}
