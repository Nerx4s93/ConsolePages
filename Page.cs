namespace ConsolePages
{
    public abstract class Page
    {
        public abstract void OnLoaded();

        public abstract void OnInputData(string data);
    }
}
