using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Api.Context
{
    public class WorkContext : IWorkContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WorkContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }        
        public Cliente Client {
            get { return GetClient(); }
        }

        private Cliente GetClient()
        {
            return GetLoggedClient();
        }

        protected T GetHttpContextFeature<T>()
        {
            return _httpContextAccessor.HttpContext?.Request == null
                ? default
                : _httpContextAccessor.HttpContext.Features.Get<T>();
        }

        private Cliente GetLoggedClient()
        {
            var claims = GetClaims();

            return claims != null ? new Cliente
            {
                Id = Guid.Parse(claims.FindFirst("Id").Value),
            } : null;
        }

        private ClaimsPrincipal GetClaims()
        {
            return _httpContextAccessor?.HttpContext?.User;
        }
    }
}
