namespace ticketManage.Utils
{
    public static class Utility
    {
        public static bool IsNullOrEmpty<T>(T value)
        {
            if (value is null)
                return true;

            if (value is string str)
                return string.IsNullOrEmpty(str);

            if (value is System.Collections.ICollection collection)
                return collection.Count == 0;

            return false;
        }

        public static bool IsNotNullOrEmpty<T>(T value)
        {
            return !IsNullOrEmpty(value);
        }
    }
}
