using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonRotation : MonoBehaviour
{
    public Vector3 _maxRotation;
    public Vector3 _minRotation;
    private float offset = -51.6f;
    public GameObject ShootPoint;
    public GameObject Bullet;
    public float ProjectileSpeed = 0;
    public float MaxSpeed;
    public float MinSpeed;
    public GameObject PotencyBar;
    private float initialScaleX;

    private void Awake()
    {
        initialScaleX = PotencyBar.transform.localScale.x;
    }
    void Update()
    {
        //var mousePos = Input.mousePosition;
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;
        var angle = (Mathf.Atan2(direction.y, direction.x) * 180f / Mathf.PI + offset);

        if (angle < _minRotation.z)
        {
            angle = _minRotation.z;
            direction = new Vector2(1, 0);
        }
        else if (angle > _maxRotation.z)
        {
            angle = _maxRotation.z;
            direction = new Vector2(0, 1);
        }

        transform.rotation = Quaternion.Euler(0, 0, angle);
        direction.Normalize();

        if (Input.GetMouseButton(0))
        {
            if (ProjectileSpeed < MaxSpeed)
            {
                ProjectileSpeed += Time.deltaTime * 4;
            }
            if (ProjectileSpeed < MinSpeed)
            {
                ProjectileSpeed = 1;
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            var projectile = Instantiate(Bullet, ShootPoint.transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = direction * ProjectileSpeed;
            ProjectileSpeed = 0;
        }
        CalculateBarScale();

    }
    public void CalculateBarScale()
    {
        PotencyBar.transform.localScale = new Vector3(Mathf.Lerp(0, initialScaleX, ProjectileSpeed / MaxSpeed),
            PotencyBar.transform.localScale.y,
            PotencyBar.transform.localScale.z);
    }
}
