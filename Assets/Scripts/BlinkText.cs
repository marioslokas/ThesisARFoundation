using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour
{

    [SerializeField] private Text _myText;
    private float alpha;
    [SerializeField] private bool _blink = true;
    private bool _increasing = true;
    public float speed = 1f;

    void Start()
    {
        StartCoroutine(BlinkColor());
    }

    IEnumerator BlinkColor()
    {
        float t = 0f, alpha = 0f;

        while (_blink)
        {
            alpha = Mathf.Lerp(0f, 1f, t);
            _myText.color = new Color(_myText.color.r, _myText.color.g, _myText.color.b, alpha);

            if (_increasing)
            {
                t += Time.deltaTime;
            }
            else
            {
                t -= Time.deltaTime;
            }

            if (alpha >= 1f)
            {
                _increasing = false;
            }
            else if (alpha <= 0f)
            {
                _increasing = true;
            }

            yield return null;
        }
    }
}
