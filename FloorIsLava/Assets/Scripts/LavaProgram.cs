// Note by Leo:
// Since there is a general fear against to modify code that works for level 1, this script is a way to reuse LavaFloorBehaviour.cs without modifying it.
// I know it's not the best programming practice, it's what I can do given the circumstances.
// LavaFloorBehaviour.cs has the UpwardsSpeed serialized field
// Actually that script considers a speed called "VelocityMultiplier"
// "VelocityMultiplier" can take 3 possible values: Normal=0.15*UpwardsSpeed, Fast=1.4*UpwardsSpeed, GameOverFast=15*UpwardsSpeed
// LavaFloorBehaviour_DIRTY.cs overwrites "Normal" speed with "publicSpeed"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaProgram : MonoBehaviour
{
    [SerializeField]
    private GameObject lava;
    [SerializeField]
    private LavaFloorBehaviour_DIRTY lavaClass; // A script component of lava

    // Start is called before the first frame update
    void Start()
    {
        lavaClass.publicSpeedOn = true;
        lavaClass.publicSpeed=0.03f; // It is like UpwardsSpeed=0.2 (0.2*.15=0.03)
    }
}
