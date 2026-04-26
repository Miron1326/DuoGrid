using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance
    {
        get; private set;
    }

    [SerializeField] AudioSource _cellSpawned;
    [SerializeField] AudioSource _cellSelected;
    [SerializeField] AudioSource _cactusAttack;
    void Start()
    {
        GameManager.Instance.OnCellSpawned += OnCellSpawned;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void OnCellSpawned()
    {
        _cellSpawned.Play();
    }
    public void OnCactusAttack()
    {
        _cactusAttack.Play();
    }
    public void OnCellSelected()
    {
        _cellSelected.Play();
    }
}
