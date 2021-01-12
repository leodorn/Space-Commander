using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireUpgrade : UpgradeBase, IUpgrade
{
    [SerializeField]
    float fireProbabiltyBonus;
    public void UpgradePlayer()
    {
        Player.instance.fireProbabily += fireProbabiltyBonus;
    }
}
