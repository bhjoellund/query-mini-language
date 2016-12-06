namespace Pippi.Qml
{
    using Ninject.Modules;
    using Parsing;

    public class QmlIoCModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILanguageProvider>().To<QmlLanguageProvider>();
            Bind<ILexer>().To<QmlLexer>();
            Bind<IAnalyser>().To<QmlAnalyser>();
            Bind<ICodeGenerator<AbstractSyntaxTree, string>>().To<SolrCodeGenerator>();
            Bind<ICompiler<string, string>>().To<QmlCompiler>().InSingletonScope();
        }
    }
}
