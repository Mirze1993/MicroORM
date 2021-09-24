using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroORM.Mediator
{
    public partial class Mediator
    {

        public Resp ToDo<Req, Resp>(Req req) where Req : IRequest where Resp : IResponse
        {
            var t = typeof(IRequestHandler<,>).MakeGenericType(typeof(Req), typeof(Resp));
            if (ORMConfig.domain == null) 
                return default(Resp);
            var currentType = ORMConfig.domain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => t.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).FirstOrDefault();

            var ins = Activator.CreateInstance(currentType);
            return ((IRequestHandler<Req, Resp>)ins).Handler(req);
        }
    }
}
