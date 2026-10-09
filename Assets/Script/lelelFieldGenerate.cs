using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.Rendering.DebugUI.Table;

public class Cell
{
    public bool Right { get; set; }
    public bool Down { get; set; }
    public bool Visited { get; set; }
    public string Letter { get; set; }

    public Cell(bool right = true, bool down = true, bool visited = false, string letter = " ")
    {
        Right = right;
        Down = down;
        Letter = letter;
        Visited = visited;
    }
}

public class LelelFieldGenerate
{
    private readonly int width = 6;
    private readonly int height = 6;
    private List<List<Cell>> level, fullLevel;
    private int endX = -1;
    private int endY = -1;
    private System.Random random = new System.Random();
    private string easteregg = "К Р  Е  Х  Т  А В  И  Т  А  Л  И Й";
    
    public LelelFieldGenerate()
    {
        level = new List<List<Cell>>(height);
        
        for (int x = 0; x < height; x++)
        {
            var row = new List<Cell>(width);
            for (int y = 0; y < width; y++)
            {
                row.Add(new Cell());
            }
            level.Add(row);
        }

        lfGenerate(0, 0);

        var rows = new List<Cell>(width + 1);

        for (int y = 0; y < width + 1; y++)
        {
            rows.Add(new Cell());
        }

        level.Insert(0, rows);

        List<string> letterArray = easteregg.Select(c => c.ToString()).ToList();

        Shuffle(letterArray, random);

        letterArray.Insert(0, " "); 
        letterArray.Add(" ");

        for (int x = 0; x < width + 1; x++)
        {
            for (int y = 0; y < height + 1; y++)
            {
                if (x == 0)
                {
                    if (y==0) level[x][y] = new Cell(false, false, true, " ");
                    else level[x][y] = new Cell(false, true, true, " ");
                    continue;
                }
                if (y == 0)
                {
                    level[x].Insert(0, new Cell(true, false, true, " "));
                }
                else
                {
                    level[x][y].Letter = letterArray[((x - 1) * 6) + (y - 1)];
                }
            }
        }
    }

    public bool Right(int x, int y) => level[x][y].Right;
    public bool Down(int x, int y) => level[x][y].Down;
    public String Letter(int x, int y) => level[x][y].Letter;

    public static void Shuffle<T>(List<T> list, System.Random rand)
    {
        if (list == null || list.Count <= 1 || rand == null)
            return;

        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    // x строка y столбец
    private void lfGenerate(int x, int y)
    {
        level[x][y].Visited = true;

        var directions = new List<(int dx, int dy)>
        {
            (1, 0), (-1, 0), (0, 1), (0, -1)
        };

        Shuffle(directions, random);

        foreach (var (dx, dy) in directions)
        {
            int newX = x + dx; // смещение по строке
            int newY = y + dy; // смещение по столбцу

            if (newX >= 0 && newX < height && newY >= 0 && newY < width && !level[newX][newY].Visited)
            {
                if (dy == 1)          
                    level[x][y].Right = false;
                else if (dy == -1)    
                    level[x][newY].Right = false; 
                else if (dx == 1)     
                    level[x][y].Down = false;
                else if (dx == -1)    
                    level[newX][y].Down = false;
                endX = newX; 
                endY = newY;
                lfGenerate(newX, newY);
            }
        }
    }
    public bool GetRight(int x, int y) => level[x][y].Right;
    public bool GetDown(int x, int y) =>level[x][y].Down;
    public string GetLetter(int x, int y) =>level[x][y].Letter;
}
