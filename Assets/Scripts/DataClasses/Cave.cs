public enum Direction { North, East, South, West }
public class Cave
{
    public Level[] Levels;

    public Cave(int depth)
    {
        Levels = new Level[depth];
        for (int i = 0; i < depth; i++)
        {
            Levels[i] = new Level(i, 10);
        }
    }
}