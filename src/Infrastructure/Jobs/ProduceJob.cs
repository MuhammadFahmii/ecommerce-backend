// ------------------------------------------------------------------------------------
// ProduceJob.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Quartz;

namespace netca.Infrastructure.Jobs
{
    /// <summary>
    /// ProduceJob
    /// </summary>
    public class ProduceJob : IJob
    {
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
