using UnityEngine;
using System.Collections;

public interface IMovementPhysics
{
    void SetMovementInput(float horizontal, float vertical);
    float DistanceToStop();
}
