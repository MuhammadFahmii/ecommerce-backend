// ------------------------------------------------------------------------------------
// TodoItemVm.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using AutoMapper;
using netca.Application.Common.Mappings;
using netca.Domain.Entities;
using netca.Domain.Enums;

namespace netca.Application.Common.Vms
{
    /// <summary>
    /// TodoItemVm
    /// </summary>
    public class TodoItemVm : AuditTableVm, IMapFrom<TodoItem>
    {
        /// <summary>
        /// List
        /// </summary>
        public TodoListVm List { get; set; }

        /// <summary>
        /// ListId
        /// </summary>
        public Guid ListId { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Note
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Priority
        /// </summary>
        public PriorityLevel Priority { get; set; }

        /// <summary>
        /// Reminder
        /// </summary>
        public DateTime? Reminder { get; set; }

        /// <summary>
        /// Done
        /// </summary>
        public bool Done { get; set; }

        /// <summary>
        /// Mapping
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TodoItemVm, TodoItem>()
                .ForMember(x => x.DomainEvents, opt => opt.Ignore());
        }
    }
}