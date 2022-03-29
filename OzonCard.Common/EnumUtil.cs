
namespace OzonCard.Common
{
    public class EnumUtil
    {
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static bool TryParseEnum<T>(string value, out object result)
        {
            return Enum.TryParse(typeof(T), value, true, out result);
        }

        public static T ParseEnumFull<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value.Split('.').Last(), true);
        }

        public static bool TryParseEnumFull<T>(string value, out object result)
        {
            return Enum.TryParse(typeof(T), value.Split('.').Last(), true, out result);
        }

        static public IEnumerable<T> GetAllValues<T>()
        {
            var list = new List<T>();

            foreach (T t in Enum.GetValues(typeof(T)))
                list.Add(t);
            return list;
        }

        static public IEnumerable<string> GetAllNames<T>(string prefix = "")
        {
            return Enum.GetNames(typeof(T)).Select(x => string.Format("{0}{1}", prefix, x));

        }
    }
}
