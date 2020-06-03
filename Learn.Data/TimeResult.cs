using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Data
{
    [ProtoContract]
    public class TimeResult
    {
        [ProtoMember(1, DataFormat = DataFormat.WellKnown)]
        public DateTime Time { get; set; }
    }
}
