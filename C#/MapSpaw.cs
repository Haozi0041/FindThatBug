using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Tilemaps;

enum Font
{
    space ,
    enter,
    newLine,
    font
}





//[ExecuteAlways]
public class MapSpaw : MonoBehaviour
{
    /// <summary>
    /// 13 换行  10 回车 先13 后10
    /// 33 为偏移量
    /// 坐标由左上角000 向下生成
    /// </summary>

    public GameObject ground;
    private int maxLine = 100;//最大行数
    private int maxRow = 200;//每行最大字符数
    private int currentMaxRow;//当前每行最大字符数
    private int lineDex;
    private int rowDex;
    public TileBase[] tileArray;//瓦片存放数组
    private Tilemap tilemap;//瓦片地图
    private int[] curntFont;//储存最近的三个ASCII字符输入 
    string textPath = "Assets/C#/MapSpaw.txt";//文档地址
  
    [ContextMenu("Play")]
    private void Start()
    {
        if (Application.IsPlaying(gameObject))
        {
            // 
        }
        else
        {
            Debug.Log("Test Begin.....");
            MapDataInit();
            tilemap = GetComponentInChildren<Tilemap>();
            TileAssestInit();
            ReadText();
            
        }
        
    }
    private void ReadText()
    {
        StreamReader sr = new StreamReader(textPath);
        int font = sr.Read();
        Font currentFontState;
        for (rowDex = 0;font !=-1 ; )
        {
            
            currentFontState = DetectFont(font);
            if (rowDex > maxRow && currentFontState == Font.font)
            {
                Debug.Log("超出每行最大列数");
                return;
            }
            if (currentFontState == Font.newLine)
            {
                SetGround(ground, rowDex, lineDex);//每次换行生成地面
                lineDex++;
                rowDex = 0;
                if (lineDex >= maxLine)
                {
                    Debug.Log(lineDex);
                    Debug.Log("超出最大行数");
                    return;
                }
            }
            if (currentFontState == Font.font || currentFontState == Font.space)//只有空格和字符进行设置
            {
                Debug.Log(lineDex + "-----" + rowDex);
                SetTile(lineDex, rowDex, FontToTileBase(font));
                
                rowDex++;//在设置完字符后更改索引
                if (rowDex > currentMaxRow)//更新最大行数；
                {
                    currentMaxRow = rowDex;
                }
                //SetTile(lineDex, rowDex, null);
            }
            font = sr.Read();
        }
        SetGround(ground, rowDex, lineDex);//最后一行底部生成地面
        sr.Close();
        Debug.Log("当前最大行数：" + (lineDex+1) + "当前每行最长的字符数为: " + currentMaxRow);
    }


    private void TileAssestInit()//添加所有字符素材32 - 126 个字符 
    {
        tileArray = Resources.LoadAll<TileBase>("Font/Words");
    }

    private void SetTile(int line ,int row,TileBase tile)//在第line 行 row 处设置字符
    {
        tilemap.SetTile(new Vector3Int(row*3, -line*6, 0), tile);
    }

    private void SetGround(GameObject ground,int length,int lineDex)
    {
        int groundeLenth = length* 3;
        float xPosition = groundeLenth / 2;
        int yPosition = lineDex * 6;
        GameObject newGround = Instantiate(ground, new Vector3(xPosition - 1, -yPosition - 1, 0), Quaternion.identity, transform.GetChild(1)) ;
        newGround.GetComponent<BoxCollider2D>().size = new Vector2(groundeLenth, 1);    
    }


    private void MapDataInit()
    {
        lineDex = 0;
        rowDex = 0;
        currentMaxRow = 0;
    }

    private Font DetectFont(int ascii)
    {
        switch (ascii)
        {
            case 32:
                return Font.space;
            case 10:
                return Font.newLine;
            case 13:
                return Font.enter;
            default:
                return Font.font;
        }
    }

    private TileBase FontToTileBase(int font)//84->T  ->51
    {
        if (DetectFont(font) != Font.font)
        {
            return null;
        }
        return tileArray[font - 33];
    }




    private IEnumerator Read()
    {
        StreamReader sr = new StreamReader(textPath);

        int ascii = sr.Read();
        while (ascii != -1)
        {
            Debug.Log(ascii);
            ascii = sr.Read();
            yield return null;
        }
        sr.Close();
    }
}
