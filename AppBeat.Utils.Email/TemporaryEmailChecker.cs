﻿#region License
// Copyright (c) 2018 AppBeat Website Monitoring - https://www.appbeat.io
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
using System;

namespace AppBeat.Utils.Email
{
    public static class TemporaryEmailChecker
    {
        /// <summary>
        /// Default singleton <seealso cref="ITemporaryEmailChecker"/> instance for determining if email is temporary / disposable.
        /// </summary>
        public static ITemporaryEmailChecker Default
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(TemporaryEmailChecker))
                    {
                        if (_instance == null)
                        {
                            _instance = new TemporaryEmailOfflineChecker();
                        }
                    }
                }

                return _instance;
            }
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException();
                }

                _instance = value;
            }

        }
        private static ITemporaryEmailChecker _instance;

    }
}
