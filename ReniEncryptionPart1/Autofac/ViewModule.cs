using Autofac;
using ViewModels.ViewModels;
using ViewModels.ViewModels.UserBlockAlg;

namespace ReniEncryptionPart1.Autofac
{
    internal class ViewModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new View.ComparisonBlockAlg() { DataContext = c.Resolve<ComparisonBlockAlgViewModel>() }).AsSelf();
            builder.Register(c => new View.CryptoTransformData() { DataContext = c.Resolve<CryptoTransformDataViewModel>() }).AsSelf();
            builder.Register((c, p) => new View.MainUserMenu() { DataContext = p.Named<MainUserMenu_ViewModel>("windows") }).AsSelf();
        }
    }
}
