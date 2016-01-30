using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VapoDataBus.Jobs
{
    using System;
    using Common.Logging;
    using Quartz;
    using Services;
    using Helper;
    [DisallowConcurrentExecution]
    public class VapoJob : IJob
    {
        private readonly IVapo _service;
        private readonly IConfigHelper _config;
        private static readonly ILog s_log = LogManager.GetLogger<VapoJob>();

        private string _folderPath
        {
            get { return _config.AppSetting("FolderPath"); }
        }

        private string _folderDatePattern
        {
            get { return _config.AppSetting("FolderDatePattern", @"^(?<year>\d{4})-(?<month>\d{2})-(?<day>\d{1,2})_(?<hour>\d{1,2})"); }
        }

        private int _numberOfDays
        {
            get { return Convert.ToInt16(_config.AppSetting("NumberOfDays", "3")); }
        }

        public VapoJob(IVapo service, IConfigHelper config)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            _service = service;

            if (config == null) throw new ArgumentNullException(nameof(config));
            _config = config;
        }

        public void Execute(IJobExecutionContext context)
        {
            _service.Execute(_folderPath, _numberOfDays, _folderDatePattern);
        }
    }
}
