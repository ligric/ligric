namespace Test
{
    public class TestTypeSecond : TestTypeAbstaction
    {
        public string Name { get; }

        public TestTypeSecond(int id, string name)
            :base(id)
        {
            Name = name;
        }
    }
}
