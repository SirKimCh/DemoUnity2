using UnityEngine;

public class EnemyIdentifier : MonoBehaviour
{
    public string id;

    private void Awake()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
        }
    }
}