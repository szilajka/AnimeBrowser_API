using System.Runtime.CompilerServices;

namespace AnimeBrowser.Common.Helpers
{
    public static class MethodNameHelper
    {
        public static string GetCurrentMethodName([CallerMemberName] string memberName = "")
        {
            return memberName;
        }
    }
}
