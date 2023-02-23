using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamereController : MonoBehaviour
{
    [SerializeField]private GameObject player;
    private GameObject[] players;
    [SerializeField]private float yAngularSpeed = 0;
    private void Update()
    {
        this.gameObject.transform.position = player.transform.position;
        RotateCamera();
    }
    public void FindCameraTarget(int player_Turn)
    {
        Debug.Log(player_Turn);
        this.transform.rotation = Quaternion.identity;
        foreach (GameObject unit in players)
        {
            if (unit.GetComponent<Unit>().unit_Num == player_Turn)
            {
                player = unit;
            }
        }
    }
    public void StartCamera()
    {
        this.transform.rotation = Quaternion.identity;
        players = GameObject.FindGameObjectsWithTag("Player");
        player = GameObject.FindGameObjectWithTag("Player");
        FindCameraTarget(1);
    }
    public void RotateCamera()
    {
        if(Input.GetKey(KeyCode.Q))
        {

            this.transform.Rotate(0, yAngularSpeed, 0, Space.Self);
        }
        else if (Input.GetKey(KeyCode.E))
        {

            this.transform.Rotate(0, -yAngularSpeed, 0, Space.Self);
        }
    }
}
