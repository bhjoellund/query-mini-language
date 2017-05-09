namespace Storytel.Qml
{
    public interface ICompiler<in TInput, out TOutput>
    {
        TOutput Compile(TInput source);
    }
}