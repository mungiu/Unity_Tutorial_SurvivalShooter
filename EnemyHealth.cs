using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    //sinking through floor
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;


    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(isSinking)
        {
            //sinking enemy per second
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Makes enemy take damage, and sets hit position.transform.
    /// </summary>
    /// <param name="amount">Amount of taken damage.</param>
    /// <param name="hitPoint">The transform.position of hit point.</param>
    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        //cancelling function is enemy already dead
        if(isDead)
            return;

        enemyAudio.Play ();

        currentHealth -= amount;

        //movind the particle system to where enemy took hit
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if(currentHealth <= 0)
            Death ();
    }


    void Death ()
    {
        isDead = true;

        //making sure dead enemies are not obstacles
        //triggers can't collide
        capsuleCollider.isTrigger = true;

        //engaging dead adnimation trigger
        anim.SetTrigger ("Dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }

    //automatically called in the animation
    public void StartSinking ()
    {
        //finding NavMeshAgent and disabling its ".enabled" component
        //NOT the entire NavMesh
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        //moving colliders makes unity recalc static geometry (because the level is changing)
        //but kinematic rigid bodies are ignored while ".Translated" (moved)
        GetComponent <Rigidbody> ().isKinematic = true;

        isSinking = true;
        ScoreManager.score += scoreValue;
        Destroy (gameObject, 2f);
    }
}
