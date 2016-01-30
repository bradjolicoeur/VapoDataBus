using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VapoDataBus.Services;

namespace VapoDataBus.Framework
{
    public static class AutofacModule
    {

        public static void Build(ContainerBuilder builder)
        {
            builder.RegisterType<VapoService>();

        }
    }
}
