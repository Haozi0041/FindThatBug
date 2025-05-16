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
    /// 13 ����  10 �س� ��13 ��10
    /// 33 Ϊƫ����
    /// ���������Ͻ�000 ��������
    /// </summary>

    public GameObject ground;
    private int maxLine = 100;//�������
    private int maxRow = 200;//ÿ������ַ���
    private int currentMaxRow;//��ǰÿ������ַ���
    private int lineDex;
    private int rowDex;
    public TileBase[] tileArray;//��Ƭ�������
    private Tilemap tilemap;//��Ƭ��ͼ
    private int[] curntFont;//�������������ASCII�ַ����� 
    string textPath = "Assets/C#/MapSpaw.txt";//�ĵ���ַ
  
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
                Debug.Log("����ÿ���������");
                return;
            }
            if (currentFontState == Font.newLine)
            {
                SetGround(ground, rowDex, lineDex);//ÿ�λ������ɵ���
                lineDex++;
                rowDex = 0;
                if (lineDex >= maxLine)
                {
                    Debug.Log(lineDex);
                    Debug.Log("�����������");
                    return;
                }
            }
            if (currentFontState == Font.font || currentFontState == Font.space)//ֻ�пո���ַ���������
            {
                Debug.Log(lineDex + "-----" + rowDex);
                SetTile(lineDex, rowDex, FontToTileBase(font));
                
                rowDex++;//���������ַ����������
                if (rowDex > currentMaxRow)//�������������
                {
                    currentMaxRow = rowDex;
                }
                //SetTile(lineDex, rowDex, null);
            }
            font = sr.Read();
        }
        SetGround(ground, rowDex, lineDex);//���һ�еײ����ɵ���
        sr.Close();
        Debug.Log("��ǰ���������" + (lineDex+1) + "��ǰÿ������ַ���Ϊ: " + currentMaxRow);
    }


    private void TileAssestInit()//��������ַ��ز�32 - 126 ���ַ� 
    {
        tileArray = Resources.LoadAll<TileBase>("Font/Words");
    }

    private void SetTile(int line ,int row,TileBase tile)//�ڵ�line �� row �������ַ�
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
