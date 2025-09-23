public class DiceOne : BaseDice {
  public override DiceType Type => DiceType.DiceOne;

  protected override DiceVisualType GetCurrentVisualType() => DiceVisualType.DiceOne;
}
