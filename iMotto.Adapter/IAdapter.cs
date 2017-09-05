namespace iMotto.Adapter
{
    public interface IAdapter
    {
        IHandler GetHandler(string code);
    }
}