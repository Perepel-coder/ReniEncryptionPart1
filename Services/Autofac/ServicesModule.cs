using Autofac;
namespace Services.Autofac
{
    public class ServicesModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ReqForCriptoAlgDB>().AsSelf();
            builder.RegisterType<DataTransformations>().AsSelf();
            builder.RegisterType<BenchmarkCryptoTransf>().AsSelf();
        }
    }
}
