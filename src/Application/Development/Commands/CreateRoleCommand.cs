﻿using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ecommerce.Application.Common.Extensions;
using ecommerce.Application.Common.Interfaces;
using ecommerce.Application.Common.Models;
using ecommerce.Application.Dtos;

namespace ecommerce.Application.Development.Commands;

/// <summary>
/// CreateRoleCommand
/// </summary>
public class CreateRoleCommand : IRequest<DocumentRootJson<ResponseGroupRoleUmsVm>>
{
    /// <summary>
    /// Gets or sets ApplicationId
    /// </summary>
    [BindRequired]
    public Guid ApplicationId { get; set; }

    /// <summary>
    /// Gets or sets ControllerList
    /// </summary>
    [BindRequired]
    public List<ControllerListDto>? ControllerList { get; set; }

    /// <summary>
    /// Handling CreateActivityMasterCommand
    /// </summary>
    public class AddPermissionRoleCommandHandler : IRequestHandler<CreateRoleCommand, DocumentRootJson<ResponseGroupRoleUmsVm>>
    {
        private readonly IUserAuthorizationService _userAuthorizationService;
        private readonly AppSetting _appSetting;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddPermissionRoleCommandHandler"/> class.
        /// </summary>
        /// <param name="userAuthorizationService">Set userAuthorizationService to get User's Attributes</param>
        /// <param name="appSetting">Set dateTime to get Application Setting</param>
        public AddPermissionRoleCommandHandler(IUserAuthorizationService userAuthorizationService, AppSetting appSetting)
        {
            _userAuthorizationService = userAuthorizationService;
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
        /// <returns>Add permission Group to UMS</returns>
        public async Task<DocumentRootJson<ResponseGroupRoleUmsVm>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var roleSettingList = _appSetting.AuthorizationServer.Role;
            var roleList = await _userAuthorizationService.GetRoleListAsync(request.ApplicationId, cancellationToken);
            var groupList = await _userAuthorizationService.GetGroupListAsync(request.ApplicationId, cancellationToken);

            var groupNameList = request?.ControllerList?
                .SelectMany(x => x.Groups)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            var responsePermission = new List<ResponseGroupRoleUms>();

            foreach (var itemRole in roleSettingList!)
            {
                var isGroupValid = true;

                itemRole!.Group = itemRole?.Group?.Distinct().ToList() ?? new();

                var isAllGroup = itemRole?.Group?.Any(x => x.Equals("*"));

                if (!isAllGroup!.Value)
                    isGroupValid = groupNameList!.All(itemRole!.Group!.Contains);

                var groupIds = itemRole!.Group!.Any(x => x.Equals("*")) ?
                    groupList.Where(x => groupNameList!.Contains(x.GroupCode))
                        .Select(x => x.GroupId!.Value)
                        .ToList() :
                    groupList.Where(x => itemRole!.Group!.Contains(x.GroupCode))
                        .Select(x => x.GroupId!.Value)
                        .ToList();

                isGroupValid &= isAllGroup.Value ?
                    groupIds.Count == groupNameList?.Count :
                    groupIds.Count == itemRole?.Group?.Count;

                if (!isGroupValid)
                {
                    responsePermission.Add(new ResponseGroupRoleUms
                    {
                        Name = itemRole?.Name!,
                        ItemList = itemRole?.Group!,
                        Response = new ResponseGroupRoleUmsDto
                        {
                            Request = "Failed",
                            Status = "One or more group is not exist or same as in AppSetting"
                        }
                    });

                    continue;
                }

                var roleId = roleList.Where(x => x.RoleCode.ToLower().Contains(itemRole?.Name?.ToLower()!))
                    .Select(x => x.RoleId)
                    .FirstOrDefault();

                var response = await _userAuthorizationService.CreateRoleAsync(
                    request!.ApplicationId,
                    itemRole?.Name!,
                    roleId,
                    groupIds,
                    cancellationToken);

                responsePermission.Add(new ResponseGroupRoleUms
                {
                    Name = itemRole?.Name!,
                    ItemList = itemRole?.Group!,
                    Response = response
                });
            }

            var result = new ResponseGroupRoleUmsVm { ResponseGroups = responsePermission };

            return JsonApiExtensions.ToJsonApi(result);
        }
    }
}