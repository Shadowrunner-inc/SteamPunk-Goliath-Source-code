using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Nathan Hales
/// This is a simple script for a trigger box inside the credits scene. Any cedit object that enters it fades over time. (Alpha fades)
/// </summary>
public class CreditEater : MonoBehaviour {

    public float fadeSpeed = 10f;

    TextMesh _textmesh;
    SpriteRenderer _backingSprite;

    private void OnTriggerEnter(Collider other)
    {
        print("Hit");
        if (other.GetComponent<TextMesh>() != null) {
            print("beep1");
            _textmesh = other.GetComponent<TextMesh>();
            _backingSprite = other.GetComponentInChildren<SpriteRenderer>();
        }
    }


    private void Update()
    {

        if (_textmesh != null && _backingSprite != null) {
            Color textcolor = _textmesh.color;

            textcolor.a -= Time.deltaTime * fadeSpeed;
            _textmesh.color = textcolor;

            Color backingColor = _backingSprite.color;
            backingColor.a -= Time.deltaTime * fadeSpeed;
            _backingSprite.color = backingColor;
        }

    }

}
