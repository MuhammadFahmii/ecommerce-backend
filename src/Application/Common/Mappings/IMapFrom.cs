// ------------------------------------------------------------------------------------
// IMapFrom.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using AutoMapper;

namespace ecommerce.Application.Common.Mappings;

/// <summary>
/// IMapFrom
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IMapFrom<T>
{
    /// <summary>
    /// Mapping
    /// </summary>
    /// <param name="profile"></param>
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
}