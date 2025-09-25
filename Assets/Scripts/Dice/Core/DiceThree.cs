public class DiceThree : BaseDice {
  public override DiceType Type => DiceType.DiceThree;

  protected override DiceVisualType GetCurrentVisualType() => DiceVisualType.DiceThree;
  public override DiceType? GetNextDiceType() => DiceType.DiceFour;
}
