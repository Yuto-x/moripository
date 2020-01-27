using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kittinrightinput : MonoBehaviour
{
    public GameObject kittin;

    //ボタン
    public GameObject obj1F;    //1階モード
    public GameObject obj2F;    //2階モード

    GameObject JudgKittin1F;
    GameObject JudgKittin2F;
    
    public void OnClick()
    {
        JudgKittin1F = GameObject.FindGameObjectWithTag("1Fkittin");
        JudgKittin2F = GameObject.FindGameObjectWithTag("2Fkittin");
        
        // 1階モードの時
        if (obj1F.activeSelf == true)
        {
            // 1階にキッチンがない場合
            if (JudgKittin1F == null)
            {
                GameObject kit = Instantiate(kittin);
                kit.tag = "1Fkittin";
                kit.transform.position = new Vector3(36, 2, 17);
                kit.transform.Rotate(0.0f, 0.0f, 0.0f);
            }
            // 1階にキッチンがある場合
            else if (JudgKittin1F != null)
            {
                Destroy(JudgKittin1F);
                GameObject kit = Instantiate(kittin);
                kit.tag = "1Fkittin";
                kit.transform.position = new Vector3(36, 2, 17);
                kit.transform.Rotate(0.0f, 0.0f, 0.0f);
            }       
        }
        // 2階モードの時
        else if (obj2F.activeSelf == true)
        {
            // 2階にキッチンがない場合
            if (JudgKittin2F == null)
            {
                GameObject kit = Instantiate(kittin);
                kit.tag = "2Fkittin";
                kit.transform.position = new Vector3(36, 6, 17);
                kit.transform.Rotate(0.0f, 0.0f, 0.0f);
            }
            // 2階にキッチンがある場合
            else if (JudgKittin2F != null)
            {
                Destroy(JudgKittin2F);
                GameObject kit = Instantiate(kittin);
                kit.tag = "2Fkittin";
                kit.transform.position = new Vector3(36, 6, 17);
                kit.transform.Rotate(0.0f, 0.0f, 0.0f);
            }
        }
    }
}
