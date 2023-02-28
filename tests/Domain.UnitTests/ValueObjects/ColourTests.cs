// ------------------------------------------------------------------------------------
// ColourTests.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------
using FluentAssertions;
using ecommerce.Domain.Exceptions;
using ecommerce.Domain.ValueObjects;
using NUnit.Framework;

namespace ecommerce.Domain.UnitTests.ValueObjects;

/// <summary>
/// ColourTests
/// </summary>
public class ColourTests
{
    /// <summary>
    /// ShouldReturnCorrectColourCode
    /// </summary>
    [Test]
    public void ShouldReturnCorrectColourCode()
    {
        const string code = "#FFFFFF";

        var colour = Colour.From(code);

        colour.Code.Should().Be(code);
    }
    
    /// <summary>
    /// ToStringReturnsCode
    /// </summary>
    [Test]
    public void ToStringReturnsCode()
    {
        var colour = Colour.White;

        colour.ToString().Should().Be(colour.Code);
    }
    
    /// <summary>
    /// ShouldPerformImplicitConversionToColourCodeString
    /// </summary>
    [Test]
    public void ShouldPerformImplicitConversionToColourCodeString()
    {
        string code = Colour.White;

        code.Should().Be("#FFFFFF");
    }
    
    /// <summary>
    /// ShouldPerformExplicitConversionGivenSupportedColourCode
    /// </summary>
    [Test]
    public void ShouldPerformExplicitConversionGivenSupportedColourCode()
    {
        var colour = (Colour)"#FFFFFF";

        colour.Should().Be(Colour.White);
    }
    
    /// <summary>
    /// ShouldThrowUnsupportedColourExceptionGivenNotSupportedColourCode
    /// </summary>
    [Test]
    public void ShouldThrowUnsupportedColourExceptionGivenNotSupportedColourCode()
    {
        FluentActions.Invoking(() => Colour.From("##FF33CC"))
            .Should().Throw<UnsupportedColourException>();
    }
}