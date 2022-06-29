using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public static bool stopAllMovement = false;
    public GameObject gameOverGhost, gameOverScreen, gameOverText, successScreen, successText;
    public AudioSource playerSource;
    public AudioClip collectCoin, iSeeYou, death;
    private Text coinCount;
    public static bool collectAmmo;
    private bool oneGameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        oneGameOver = false;
        collectAmmo = false;
        stopAllMovement = false;
        coinCount = GameObject.Find("CoinsCounter").gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (string.Equals(coinCount.text, "617/617"))
        {
            if (!oneGameOver)
            {
                oneGameOver = true;
                Rigidbody playerRb = this.gameObject.GetComponent<Rigidbody>();
                playerRb.constraints = RigidbodyConstraints.FreezeAll;
                this.gameObject.GetComponent<FirstPersonController>().enabled = false;
                this.gameObject.GetComponent<CharacterController>().enabled = false;
                GameObject.Find("Ghosts").gameObject.SetActive(false);
                GameObject.Find("HudCanvas").gameObject.SetActive(false);
                GameObject.Find("WeaponPivot").gameObject.SetActive(false);
                Invoke("closeScreenSuccess", 1.5f);
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ghost")
        {
            Debug.Log("Collision with ghost!");
            playerSource.PlayOneShot(death);
            disableFunctions(col);
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "GhostRange")
        {
            LightSwitching.startSwitching = true;
            playerSource.PlayOneShot(iSeeYou);
            Debug.Log("Trigger on!");
        }
        if (col.gameObject.tag == "Coin")
        {
            // Debug.Log("Collision with coin!");
            playerSource.PlayOneShot(collectCoin);
            updateCounter();
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "Ammo")
        {
            // Debug.Log("Collision with coin!");
            //    coinSource.PlayOneShot(collectCoin);
            if (Shotgun.bullets != 3)
            {
                collectAmmo = true;
                Destroy(col.gameObject);
            }
        }
    }

    void disableFunctions(Collision col)
    {
        Rigidbody ghostRb = col.gameObject.GetComponent<Rigidbody>();
        ghostRb.constraints = RigidbodyConstraints.FreezeAll;
        Rigidbody playerRb = this.gameObject.GetComponent<Rigidbody>();
        playerRb.constraints = RigidbodyConstraints.FreezeAll;
        this.gameObject.GetComponent<FirstPersonController>().enabled = false;
        this.gameObject.GetComponent<CharacterController>().enabled = false;
        disablePlayerObjects(col);

    }

    void disablePlayerObjects(Collision col)
    {
        Destroy(col.gameObject);
        GameObject.Find("HudCanvas").gameObject.SetActive(false);
        GameObject.Find("WeaponPivot").gameObject.SetActive(false);
        GameObject.Find("ChromaticAberation").gameObject.GetComponent<Animator>().Play("ChromaAberation", -1, 0f);
        GameObject.Find("PlayerCamera").gameObject.GetComponent<Animator>().Play("DeathCamera", -1, 0f);
        Invoke("showGhost", 1f);
    }

    void showGhost()
    {
        gameOverGhost.SetActive(true);
        Invoke("closeScreenGameOver", 3f);
    }

    void closeScreenGameOver()
    {
        gameOverScreen.SetActive(true);
        Invoke("showGameOverText", 1.2f);
        Invoke("changeScene", 5.2f);
    }

    void closeScreenSuccess()
    {
        successScreen.SetActive(true);
        Invoke("showSuccessText", 1.2f);
        Invoke("changeScene", 5.2f);
    }

    void changeScene()
    {
        SceneManager.LoadScene("Main");
    }

    void showGameOverText()
    {
        gameOverText.SetActive(true);
    }

    void showSuccessText()
    {
        successText.SetActive(true);
    }

    void updateCounter()
    {
        int parseNum;
        string[] splitCounter = coinCount.text.Split('/');
        string first = splitCounter[0];
        bool parsing = int.TryParse(first, out parseNum);
        if (parsing)
        {
            parseNum = parseNum + 1;
            string result = parseNum.ToString() + "/" + "617";
            coinCount.text = result;
        }
    }
}
