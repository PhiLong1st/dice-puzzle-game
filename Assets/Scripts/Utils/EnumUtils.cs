using System;

public static class EnumUtils {
  public static T RandomEnumValue<T>() where T : Enum {
    var values = Enum.GetValues(typeof(T));
    int random = UnityEngine.Random.Range(0, values.Length);
    return (T)values.GetValue(random);
  }
}
