using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SeguimientoCRUD
{
    public class SeguimientoGetAll
    {
        public class GetAll : IRequest<List<Seguimientos>>
        {

        }

        public class Handler : IRequestHandler<GetAll, List<Seguimientos>>
        {
            private readonly BdContext _bdContext;

            public Handler(BdContext bdContext)
            {
                _bdContext = bdContext;
            }

            public async Task<List<Seguimientos>> Handle(GetAll request, CancellationToken cancellationToken)
            {
                var seguimiento = await _bdContext.Seguimiento.ToListAsync();
                return seguimiento;
            }
        }


    }
}
