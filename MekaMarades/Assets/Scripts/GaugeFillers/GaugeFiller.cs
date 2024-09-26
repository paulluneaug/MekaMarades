using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GaugeFiller : MonoBehaviour
{
    public abstract float GetAdditionalFilling(float deltaTime);
}
