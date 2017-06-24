using UnityEngine;

public class RoomNameGenerator
{
    private enum StringType { RoomNoun, Noun, Adjective }

    private static string[] _roomNouns;
    public static string[] RoomNouns
    {
        get
        {
            if (_roomNouns == null)
            {
                TextAsset textAsset = Resources.Load("Text/RoomNouns") as TextAsset;
                _roomNouns = textAsset.text.Split(new string[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            }
            return _roomNouns;
        }
    }

    private static string[] _nouns;
    public static string[] Nouns
    {
        get
        {
            if (_nouns == null)
            {
                TextAsset textAsset = Resources.Load("Text/Nouns") as TextAsset;
                _nouns = textAsset.text.Split(new string[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            }
            return _nouns;
        }
    }

    private static string[] _adjectives;
    public static string[] Adjectives
    {
        get
        {
            if (_adjectives == null)
            {
                TextAsset textAsset = Resources.Load("Text/Adjectives") as TextAsset;
                _adjectives = textAsset.text.Split(new string[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            }
            return _adjectives;
        }
    }

    public static string GetRandomName()
    {
        int style = Random.Range(0, 3);
        string name = "";

        if (style == 0)
        {
            name = GetRandomString(StringType.RoomNoun) + " of " + GetRandomString(StringType.Noun);
        }
        else if (style == 1)
        {
            name = "The " + GetRandomString(StringType.Adjective) + " " + GetRandomString(StringType.RoomNoun);
        }
        else
        {
            name = "The " + GetRandomString(StringType.Adjective) + " " + GetRandomString(StringType.RoomNoun)
                + " of " + GetRandomString(StringType.Noun);
        }

        return name;
    }


    private static string GetRandomString(StringType type)
    {
        if (type == StringType.Adjective)
        {
            int index = Random.Range(0, Adjectives.Length);
            return Adjectives[index];
        }
        else if (type == StringType.Noun)
        {
            int index = Random.Range(0, Nouns.Length);
            return Nouns[index];
        }
        else if (type == StringType.RoomNoun)
        {
            int index = Random.Range(0, RoomNouns.Length);
            return RoomNouns[index];
        }
        return "";
    }
}
