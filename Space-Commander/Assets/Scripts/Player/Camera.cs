using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    [SerializeField]
    GameObject ship

    //Pour coller la camera au joueur
    void Update() => transform.position = new Vector3(ship.transform.position.x, ship.transform.position.y, transform.position.z);
}
