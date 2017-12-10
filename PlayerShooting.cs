using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;


    float timer;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
        timer += Time.deltaTime;

		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
            Shoot ();

        //disabling fire effects after pre-set effectsDisplayTime
        if(timer >= timeBetweenBullets * effectsDisplayTime)
            DisableEffects ();
    }

    // Disable gun shoot effects components.
    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        //making sure particles are stopped before play
        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        //setting start position of gunLine
        gunLine.SetPosition (0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        //Calculating how to draw gunLine according to "Raycast" hit info
        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            //checking if hit object has "HP" (is alive)
            if(enemyHealth != null)
            {
                //calling TakeDamage function on enemy
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            }
            //setting end position of gunLine
            gunLine.SetPosition (1, shootHit.point);
        }
        else
            //setting end position of gunLine if nothing is hit by "range" length
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
    }
}
