using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



/**
 *森君へ！！！！！！！！！！！！！
 * 変数名わかりやすく変えてくれるのはありがたいですが、
 * その変数が何の変数かコメントを残しておいてください
 * 次に作業するときにわかりません
 * 
 * ぶん殴るぞ
 * 
 * やってみろ
 */
public class maincontrol : MonoBehaviour
{

    GameObject[] ArrayList;

    //プレハブ
    public GameObject grand;    //1階のマス
    public GameObject masutile; //床
    public GameObject masuwall; //壁
    public GameObject secondgrand;  //2階のマス
    public GameObject blackcube;  //既存の壁

    //ボタン
    public GameObject obj1F;    //1階モード
    public GameObject obj2F;    //2階モード
    public GameObject roomb;    //部屋作成モード
    public GameObject deleteb;  //部屋削除モード

    private RaycastHit hit;

    string startObjectName = "";
    string endObjectName = "";

    //3次元の配列を置く
    int[,,] field = new int[70, 10, 40];
    int count = 0;

    //オブジェクト判定用の変数　種類を増やす
    string Wall = "masu";

    //クリック用の変数
    Vector3 tmp = new Vector3(0, 0, 0);
    Vector3 tmp1 = new Vector3(0, 0, 0);
    Vector3 tmp2 = new Vector3(0, 0, 0);
    Vector3 tmp3 = new Vector3(0, 0, 0);

    //オブジェクトやタグの番号付けの変数
    int cubecount = 0;
    int blackcubecount = 0;
    int FirstfRoomCount = 1;
    int SecondRoomCount = 1;

    bool SecondStart = true;
    bool SecondGrandDel = false;

    private List<GameObject> WallTopList = new List<GameObject>();

    //オブジェクト削除用変数
    Vector3 DelTagPos = new Vector3(0, 0, 0);
    string ObjDelName = "";

    GameObject[] SecondGrandDelTag;
    GameObject[] deletetag;

