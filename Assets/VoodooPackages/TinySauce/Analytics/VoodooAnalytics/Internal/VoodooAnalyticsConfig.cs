using JetBrains.Annotations;

namespace Voodoo.Tiny.Sauce.Internal.Analytics
{
    public static class VoodooAnalyticsConfig
    { 
        [CanBeNull] public static AnalyticsConfig AnalyticsConfig { get; set; }
    }
}