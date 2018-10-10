using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Core.Helper
{
    //瞬态故障处理，Polly来作为异常等待重试机制
   public class PollyHelper
    {
        public static void WaitAndRetry<T>(Action execution, int maxRetryAttempts =3) where T:Exception
        {
            var pauseBetweenFailures = TimeSpan.FromSeconds(2);

            var retryPolicy = Policy.Handle<T>().WaitAndRetry(maxRetryAttempts, i => pauseBetweenFailures, (ex, t) =>
                                    {
                                        execution();
                                    });
            retryPolicy.Execute(() =>
            {
                execution();
            });
        }
    }
}
