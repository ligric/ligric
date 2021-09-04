namespace Test
{
    public class SomeModel<T> where T : TestTypeAbstaction
    {
        public IList<T> SomeList = new List<T>();

        public SomeModel(IList<T> list)
        {
            SomeList = list;
        }
    }
}
