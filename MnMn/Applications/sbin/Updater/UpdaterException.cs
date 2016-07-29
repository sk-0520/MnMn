﻿/*
This file is part of Updater.

Updater is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Updater is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Updater.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Runtime.Serialization;

namespace ContentTypeTextNet.Pe.SystemApplications.Updater
{
    public enum UpdaterCode
    {
        Unknown,
        NotFoundArgument,
        ScriptCompile,
    }
    /// <summary>
    /// Desctiption of PeUpdaterException.
    /// </summary>
    [Serializable]
    public class UpdaterException: Exception, ISerializable
    {
        public UpdaterException()
            : base(UpdaterCode.Unknown.ToString())
        {
            UpdaterCode = UpdaterCode.Unknown;
        }

        public UpdaterException(UpdaterCode pc)
            : base(pc.ToString())
        {
            UpdaterCode = pc;
        }

        public UpdaterCode UpdaterCode { get; private set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}