using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Unfinished
/// </summary>
[RequireComponent(typeof(TextMesh))]
public class Fadeer : MonoBehaviour {

    [SerializeField] float fadeSpeed, waitTimer, OnTimer, OffTimer;


    TextMesh _textmesh;
    SpriteRenderer _backingSprite;

    float timeKepper = 0f;
    bool BIsOn = false;


    // Use this for initialization
    void Start () {
        _textmesh = GetComponent<TextMesh>();
        _backingSprite = GetComponentInChildren<SpriteRenderer>();
    }


    void FadeINAlpha() {

            Color textcolor = _textmesh.color;
            Color backingColor = _backingSprite.color;


            textcolor.a += Time.deltaTime * fadeSpeed;
            backingColor.a += Time.deltaTime * fadeSpeed;

        _backingSprite.color = backingColor;
        _textmesh.color = textcolor;

        if (_textmesh.color.a >= 255f && _backingSprite.color.a >= 255f) BIsOn = true;
    }

    void FadeOutAlpha()
    {

        Color textcolor = _textmesh.color;
        Color backingColor = _backingSprite.color;


        textcolor.a -= Time.deltaTime * fadeSpeed;
        backingColor.a -= Time.deltaTime * fadeSpeed;



        _backingSprite.color = backingColor;
        _textmesh.color = textcolor;

        if (_textmesh.color.a >= 0f && _backingSprite.color.a >= 0f) 
        gameObject.SetActive(false);
    }


    private void Update()
    {
       
    }

}
