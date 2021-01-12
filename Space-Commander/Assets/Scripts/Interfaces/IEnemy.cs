﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void TakeDammage(int damage);
    void Freeze();
    void Burn();
    void AddHp(int hp);
}
