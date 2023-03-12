using System;
using MediatR;

namespace Ligric.Service.AuthService.Infrastructure.Persistence.Commands
{
	public interface ICommand : IRequest
	{
		Guid Id { get; }
	}

	public interface ICommand<out TResult> : IRequest<TResult>
	{
		Guid Id { get; }
	}
}