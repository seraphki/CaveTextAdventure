public static class Helpers
{
    public static bool StringLooseCompare(string stringa, string stringb)
    {
        return stringa.Trim().ToLower() == stringb.Trim().ToLower();
    }
}
