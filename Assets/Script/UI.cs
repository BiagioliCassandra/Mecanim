using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    private Animator _animatorCharacter;
    private void Start()
    {
        _animatorCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    public void TheBegin()
    {
        _animatorCharacter.SetBool("Start", true);
        StartCoroutine(TheBeginCoroutine());
    }

    public void MenuFight()
    {
        SceneManager.LoadScene("MenuFight");
    }

    IEnumerator TheBeginCoroutine()
    {
        yield return new WaitForSeconds(1.25f);
        SceneManager.LoadScene("SampleScene");
    }

    public void TheEnd()
    {
        Application.Quit();
    }
}
