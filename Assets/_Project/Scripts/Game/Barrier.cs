using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public static event System.Action<int> onCharge;

    public Colors colorID;
    public ForceBarrier forceBarrierPrefab;

    private bool _charging;
    private float _charge = 100;

    private void Update()
    {
        if(!_charging && Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.PlaySFX(SFXOccurrence.FORCE_BARRIER);
            Instantiate(forceBarrierPrefab, transform.position, transform.rotation).Initialize(transform.up);
            _charge = 0f;
            _charging = true;
        }

        if(_charging)
        {
            _charge = Mathf.Clamp(_charge + 30 * Time.deltaTime, 0, 100);
            onCharge?.Invoke((int)_charge);

            if(_charge == 100)
            {
                _charging = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D p_other)
    {
        if (p_other.tag == "Laser")
        {
            Laser __laser = p_other.GetComponent<Laser>();

            if(__laser.belongPlayer)
            {
                __laser.ChangeColor(colorID);
            }
            else
            {
                if (__laser.colorID == colorID && !__laser.reflected)
                {
                    AudioManager.PlaySFX(SFXOccurrence.LASER_EXPLOSION, 0, 1.2f);
                    CameraManager.ShakeScreen(0.1f, 0.1f);
                    __laser.Reverse();
                }
            }
        }
    }
}
