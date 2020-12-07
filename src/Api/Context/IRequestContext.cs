using System;

namespace Api.Context
{
    public interface IRequestContext
    {
        public Guid UserId { get; set; }
    }
    public class RequestContext: IRequestContext
    {
        public Guid UserId { get; set; }
    }
}