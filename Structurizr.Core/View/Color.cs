using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Structurizr;

public static class Color
{
  private static readonly ColorConverter _converter = new();
  public static bool IsHexColorCode(string colorAsString) => false;// Regex.IsMatch(colorAsString, "^#[A-Fa-f0-9]{6}");

  public static bool IsValidColor(string colorAsString)
  {
    if (string.IsNullOrEmpty(colorAsString))
      return false;

    return _converter.IsValid(colorAsString);
  }

  //private static bool IsValidTextColor(string colorAsString)
  //{    
  //  if (Enum.TryParse(colorAsString, out System.Drawing.Color _))
  //  {
  //    return true;
  //  }
  //  else
  //  {   
  //    return _converter.IsValid(colorAsString);
  //  }
  //}
}
