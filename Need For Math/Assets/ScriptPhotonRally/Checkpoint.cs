using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Photon.Pun;

/// <summary>
/// Static checkpoints for registering race progress (CarRaceControl).
/// Forms a linked list based on the "next" attribute.
/// </summary>
public class Checkpoint : MonoBehaviour {

    public Transform skillGUI;

    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;

    // must be be true on last (lap end) checkpoint only.
    public bool isStartFinish;

	public Checkpoint next;

    public Weapon[] weapons;

    public Bullet bullet1;
    public Bullet bullet2;
    public Bullet bullet3;

    public Transform soruWindow;
    public Transform soruText;
    public Transform cevapInputField;

    public static int a;
    public static int b;
    public static int cevap;

    public static Collider otherx;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("SkillCollision");

        if (other.attachedRigidbody.gameObject.tag == "Player")
        {
            if (other.attachedRigidbody.GetComponent<PhotonView>().IsMine)
            {
                otherx = other;

                cevapInputField.GetComponent<InputField>().text = "";

                System.Random random = new System.Random();
                a = random.Next(1, 30);
                b = random.Next(1, 30);

                if (a % b == 0)
                {
                    //label1.Text = a + " / " + b + " = ?";
                    soruText.GetComponent<Text>().text = a + " / " + b + " = ?";
                    cevap = a / b;
                }
                else if (a <= 10 && b <= 10)
                {
                    //label1.Text = a + " * " + b + " = ?";
                    soruText.GetComponent<Text>().text = a + " * " + b + " = ?";
                    cevap = a * b;
                }
                else if (a > b)
                {
                    //label1.Text = a + " + " + b + " = ?";
                    soruText.GetComponent<Text>().text = a + " + " + b + " = ?";
                    cevap = a + b;
                }
                else if (b > a)
                {
                    //label1.Text = b + " - " + a + " = ?";
                    soruText.GetComponent<Text>().text = b + " - " + a + " = ?";
                    cevap = b - a;
                }
                else if (a == b)
                {
                    //label1.Text = a + " - " + b + " = ?";
                    soruText.GetComponent<Text>().text = a + " - " + b + " = ?";
                    cevap = a - b;
                }

                soruWindow.GetComponentInChildren<Animator>().Play("Exit Panel In");

                /*int temp = Random.Range(0, 3);

                if (temp == 0)
                {
                    skillGUI.GetComponent<Image>().sprite = sprite1;

                    weapons = other.attachedRigidbody.GetComponentsInChildren<Weapon>();

                    for (int index = 0; index < weapons.Length; index++)
                    {
                        weapons[index].currentAmmo = 1;
                        weapons[index].bullet = bullet1;
                    }
                }
                else if (temp == 1)
                {
                    skillGUI.GetComponent<Image>().sprite = sprite2;

                    weapons = other.attachedRigidbody.GetComponentsInChildren<Weapon>();

                    for (int index = 0; index < weapons.Length; index++)
                    {
                        weapons[index].currentAmmo = 1;
                        weapons[index].bullet = bullet2;
                    }
                }
                else if (temp == 2)
                {
                    skillGUI.GetComponent<Image>().sprite = sprite3;

                    weapons = other.attachedRigidbody.GetComponentsInChildren<Weapon>();

                    for (int index = 0; index < weapons.Length; index++)
                    {
                        weapons[index].currentAmmo = 1;
                        weapons[index].bullet = bullet3;
                    }
                }*/
            }
        }
    }

    public void SoruCevaplaClick()
    {
        Time.timeScale = 1;

        //int cevapInput = Int32.Parse(cevapInputField.GetComponent<InputField>().text);

        if (true)
        {
            int temp = UnityEngine.Random.Range(0, 3);

            if (temp == 0)
            {
                skillGUI.GetComponent<Image>().sprite = sprite1;

                weapons = otherx.attachedRigidbody.GetComponentsInChildren<Weapon>();

                for (int index = 0; index < weapons.Length; index++)
                {
                    weapons[index].currentAmmo = 4;
                    weapons[index].bullet = bullet1;
                }
            }
            else if (temp == 1)
            {
                skillGUI.GetComponent<Image>().sprite = sprite2;

                weapons = otherx.attachedRigidbody.GetComponentsInChildren<Weapon>();

                for (int index = 0; index < weapons.Length; index++)
                {
                    weapons[index].currentAmmo = 4;
                    weapons[index].bullet = bullet2;
                }
            }
            else if (temp == 2)
            {
                skillGUI.GetComponent<Image>().sprite = sprite3;

                weapons = otherx.attachedRigidbody.GetComponentsInChildren<Weapon>();

                for (int index = 0; index < weapons.Length; index++)
                {
                    weapons[index].currentAmmo = 4;
                    weapons[index].bullet = bullet3;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Stay");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
    }

    private void Update()
    {
        if (soruWindow.GetComponent<CanvasGroup>().alpha == 1)
        {
            if (soruWindow.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Exit Panel In") && soruWindow.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                Time.timeScale = 0.00001f;
            }
            else if(soruWindow.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Exit Panel Out"))
            {
                Time.timeScale = 1;
            }
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
