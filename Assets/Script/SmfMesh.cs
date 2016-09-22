using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

public class Triangle3D
{
    public uint p1_;
    public uint p2_;
    public uint p3_;

    public Triangle3D(uint p1, uint p2, uint p3)
    {
        p1_ = p1;
        p2_ = p2;
        p3_ = p3;
    }
}

[Serializable]
public class SmfMesh : UnityEngine.Object
{
    public List<Vector3> m_listPoints = new List<Vector3>();
    public List<Vector3> m_listNormals = new List<Vector3>();
    public List<Vector2> m_listUVs = new List<Vector2>();
    public List<Triangle3D> m_listTriangles = new List<Triangle3D>();

    public float m_middleX;
    public float m_middleY;
    public float m_middleZ;

    public string m_MatShaderName;
    public string m_MatMainTextureName;

    //读取smf文件，包含定点坐标，法线，文理uv，网格索引，材质shader，材质文理
    public void loadFile(string filename){
        
        FileStream aFile = new FileStream(filename, FileMode.OpenOrCreate);
        StreamReader sr = new StreamReader(aFile);
        string content = sr.ReadToEnd();
        string[] contents = content.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

        int curLine = 0;
        while (curLine < contents.Length)
        {
            string[] lineContents = contents[curLine].Split(new string[] { "\t"," " }, StringSplitOptions.RemoveEmptyEntries);
            if (lineContents[0] == "v")
            {
                int curStr = 1;
                m_listPoints.Add(new Vector3((float)Convert.ToDouble(lineContents[curStr++]), (float)Convert.ToDouble(lineContents[curStr++]), (float)Convert.ToDouble(lineContents[curStr++])));
                m_listUVs.Add(new Vector2((float)Convert.ToDouble(lineContents[curStr++]), (float)Convert.ToDouble(lineContents[curStr++])));
                m_listNormals.Add(new Vector3((float)Convert.ToDouble(lineContents[curStr++]), (float)Convert.ToDouble(lineContents[curStr++]), (float)Convert.ToDouble(lineContents[curStr++])));
            }
            else if (lineContents[0] == "f")
            {
                int curStr = 1;
                m_listTriangles.Add(new Triangle3D(Convert.ToUInt32(lineContents[curStr++]), Convert.ToUInt32(lineContents[curStr++]), Convert.ToUInt32(lineContents[curStr++])));
            }
            else if (lineContents[0].StartsWith("m"))
            {
                string[] con = contents[curLine].Split(',');
                m_MatShaderName = con[1];
                m_MatMainTextureName = con[2];
            }
            curLine++;
        }
        sr.Close();
        aFile.Close();

        for(int i = 0; i < m_listPoints.Count; i++)
        {
            m_middleX += m_listPoints[i].x;
            m_middleY += m_listPoints[i].y;
            m_middleZ += m_listPoints[i].z;
        }
        m_middleX /= m_listPoints.Count;
        m_middleY /= m_listPoints.Count;
        m_middleZ /= m_listPoints.Count;
    }

    public void saveFile(string filepath, float width, float height, int row, int col, string shaderName, string texName)
    {

        StringBuilder sb = new StringBuilder("");
        for(int i = 0; i < row+1; i++)
            for(int j = 0; j < col+1; j++)
            {
                //坐标
                sb.Append("v ");
                sb.Append(j * width);
                sb.Append(" ");
                sb.Append(i * height);
                sb.Append(" 0  \t");
                //uv
                sb.Append((float)j / col);
                sb.Append(" ");
                sb.Append((float)i / row);
                sb.Append("     \t");
                //normals
                sb.Append("0 0 1\r\n");
            }
        for(int i = 0; i < row; i++)
            for(int j = 0; j < col; j++)
            {
                int x = i * (col + 1) + j;
                int xUp = (i + 1) * (col + 1) + j;
                sb.Append("f ");
                sb.Append(x);
                sb.Append(" ");
                sb.Append(x + 1);
                sb.Append(" ");
                sb.Append(xUp + 1);

                sb.Append("\r\nf ");
                sb.Append(x);
                sb.Append(" ");
                sb.Append(xUp + 1);
                sb.Append(" ");
                sb.Append(xUp);
                sb.Append("\r\n");
            }
        sb.Append("m,");
        sb.Append(shaderName);
        sb.Append(",");
        sb.Append(texName);
        sb.Append("\r\n");

        FileStream fs = new FileStream(filepath, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(sb.ToString());

        sw.Close();
        fs.Close();


    }
}
