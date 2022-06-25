using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Properties : MonoBehaviour
{
    public int health = 1000;
    public WeaponObject currentGun;
    public GameObject gun1;
    public GameObject gun2;
    public GameObject gun3;
    public GameObject gun4;
    public GameObject secondPistol;
    private int activeWeapon;
    public GameObject lArmUp,lArmDown,rArmUp,rArmDown;
    //Hand positions for the spiker pistol
    private Quaternion lArmUpR1 = Quaternion.Euler(new Vector3(90f, 82f, 95f));
    private Quaternion lArmDownR1 = Quaternion.Euler(new Vector3(20f, 13f, 55f));
    private Quaternion rArmUpR1 = Quaternion.Euler(new Vector3(-90f, 28f, 15f));
    private Quaternion rArmDownR1 = Quaternion.Euler(new Vector3(-20f, 13f, 55f));
    //Hand positions for the ac-5 assault rifle
    private Quaternion lArmUpR2 = Quaternion.Euler(new Vector3(10f, 20f, 0f));
    private Quaternion lArmDownR2 = Quaternion.Euler(new Vector3(10f, 3f, 45f));
    private Quaternion rArmUpR2 = Quaternion.Euler(new Vector3(-10f, -20f, 0f));
    private Quaternion rArmDownR2 = Quaternion.Euler(new Vector3(-70f, 3f, 45f));
    //Hand positions for the hydra sniper rifle
    private Quaternion lArmUpR3 = Quaternion.Euler(new Vector3(10f, 20f, 0f));
    private Quaternion lArmDownR3 = Quaternion.Euler(new Vector3(10f, 3f, 45f));
    private Quaternion rArmUpR3 = Quaternion.Euler(new Vector3(-85f, -20f, 0f));
    private Quaternion rArmDownR3 = Quaternion.Euler(new Vector3(-5f, 3f, 45f));
    //Hand positions for the brute death rocket launcher
    private Quaternion lArmUpR4 = Quaternion.Euler(new Vector3(10f, 20f, 0f));
    private Quaternion lArmDownR4 = Quaternion.Euler(new Vector3(10f, 3f, 45f));
    private Quaternion rArmUpR4 = Quaternion.Euler(new Vector3(-60f, -20f, 0f));
    private Quaternion rArmDownR4 = Quaternion.Euler(new Vector3(-50f, 3f, 45f));
    public float speed;

    public Slider healthBar;
    public Text playerHealth;
    public Image healthFill;
    public Slider ammoBar;
    public Text playerAmmo;

    public GameObject DeathScreen;
    private ThirdPersonCameraController cameraControl;
    [HideInInspector] public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.isDead = false;
        Time.timeScale = 1f;
        currentHealth = health;
        currentGun = gun2.GetComponent(typeof(WeaponObject)) as WeaponObject;

        cameraControl = GameObject.Find("Main Camera").GetComponent<ThirdPersonCameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = (float)currentHealth / health;
        playerHealth.text = (healthBar.value * 100) + "%";
        if (currentHealth <= 0)
            Die();
        else if (currentHealth <= (health * 0.2))
            healthFill.color = Color.red;
        else if (currentHealth <= (health * 0.6))
            healthFill.color = Color.yellow;

        ammoBar.value = (float)currentGun.currentAmmo / currentGun.maxClipAmmo;
        if (activeWeapon == 4)
            playerAmmo.text = (ammoBar.value * 100) + "%";
        else
            playerAmmo.text = currentGun.currentAmmo + "/" + currentGun.totalAmmo;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentGun.gameObject.SetActive(false);
            gun1.gameObject.SetActive(true);
            secondPistol.gameObject.SetActive(true);
            currentGun = gun1.GetComponent(typeof(WeaponObject)) as WeaponObject;
            activeWeapon = 1;
            //AimWeapon();

            SetAim(0.02f, -0.31f, 3.5f, 2f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentGun.gameObject.SetActive(false);
            secondPistol.gameObject.SetActive(false);
            gun2.gameObject.SetActive(true);
            currentGun = gun2.GetComponent(typeof(WeaponObject)) as WeaponObject;
            activeWeapon = 2;

            //SetAim(0, 1f, -1.75f, 1);
            //SetAim(0.5f, 0, 1.75f, 1);
            SetAim(0.32125f, -0.734f, 3.5f, 1f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentGun.gameObject.SetActive(false);
            secondPistol.gameObject.SetActive(false);
            gun3.gameObject.SetActive(true);
            currentGun = gun3.GetComponent(typeof(WeaponObject)) as WeaponObject;
            activeWeapon = 3;

            SetAim(0.435f, -1.17f, 14f, 8f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentGun.gameObject.SetActive(false);
            secondPistol.gameObject.SetActive(false);
            gun4.gameObject.SetActive(true);
            currentGun = gun4.GetComponent(typeof(WeaponObject)) as WeaponObject;
            activeWeapon = 4;

            SetAim(0.3675f, -0.39f, 3.5f, 1f);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            currentGun.gameObject.SetActive(false);
            secondPistol.gameObject.SetActive(false);
            activeWeapon = 0;

            //I need this for testing
            //SetAim(0.5f, 0, 1.75f, 1);
        }
        
    }
    private void LateUpdate()
    {
        switch (activeWeapon)
        {
            case 1:
                SetArmsWeapon1();
                break;
            case 2:
                SetArmsWeapon2();
                break;
            case 3:
                SetArmsWeapon3();
                break;
            case 4:
                SetArmsWeapon4();
                break;
        }
    }

    private void FixedUpdate()
    {
        //For fixing the player if they get stuck
        if (Input.GetKeyDown(KeyCode.B))
        {
            transform.Translate(0, 0.1f, -0.05f, Space.Self);
        }
    }

    void SetArmsWeapon1()
    {
        rArmUp.transform.localRotation = rArmUpR1;
        rArmDown.transform.localRotation = rArmDownR1;
        lArmDown.transform.localRotation = lArmDownR1;
        lArmUp.transform.localRotation = lArmUpR1;
    }
    void SetArmsWeapon2()
    {
        rArmUp.transform.localRotation = rArmUpR2;
        rArmDown.transform.localRotation = rArmDownR2;
        lArmDown.transform.localRotation = lArmDownR2;
        lArmUp.transform.localRotation = lArmUpR2;
    }
    public void SetArmsWeapon3()
    {
        rArmUp.transform.localRotation = rArmUpR3;
        rArmDown.transform.localRotation = rArmDownR3;
        lArmDown.transform.localRotation = lArmDownR3;
        lArmUp.transform.localRotation = lArmUpR3;
    }
    void SetArmsWeapon4()
    {
        rArmUp.transform.localRotation = rArmUpR4;
        rArmDown.transform.localRotation = rArmDownR4;
        lArmDown.transform.localRotation = lArmDownR4;
        lArmUp.transform.localRotation = lArmUpR4;
    }

    void SetAim(float aimOffsetX, float aimOffsetY, float zoomDistance, float speed)
    {
        cameraControl.zoomedInPosition.localPosition = Vector3.zero;
        cameraControl.zoomedInPosition.Translate(new Vector3(aimOffsetX, aimOffsetY, zoomDistance));    //Sets our zoom position based off our inputs
        cameraControl.zoomSpeed = 5 * speed;
    }
    //rotates torso to aim at mouse
    //void AimWeapon()
    //{
    //    Vector3 from = gameObject.transform.position;
    //    Vector3 to = gameObject.transform.position+transform.forward;
    //    RaycastHit hit;
    //    Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

    //    if (Physics.Raycast(ray, out hit))
    //    {

    //        to = hit.point;

    //        to.y += 2.0f;
    //        Debug.Log(hit.point);
    //    }



    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            //Debug.Log("hit"+health);
            //health -= 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Bullet(Clone)" || other.gameObject.name == "Explosive Plasma Bolt(Clone)" || other.gameObject.name == "Explosion(Clone)")
        {
            Rigidbody bullet = other.gameObject.GetComponent<Rigidbody>();
            currentHealth -= (int)bullet.mass;
            //Debug.Log("hit" + health);
        }
    }

    private void Die()
    {
        Debug.Log("Player Died");
        GameManager.isDead = true;
        DeathScreen.SetActive(true);
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
