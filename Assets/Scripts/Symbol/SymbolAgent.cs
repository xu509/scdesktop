using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolAgent : MonoBehaviour
{
    [SerializeField] SpriteRenderer _image;

    private static float SPEED_CONST = 0.5f;
    private float _speed = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }


    public void Init(float speed, Vector3 birthPosition, float scaleFactor)
    {
        _speed = speed;
        transform.position = birthPosition;

        var scale = GetComponent<Transform>().localScale;
        scale.x *= scaleFactor;
        scale.y *= scaleFactor;
        GetComponent<Transform>().localScale = scale;


        int f = Random.Range(0, 3);

        string coverAddress = "symbol/";

        //Debug.Log("coverAddress : " + coverAddress);

        if (f == 0)
        {
            coverAddress += "泡泡1";
        }
        else if (f == 1)
        {
            coverAddress += "泡泡2";
        }
        else
        {
            coverAddress += "泡泡3";
        }

        var sprite = Resources.Load<Sprite>(coverAddress);


        _image.sprite = sprite;

    }










    // Update is called once per frame
    void Update()
    {
        var to = new Vector3(-Time.deltaTime * SPEED_CONST * (_speed + 1), 0, 0);

        transform.Translate(to);

        CheckOverBords();
    }



    private void CheckOverBords()
    {
        Vector3 v = Camera.main.WorldToScreenPoint(transform.position);

        if (v.x < (0 - 200))
        {
            Destroy(this.gameObject);

        }
    }
}
