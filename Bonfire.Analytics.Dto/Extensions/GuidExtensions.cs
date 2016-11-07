using System;
using Sitecore.Data;

namespace Bonfire.Analytics.Dto.Extensions
{
    public static class GuidExtensions
    {
        public static ID ToId(this Guid? source)
        {
            return source != Guid.Empty ? new ID(source.ToGuid()) : null;
        }

        public static Guid ToGuid(this Guid? source)
        {
            return source ?? Guid.Empty;
        }
    }
}
