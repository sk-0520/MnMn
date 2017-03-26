using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ContentTypeTextNet.MnMn.MnMn.Define
{
    public enum PointingGestureDirection
    {
        [XmlEnum("none")]
        None,
        [XmlEnum("up")]
        Up,
        [XmlEnum("down")]
        Down,
        [XmlEnum("left")]
        Left,
        [XmlEnum("right")]
        Right,
    }
}
