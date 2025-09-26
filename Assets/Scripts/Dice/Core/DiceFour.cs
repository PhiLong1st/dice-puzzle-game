public class DiceFour : BaseDice {
  public override DiceType Type => DiceType.DiceFour;

  protected override DiceVisualType GetCurrentVisualType() => DiceVisualType.DiceFour;
  public override DiceType? GetNextDiceType() => DiceType.DiceFive;
}
