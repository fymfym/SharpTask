using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTaskTask
{
    public class OneTimeTrigger : TaskTriggerInterface
    {
        string _name;
        string _description;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public int Sequence { get; set; }

        public Date TriggerDate
        {
            get
            {
                return _triggerDate;
            }
            set
            {
                _triggerDate = value;
            }
        }

        public Time TriggerTime
        {
            get
            {
                return _triggerTime;
            }
            set
            {
                _triggerTime = value;
            }
        }

        Date _triggerDate;
        Time _triggerTime;

        public OneTimeTrigger(Date TriggerDate, Time TriggerTime)
        {
            _triggerDate = TriggerDate;
            _triggerTime = TriggerTime;
            _name = "OneTimeTrigger";
            _description = "Executes at 'Triger date/ trigger time' only";
        }

        public override string ToString()
        {
            return string.Format("Name: {0} - TriggerOnce: {1} {2}", Name, TriggerDate.ToString(), TriggerTime.ToString());
        }

        public bool ShouldRunNow(DateTime CurrentTime)
        {
            DateTime trigger = _triggerDate.DateTimeObject; // new DateTime(_triggerDate.Ticks + _triggerTime.Ticks);
            trigger = new DateTime(trigger.Ticks + _triggerTime.DateTimeObject.Ticks);
            
            var ts = new TimeSpan(CurrentTime.Ticks - trigger.Ticks).TotalSeconds;
            if (ts < 0) return false;
            if ((ts >= 0) && (ts <= 5)) return true;

            return false;
        }
    }
}
