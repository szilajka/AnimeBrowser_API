using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AnimeBrowser.Common.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class MethodNameHelper
    {
        public static string GetCurrentMethodName([CallerMemberName] string memberName = "")
        {
            return memberName;
        }
    }
}
