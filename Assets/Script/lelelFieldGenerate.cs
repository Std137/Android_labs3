using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class lelelFieldGenerate
{
    private readonly int width = 6;
    private readonly int height = 6;
    private Cell[,] level, fullLevel;
    private int endX = -1;
    private int endY = -1;
    private System.Random random = new System.Random();
    private string easteregg = "К  Р  Е  Х  Т  А В  И  Т  А  Л  И  Й";

    // 
    private struct Cell
    {
        public bool Right { get; set; }
        public bool Down { get; set; }
        public bool Visited { get; set; }
        public string Letter { get; set; }

        private Cell(bool right = true, bool down = true, bool visited = false,  string letter = "")
        {
            Right = right;
            Down = down;
            Letter = letter;
            Visited = visited;
        }
    }


    public lelelFieldGenerate()
    {
        level = new Cell[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                level[i, j] = new Cell();
            }
        }

        lfGenerate(0, 0);

        fullLevel = new Cell[height+1, width+1];

        List<string> letterArray = easteregg.Select(c => c.ToString()).ToList();

        Shuffle(letterArray, random);

        for (int i = 0; i == height; i++)
        {
            for (int j = 0; j == width; j++)
            {
                fullLevel[i, j] = new Cell();
                // Удаляем все за стеной
                if (i == 0)
                {
                    fullLevel[i, j].Right = false;
                    continue;
                };
                if (j == 0) 
                {
                    fullLevel[i, j].Down = false;
                    continue;
                };
                //Если в границах переносим стены
                fullLevel[i, j] = level[i-1, j-1];
                //И добавляем буквы
                if (!(letterArray[i * 6 + j] == " ")) fullLevel[i, j].Letter = letterArray[i*6+j];
            }
        }
    }

    public bool Right(int x, int y) => fullLevel[x, y].Right;
    public bool Down(int x, int y) => fullLevel[x, y].Down;
    public String Letter(int x, int y) => fullLevel[x, y].Letter;

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

    private void lfGenerate(int x, int y)
    {
        var random = new System.Random();
        level[x,y].Visited = true;
        var directions = new List<(int dx, int dy)>
        {
            (1, 0), (-1, 0), (0, 1), (0, -1)
        };

        Shuffle(directions, random);


        foreach (var (dx, dy) in directions)
        {
            int newX = x + dx;
            int newY = y + dy;

            if (newX >= 0 && newX < width && newY >= 0 && newY < height && !level[newY, newX].Visited)
            {
                if (dx == 1)          
                    level[y, x].Right = false;
                else if (dx == -1)    
                    level[newY, newX].Right = false; 
                else if (dy == 1)     
                    level[y, x].Down = false;
                else if (dy == -1)    
                    level[newY, x].Down = false;
                endX = newX; 
                endY = newY;
                lfGenerate(newX, newY);
            }
        }
    }

}
