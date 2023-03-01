internal class Program
{
    private static void Main(string[] args)
    {
        char[,] map = ReadMap("map.txt");

        DrawMap(map);
    }

    private static void DrawMap(char[,] map)
    {
        for (int y = 0; y < map.GetLength(1); ++y)
        {
            for (int x = 0; x < map.GetLength(0); ++x)
                Console.Write(map[x, y]);
            Console.Write('\n');
        }
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