namespace Pippi.Qml
{
    public interface ICompiler<in TInput, out TOutput>
    {
        TOutput Compile(TInput source);
    }
}