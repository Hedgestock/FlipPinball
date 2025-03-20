// Online C# Editor for free
// Write, Edit and Run your C# code using C# Online Compiler

using System;

public class HelloWorld
{
    public static void Main(string[] args)
    {
        const int s = 15;
        const int max = 9;
        for (int i = 1; i < max ;i++)
        {
            Console.WriteLine (
                $"{100000 * (long)Math.Pow((i),   (i+1f)/2),s}({i,2})"+
                $"{100000 * (long)Math.Pow((i+1), (i+1f)/2),s}({i,2})"+
                $"{200000 * (long)Math.Pow((i),   (i+1f)/2),s}({i,2})"+
                $"{200000 * (long)Math.Pow((i+1), (i+1f)/2),s}({i,2})"
                );
        }
        Console.WriteLine ();
        for (int i = 1; i < max ;i++)
        {
            Console.WriteLine (
                $"{100000 * (long)Math.Pow((i),   i/2f),s}({i,2})"+
                $"{100000 * (long)Math.Pow((i+1), i/2f),s}({i,2})"+
                $"{200000 * (long)Math.Pow((i),   i/2f),s}({i,2})"+
                $"{200000 * (long)Math.Pow((i+1), i/2f),s}({i,2})"
                );
        }
        Console.WriteLine ();
        for (int i = 1; i < max ;i++)
        {
            Console.WriteLine (
                $"{100000 * (long)Math.Pow((i),   (i+1f)),s}({i,2})"+
                $"{100000 * (long)Math.Pow((i+1), (i+1f)),s}({i,2})"+
                $"{200000 * (long)Math.Pow((i),   (i+1f)),s}({i,2})"+
                $"{200000 * (long)Math.Pow((i+1), (i+1f)),s}({i,2})"
                );
        }
        Console.WriteLine ();
        for (int i = 1; i < max ;i++)
        {
            Console.WriteLine (
                $"{100000 * (long)Math.Pow((i),   i),s}({i,2})"+
                $"{100000 * (long)Math.Pow((i+1), i),s}({i,2})"+
                $"{200000 * (long)Math.Pow((i),   i),s}({i,2})"+
                $"{200000 * (long)Math.Pow((i+1), i),s}({i,2})"
                );
        }
    }
}