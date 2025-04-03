using System;
using UnityEngine;

public interface IInput
{
    Vector2 MovementInput { get; }

    event Action OnActionInput;
}