using UnityEngine;
using TMPro;

public class CheckPoint : MonoBehaviour
{
    Path path;
    [SerializeField] TMP_Text levels;

    int NectLevel;

    void Start()
    {
        path = FindFirstObjectByType<Path>();
        NectLevel = path.LevelReturner();
        LevelIncreaser();
    }
    
    void LevelIncreaser()
    {
        levels.text = "LEVEL : " + NectLevel; 
    }
}
