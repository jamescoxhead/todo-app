using Microsoft.AspNetCore.Identity;

namespace TodoApp.Application.UnitTests;

public static class TestHelpers
{
    public static Mock<UserManager<TUser>> CreateMockUserManager<TUser>() where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();
        var manager = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);

        manager.Object.UserValidators.Add(new UserValidator<TUser>());
        manager.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

        return manager;
    }
}
