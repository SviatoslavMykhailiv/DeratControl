using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common {
  public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse> {
    private readonly ICurrentDateService currentDateService;
    private readonly ICurrentUserProvider currentUserProvider;

    public BaseRequestHandler(ICurrentDateService currentDateService, ICurrentUserProvider currentUserProvider) {
      this.currentDateService = currentDateService;
      this.currentUserProvider = currentUserProvider;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken) {
      return await Handle(new RequestContext(currentUserProvider.User, currentDateService.CurrentDate), request, cancellationToken);
    }

    protected abstract Task<TResponse> Handle(RequestContext context, TRequest request, CancellationToken cancellationToken);
  }

  public abstract class BaseRequestHandler<TRequest> : IRequestHandler<TRequest> where TRequest : IRequest {
    private readonly ICurrentDateService currentDateService;
    private readonly ICurrentUserProvider currentUserProvider;

    public BaseRequestHandler(ICurrentDateService currentDateService, ICurrentUserProvider currentUserProvider) {
      this.currentDateService = currentDateService;
      this.currentUserProvider = currentUserProvider;
    }

    public async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken) {
      return await Handle(new RequestContext(currentUserProvider.User, currentDateService.CurrentDate), request, cancellationToken);
    }

    protected abstract Task<Unit> Handle(RequestContext context, TRequest request, CancellationToken cancellationToken);
  }
}
