using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    // P R O P E R T I E S
    private int damage;
    public bool Attacked;
    public ParticleSystem slashFX;
    public float MeleeRange;
    Timer coolDownTimer;
    [SerializeField] private float coolDownDuration = 1;

    [Header("References")]
    Player playerRef;
    [SerializeField] GameObject HitBox;

    [Header("Animation Settings")]
    [SerializeField] Animator MeleeAnim;

    [SerializeField]
    public AudioSource meleeSource;
    public AudioClip swingSound;
    public float volume = 1f;

    // U N I T Y   M E T H O D S
    private void OnLevelWasLoaded(int level)
    {
        GameObject go = GameObject.Find("HitBox");
        playerRef = go.GetComponent<Player>();
    }
    private void Awake()
    {
        coolDownTimer = new Timer();

        playerRef = GetComponent<Player>();

        if (playerRef != null )
        {
            damage = playerRef.Damage;
            coolDownDuration = playerRef.damageCooldownDuration;
        }

        Attacked = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        MeleeAnim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        coolDownTimer.UpdateTimer(Time.time);
        ListenForAttack();
    }


    // M I S C   M E T H O D S

    void ListenForAttack()
    {
        //Input.GetKeyDown(KeyCode.RightShift)
        if (Input.GetButtonDown("Fire2"))
        {
            if (coolDownTimer.State == TimerState.Off 
                || coolDownTimer.State == TimerState.Ended)
            {
                UnityEngine.Debug.Log("MELEE ATTACK PERFORMED!!");
                Attacked = true;
                coolDownTimer.StartTimer(Time.time, coolDownDuration);
                //MeleeAnim.SetBool("Attack", true);
                MeleeAnim.Play("MeleeAttack");
                SpawnSlashFX();
            }
            
            
        }
    }

    
    public void Performed()
    {
        Attacked = false;
        //MeleeAnim.SetBool("Attack", false);
        //DestroyImmediate(slashFX);

        coolDownTimer.ResetTimer();
        //resets the position of the slashFX
        //slashFX.gameObject.transform.position = transform.position;
    }
    void SpawnSlashFX()
    {
        meleeSource.PlayOneShot(swingSound, volume);
        slashFX.Play();

        //RaycastHit hit;
        //Ray dodgeRay = new Ray(transform.position, transform.forward);
        //// if we hit something, play spawn the slashFX at the hit thing's position (play the VFX on that target)
        //if (Physics.Raycast(dodgeRay, out hit, MeleeRange))
        //{
        //    float distance = hit.distance;
        //    Debug.Log("HIT THING!!");

        //    if (hit.collider != null)
        //    {
        //        Debug.Log("PLAYED VFX ON THING!!");
        //        //Instantiate(slashFX, hit.transform.position, Quaternion.identity, this.transform);
        //        slashFX.gameObject.transform.position = hit.transform.position;
        //        slashFX.Play();
        //    }

        //}
        //// if we don't hit something, play the slashFX at the player's range
        //else
        //{
        //    Debug.Log("didn't hit thing. No VFX on target");
        //    //Instantiate(slashFX, this.transform);
        //    slashFX.Play();
        //}
    }
}
