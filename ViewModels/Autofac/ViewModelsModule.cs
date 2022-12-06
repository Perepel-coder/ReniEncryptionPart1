using Autofac;
using MediatR;
using QueryCQRS.Queries;
using System;
using ViewModels.ViewModels;
using ViewModels.ViewModels.UserBlockAlg;

namespace ViewModels.Autofac
{
    public class ViewModelsModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new QueriesModule());
            builder.Register(c => new ComparisonBlockAlgViewModel( c.Resolve<IMediator>())).AsSelf();
            builder.Register(c => new CryptoTransformDataViewModel( c.Resolve<IMediator>())).AsSelf();
            builder.Register((c, p) => new MainUserMenu_ViewModel(p.Named<Action[]>("windows"))).AsSelf();
        }
    }
}
