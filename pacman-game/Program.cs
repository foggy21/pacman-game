﻿using System;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.CursorVisible = false;

        bool gameLoop = true;
        int playerPositionX = 1, playerPositionY = 1;
        int countOfEnemies;

        char player = '@';

        char[,] map = ReadMap("map.txt");
        int[,] positionsOfEnemies = ReadPositionsOfEnemies("positions of enemies.txt", out countOfEnemies);
        
        DrawMap(map, ConsoleColor.Blue);
        DrawEnemies(map, positionsOfEnemies);
        DrawEntity(map, playerPositionX, playerPositionY, player, ConsoleColor.Yellow);

        Task.Run(() =>
        {
            while(gameLoop)
                HoldInput(map, ref playerPositionX, ref playerPositionY, player);
        });

        while (gameLoop)
        {
            MoveEnemies(map, positionsOfEnemies);
            DrawEnemies(map, positionsOfEnemies);
            Thread.Sleep(500);
        }
    }
    
    private static void HoldInput(char[,] map, ref int positionX, ref int positionY, char entityPlayer) 
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey();

        int oldPositionX = positionX;
        int oldPositionY = positionY;

        switch (keyInfo.Key)
        {
            case (ConsoleKey.UpArrow):
                oldPositionY--;
                break;
            case (ConsoleKey.DownArrow):
                oldPositionY++;
                break;
            case (ConsoleKey.LeftArrow):
                oldPositionX--;
                break;
            case (ConsoleKey.RightArrow):
                oldPositionX++;
                break;
            default:
                Console.Write("\b \b");
                return;
        }

        if (map[oldPositionX, oldPositionY] != '#')
        {
            DrawEntity(map, oldPositionX, oldPositionY, entityPlayer, ConsoleColor.Yellow);
            DeleteEntity(positionX, positionY);
            positionX = oldPositionX;
            positionY = oldPositionY;
        }
    }

    private static void MoveEnemies(char[,] map, int[,] positionsOfEnemies) 
    {
        Random randomDirection = new Random();
        int directionX, directionY;
        int newPositionX, newPositionY;
        for (int x = 0; x < positionsOfEnemies.GetLength(0); ++x)
        {
            directionX = randomDirection.Next(-1, 2);
            directionY = randomDirection.Next(-1, 2);
            newPositionX = positionsOfEnemies[x, 0] + directionX;
            newPositionY = positionsOfEnemies[x, 1] + ((directionX != 0) ? directionY * 0 : directionY);

            if (map[newPositionX, newPositionY] != '#')
            {
                DeleteEntity(positionsOfEnemies[x, 0], positionsOfEnemies[x, 1]);
                positionsOfEnemies[x, 0] = newPositionX;
                positionsOfEnemies[x, 1] = newPositionY;
            }
        }
    }

    private static void DeleteEntity(int x, int y)
    {
        Console.SetCursorPosition(x+1, y);
        Console.Write("\b \b");
    }

    private static void DrawEntity(char[,] map, int x, int y, char entity, ConsoleColor color = ConsoleColor.White)
    {
        ConsoleColor defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = color;

        if (map[x, y] != '#')
        {
            Console.SetCursorPosition(x, y);
            Console.Write(entity);
        }

        Console.ForegroundColor = defaultColor;
    }

    private static void DrawEnemies(char[,] map, int[,] positionsOfEnemies)
    {
        char enemy = '^';

        for (int x = 0; x < positionsOfEnemies.GetLength(0); ++x)
        {
            int enemyPosX = positionsOfEnemies[x, 0];
            int enemyPosY = positionsOfEnemies[x, 1];

            DrawEntity(map, enemyPosX, enemyPosY, enemy, ConsoleColor.Red);
        }
    }

    private static void DrawMap(char[,] map, ConsoleColor color = ConsoleColor.White)
    {
        ConsoleColor defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = color;

        for (int y = 0; y < map.GetLength(1); ++y)
        {
            for (int x = 0; x < map.GetLength(0); ++x)
                Console.Write(map[x, y]);
            Console.Write('\n');
        }

        Console.ForegroundColor = defaultColor;
    }

    private static char[,] ReadMap(string path)
    {
        string[] lines = File.ReadAllLines(path);
        char[,] map = new char[GetMaxLengthOfRow(lines), lines.Length];

        for (int y = 0; y < map.GetLength(1); ++y)
            for (int x = 0; x < map.GetLength(0); ++x)
                map[x, y] = lines[y][x];

        return map;
    }

    private static int[,] ReadPositionsOfEnemies(string path, out int countOfEnemies)
    {
        string[] file = File.ReadAllLines(path);
        countOfEnemies = file.Length;
        int[,] positionOfEnemies = new int[countOfEnemies, 2];

        int x = 0;
        foreach(string line in file)
        {
            string[] position = line.Split(' ');
            positionOfEnemies[x, 0] = Convert.ToInt32(position[0]);
            positionOfEnemies[x++, 1] = Convert.ToInt32(position[1]);
        }
        return positionOfEnemies;
    }

    private static int GetMaxLengthOfRow(string[] rows)
    {
        int maxLength = rows[0].Length;
        foreach (string row in rows)
        {
            if (row.Length > maxLength) 
                maxLength = row.Length;
        }
        return maxLength;
    }
}