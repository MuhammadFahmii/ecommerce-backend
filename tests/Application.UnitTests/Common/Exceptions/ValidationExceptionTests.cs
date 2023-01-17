// ------------------------------------------------------------------------------------
// ValidationExceptionTests.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentValidation.Results;
using netca.Application.Common.Exceptions;
using NUnit.Framework;

namespace netca.Application.UnitTests.Common.Exceptions;

/// <summary>
/// ValidationExceptionTests
/// </summary>

public class ValidationExceptionTests
{
    /// <summary>
    /// DefaultConstructorCreatesAnEmptyErrorDictionary
    /// </summary>
    [Test]
    public void DefaultConstructorCreatesAnEmptyErrorDictionary()
    {
        var actual = new ValidationException().Errors;

        actual?.Keys.Should().BeEquivalentTo(Array.Empty<string>());
    }

    /// <summary>
    /// SingleValidationFailureCreatesASingleElementErrorDictionary
    /// </summary>
    [Test]
    public void SingleValidationFailureCreatesASingleElementErrorDictionary()
    {
        var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Age", "must be over 18"),
            };

        var actual = new ValidationException(failures).Errors;

        actual?.Keys.Should().BeEquivalentTo(new string[] { "Age" });
        actual?["Age"].Should().BeEquivalentTo(new string[] { "must be over 18" });
    }

    /// <summary>
    /// MultipleValidationFailureForMultiplePropertiesCreatesAMultipleElementErrorDictionaryEachWithMultipleValues
    /// </summary>
    [Test]
    public void MultipleValidationFailureForMultiplePropertiesCreatesAMultipleElementErrorDictionaryEachWithMultipleValues()
    {
        var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Age", "must be 18 or older"),
                new ValidationFailure("Age", "must be 25 or younger"),
                new ValidationFailure("Password", "must contain at least 8 characters"),
                new ValidationFailure("Password", "must contain a digit"),
                new ValidationFailure("Password", "must contain upper case letter"),
                new ValidationFailure("Password", "must contain lower case letter"),
            };

        var actual = new ValidationException(failures).Errors;

        actual?.Keys.Should().BeEquivalentTo(new string[] { "Password", "Age" });

        actual?["Age"].Should().BeEquivalentTo(new string[]
        {
                "must be 25 or younger",
                "must be 18 or older",
        });

        actual?["Password"].Should().BeEquivalentTo(new string[]
        {
                "must contain lower case letter",
                "must contain upper case letter",
                "must contain at least 8 characters",
                "must contain a digit",
        });
    }
}