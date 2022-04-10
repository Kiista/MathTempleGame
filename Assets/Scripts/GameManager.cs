using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public PlayerData player1;
    public PlayerData player2;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        player1 = new PlayerData();
        player2 = new PlayerData();
    }

}
