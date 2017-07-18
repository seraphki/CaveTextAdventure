using UnityEngine;

public enum NameGenerationStringType { RoomNoun, Noun, Verb }
public class RoomNameGenerator
{
    private static bool _allWordsLoaded
    {
        get
        {
            return (RoomNouns.Length > 0 && Nouns.Length > 0 && Verbs.Length > 0);
        }
    }

    private static string[] _roomNouns;
    public static string[] RoomNouns
    {
        get
        {
            if (_roomNouns == null)
            {
                string text = Helpers.LoadNameGenerationFile(NameGenerationStringType.RoomNoun);
                _roomNouns = text.Split(new string[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
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
                string text = Helpers.LoadNameGenerationFile(NameGenerationStringType.Noun);
                _nouns = text.Split(new string[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            }
            return _nouns;
        }
    }

    private static string[] _verbs;
    public static string[] Verbs
    {
        get
        {
            if (_verbs == null)
            {
                string text = Helpers.LoadNameGenerationFile(NameGenerationStringType.Verb);
                _verbs = text.Split(new string[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            }
            return _verbs;
        }
    }

    public static string GetRandomName()
    {
        int style = Random.Range(0, 3);
        string name = "";

        if (_allWordsLoaded)
        {
            if (style == 0)
            {
                name = "The " + GetRandomString(NameGenerationStringType.RoomNoun) 
                    + " of " + GetRandomString(NameGenerationStringType.Noun);
            }
            else if (style == 1)
            {
                name = "The " + GetRandomString(NameGenerationStringType.Verb) 
                    + " " + GetRandomString(NameGenerationStringType.RoomNoun);
            }
            else
            {
                name = "The " + GetRandomString(NameGenerationStringType.Verb) 
                    + " " + GetRandomString(NameGenerationStringType.RoomNoun)
                    + " of " + GetRandomString(NameGenerationStringType.Noun);
            }
        }
        else
        {
            name = "The Room";
        }

        return name;
    }


    private static string GetRandomString(NameGenerationStringType type)
    {
        if (type == NameGenerationStringType.Verb)
        {
            int index = Random.Range(0, Verbs.Length);
            return Verbs[index];
        }
        else if (type == NameGenerationStringType.Noun)
        {
            int index = Random.Range(0, Nouns.Length);
            return Nouns[index];
        }
        else if (type == NameGenerationStringType.RoomNoun)
        {
            int index = Random.Range(0, RoomNouns.Length);
            return RoomNouns[index];
        }
        return "";
    }
}
