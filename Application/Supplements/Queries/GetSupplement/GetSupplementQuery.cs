using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Supplements.Queries.GetSupplement
{
    public record GetSupplementQuery : IRequest<SupplementDto>
    {
        public GetSupplementQuery(Guid supplementId)
        {
            SupplementId = supplementId;
        }

        public Guid SupplementId { get; init; }

        public class GetSupplementQueryHandler : BaseRequestHandler<GetSupplementQuery, SupplementDto>
        {
            private readonly IMapper mapper;
            private readonly IMemoryCache cache;
            private readonly IDeratControlDbContext db;

            public GetSupplementQueryHandler(
                ICurrentDateService currentDateService,
                ICurrentUserProvider currentUserProvider,
                IMapper mapper,
                IMemoryCache cache,
                IDeratControlDbContext db) : base(currentDateService, currentUserProvider)
            {
                this.mapper = mapper;
                this.cache = cache;
                this.db = db;
            }

            protected override async Task<SupplementDto> Handle(RequestContext context, GetSupplementQuery request, CancellationToken cancellationToken)
            {
                var cached = cache.Get<List<SupplementDto>>($"{nameof(Supplement)}-{context.CurrentUser.UserId}");

                if (cached is null)
                {
                    return await db
                    .Supplements
                    .ProjectTo<SupplementDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(t => t.SupplementId == request.SupplementId) ?? throw new NotFoundException();
                }

                return cached.Find(c => c.SupplementId == request.SupplementId);
            }
        }
    }
}