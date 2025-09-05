using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceInputQueue : MonoBehaviour
{
  [Header("Capacity")]
  [SerializeField, Range(1, 5)] private int length = 5;
  private readonly Queue<Dice> queue = new();
  public int Capacity => length;
  public int Count => queue.Count;

  public bool IsValidPolicy() => Count < Capacity;

  public bool Enqueue(Dice item)
  {
    if (!IsValidPolicy()) return false;
    queue.Enqueue(item);
    return true;
  }

  public bool Dequeue(out Dice item)
  {
    if (queue.Count == 0)
    {
      item = null;
      return false;
    }

    item = queue.Dequeue();
    return true;
  }

  public Dice Peek() => queue.Peek();
}