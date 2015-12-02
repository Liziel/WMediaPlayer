namespace SharedDispatcher
{
    public class Dispatchable
    {
        internal object[]   ParamsList;
        internal string     Name;

        public Dispatchable(string name, object[] paramsList)
        {
            ParamsList = paramsList;
            Name = name;
        }

        public static SharedDispatcher.Dispatchable Create(string name, params object[] list)
        {
            return new Dispatchable(name, list);
        }

    }
}