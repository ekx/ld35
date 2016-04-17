using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
    public void RemoveObstacle()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        GameController.Instance.OnGameOver();
    }
}
