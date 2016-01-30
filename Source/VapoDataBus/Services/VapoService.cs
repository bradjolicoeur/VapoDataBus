using Common.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VapoDataBus.Services
{
    public class VapoService : IVapo
    {
        private static readonly ILog s_log = LogManager.GetLogger<VapoService>();

        public void Execute(string folder, int days, string dateFormat)
        {
            s_log.DebugFormat("Start Vaporizing Folders in {0}", folder);
            var dir = new DirectoryInfo(folder);

            foreach (var item in dir.GetDirectories())
            {
                Regex rgx = new Regex(dateFormat);
                Match m = rgx.Match(item.Name);
                if (m.Success)
                {
                    var groupNames = rgx.GetGroupNames();

                    int hour = groupNames.Contains("hour") ? Convert.ToInt32(m.Groups["hour"].Value) : 0;

                    DateTime d = new DateTime(Convert.ToInt32(m.Groups["year"].Value),
                                              Convert.ToInt32(m.Groups["month"].Value),
                                              Convert.ToInt32(m.Groups["day"].Value),
                                              hour, 0, 0);

                    if (d < DateTime.Now.AddDays(days * -1))
                    {
                        var name = item.FullName;
                        s_log.DebugFormat("Deleting folder {0}", name);
                        item.Delete(true);
                        s_log.DebugFormat("Finished Deleting folder {0}", name);
                    }

                }

                s_log.DebugFormat("Finished Vaporizing Folders in {0}", folder);
            }
        }
    }
}
