using System;
using System.Collections.Generic;
using ProtoBuf;

namespace MicrobitServer
{
    [ProtoContract]
    public class MicrobitData
    {
        [ProtoMember(1)]
        public bool Left;
        [ProtoMember(2)]
        public bool Right;
        [ProtoMember(3)]
        public bool Front;
        [ProtoMember(4)]
        public bool ButtonA;
        [ProtoMember(5)]
        public bool ButtonB;

        public MicrobitData()
        {
        }

        public override string ToString()
        {
            return string.Format("Left: {0}, Right: {1}, Front: {2}, ButtonA {3}, ButtonB: {4}",
                Left, Right, Front, ButtonA, ButtonB);
        }
    }
}
