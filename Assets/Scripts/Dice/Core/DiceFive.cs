public class DiceFive : BaseDice {
  public override DiceType Type => DiceType.DiceFive;

  protected override DiceVisualType GetCurrentVisualType() => DiceVisualType.DiceFive;
}
