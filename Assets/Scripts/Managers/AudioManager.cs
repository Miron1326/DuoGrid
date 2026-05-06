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
    [SerializeField] AudioSource _turretAttack;
    [SerializeField] AudioSource _bloodSuckerAttack;
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
<<<<<<< HEAD

    public void OnTurretAttack()
    {
        _turretAttack.Play();
    }

    public void OnBloodSuckerAttack()
    {
        _bloodSuckerAttack.Play();
    }
=======
>>>>>>> db38b45ec418d9a84c03753d8c7b39ec48e8c611
}
