﻿using Ligric.Application.Configuration.Commands;
using Ligric.Common.Dto;

namespace Ligric.Application.Users.RegisterUser
{
    public class RegisterUserCommand : CommandBase<UserDto>
    {
        public string Login { get; }      

        public string Password { get; }

        public RegisterUserCommand(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}