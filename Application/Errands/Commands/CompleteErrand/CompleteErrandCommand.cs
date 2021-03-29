using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Errands.Commands.CompleteErrand {
  public class CompleteErrandCommand : IRequest {

    public class CompleteErrandCommandHandler : IRequestHandler<CompleteErrandCommand> {
      public async Task<Unit> Handle(CompleteErrandCommand request, CancellationToken cancellationToken) {


        return Unit.Value;
      }
    }

  }
}
