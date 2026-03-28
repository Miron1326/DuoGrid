using UnityEngine;

public class StilisticManager : MonoBehaviour
{
    public static StilisticManager instance;
    public StilisticType Currenttype;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToStandartStilistic()
    {
        Currenttype = StilisticType.Standart;
    }

    public void ChangeToDesertStilistic()
    {
        Currenttype = StilisticType.Desert;
    }
}
public enum StilisticType
{
    Standart,
    Desert
}
