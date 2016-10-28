using UnityEngine;
using System.Collections;
using DTool;

public class GameManager : MonoBehaviour
{
    public LiveObject Player;
    public Vector3 PlayerStartPos = new Vector3(0, 1, 0);

    PlayerController playerController;

    void Awake()
    {

    }

    void Start()
    {
        PlayerInput pi = this.GetComponent<PlayerInput>();
        pi.Init(pi);

        Player.Init(IDBuilder.GetID(), 100, PlayerStartPos);
        playerController = Player.GetComponent<PlayerController>();
        playerController.Init();
    }

    void Update()
    {
     
    }
}
