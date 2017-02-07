using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Savannah.Tests
{
    internal static class AssertExtra
    {
        internal static void ThrowsException<TException>(Action action, string expectedExceptionMessage) where TException : Exception
        {
            try
            {
                action();
                Assert.Fail($"Expected exception of type {typeof(TException).Name} was not thrown.");
            }
            catch (TException actualException)
            {
                if (!string.Equals(expectedExceptionMessage, actualException.Message, StringComparison.Ordinal))
                    Assert.Fail($"Expected exception messge <{expectedExceptionMessage}> but received <{actualException.Message}> instead.");
            }
        }

        internal static async Task ThrowsExceptionAsync<TException>(Func<Task> asyncAction) where TException : Exception
        {
            try
            {
                await asyncAction();
                Assert.Fail($"Expected exception of type {typeof(TException).Name} was not thrown.");
            }
            catch (TException)
            {
            }
        }

        internal static async Task ThrowsExceptionAsync<TException>(Func<Task> asyncAction, string expectedExceptionMessage) where TException : Exception
        {
            try
            {
                await asyncAction();
                Assert.Fail($"Expected exception of type {typeof(TException).Name} was not thrown.");
            }
            catch (TException actualException)
            {
                if (!string.Equals(expectedExceptionMessage, actualException.Message, StringComparison.Ordinal))
                    Assert.Fail($"Expected exception messge <{expectedExceptionMessage}> but received <{actualException.Message}> instead.");
            }
        }
    }
}