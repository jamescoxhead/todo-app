using Microsoft.AspNetCore.Identity;

namespace TodoApp.Application.UnitTests;

public static class TestHelpers
{
    public static UserManager<TUser> CreateMockUserManager<TUser>() where TUser : class
    {
        var store = Substitute.For<IUserStore<TUser>>();
        var manager = Substitute.For<UserManager<TUser>>(store, null, null, null, null, null, null, null, null);

        manager.UserValidators.Add(new UserValidator<TUser>());
        manager.PasswordValidators.Add(new PasswordValidator<TUser>());

        return manager;
    }
}
