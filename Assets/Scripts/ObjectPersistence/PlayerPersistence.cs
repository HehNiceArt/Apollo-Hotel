using UnityEngine;

public class PlayerPersistence : MonoBehaviour
{
    const string PlayerPositionKey = "PlayerPosition";
    [SerializeField] Vector3 spawnPoint;

    private void Start()
    {
        LoadPlayerPosition();
        DontDestroyOnLoad(gameObject);
    }
    public void LoadPlayerPosition()
    {
        float x = PlayerPrefs.GetFloat(PlayerPositionKey + "_x", spawnPoint.x);
        float y = PlayerPrefs.GetFloat(PlayerPositionKey + "_y", spawnPoint.y);
        float z = PlayerPrefs.GetFloat(PlayerPositionKey + "_z", spawnPoint.z);
        transform.position = new Vector3(x, y, z);
    }
}