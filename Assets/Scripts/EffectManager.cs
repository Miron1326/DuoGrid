using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private ParticleSystem _damage1;
    private ParticleSystem _damage2;
    private ParticleSystem _explosion;
    private void Start()
    {
        _damage1 = GameObject.Find("PlayerOneDamageCount").GetComponent<ParticleSystem>();
        _damage2 = GameObject.Find("PlayerTwoDamageCount").GetComponent<ParticleSystem>();
        _explosion = GameObject.Find("ExplosionEffect").GetComponent<ParticleSystem>();
    }
    public void Player1Damaged(Vector3 PositionEffect)
    {
        _damage1.gameObject.transform.position = PositionEffect;
        _damage1.Play();
    }
    public void Player2Damaged(Vector3 PositionEffect)
    {
        _damage2.gameObject.transform.position = PositionEffect;
        _damage2.Play();
    }
    public void PlayerExplosion(Vector3 PositionEffect) 
    {
        _explosion.gameObject.transform.position = PositionEffect;
        _explosion.Play();
    }
}
