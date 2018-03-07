# AppBeat.Utils
Useful utilities written in C# - compatible with .NET Standard

## Examples ##

### Test if email address is temporary / disposable ###

```csharp
using System;
using AppBeat.Utils.Email;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            string testEmail = "username@mailinator.com";

            //using with extension methods
            bool isTemporaryEmail = testEmail.IsTemporaryEmail(); //true

            //using default temporary email checker
            isTemporaryEmail = TemporaryEmailChecker.Default.IsTemporary(testEmail); //true

            //using explicit offline temporary email checker implementation
            ITemporaryEmailChecker emailChecker = new TemporaryEmailOfflineChecker();
            isTemporaryEmail = emailChecker.IsTemporary(testEmail); //true

            //test valid email
            testEmail = $"{Guid.NewGuid().ToString("N")}@gmail.com";
            isTemporaryEmail = testEmail.IsTemporaryEmail(); //false
        }
    }
}
```

## Questions? ##

Please feel free to [contact us](https://www.appbeat.io/contact?subject=Question%20about%20AppBeat.Utils%20library) if you have any questions or suggestions.