    // Start is called before the first frame update
    void Start()
    {
        /*
         * 地面生成の処理(森)
         * -----------------------------------------------------------------
         */
        // 1と0を代入
        // 初期化
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int k = 0; k < field.GetLength(2); k++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (j == 0)
                    {
                        field[i, j, k] = 1;
                    }
                    else
                    {
                        field[i, j, k] = 0;
                    }
                }

            }
        }

        //各インデックスに代入された値をもとに生成するorしない

        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                for (int k = 0; k < field.GetLength(2); k++)
                {
                    //インデックスの値が1の時、cubeを生成
                    if (field[i, j, k] == 1)
                    {
                        GameObject wall = Instantiate(grand);
                        // wallの名前の変更 countによって一つ一つに番号をふる
                        wall.name = "grand" + count;
                        wall.transform.position = new Vector3(i, j, k);
                        count++;
                    }
                }
            }
        }
        /*
         * (地面生成処理)
         * -----------------------------------------------------------------
         */

    }

    // Update is called once per frame
    void Update()
    {
        //ボタンの読み込み
        obj1F = GameObject.Find("1F");
        obj2F = GameObject.Find("2F");
        roomb = GameObject.Find("roomcreateOff");
        deleteb = GameObject.Find("roomdelete");

        
        //ボタン下のオブジェクトに反応させない
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        

        //1Fと2Fに切り替えを行う
        if (obj1F != null)
        {
            //部屋の出力、壁の出力、削除等を入力
            if (SecondGrandDel == true)//2階の地面の削除、1階の高い部分を出力
            {
                SecondGrandDel = false;
                SecondStart = true;



                // 2階の地面があった場所の配列の値を戻すため0と1を代入
                for (int i = 0; i < field.GetLength(0); i++)
                {
                    for (int j = 0; j < field.GetLength(2); j++)
                    {
                        if (field[i, 4, j] == 1)
                        {
                            field[i, 4, j] = 0;
                        }
                        if (field[i, 4, j] == 2)
                        {
                            field[i, 4, j] = 1;
                        }
                    }
                }
                // 2階の地面の削除

                foreach (GameObject GrandDel in SecondGrandDelTag)
                {

                    Destroy(GrandDel);
                }

                // 一度非表示にした壁の高い場所を表示
                foreach (GameObject WallTopActive in WallTopList)
                {
                    WallTopActive.SetActive(true);
                }
            }

            //1階の部屋生成
            if (roomb != null)
            {
                //ボタンを押したとき
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    //スタートの座標を取得
                    if (Physics.Raycast(ray, out hit))
                    {
                        startObjectName = hit.collider.gameObject.name;

                        tmp = GameObject.Find(startObjectName).transform.position;

                        tmp1 = tmp;


                    }

                }
                //ボタンを離した処理
                if (Input.GetMouseButtonUp(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    //エンドの座標を取得
                    if (Physics.Raycast(ray, out hit))
                    {
                        endObjectName = hit.collider.gameObject.name;

                        tmp = GameObject.Find(endObjectName).transform.position;

                        tmp2 = tmp;


                        // 括弧内の文字列が入っているとオブジェクト生成の処理に入らない
                        if (startObjectName.Contains("masutile") || endObjectName.Contains("masuwall"))
                        {

                        }
                        else
                        {
                            if (tmp1.x <= tmp2.x || tmp1.z >= tmp2.z)
                            {
                                tmp3.x = tmp2.x - tmp1.x;
                                tmp3.z = tmp1.z - tmp2.z;

                                for (int s = 0; tmp3.z >= s; s++)
                                {
                                    tmp.z = tmp1.z - s;

                                    for (int t = 0; tmp3.x >= t; t++)
                                    {
                                        tmp.x = tmp1.x + t;
                                        tmp.y = tmp1.y;

                                        //選択した座標を配列の要素番号に代入からのその上に床オブジェクトを配置
                                        field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z] = 1;

                                        //インデックスの値が1の時、cubeを生成
                                        if (field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z] == 1)
                                        {
                                            //Assetsからmasuを取得
                                            GameObject floor = Instantiate(masutile);
                                            //オブジェクト生成時に"masu番号"に名前変更
                                            floor.name = "masutile" + count;
                                            floor.transform.position = new Vector3(tmp.x, tmp.y + 1, tmp.z);
                                            count++;
                                            floor.tag = "1fRoom" + FirstfRoomCount;
                                        }
                                    }
                                }
                                for (int s = 0; tmp3.z >= s; s++)
                                {
                                    tmp.z = tmp1.z - s;

                                    for (int t = 0; tmp3.x >= t; t++)
                                    {
                                        tmp.x = tmp1.x + t;
                                        tmp.y = tmp1.y;

                                        if (field[(int)tmp.x + 1, (int)tmp.y + 1, (int)tmp.z] == 0 || field[(int)tmp.x - 1, (int)tmp.y + 1, (int)tmp.z] == 0 || field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z + 1] == 0 || field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z - 1] == 0)
                                        {
                                            // 選択した座標を配列の要素番号に代入からのその上に壁オブジェクトを配置
                                            field[(int)tmp.x, (int)tmp.y + 2, (int)tmp.z] = 1;
                                            field[(int)tmp.x, (int)tmp.y + 3, (int)tmp.z] = 1;
                                            field[(int)tmp.x, (int)tmp.y + 4, (int)tmp.z] = 1;

                                            //インデックスの値が1の時、cubeを生成
                                            for (int u = 2; u <= 4; u++)
                                            {
                                                if (field[(int)tmp.x, (int)tmp.y + u, (int)tmp.z] == 1)
                                                {
                                                    //Assetsからmasuを取得
                                                    GameObject wall = Instantiate(masuwall);

                                                    //オブジェクト生成時に"masu番号"に名前変更
                                                    wall.name = "masuwall" + count;
                                                    wall.transform.position = new Vector3(tmp.x, tmp.y + u, tmp.z);
                                                    count++;
                                                    wall.tag = "1fRoom" + FirstfRoomCount;
                                                    // 1番上の壁をListに格納
                                                    if (u == 4)
                                                    {
                                                        WallTopList.Add(wall);
                                                    }

                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                    FirstfRoomCount++;
                }
            }
            //削除する
            if (deleteb != null)
            {
                //ボタンを押した処理
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    //座標を取得
                    if (Physics.Raycast(ray, out hit))
                    {
                        startObjectName = hit.collider.gameObject.name;

                        tmp = GameObject.Find(startObjectName).transform.position;

                    }
                    GameObject tagname = GameObject.Find(startObjectName);

                    GameObject[] deletetag = GameObject.FindGameObjectsWithTag(tagname.tag);

                    foreach (GameObject gameObj in deletetag)
                    {
                        ObjDelName = gameObj.name;
                        DelTagPos = GameObject.Find(ObjDelName).transform.position;

                        field[(int)DelTagPos.x, (int)DelTagPos.y, (int)DelTagPos.z] = 0;

                        Destroy(gameObj);
                    }
                }
            }

        }
        else if (obj2F != null)
        {
            //2F地盤の出力、部屋の出力、壁の出力、削除等を入力
            /*
             * 2階の地面生成の処理
             * -----------------------------------------------------------------
             */
            if (SecondStart == true)
            {
                SecondStart = false;
                SecondGrandDel = true; // 1階の処理で1回目に入らなかった処理に入れる

                //ArrayList = WallTopList.ToArray();
                foreach (GameObject delObj in WallTopList.ToArray())
                {
                    if (delObj == null)
                    {
                        WallTopList.Remove(delObj);
                    }

                }
                foreach (GameObject delObjTes in WallTopList)
                {
                    delObjTes.SetActive(false);
                }
                //1と0を代入
                for (int i = 0; i < field.GetLength(0); i++)
                {
                    for (int j = 0; j < field.GetLength(2); j++)
                    {
                        if (field[i, 4, j] == 1)
                        {
                            field[i, 4, j] = 2;
                        }
                        else if (field[i, 4, j] == 0)
                        {
                            field[i, 4, j] = 1;
                        }
                    }
                }

                //各インデックスに代入された値をもとに生成するorしない

                for (int i = 0; i < field.GetLength(0); i++)
                {
                    for (int j = 0; j < field.GetLength(2); j++)
                    {
                        //インデックスの値が1の時、cubeを生成
                        if (field[i, 4, j] == 1)
                        {
                            GameObject wall = Instantiate(secondgrand);
                            // wallの名前の変更 countによって一つ一つに番号をふる
                            wall.name = "secondgrand" + count;
                            wall.transform.position = new Vector3(i, 4, j);
                            count++;
                        }//インデックスの値が2の時、cubeを生成
                        else if (field[i, 4, j] == 2)
                        {
                            GameObject wall = Instantiate(blackcube);
                            //wallの名前の変更 countによって一つ一つに番号をふる
                            wall.name = "blackcube" + blackcubecount;
                            wall.transform.position = new Vector3(i, 4, j);
                            blackcubecount++;
                        }
                    }
                }
                // 配列に括弧内で指定したタグが付いているオブジェクトを格納
                SecondGrandDelTag = GameObject.FindGameObjectsWithTag("2fgrand");
            }
            /*
             * (2階の地面生成処理)
             * -----------------------------------------------------------------
             */
            //2階の部屋生成
            if (roomb != null)
            {
                //ボタンを押したとき
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    //スタートの座標を取得
                    if (Physics.Raycast(ray, out hit))
                    {
                        startObjectName = hit.collider.gameObject.name;

                        tmp = GameObject.Find(startObjectName).transform.position;

                        tmp1 = tmp;


                    }

                }
                //ボタンを離した処理
                if (Input.GetMouseButtonUp(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    //エンドの座標を取得
                    if (Physics.Raycast(ray, out hit))
                    {
                        endObjectName = hit.collider.gameObject.name;

                        tmp = GameObject.Find(endObjectName).transform.position;

                        tmp2 = tmp;


                        // 括弧内の文字列が入っているとオブジェクト生成の処理に入らない
                        if (startObjectName.Contains("masutile") || endObjectName.Contains("masuwall"))
                        {

                        }
                        else
                        {
                            if (tmp1.x <= tmp2.x || tmp1.z >= tmp2.z)
                            {
                                tmp3.x = tmp2.x - tmp1.x;
                                tmp3.z = tmp1.z - tmp2.z;

                                for (int s = 0; tmp3.z >= s; s++)
                                {
                                    tmp.z = tmp1.z - s;

                                    for (int t = 0; tmp3.x >= t; t++)
                                    {
                                        tmp.x = tmp1.x + t;
                                        tmp.y = tmp1.y;

                                        //選択した座標を配列の要素番号に代入からのその上に床オブジェクトを配置
                                        field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z] = 1;

                                        //インデックスの値が1の時、cubeを生成
                                        if (field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z] == 1)
                                        {
                                            //Assetsからmasuを取得
                                            GameObject floor = Instantiate(masutile);
                                            //オブジェクト生成時に"masu番号"に名前変更
                                            floor.name = "masutile" + count;
                                            floor.transform.position = new Vector3(tmp.x, tmp.y + 1, tmp.z);
                                            count++;
                                            floor.tag = "2fRoom" + SecondRoomCount;
                                        }
                                    }
                                }
                                for (int s = 0; tmp3.z >= s; s++)
                                {
                                    tmp.z = tmp1.z - s;

                                    for (int t = 0; tmp3.x >= t; t++)
                                    {
                                        tmp.x = tmp1.x + t;
                                        tmp.y = tmp1.y;

                                        if (field[(int)tmp.x + 1, (int)tmp.y + 1, (int)tmp.z] == 0 || field[(int)tmp.x - 1, (int)tmp.y + 1, (int)tmp.z] == 0 || field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z + 1] == 0 || field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z - 1] == 0)
                                        {
                                            // 選択した座標を配列の要素番号に代入からのその上に壁オブジェクトを配置
                                            field[(int)tmp.x, (int)tmp.y + 2, (int)tmp.z] = 1;
                                            field[(int)tmp.x, (int)tmp.y + 3, (int)tmp.z] = 1;
                                            field[(int)tmp.x, (int)tmp.y + 4, (int)tmp.z] = 1;

                                            //インデックスの値が1の時、cubeを生成
                                            for (int u = 2; u <= 4; u++)
                                            {
                                                if (field[(int)tmp.x, (int)tmp.y + u, (int)tmp.z] == 1)
                                                {
                                                    //Assetsからmasuを取得
                                                    GameObject wall = Instantiate(masuwall);

                                                    //オブジェクト生成時に"masu番号"に名前変更
                                                    wall.name = "masuwall" + count;
                                                    wall.transform.position = new Vector3(tmp.x, tmp.y + u, tmp.z);
                                                    count++;
                                                    wall.tag = "2fRoom" + SecondRoomCount;
                                                    // 1番上の壁をListに格納
                                                    if (u == 4)
                                                    {
                                                        WallTopList.Add(wall);
                                                    }

                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                    SecondRoomCount++;
                }

            }
            //削除する
            if (deleteb != null)
            {
                //ボタンを押した処理
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    //座標を取得
                    if (Physics.Raycast(ray, out hit))
                    {
                        startObjectName = hit.collider.gameObject.name;

                        tmp = GameObject.Find(startObjectName).transform.position;

                    }
                    GameObject tagname = GameObject.Find(startObjectName);

                    GameObject[] deletetag = GameObject.FindGameObjectsWithTag(tagname.tag);

                    foreach (GameObject gameObj in deletetag)
                    {
                        ObjDelName = gameObj.name;
                        DelTagPos = GameObject.Find(ObjDelName).transform.position;

                        field[(int)DelTagPos.x, (int)DelTagPos.y, (int)DelTagPos.z] = 0;

                        Destroy(gameObj);
                    }
                }
            }
        }

    }
}
