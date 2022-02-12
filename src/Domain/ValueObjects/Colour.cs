// ------------------------------------------------------------------------------------
// Colour.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using netca.Domain.Common;
using netca.Domain.Exceptions;

namespace netca.Domain.ValueObjects;

/// <summary>
/// Colour
/// </summary>
public class Colour : ValueObject
{
    static Colour()
    {
    }

    private Colour()
    {
    }

    private Colour(string code)
    {
        Code = code;
    }

    /// <summary>
    /// Colour
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static Colour From(string code)
    {
        var colour = new Colour { Code = code };

        if (!SupportedColours.Contains(colour))
        {
            throw new UnsupportedColourException(code);
        }

        return colour;
    }

    /// <summary>
    /// Gets white
    /// </summary>
    public static Colour White => new Colour("#FFFFFF");

    /// <summary>
    /// Gets red
    /// </summary>
    public static Colour Red => new Colour("#FF5733");

    /// <summary>
    /// Gets orange
    /// </summary>
    public static Colour Orange => new Colour("#FFC300");

    /// <summary>
    /// Gets yellow
    /// </summary>
    public static Colour Yellow => new Colour("#FFFF66");

    /// <summary>
    /// Gets green
    /// </summary>
    public static Colour Green => new Colour("#CCFF99");

    /// <summary>
    /// Gets blue
    /// </summary>
    public static Colour Blue => new Colour("#6666FF");

    /// <summary>
    /// Gets purple
    /// </summary>
    public static Colour Purple => new Colour("#9966CC");

    /// <summary>
    /// Gets grey
    /// </summary>
    public static Colour Grey => new Colour("#999999");

    /// <summary>
    /// Gets code
    /// </summary>
    public string Code { get; private set; } = null!;

    /// <summary>
    /// string
    /// </summary>
    /// <param name="colour"></param>
    /// <returns></returns>
    public static implicit operator string(Colour colour)
    {
        return colour.ToString();
    }

    /// <summary>
    /// Colour
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static explicit operator Colour(string code)
    {
        return From(code);
    }

    /// <summary>
    /// ToString
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Code;
    }

    /// <summary>
    /// Gets supportedColours
    /// </summary>
    protected static IEnumerable<Colour> SupportedColours
    {
        get
        {
            yield return White;
            yield return Red;
            yield return Orange;
            yield return Yellow;
            yield return Green;
            yield return Blue;
            yield return Purple;
            yield return Grey;
        }
    }

    /// <summary>
    /// GetEqualityComponents
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
}