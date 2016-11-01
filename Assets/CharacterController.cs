using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour
{


    [System.Serializable]
    public class MoveSettings
    {
        public float forwardVel = 12f;
        public float rotateVel = 100f;
        public float jumpVel = 25;
        public float distToGround = 0.1f;
        public LayerMask ground;
    }

    [System.Serializable]
    public class PhysSettings
    {
        public float downAccel = 0.75f;
        public float recoil_Force = 4f;

    }

    [System.Serializable]
    public class InputSettings
    {
        public float inputDelay = 0.1f;
        public string FORWARD_AXIS = "Vertical";
        public string TURN_AXIS = "Horizontal";
        public string JUMP_AXIS = "Jump";
        public KeyCode SHOOT_BUTTON = KeyCode.K;

    }

    [System.Serializable]
    public class DamageSettings
    {
        public Vector3 bulletOffset = new Vector3(0, 0, 8);
		public Vector3 casingOffset = new Vector3(0, 0, 6);
		public Rigidbody rBody;
        public GameObject Bullet;
		public GameObject Casing;
		public float shotCooldown = 0.3f;
        public float bulletSpeed = 1000;
		public float casingEjectSpeed = 15;
    }

    public MoveSettings moveSetting = new MoveSettings();
    public PhysSettings physSetting = new PhysSettings();
    public InputSettings inputSetting = new InputSettings();
    public DamageSettings damageSetting = new DamageSettings();

    Vector3 velocity = Vector3.zero;
    Quaternion targetRotation;
    Rigidbody rBody;
    float forwardInput, turnInput, jumpInput, fired;
    bool shootInput;
	Vector3 bulletSpawn = Vector3.zero;
	Vector3 casingSpawn = Vector3.zero;

    private float maxVerticalVelocity = 0;

    public Quaternion TargetRotation
    {

        get { return targetRotation; }

    }

    void OnCollisionEnter(Collision C)
    {
        if (C.gameObject.tag == "Ground")
        {
            Debug.Log("MaxVec = " + maxVerticalVelocity);
            CalculateDamage();
        }

    }

    void CalculateDamage()
    {
        if ((maxVerticalVelocity * -1) > (moveSetting.jumpVel + 10))
        {
            Debug.Log("TOOK DAMAGE");
            this.SendMessage("TakeDamage", (maxVerticalVelocity * -1) / 5);
            maxVerticalVelocity = 0;

        }
    }

    bool Grounded()
    {

        return Physics.Raycast(transform.position, Vector3.down, moveSetting.distToGround, moveSetting.ground);
    }


    // Use this for initialization
    void Start()
    {

        targetRotation = transform.rotation;
        if (GetComponent<Rigidbody>())
        {
            rBody = GetComponent<Rigidbody>();
        }
        else {
            Debug.LogError("The Character need a Rigidbody");
        }
        forwardInput = turnInput = jumpInput = 0;
    }

    void GetInput()
    {
        forwardInput = Input.GetAxis(inputSetting.FORWARD_AXIS);
        turnInput = Input.GetAxis(inputSetting.TURN_AXIS);
        jumpInput = Input.GetAxisRaw(inputSetting.JUMP_AXIS);
        shootInput = Input.GetKey(inputSetting.SHOOT_BUTTON);

    }

    void Update()
    {
        GetInput();
        Turn();
    }

    void FixedUpdate()
    {
        Run();
        Jump();
        SpawnSpike();

        rBody.velocity = rBody.transform.TransformDirection(velocity);
    }

    void Run()
    {
        if (Mathf.Abs(forwardInput) > inputSetting.inputDelay)
        {
            velocity.z = moveSetting.forwardVel * forwardInput;
        }
        else {
            velocity.z = 0;
        }
    }

    void Turn()
    {
        if (Mathf.Abs(turnInput) > inputSetting.inputDelay)
        {
            targetRotation *= Quaternion.AngleAxis(moveSetting.rotateVel * turnInput * Time.deltaTime, Vector3.up);
            transform.rotation = targetRotation;
        }
    }
    void Jump()
    {
        if (jumpInput > 0 && Grounded())
        {
            velocity.y = moveSetting.jumpVel;
        }
        else if (jumpInput == 0 && Grounded())
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y -= physSetting.downAccel;
            if (velocity.y < maxVerticalVelocity)
            {
                maxVerticalVelocity = velocity.y;
            }
        }

    }
    void SpawnSpike()
    {
        if (shootInput == true)
        {   
            if(Time.time - fired > damageSetting.shotCooldown)
            { 
                fired = Time.time;
                bulletSpawn = this.TargetRotation * damageSetting.bulletOffset;
                bulletSpawn += this.transform.position;
				GameObject bulletInstance = (GameObject)Instantiate(damageSetting.Bullet, bulletSpawn, Quaternion.identity);
                bulletInstance.GetComponent<Rigidbody>().AddForce(transform.forward * damageSetting.bulletSpeed);
                AudioSource audio = GetComponent<AudioSource>();
                audio.Play();
                audio.Play(44100);
                casingSpawn = this.TargetRotation * damageSetting.casingOffset;
                casingSpawn += this.transform.position;
                this.GetComponent<Rigidbody>().AddForce(transform.forward*(physSetting.recoil_Force*-1));
                GameObject casingInstance = (GameObject)Instantiate(damageSetting.Casing, casingSpawn, this.targetRotation);
                casingInstance.GetComponent<Rigidbody>().AddForce(transform.right * damageSetting.casingEjectSpeed);

            }
        }
    }
}
