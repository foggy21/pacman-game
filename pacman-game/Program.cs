internal class Program
{
    private static void Main(string[] args)
    {
        Console.CursorVisible = false;

        bool gameLoop = true;
        int playerPositionX = 1, playerPositionY = 1;
        char player = '@';

        char[,] map = ReadMap("map.txt");

        DrawMap(map, ConsoleColor.Blue);
        DrawEntity(map, playerPositionX, playerPositionY, player, ConsoleColor.Yellow);

        while (gameLoop)
        {
            HoldInput(map, ref playerPositionX, ref playerPositionY, player);
        }
        
    }
    
    private static bool HoldInput(char[,] map, ref int positionX, ref int positionY, char entityPlayer) 
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
                break;
        }

        if (map[oldPositionX, oldPositionY] != '#')
        {
            DrawEntity(map, oldPositionX, oldPositionY, entityPlayer, ConsoleColor.Yellow);
            DeleteEntity(positionX, positionY);
            positionX = oldPositionX;
            positionY = oldPositionY;
            return true;
        }
        return false;
    }

    private static void DeleteEntity(int x, int y)
    {
        Console.SetCursorPosition(x+1, y);
        Console.Write("\b ");
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