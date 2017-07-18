namespace FTPUse_gui.Helpers
{
    public static class Extentions
    {
        public static T DefaultReplace<T>(this T value, T alternate) => value.Equals(default(T)) ? alternate : value;
    }
}
