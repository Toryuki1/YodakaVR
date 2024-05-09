using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
//黑屏，用一张全黑图片覆盖屏幕，调整透明度使用curve。
public class BlackScreen : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;//覆盖屏幕的一张全黑图片，我选择挂在Camera下面，这样做如果相机是移动的就很方便
    public AnimationCurve curve; //在Inspector上调整自己喜欢的曲线
    [Range(0.5f, 2f)]public float speed = 1f; //控制渐入渐出的速度
 
    private void Awake()
    {
        if (spriteRenderer == null) 
            spriteRenderer = GetComponent<SpriteRenderer>();
    }
 
    //开启自动播放黑屏
    private void OnEnable()
    {
        StartCoroutine(Black());
    }
 
    Color tmpColor; //用于传递颜色的变量
    public IEnumerator Black()
    {
        float timer = 0f;
        tmpColor = spriteRenderer.color;
        do
        {
            timer += Time.deltaTime;
            SetColorAlpha(curve.Evaluate(timer * speed));
            yield return null;
 
        } while (tmpColor.a > 0);
        gameObject.SetActive(false);
    }
 
    //通过调整图片的透明度实现渐入渐出
    void SetColorAlpha(float a)
    {
        tmpColor.a = a;
        spriteRenderer.color = tmpColor;
    }
}