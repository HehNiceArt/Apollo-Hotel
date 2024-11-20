using UnityEngine;

public class CheckLevel : MonoBehaviour
{
    [SerializeField] bool firstLevel;
    [SerializeField] bool secondLevel;
    [SerializeField] bool thirdLevel;
    [SerializeField] bool fourthLevel;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Level"))
        {
            Level level = other.gameObject.GetComponent<Level>();
            level.LoadScene();
        }
    }
    public void CheckWhichLevel(GameObject level)
    {
        // if(level.layer == "1stLayer")
        //     firstLevel = true;
        // }
        // if(level.CompareTag("2ndLevel")){
        //     firstLevel = true;
        // }
        // if(level.CompareTag("3rdLevel")){
        //     firstLevel = true;
        // }
        // if(level.CompareTag("4thLevel")){
        //     firstLevel = true;
        // }
    }
}