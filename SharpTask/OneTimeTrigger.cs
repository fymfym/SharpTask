using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskTask
{
    public class OneTimeTrigger : TaskTriggerInterface
    {
        public DateTime TriggerOnce;
        public string Name {get; set;}

        public int CheckSequence
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        DateTime _triggerDateTime;

        public OneTimeTrigger(DateTime TriggerDateTime)
        {
            _triggerDateTime = TriggerDateTime;
            Name = "One time trigger";
        }

        public override string ToString()
        {
            return string.Format("Name: {0} - TriggerOnce: {1}", Name, TriggerOnce.ToString());
        }

        public bool ShouldRunNow(DateTime CurrentTime)
        {
            var ts = new TimeSpan(CurrentTime.Ticks - _triggerDateTime.Ticks).TotalSeconds;
            if (ts < 0) return false;
            if ((ts >= 0) && (ts <= 5)) return true;

            return false;
        }
    }
}
