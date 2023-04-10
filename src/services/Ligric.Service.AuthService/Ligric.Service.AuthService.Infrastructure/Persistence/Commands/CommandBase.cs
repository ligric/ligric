﻿using System;

namespace Ligric.Service.AuthService.Infrastructure.Persistence.Commands
{
	public class CommandBase : ICommand
	{
		public Guid Id { get; }

		public CommandBase()
		{
			Id = Guid.NewGuid();
		}

		protected CommandBase(Guid id)
		{
			Id = id;
		}
	}

	public abstract class CommandBase<TResult> : ICommand<TResult>
	{
		public Guid Id { get; }

		protected CommandBase()
		{
			Id = Guid.NewGuid();
		}

		protected CommandBase(Guid id)
		{
			Id = id;
		}
	}
}