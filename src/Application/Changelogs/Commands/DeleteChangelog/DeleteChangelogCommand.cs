// ------------------------------------------------------------------------------------
// DeleteChangelogCommand.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using MediatR;
using ecommerce.Application.Common.Interfaces;
using ecommerce.Application.Common.Models;
using Z.EntityFramework.Plus;

namespace ecommerce.Application.Changelogs.Commands.DeleteChangelog;

/// <summary>
/// DeleteChangelogCommand
/// </summary>
public class DeleteChangelogCommand : IRequest<Unit>
{
    /// <summary>
    /// Handling DeleteChangelogCommand
    /// </summary>
    public class DeleteChangelogCommandHandler : IRequestHandler<DeleteChangelogCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly AppSetting _appSetting;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteChangelogCommandHandler"/> class.
        /// </summary>
        /// <param name="context">Set context to perform CRUD into Database</param>
        /// <param name="appSetting">Set dateTime to get Application Setting</param>
        /// <returns></returns>
        public DeleteChangelogCommandHandler(
            IApplicationDbContext context, AppSetting appSetting)
        {
            _context = context;
            _appSetting = appSetting;
        }

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="request">
        /// The encapsulated request body
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token to perform cancel the operation
        /// </param>
        /// <returns>A bool true or false</returns>
        public async Task<Unit> Handle(DeleteChangelogCommand request, CancellationToken cancellationToken)
        {
            var lifeTime = _appSetting.DataLifetime.Changelog;
            var lifeTimeEpoch  = DateTimeOffset.UtcNow.AddDays(-lifeTime).ToUnixTimeMilliseconds();
            await _context.Changelogs.Where(x => lifeTimeEpoch > x.ChangeDate)
                .DeleteAsync(x => x.BatchSize = 1000, cancellationToken);
            return await Task.FromResult(Unit.Value);
        }
    }
}