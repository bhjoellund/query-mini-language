namespace Pippi.Qml
{
    internal interface ICodeGenerator<in TInput, out TOutput>
    {
        TOutput Generate(TInput input);
    }
}
