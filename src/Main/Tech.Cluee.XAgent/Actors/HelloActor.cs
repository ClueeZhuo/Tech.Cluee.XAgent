using NewLife.Log;
using NewLife.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Cluee.XAgent.Actors
{
    public class HelloActor : Actor
    {
        public HelloActor()
        {
        }
        protected override async Task ReceiveAsync(ActorContext context)
        {
            PrintService.IsWorking = true;
            XTrace.WriteLine("WORKING+++++++++++++++");

            await Task.Delay(1000 * 10);

            XTrace.WriteLine("WORKED----------");

            PrintService.IsWorking = false;
        }
    }
}
