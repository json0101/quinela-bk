using MediatR;
using Quinela.Application.Common.Results;
using UserApp.Service.Services.UsersScreens;

namespace Quinela.Application.Features.Auth.Menu
{
    public sealed record GetUserMenuQuery(int UserId) : IRequest<Result<object>>;

    internal sealed class GetUserMenuQueryHandler : IRequestHandler<GetUserMenuQuery, Result<object>>
    {
        private readonly IUserScreenService _userScreenService;

        public GetUserMenuQueryHandler(IUserScreenService userScreenService)
        {
            _userScreenService = userScreenService;
        }

        public Task<Result<object>> Handle(GetUserMenuQuery request, CancellationToken ct)
        {
            var menu = _userScreenService.GetMenu(request.UserId);
            return Task.FromResult(Result.Success<object>(menu));
        }
    }
}
