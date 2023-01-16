// ------------------------------------------------------------------------------------
// RequestLoggerTests.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using netca.Application.Common.Behaviors;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Models;
using netca.Application.TodoItems.Commands.CreateTodoItem;
using NUnit.Framework;

namespace netca.Application.UnitTests.Common.Behaviors;

/// <summary>
/// RequestLoggerTests
/// </summary>
public class RequestLoggerTests
{
    private Mock<ILogger<CreateTodoItemCommand>> _logger = null!;
    private Mock<IUserAuthorizationService> _userAuthorizationService = null!;
    private Mock<AppSetting> _appSetting = null!;

    /// <summary>
    /// 
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CreateTodoItemCommand>>();
        _userAuthorizationService = new Mock<IUserAuthorizationService>();
        _userAuthorizationService.Setup(x => x.GetAuthorizedUser()).Returns(MockData.GetAuthorizedUser());
        _appSetting = new Mock<AppSetting>();
    }

    /// <summary>
    /// ShouldCallGetUserNameAsyncOnceIfAuthenticated
    /// </summary>
    [Test]
    public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
    {
        var requestLogger = new LoggingBehavior<CreateTodoItemCommand>(_logger.Object, _userAuthorizationService.Object, _appSetting.Object);
        await requestLogger.Process(new CreateTodoItemCommand { ListId = Guid.NewGuid(), Title = "title" }, new CancellationToken());
        _userAuthorizationService.Verify(i => i.GetAuthorizedUser(), Times.Once);
    }

    /// <summary>
    /// ShouldAuthorizedUseSameAsMockAuthorizedUser
    /// </summary>
    [Test]
    public async Task ShouldAuthorizedUseSameAsMockAuthorizedUser()
    {
        var requestLogger = new LoggingBehavior<CreateTodoItemCommand>(_logger.Object, _userAuthorizationService.Object, _appSetting.Object);
        await requestLogger.Process(new CreateTodoItemCommand { ListId = Guid.NewGuid(), Title = "title" },
            new CancellationToken());
        var user = _userAuthorizationService.Object.GetAuthorizedUser();
        Assert.AreEqual(user.UserId, MockData.GetAuthorizedUser().UserId);
        Assert.AreEqual(user.UserName, MockData.GetAuthorizedUser().UserName);
    }
}