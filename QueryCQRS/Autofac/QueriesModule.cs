using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using System.Reflection;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using QueryCQRS.Handlers;
using Services;
using Services.Autofac;
using Module = Autofac.Module;

namespace QueryCQRS.Queries
{
    public class QueriesModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<ServicesModule>();
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>),
                typeof(INotificationHandler<>),
                typeof(IStreamRequestHandler<,>)
            };


            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder
                    .RegisterAssemblyTypes(typeof(Ping).GetTypeInfo().Assembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();
            }

            builder.Register(c=> new GetDataFromFileHANDLER<string>(c.Resolve<DataTransformations>()))
               .As<IRequestHandler<GetDataFromFileREQUEST<string>, GetInputFromFileRESPONSE<string>>>();
            
            builder.Register(c => new GetDataFromFileHANDLER<DataTable>(c.Resolve<DataTransformations>()))
                .As<IRequestHandler<GetDataFromFileREQUEST<DataTable>, GetInputFromFileRESPONSE<DataTable>>>();

            builder.Register(c => new SaveDataInFileHANDLER<DataTable, string>(c.Resolve<DataTransformations>()))
               .As<IRequestHandler<SaveDataInFileREQUEST<DataTable, string>, SaveDataInFileRESPONSE>>();

            builder.Register(c => new SaveDataInFileHANDLER<string, string>(c.Resolve<DataTransformations>()))
              .As<IRequestHandler<SaveDataInFileREQUEST<string, string>, SaveDataInFileRESPONSE>>();

            builder.Register(c => new SaveDataInFileHANDLER<DataTable, DataTable>(c.Resolve<DataTransformations>()))
              .As<IRequestHandler<SaveDataInFileREQUEST<DataTable, DataTable>, SaveDataInFileRESPONSE>>();

            builder.Register(c => new ReqForCriptoAlgDB_HANDLER(c.Resolve<ReqForCriptoAlgDB>()))
               .As<IRequestHandler<ReqForCriptoAlgDB_REQUEST, ReqForCriptoAlgDB_RESPONSE>>();

            builder.Register(c => new BenchmarkHANDLER(c.Resolve<BenchmarkCryptoTransf>()))
              .As<IRequestHandler<BenchmarkREQUEST, BenchmarkRESPONSE>>();

            builder.Register(c => new CryptoTransformHANDLER<string, string>())
              .As<IRequestHandler<CryptoTransformREQUEST<string, string>, CryptoTransformRESPONSE<string>>>();

            builder.Register(c => new CryptoTransformHANDLER<DataTable, string>())
             .As<IRequestHandler<CryptoTransformREQUEST<DataTable, string>, CryptoTransformRESPONSE<string>>>();

            builder.Register(c => new CryptoTransformHANDLER<string, DataTable>())
            .As<IRequestHandler<CryptoTransformREQUEST<string, DataTable>, CryptoTransformRESPONSE<DataTable>>>();

            builder.Register(c => new SaveXmlSettingFileHANDLER())
               .As<IRequestHandler<SaveXmlSettingFileREQUEST, SaveXmlSettingFileRESPONSE>>();

            builder.Register(c => new OpenXmlSettingFileHANDLER())
              .As<IRequestHandler<OpenXmlREQUEST, OpenXmlRESPONSE>>();

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
        }
    }
}
