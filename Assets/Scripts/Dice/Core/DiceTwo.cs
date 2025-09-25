public class DiceTwo : BaseDice {
  public override DiceType Type => DiceType.DiceTwo;

  protected override DiceVisualType GetCurrentVisualType() => DiceVisualType.DiceTwo;
  public override DiceType? GetNextDiceType() => DiceType.DiceThree;
}

