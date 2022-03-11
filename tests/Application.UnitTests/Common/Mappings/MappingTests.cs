// ------------------------------------------------------------------------------------
// MappingTests.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;
using AutoMapper;
using FluentAssertions;
using netca.Application.Common.Mappings;
using netca.Application.Common.Vms;
using netca.Domain.Entities;
using NUnit.Framework;

namespace netca.Application.UnitTests.Common.Mappings;

/// <summary>
/// MappingTests
/// </summary>

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    /// <summary>
    /// MappingTests
    /// </summary>
    public MappingTests()
    {
        _configuration = new MapperConfiguration(config => 
            config.AddProfile<MappingProfile>());

        _mapper = _configuration.CreateMapper();
    }

    /// <summary>
    /// ShouldHaveValidConfiguration
    /// </summary>
    [Test]
    public void ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    /// <summary>
    /// ShouldSupportMappingFromSourceToDestination
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    [Test]
    [TestCase(typeof(TodoList), typeof(TodoListVm))]
    [TestCase(typeof(TodoItem), typeof(TodoItem))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var test = _mapper.Map(GetInstanceOf(source), source, destination);
        test.Should().BeAssignableTo(destination);
    }
    
    /// <summary>
    /// GetInstanceOf
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private object GetInstanceOf(Type type)
    {
        return type.GetConstructor(Type.EmptyTypes) != null ? Activator.CreateInstance(type)! : FormatterServices.GetUninitializedObject(type);
    }
}