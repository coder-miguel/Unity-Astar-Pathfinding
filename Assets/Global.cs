using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static GameGrid grid;
    public static GameObject player;
    public void Awake()
    {
        grid   = GetComponent<GameGrid>();
        player = GameObject.Find("Player");
    }

}
