using System;
using System.Collections.Generic;

public enum IncomingDiceShape {
  DominoV,        // 2x1 vertical
  DominoH,        // 1x2 horizontal
  DiagonalPairA,  // 2x2 diagonal (note: not orthogonally connected)
  Square2x2,      // 2x2 full
  DiagonalPairB,  // duplicate of A (consider removing/aliasing)
  L_TopRight_3x2, // 3x2 L (arm to the right at the top)
  Line3H,         // 1x3 line
  Line3V,         // 3x1 line
  Corner_TL_2x2,  // 2x2 corner (missing BR)
  L_BottomRight_3x2,
  L_DownLeft_2x3,
  L_DownRight_2x3,
  T_Down_2x3,     // bar on top, stem down
  T_Right_3x2,    // bar vertical, stem to right
  S_2x3,          // S-shape
  Z_2x3           // Z-shape
}

public static class IncomingDiceTemplate {
  private static readonly Dictionary<IncomingDiceShape, int[,]> storage = new() {
    // Originals (renamed)
    { IncomingDiceShape.DominoV,        new int[2,2] { { 1, 0 }, { 1, 0 } } },           // Type1
    { IncomingDiceShape.DominoH,        new int[2,2] { { 1, 1 }, { 0, 0 } } },           // Type2
    { IncomingDiceShape.DiagonalPairA,  new int[2,2] { { 1, 0 }, { 0, 1 } } },           // Type3
    { IncomingDiceShape.Square2x2,      new int[2,2] { { 1, 1 }, { 1, 1 } } },           // Type4
    { IncomingDiceShape.DiagonalPairB,  new int[2,2] { { 1, 0 }, { 0, 1 } } },           // Type5 (duplicate of A)
    { IncomingDiceShape.L_TopRight_3x2, new int[3,2] { { 1, 1 }, { 1, 0 }, { 1, 0 } } }, // Type6

    // Additions (≤ 3x3)
    { IncomingDiceShape.Line3H,         new int[1,3] { { 1, 1, 1 } } },                   // Type7
    { IncomingDiceShape.Line3V,         new int[3,1] { { 1 }, { 1 }, { 1 } } },          // Type8

    { IncomingDiceShape.Corner_TL_2x2,  new int[2,2] { { 1, 1 }, { 1, 0 } } },           // Type9 (missing bottom-right)
    { IncomingDiceShape.L_BottomRight_3x2, new int[3,2] { { 1, 0 }, { 1, 0 }, { 1, 1 } } }, // Type10
    { IncomingDiceShape.L_DownLeft_2x3, new int[2,3] { { 1, 1, 1 }, { 1, 0, 0 } } },     // Type11
    { IncomingDiceShape.L_DownRight_2x3,new int[2,3] { { 1, 1, 1 }, { 0, 0, 1 } } },     // Type12

    { IncomingDiceShape.T_Down_2x3,     new int[2,3] { { 1, 1, 1 }, { 0, 1, 0 } } },     // Type13
    { IncomingDiceShape.T_Right_3x2,    new int[3,2] { { 1, 0 }, { 1, 1 }, { 1, 0 } } }, // Type14

    { IncomingDiceShape.S_2x3,          new int[2,3] { { 0, 1, 1 }, { 1, 1, 0 } } },     // Type15
    { IncomingDiceShape.Z_2x3,          new int[2,3] { { 1, 1, 0 }, { 0, 1, 1 } } },     // Type16
  };

  public static int[,] RandomTemplate() {
    var random = EnumUtils.RandomEnumValue<IncomingDiceShape>();
    return GetTemplate(random);
  }

  public static int[,] GetTemplate(IncomingDiceShape shape) =>
    (int[,])storage[shape].Clone();
}
