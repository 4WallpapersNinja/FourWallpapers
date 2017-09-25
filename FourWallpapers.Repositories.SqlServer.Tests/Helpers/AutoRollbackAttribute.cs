using System;
using System.Reflection;
using System.Threading;
using Xunit.Sdk;

namespace FourWallpapers.Repositories.SqlServer.Tests.Helpers
{
    /// <summary>
    ///     Apply this attribute to your test method to automatically clear data created
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class AutoRollbackAttribute : BeforeAfterTestAttribute
    {
        /// <summary>
        ///     Rolls back the transaction.
        /// </summary>
        public override void After(MethodInfo methodUnderTest)
        {
            Database.CleanDatabase();
        }
    }
}