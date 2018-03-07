using System;
using Xunit;

namespace AppBeat.Utils.Email.Tests
{
    public class TemporaryEmailOfflineCheckerTests
    {
        public TemporaryEmailOfflineCheckerTests()
        {
            TemporaryEmailChecker.Default = new TemporaryEmailOfflineChecker();
        }

        [Theory]
        [InlineData("username@bobmail.info")]
        [InlineData("USERNAME_UPPERCASE@BOBMAIL.INFO")]
        [InlineData("uSeRnAmE_mIxEd@BoBmAiL.iNfO")]
        [InlineData(" username@bobmail.info ")]
        [InlineData("mailinator.com")]
        [InlineData(" mailinator.com ")]
        [InlineData("@", null, typeof(ArgumentException))]
        [InlineData(".@", null, typeof(ArgumentException))]
        [InlineData("@.", null, typeof(ArgumentException))]
        [InlineData("username@127.0.0.1", false)]
        [InlineData("username@localhost", false)]
        [InlineData("username@test", false)]
        public void TemporaryEmailOfflineChecker_TestAddressesAndDomains(string emailOrDomain, bool? isTemporary = true, Type exceptionType = null)
        {
            if (exceptionType != null)
            {
                Assert.Throws(exceptionType, () => TemporaryEmailChecker.Default.IsTemporary(emailOrDomain));
                Assert.Throws(exceptionType, () => emailOrDomain.IsTemporaryEmail()); //check also extension method
            }
            else
            {
                var actualIsTemporary = TemporaryEmailChecker.Default.IsTemporary(emailOrDomain);
                Assert.Equal(isTemporary, actualIsTemporary);

                //check also extension method
                actualIsTemporary = emailOrDomain.IsTemporaryEmail();
                Assert.Equal(isTemporary, actualIsTemporary);
            }
        }

        [Fact]
        public void TemporaryEmailOfflineChecker_EnsureAllDomainsAreNormalized()
        {
            var instance = new Email.TemporaryEmailOfflineChecker();

            foreach (var domain in instance.Domains)
            {
                var normalizedDomain = domain.ToLowerInvariant();
                Assert.Equal(normalizedDomain, domain);
            }
        }
    }
}
