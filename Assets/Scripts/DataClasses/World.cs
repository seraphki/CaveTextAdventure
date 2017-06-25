public class World
{
    public WorldData StaticData;
    private Level[] _worldLevels;

    public int LevelCount
    {
        get
        {
            return _worldLevels.Length;
        }
    }

    public World()
    {
        StaticData = WorldDatabase.GetWorldData();
        _worldLevels = new Level[LevelDatabase.LevelCount];
        for (int i = 0; i < _worldLevels.Length; i++)
        {
            _worldLevels[i] = new Level(i + 1);
        }
    }

    public Level GetLevel(int index)
    {
        if (index < _worldLevels.Length)
            return _worldLevels[index];

        return null;
    }
}