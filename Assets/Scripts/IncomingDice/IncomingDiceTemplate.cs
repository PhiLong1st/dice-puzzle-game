using System.Collections.Generic;

public enum IncomingDiceTypeTemplate { Type1, Type2, Type3, Type4, Type5, Type6 };

public class IncomingDiceTemplate
{
  private static readonly Dictionary<IncomingDiceTypeTemplate, int[,]> storage = new() {
    { IncomingDiceTypeTemplate.Type1, new int[1, 1] { { 1 } } },
    { IncomingDiceTypeTemplate.Type2, new int[1, 2] { { 1, 1 } } },
    { IncomingDiceTypeTemplate.Type3, new int[2, 1] { { 1 }, { 1 } } },
    { IncomingDiceTypeTemplate.Type4, new int[2, 2] { { 1, 1 }, { 1, 1 } } },
    { IncomingDiceTypeTemplate.Type5, new int[2, 2] { { 1, 0 }, { 0, 1 } } },
    { IncomingDiceTypeTemplate.Type6, new int[2, 2] { { 0, 1 }, { 1, 0 } } },
  };

  public static int[,] RandomTemplate()
  {
    var randomType = EnumUtils.RandomEnumValue<IncomingDiceTypeTemplate>();
    storage.TryGetValue(randomType, out int[,] template);
    return (int[,])template.Clone();
  }
}