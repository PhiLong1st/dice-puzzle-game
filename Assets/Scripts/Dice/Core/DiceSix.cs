public class DiceSix : BaseDice {
  public override DiceType Type => DiceType.DiceSix;

  protected override DiceVisualType GetCurrentVisualType() => DiceVisualType.DiceSix;
  public override DiceType? GetNextDiceType() => null;
}
