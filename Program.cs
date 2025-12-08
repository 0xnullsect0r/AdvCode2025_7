using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class AdvCode2025_7
{
    static void Main()
    {
        string[] lines = null;
        
        string[] possiblePaths = new[] 
        { 
            "input.txt",
            "../../../input.txt",
            "../../../../input.txt"
        };
        
        foreach (var path in possiblePaths)
        {
            if (File.Exists(path))
            {
                lines = File.ReadAllLines(path);
                Console.WriteLine($"Reading from: {Path.GetFullPath(path)}");
                break;
            }
        }
        
        if (lines == null)
        {
            Console.WriteLine("ERROR: Could not find input.txt");
            Console.WriteLine("Please create input.txt in one of these locations:");
            Console.WriteLine("  - Project root: C:\\Users\\ari\\RiderProjects\\AdvCode2025_7\\input.txt");
            Console.WriteLine("  - Or in the same folder as the .exe");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
            return;
        }
        

        lines = lines.Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        
        if (lines.Length == 0)
        {
            Console.WriteLine("ERROR: Input file is empty");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
            return;
        }
        
        Console.WriteLine($"Grid size: {lines.Length} rows");
        
        int maxLen = lines.Max(l => l.Length);
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length < maxLen)
            {
                lines[i] = lines[i].PadRight(maxLen, '.');
            }
        }
        Console.WriteLine($"Grid width: {maxLen} columns");
        
        int splits = CountBeamSplits(lines);
        Console.WriteLine($"\nTotal beam splits: {splits}");
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static int CountBeamSplits(string[] grid)
    {
        if (grid == null || grid.Length == 0)
            return 0;
            
        int height = grid.Length;
        int width = grid[0].Length;
        int splitCount = 0;
        
        int startRow = -1;
        int startCol = -1;
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < grid[row].Length && col < width; col++)
            {
                if (grid[row][col] == 'S')
                {
                    startRow = row;
                    startCol = col;
                    break;
                }
            }
            if (startRow != -1) break;
        }
        
        if (startRow == -1 || startCol == -1)
        {
            Console.WriteLine("Error: Could not find starting position 'S'");
            return 0;
        }
        
        Console.WriteLine($"Starting at row {startRow}, col {startCol}");
        
        HashSet<(int row, int col)> visited = new HashSet<(int, int)>();
        
        Queue<(int row, int col)> queue = new Queue<(int, int)>();
        queue.Enqueue((startRow + 1, startCol));
        
        while (queue.Count > 0)
        {
            var (row, col) = queue.Dequeue();
            
            while (true)
            {
                if (row < 0 || row >= height || col < 0 || col >= width)
                    break;
                
                if (col >= grid[row].Length)
                    break;
                    
                char cell = grid[row][col];
                
                if (cell == '^')
                {
                    if (!visited.Contains((row, col)))
                    {
                        visited.Add((row, col));
                        splitCount++;

                        if (col > 0)
                            queue.Enqueue((row + 1, col - 1));
                        if (col < width - 1)
                            queue.Enqueue((row + 1, col + 1));
                    }
                    break;
                }
                
                row++;
            }
        }
        
        Console.WriteLine($"Total splitters activated: {splitCount}");
        
        return splitCount;
    }
}