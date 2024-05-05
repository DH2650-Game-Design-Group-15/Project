using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace DestroyIt
{
    /// <summary>This script manages all player input.</summary>
    [DisallowMultipleComponent]
    public class InputManager : MonoBehaviour
    {
        public ParticleSystem muzzleFlash;
        [Range(1, 30)]
        public float startDistance = 1.5f; 		    // The distance projectiles/missiles will start in front of the player.
        public WeaponType startingWeapon = WeaponType.Melee;   // The weapon the player will start with.
                                                               // public GameObject groundChurnPrefab;
        [Range(0.1f, .5f)]
        public float timeSlowSpeed = 0.25f;
        public GameObject windZone;
        public WeaponType SelectedWeapon { get; set; }

        private bool timeSlowed;
        private bool timeStopped;
        private float meleeAttackDelay;
        private float lastMeleeTime;
        private CharacterController firstPersonController;
        private Transform axeTransform;             // The location of the axe.
        private readonly LoadSceneParameters lsp = new LoadSceneParameters { loadSceneMode = LoadSceneMode.Single, localPhysicsMode = LocalPhysicsMode.None };

        // Hide the default constructor (use InputManager.Instance instead).
        private InputManager() { }

        public static InputManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (Camera.main == null || Camera.main.transform.parent == null) return;
            firstPersonController = Camera.main.transform.parent.GetComponent<CharacterController>();
            if (firstPersonController == null)
                Debug.LogError("InputManager: Could not find Character Controller on Main Camera parent.");
            
            foreach (Transform trans in Camera.main.transform)
            {
                switch (trans.name)
                {
                    case "WeaponPosition-Axe":
                        axeTransform = trans;
                        break;
                    default: 
                        break;
                }
            }

            meleeAttackDelay = 0.6f; // Limit melee attacks to one every 1/2 second.
            lastMeleeTime = 0f;

            
            // Set active weapon from player preferences.
            int playerPrefWeapon = PlayerPrefs.GetInt("SelectedWeapon", -1);
            if (playerPrefWeapon == -1)
                SelectedWeapon = startingWeapon;
            else
	            SelectedWeapon = (WeaponType)playerPrefWeapon;
	        
            // Set HUD visibility options from player preferences.

	        
            SetActiveWeapon();
        }

        private void Update()
        {
       

            if (Input.GetButtonDown("Fire1"))
            {
                //Cursor.lockState = CursorLockMode.Locked;

                switch (SelectedWeapon)
                {


                    case WeaponType.Melee:
                        if (Time.time >= (lastMeleeTime + meleeAttackDelay))
                            MeleeAttack();
                        break;

                }
            }

            // Continuous melee attack from holding the button down (useful for chopping trees in an MMO/survival game)
            if (Input.GetButton("Fire1") && SelectedWeapon == WeaponType.Melee && Time.time >= (lastMeleeTime + meleeAttackDelay))
                MeleeAttack();

            // Time Slow
            if (Input.GetKeyUp("t"))
            {
                timeSlowed = !timeSlowed;
            }

            // Time Stop
            if (Input.GetKeyUp("y"))
            {
                timeStopped = !timeStopped;
            }
            
            // Do this every frame for rigidbodies that enter the scene, so they have smooth frame interpolation.
            // TODO: can probably run this more efficiently at a set rate, like a few times per second - not every frame.
            if (timeSlowed)
            {
                foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
                {
                    foreach (Rigidbody rb in go.GetComponentsInChildren<Rigidbody>())
                        rb.interpolation = RigidbodyInterpolation.Interpolate;
                }
            }

            // if (Input.GetKeyUp("q"))
            // {
            //     SelectedWeapon = WeaponHelper.GetPrevious(SelectedWeapon);
            //     PlayerPrefs.SetInt("SelectedWeapon", (int)SelectedWeapon);
            //     SetActiveWeapon();
            // }
            // if (Input.GetKeyUp("e"))
            // {
            //     SelectedWeapon = WeaponHelper.GetNext(SelectedWeapon);
            //     PlayerPrefs.SetInt("SelectedWeapon", (int)SelectedWeapon);
            //     SetActiveWeapon();
            // }

            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            if (scrollWheel > 0f) // scroll up
            {
                SelectedWeapon = WeaponHelper.GetNext(SelectedWeapon);
                PlayerPrefs.SetInt("SelectedWeapon", (int)SelectedWeapon);
                SetActiveWeapon();
            }
            if (scrollWheel < 0f) // scroll down
            {
                SelectedWeapon = WeaponHelper.GetPrevious(SelectedWeapon);
                PlayerPrefs.SetInt("SelectedWeapon", (int)SelectedWeapon);
                SetActiveWeapon();
            }
        }

        private void EnableWindZone()
        {
            if (windZone != null)
                windZone.SetActive(true);
        }

        private void SetActiveWeapon()
        {
            axeTransform.gameObject.SetActive(SelectedWeapon == WeaponType.Melee);

        }

        private void MeleeAttack()
        {
            Animation anim = axeTransform.GetComponentInChildren<Animation>();
            anim.Play("Axe Swinging");
            lastMeleeTime = Time.time;
            Invoke("BroadcastMeleeDamage", .2f);
        }


        private void BroadcastMeleeDamage()
        {
            firstPersonController.BroadcastMessage("OnMeleeDamage");
        }




    }
}