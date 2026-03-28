using UnityEngine;

public class BloodMoonManager : MonoBehaviour
{
    public static BloodMoonManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void CheckToBloodMoonSpawn()
    {

    }
}
