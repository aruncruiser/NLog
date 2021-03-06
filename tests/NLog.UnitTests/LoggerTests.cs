// 
// Copyright (c) 2004-2017 Jaroslaw Kowalski <jaak@jkowalski.net>, Kim Christensen, Julian Verdurmen
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer. 
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
// 
// * Neither the name of Jaroslaw Kowalski nor the names of its 
//   contributors may be used to endorse or promote products derived from this
//   software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
// THE POSSIBILITY OF SUCH DAMAGE.
// 

namespace NLog.UnitTests
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using NLog.Targets;
    using NLog.Config;
    using Xunit;

    public class LoggerTests : NLogTestBase
    {
        private CultureInfo NLCulture = GetCultureInfo("nl-nl");

        [Fact]
        public void TraceTest()
        {
            // test all possible overloads of the Trace() method

            for (int enabled = 0; enabled < 2; ++enabled)
            {
                if (enabled == 0)
                {
                    LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='' writeTo='debug' />
                    </rules>
                </nlog>");
                }
                else
                {
                    LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='Trace' writeTo='debug' />
                    </rules>
                </nlog>");
                }

                ILogger logger = LogManager.GetLogger("A");
                LogManager.Configuration.DefaultCultureInfo = CultureInfo.InvariantCulture;

                logger.Trace("message");
                if (enabled == 1) AssertDebugLastMessage("debug", "message");

                logger.Trace((object)"message");
                if (enabled == 1) AssertDebugLastMessage("debug", "message");

                logger.Trace(CultureInfo.InvariantCulture, "message{0}", (object)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Trace("message{0}{1}", 1, 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message12");

                logger.Trace("message{0}{1}{2}", 1, 2, 3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message123");

                logger.Trace(NLCulture, "message{0}{1}{2}", 1.4, 2.5, 3.6);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1,42,53,6");

                logger.Trace("message{0}", (float)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Trace("message{0}", (double)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Trace("message{0}", (decimal)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Trace("message{0}", (object)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Trace(NLCulture, "message{0}", (object)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2,3");

                logger.Trace("message{0}", (ulong)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Trace(CultureInfo.InvariantCulture, "message{0}", (ulong)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Trace("message{0}", (long)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Trace(CultureInfo.InvariantCulture, "message{0}", (long)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Trace("message{0}", (uint)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Trace(CultureInfo.InvariantCulture, "message{0}", (uint)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Trace("message{0}", 1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Trace(CultureInfo.InvariantCulture, "message{0}", 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Trace("message{0}", (ushort)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Trace(CultureInfo.InvariantCulture, "message{0}", (ushort)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Trace("message{0}", (sbyte)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Trace(CultureInfo.InvariantCulture, "message{0}", (sbyte)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Trace("message{0}", this);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                logger.Trace(CultureInfo.InvariantCulture, "message{0}", this);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                logger.Trace("message{0}", (short)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Trace(CultureInfo.InvariantCulture, "message{0}", (short)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Trace("message{0}", (byte)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Trace(CultureInfo.InvariantCulture, "message{0}", (byte)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Trace("message{0}", 'c');
                if (enabled == 1) AssertDebugLastMessage("debug", "messagec");

                logger.Trace(CultureInfo.InvariantCulture, "message{0}", 'd');
                if (enabled == 1) AssertDebugLastMessage("debug", "messaged");

                logger.Trace("message{0}", "ddd");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd");

                logger.Trace(CultureInfo.InvariantCulture, "message{0}", "eee");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee");

                logger.Trace("message{0}{1}", "ddd", 1);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1");

                logger.Trace(CultureInfo.InvariantCulture, "message{0}{1}", "eee", 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2");

                logger.Trace("message{0}{1}{2}", "ddd", 1, "eee");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1eee");

                logger.Trace(CultureInfo.InvariantCulture, "message{0}{1}{2}", "eee", 2, "fff");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fff");

                logger.Trace("message{0}{1}{2}{3}", "eee", 2, "fff", "ggg");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fffggg");

                logger.Trace("message{0}", true);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageTrue");

                logger.Trace(CultureInfo.InvariantCulture, "message{0}", false);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageFalse");

                logger.Trace(CultureInfo.InvariantCulture, "message{0}", (float)2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                logger.Trace(CultureInfo.InvariantCulture, "message{0}", 2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                logger.Trace(CultureInfo.InvariantCulture, "message{0}", (decimal)2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

#pragma warning disable 0618
                // Obsolete method requires testing until removed.
                logger.TraceException("message", new Exception("test"));
                if (enabled == 1) AssertDebugLastMessage("debug", "messagetest");
#pragma warning restore 0618

                logger.Trace(new Exception("test"), "message");
                if (enabled == 1) AssertDebugLastMessage("debug", "messagetest");

                logger.Trace(new Exception("test"), "message {0}", "from parameter");
                if (enabled == 1) AssertDebugLastMessage("debug", "message from parametertest");

                logger.Trace(delegate { return "message from lambda"; });
                if (enabled == 1) AssertDebugLastMessage("debug", "message from lambda");

                if (enabled == 0)
                    AssertDebugCounter("debug", 0);
            }
        }

        [Fact]
        public void DebugTest()
        {
            // test all possible overloads of the Debug() method

            for (int enabled = 0; enabled < 2; ++enabled)
            {
                if (enabled == 0)
                {
                    LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='' writeTo='debug' />
                    </rules>
                </nlog>");
                }
                else
                {
                    LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='Debug' writeTo='debug' />
                    </rules>
                </nlog>");
                }

                ILogger logger = LogManager.GetLogger("A");
                LogManager.Configuration.DefaultCultureInfo = CultureInfo.InvariantCulture;

                logger.Debug("message");
                if (enabled == 1) AssertDebugLastMessage("debug", "message");

                logger.Debug((object)"message");
                if (enabled == 1) AssertDebugLastMessage("debug", "message");

                logger.Debug(CultureInfo.InvariantCulture, "message{0}", (object)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Debug("message{0}{1}", 1, 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message12");

                logger.Debug("message{0}{1}{2}", 1, 2, 3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message123");

                logger.Debug(NLCulture, "message{0}{1}{2}", 1.4, 2.5, 3.6);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1,42,53,6");

                logger.Debug("message{0}", (float)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Debug("message{0}", (double)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Debug("message{0}", (decimal)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Debug("message{0}", (object)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Debug(NLCulture, "message{0}", (object)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2,3");

                logger.Debug("message{0}", (ulong)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Debug(CultureInfo.InvariantCulture, "message{0}", (ulong)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Debug("message{0}", (long)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Debug(CultureInfo.InvariantCulture, "message{0}", (long)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Debug("message{0}", (uint)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Debug(CultureInfo.InvariantCulture, "message{0}", (uint)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Debug("message{0}", 1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Debug(CultureInfo.InvariantCulture, "message{0}", 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Debug("message{0}", (ushort)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Debug(CultureInfo.InvariantCulture, "message{0}", (ushort)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Debug("message{0}", (sbyte)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Debug(CultureInfo.InvariantCulture, "message{0}", (sbyte)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Debug("message{0}", this);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                logger.Debug(CultureInfo.InvariantCulture, "message{0}", this);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                logger.Debug("message{0}", (short)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Debug(CultureInfo.InvariantCulture, "message{0}", (short)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Debug("message{0}", (byte)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Debug(CultureInfo.InvariantCulture, "message{0}", (byte)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Debug("message{0}", 'c');
                if (enabled == 1) AssertDebugLastMessage("debug", "messagec");

                logger.Debug(CultureInfo.InvariantCulture, "message{0}", 'd');
                if (enabled == 1) AssertDebugLastMessage("debug", "messaged");

                logger.Debug("message{0}", "ddd");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd");

                logger.Debug(CultureInfo.InvariantCulture, "message{0}", "eee");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee");

                logger.Debug("message{0}{1}", "ddd", 1);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1");

                logger.Debug(CultureInfo.InvariantCulture, "message{0}{1}", "eee", 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2");

                logger.Debug("message{0}{1}{2}", "ddd", 1, "eee");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1eee");

                logger.Debug(CultureInfo.InvariantCulture, "message{0}{1}{2}", "eee", 2, "fff");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fff");

                logger.Debug("message{0}{1}{2}{3}", "eee", 2, "fff", "ggg");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fffggg");

                logger.Debug("message{0}", true);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageTrue");

                logger.Debug(CultureInfo.InvariantCulture, "message{0}", false);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageFalse");

                logger.Debug(CultureInfo.InvariantCulture, "message{0}", (float)2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                logger.Debug(CultureInfo.InvariantCulture, "message{0}", 2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                logger.Debug(CultureInfo.InvariantCulture, "message{0}", (decimal)2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

#pragma warning disable 0618
                // Obsolete method requires testing until removed.
                logger.DebugException("message", new Exception("test"));
                if (enabled == 1) AssertDebugLastMessage("debug", "messagetest");
#pragma warning restore 0618

                logger.Debug(new Exception("test"), "message");
                if (enabled == 1) AssertDebugLastMessage("debug", "messagetest");

                logger.Debug(new Exception("test"), "message {0}", "from parameter");
                if (enabled == 1) AssertDebugLastMessage("debug", "message from parametertest");

                logger.Debug(delegate { return "message from lambda"; });
                if (enabled == 1) AssertDebugLastMessage("debug", "message from lambda");

                if (enabled == 0)
                    AssertDebugCounter("debug", 0);
            }
        }

        [Fact]
        public void InfoTest()
        {
            // test all possible overloads of the Info() method

            for (int enabled = 0; enabled < 2; ++enabled)
            {
                if (enabled == 0)
                {
                    LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='' writeTo='debug' />
                    </rules>
                </nlog>");
                }
                else
                {
                    LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='Info' writeTo='debug' />
                    </rules>
                </nlog>");
                }

                ILogger logger = LogManager.GetLogger("A");
                LogManager.Configuration.DefaultCultureInfo = CultureInfo.InvariantCulture;

                logger.Info("message");
                if (enabled == 1) AssertDebugLastMessage("debug", "message");

                logger.Info((object)"message");
                if (enabled == 1) AssertDebugLastMessage("debug", "message");

                logger.Info(CultureInfo.InvariantCulture, "message{0}", (object)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Info("message{0}{1}", 1, 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message12");

                logger.Info("message{0}{1}{2}", 1, 2, 3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message123");

                logger.Info(NLCulture, "message{0}{1}{2}", 1.4, 2.5, 3.6);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1,42,53,6");

                logger.Info("message{0}", (float)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Info("message{0}", (double)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Info("message{0}", (decimal)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Info("message{0}", (object)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Info(NLCulture, "message{0}", (object)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2,3");

                logger.Info("message{0}", (ulong)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Info(CultureInfo.InvariantCulture, "message{0}", (ulong)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Info("message{0}", (long)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Info(CultureInfo.InvariantCulture, "message{0}", (long)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Info("message{0}", (uint)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Info(CultureInfo.InvariantCulture, "message{0}", (uint)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Info("message{0}", 1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Info(CultureInfo.InvariantCulture, "message{0}", 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Info("message{0}", (ushort)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Info(CultureInfo.InvariantCulture, "message{0}", (ushort)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Info("message{0}", (sbyte)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Info(CultureInfo.InvariantCulture, "message{0}", (sbyte)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Info("message{0}", this);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                logger.Info(CultureInfo.InvariantCulture, "message{0}", this);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                logger.Info("message{0}", (short)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Info(CultureInfo.InvariantCulture, "message{0}", (short)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Info("message{0}", (byte)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Info(CultureInfo.InvariantCulture, "message{0}", (byte)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Info("message{0}", 'c');
                if (enabled == 1) AssertDebugLastMessage("debug", "messagec");

                logger.Info(CultureInfo.InvariantCulture, "message{0}", 'd');
                if (enabled == 1) AssertDebugLastMessage("debug", "messaged");

                logger.Info("message{0}", "ddd");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd");

                logger.Info(CultureInfo.InvariantCulture, "message{0}", "eee");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee");

                logger.Info("message{0}{1}", "ddd", 1);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1");

                logger.Info(CultureInfo.InvariantCulture, "message{0}{1}", "eee", 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2");

                logger.Info("message{0}{1}{2}", "ddd", 1, "eee");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1eee");

                logger.Info(CultureInfo.InvariantCulture, "message{0}{1}{2}", "eee", 2, "fff");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fff");

                logger.Info("message{0}{1}{2}{3}", "eee", 2, "fff", "ggg");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fffggg");

                logger.Info("message{0}", true);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageTrue");

                logger.Info(CultureInfo.InvariantCulture, "message{0}", false);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageFalse");

                logger.Info(CultureInfo.InvariantCulture, "message{0}", (float)2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                logger.Info(CultureInfo.InvariantCulture, "message{0}", 2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                logger.Info(CultureInfo.InvariantCulture, "message{0}", (decimal)2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

#pragma warning disable 0618
                // Obsolete method requires testing until removed.
                logger.InfoException("message", new Exception("test"));
                if (enabled == 1) AssertDebugLastMessage("debug", "messagetest");
#pragma warning restore 0618

                logger.Info(new Exception("test"), "message");
                if (enabled == 1) AssertDebugLastMessage("debug", "messagetest");

                logger.Info(new Exception("test"), "message {0}", "from parameter");
                if (enabled == 1) AssertDebugLastMessage("debug", "message from parametertest");

                logger.Info(delegate { return "message from lambda"; });
                if (enabled == 1) AssertDebugLastMessage("debug", "message from lambda");

                if (enabled == 0)
                    AssertDebugCounter("debug", 0);
            }
        }

        [Fact]
        public void WarnTest()
        {
            // test all possible overloads of the Warn() method

            for (int enabled = 0; enabled < 2; ++enabled)
            {
                if (enabled == 0)
                {
                    LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='' writeTo='debug' />
                    </rules>
                </nlog>");
                }
                else
                {
                    LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='Warn' writeTo='debug' />
                    </rules>
                </nlog>");
                }

                ILogger logger = LogManager.GetLogger("A");
                LogManager.Configuration.DefaultCultureInfo = CultureInfo.InvariantCulture;

                logger.Warn("message");
                if (enabled == 1) AssertDebugLastMessage("debug", "message");

                logger.Warn((object)"message");
                if (enabled == 1) AssertDebugLastMessage("debug", "message");

                logger.Warn(CultureInfo.InvariantCulture, "message{0}", (object)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Warn("message{0}{1}", 1, 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message12");

                logger.Warn("message{0}{1}{2}", 1, 2, 3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message123");

                logger.Warn(NLCulture, "message{0}{1}{2}", 1.4, 2.5, 3.6);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1,42,53,6");

                logger.Warn("message{0}", (float)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Warn("message{0}", (double)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Warn("message{0}", (decimal)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Warn("message{0}", (object)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Warn(NLCulture, "message{0}", (object)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2,3");

                logger.Warn("message{0}", (ulong)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Warn(CultureInfo.InvariantCulture, "message{0}", (ulong)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Warn("message{0}", (long)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Warn(CultureInfo.InvariantCulture, "message{0}", (long)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Warn("message{0}", (uint)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Warn(CultureInfo.InvariantCulture, "message{0}", (uint)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Warn("message{0}", 1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Warn(CultureInfo.InvariantCulture, "message{0}", 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Warn("message{0}", (ushort)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Warn(CultureInfo.InvariantCulture, "message{0}", (ushort)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Warn("message{0}", (sbyte)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Warn(CultureInfo.InvariantCulture, "message{0}", (sbyte)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Warn("message{0}", this);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                logger.Warn(CultureInfo.InvariantCulture, "message{0}", this);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                logger.Warn("message{0}", (short)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Warn(CultureInfo.InvariantCulture, "message{0}", (short)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Warn("message{0}", (byte)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Warn(CultureInfo.InvariantCulture, "message{0}", (byte)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Warn("message{0}", 'c');
                if (enabled == 1) AssertDebugLastMessage("debug", "messagec");

                logger.Warn(CultureInfo.InvariantCulture, "message{0}", 'd');
                if (enabled == 1) AssertDebugLastMessage("debug", "messaged");

                logger.Warn("message{0}", "ddd");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd");

                logger.Warn(CultureInfo.InvariantCulture, "message{0}", "eee");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee");

                logger.Warn("message{0}{1}", "ddd", 1);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1");

                logger.Warn(CultureInfo.InvariantCulture, "message{0}{1}", "eee", 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2");

                logger.Warn("message{0}{1}{2}", "ddd", 1, "eee");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1eee");

                logger.Warn(CultureInfo.InvariantCulture, "message{0}{1}{2}", "eee", 2, "fff");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fff");

                logger.Warn("message{0}{1}{2}{3}", "eee", 2, "fff", "ggg");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fffggg");

                logger.Warn("message{0}", true);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageTrue");

                logger.Warn(CultureInfo.InvariantCulture, "message{0}", false);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageFalse");

                logger.Warn(CultureInfo.InvariantCulture, "message{0}", (float)2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                logger.Warn(CultureInfo.InvariantCulture, "message{0}", 2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                logger.Warn(CultureInfo.InvariantCulture, "message{0}", (decimal)2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

#pragma warning disable 0618
                // Obsolete method requires testing until removed.
                logger.WarnException("message", new Exception("test"));
                if (enabled == 1) AssertDebugLastMessage("debug", "messagetest");
#pragma warning restore 0618

                logger.Warn(new Exception("test"), "message");
                if (enabled == 1) AssertDebugLastMessage("debug", "messagetest");

                logger.Warn(new Exception("test"), "message {0}", "from parameter");
                if (enabled == 1) AssertDebugLastMessage("debug", "message from parametertest");

                logger.Warn(delegate { return "message from lambda"; });
                if (enabled == 1) AssertDebugLastMessage("debug", "message from lambda");

                if (enabled == 0)
                    AssertDebugCounter("debug", 0);
            }
        }

        [Fact]
        public void ErrorTest()
        {
            // test all possible overloads of the Error() method

            for (int enabled = 0; enabled < 2; ++enabled)
            {
                if (enabled == 0)
                {
                    LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='' writeTo='debug' />
                    </rules>
                </nlog>");
                }
                else
                {
                    LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='Error' writeTo='debug' />
                    </rules>
                </nlog>");
                }

                ILogger logger = LogManager.GetLogger("A");
                LogManager.Configuration.DefaultCultureInfo = CultureInfo.InvariantCulture;

                logger.Error("message");
                if (enabled == 1) AssertDebugLastMessage("debug", "message");

                logger.Error((object)"message");
                if (enabled == 1) AssertDebugLastMessage("debug", "message");

                logger.Error(CultureInfo.InvariantCulture, "message{0}", (object)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Error("message{0}{1}", 1, 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message12");

                logger.Error("message{0}{1}{2}", 1, 2, 3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message123");

                logger.Error(NLCulture, "message{0}{1}{2}", 1.4, 2.5, 3.6);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1,42,53,6");

                logger.Error("message{0}", (float)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Error("message{0}", (double)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Error("message{0}", (decimal)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Error("message{0}", (object)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Error(NLCulture, "message{0}", (object)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2,3");

                logger.Error("message{0}", (ulong)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Error(CultureInfo.InvariantCulture, "message{0}", (ulong)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Error("message{0}", (long)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Error(CultureInfo.InvariantCulture, "message{0}", (long)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Error("message{0}", (uint)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Error(CultureInfo.InvariantCulture, "message{0}", (uint)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Error("message{0}", 1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Error(CultureInfo.InvariantCulture, "message{0}", 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Error("message{0}", (ushort)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Error(CultureInfo.InvariantCulture, "message{0}", (ushort)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Error("message{0}", (sbyte)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Error(CultureInfo.InvariantCulture, "message{0}", (sbyte)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Error("message{0}", this);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                logger.Error(CultureInfo.InvariantCulture, "message{0}", this);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                logger.Error("message{0}", (short)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Error(CultureInfo.InvariantCulture, "message{0}", (short)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Error("message{0}", (byte)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Error(CultureInfo.InvariantCulture, "message{0}", (byte)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Error("message{0}", 'c');
                if (enabled == 1) AssertDebugLastMessage("debug", "messagec");

                logger.Error(CultureInfo.InvariantCulture, "message{0}", 'd');
                if (enabled == 1) AssertDebugLastMessage("debug", "messaged");

                logger.Error("message{0}", "ddd");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd");

                logger.Error(CultureInfo.InvariantCulture, "message{0}", "eee");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee");

                logger.Error("message{0}{1}", "ddd", 1);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1");

                logger.Error(CultureInfo.InvariantCulture, "message{0}{1}", "eee", 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2");

                logger.Error("message{0}{1}{2}", "ddd", 1, "eee");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1eee");

                logger.Error(CultureInfo.InvariantCulture, "message{0}{1}{2}", "eee", 2, "fff");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fff");

                logger.Error("message{0}{1}{2}{3}", "eee", 2, "fff", "ggg");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fffggg");

                logger.Error("message{0}", true);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageTrue");

                logger.Error(CultureInfo.InvariantCulture, "message{0}", false);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageFalse");

                logger.Error(CultureInfo.InvariantCulture, "message{0}", (float)2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                logger.Error(CultureInfo.InvariantCulture, "message{0}", 2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                logger.Error(CultureInfo.InvariantCulture, "message{0}", (decimal)2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

#pragma warning disable 0618
                // Obsolete method requires testing until removed.
                logger.ErrorException("message", new Exception("test"));
                if (enabled == 1) AssertDebugLastMessage("debug", "messagetest");
#pragma warning restore 0618

                logger.Error(new Exception("test"), "message");
                if (enabled == 1) AssertDebugLastMessage("debug", "messagetest");

                logger.Error(new Exception("test"), "message {0}", "from parameter");
                if (enabled == 1) AssertDebugLastMessage("debug", "message from parametertest");

                logger.Error(delegate { return "message from lambda"; });
                if (enabled == 1) AssertDebugLastMessage("debug", "message from lambda");

                if (enabled == 0)
                    AssertDebugCounter("debug", 0);
            }
        }

        [Fact]
        public void FatalTest()
        {
            // test all possible overloads of the Fatal() method

            for (int enabled = 0; enabled < 2; ++enabled)
            {
                if (enabled == 0)
                {
                    LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='' writeTo='debug' />
                    </rules>
                </nlog>");
                }
                else
                {
                    LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='Fatal' writeTo='debug' />
                    </rules>
                </nlog>");
                }

                ILogger logger = LogManager.GetLogger("A");
                LogManager.Configuration.DefaultCultureInfo = CultureInfo.InvariantCulture;

                logger.Fatal("message");
                if (enabled == 1) AssertDebugLastMessage("debug", "message");

                logger.Fatal((object)"message");
                if (enabled == 1) AssertDebugLastMessage("debug", "message");

                logger.Fatal(CultureInfo.InvariantCulture, "message{0}", (object)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Fatal("message{0}{1}", 1, 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message12");

                logger.Fatal("message{0}{1}{2}", 1, 2, 3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message123");

                logger.Fatal(NLCulture, "message{0}{1}{2}", 1.4, 2.5, 3.6);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1,42,53,6");

                logger.Fatal("message{0}", (float)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Fatal("message{0}", (double)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Fatal("message{0}", (decimal)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Fatal("message{0}", (object)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                logger.Fatal(NLCulture, "message{0}", (object)2.3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2,3");

                logger.Fatal("message{0}", (ulong)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Fatal(CultureInfo.InvariantCulture, "message{0}", (ulong)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Fatal("message{0}", (long)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Fatal(CultureInfo.InvariantCulture, "message{0}", (long)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Fatal("message{0}", (uint)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Fatal(CultureInfo.InvariantCulture, "message{0}", (uint)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Fatal("message{0}", 1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Fatal(CultureInfo.InvariantCulture, "message{0}", 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Fatal("message{0}", (ushort)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Fatal(CultureInfo.InvariantCulture, "message{0}", (ushort)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Fatal("message{0}", (sbyte)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Fatal(CultureInfo.InvariantCulture, "message{0}", (sbyte)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Fatal("message{0}", this);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                logger.Fatal(CultureInfo.InvariantCulture, "message{0}", this);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                logger.Fatal("message{0}", (short)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Fatal(CultureInfo.InvariantCulture, "message{0}", (short)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Fatal("message{0}", (byte)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.Fatal(CultureInfo.InvariantCulture, "message{0}", (byte)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.Fatal("message{0}", 'c');
                if (enabled == 1) AssertDebugLastMessage("debug", "messagec");

                logger.Fatal(CultureInfo.InvariantCulture, "message{0}", 'd');
                if (enabled == 1) AssertDebugLastMessage("debug", "messaged");

                logger.Fatal("message{0}", "ddd");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd");

                logger.Fatal(CultureInfo.InvariantCulture, "message{0}", "eee");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee");

                logger.Fatal("message{0}{1}", "ddd", 1);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1");

                logger.Fatal(CultureInfo.InvariantCulture, "message{0}{1}", "eee", 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2");

                logger.Fatal("message{0}{1}{2}", "ddd", 1, "eee");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1eee");

                logger.Fatal(CultureInfo.InvariantCulture, "message{0}{1}{2}", "eee", 2, "fff");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fff");

                logger.Fatal("message{0}{1}{2}{3}", "eee", 2, "fff", "ggg");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fffggg");

                logger.Fatal("message{0}", true);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageTrue");

                logger.Fatal(CultureInfo.InvariantCulture, "message{0}", false);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageFalse");

                logger.Fatal(CultureInfo.InvariantCulture, "message{0}", (float)2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                logger.Fatal(CultureInfo.InvariantCulture, "message{0}", 2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                logger.Fatal(CultureInfo.InvariantCulture, "message{0}", (decimal)2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

#pragma warning disable 0618
                // Obsolete method requires testing until removed.
                logger.FatalException("message", new Exception("test"));
                if (enabled == 1) AssertDebugLastMessage("debug", "messagetest");
#pragma warning restore 0618

                logger.Fatal(new Exception("test"), "message");
                if (enabled == 1) AssertDebugLastMessage("debug", "messagetest");

                logger.Fatal(new Exception("test"), "message {0}", "from parameter");
                if (enabled == 1) AssertDebugLastMessage("debug", "message from parametertest");

                logger.Fatal(delegate { return "message from lambda"; });
                if (enabled == 1) AssertDebugLastMessage("debug", "message from lambda");

                if (enabled == 0)
                    AssertDebugCounter("debug", 0);
            }
        }

        [Fact]
        public void LogTest()
        {
            // test all possible overloads of the Log(level) method

            foreach (LogLevel level in new LogLevel[] { LogLevel.Debug, LogLevel.Info, LogLevel.Warn, LogLevel.Error, LogLevel.Fatal })
            {

                for (int enabled = 0; enabled < 2; ++enabled)
                {
                    if (enabled == 0)
                    {
                        LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='' writeTo='debug' />
                    </rules>
                </nlog>");
                    }
                    else
                    {
                        LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='" + level.Name + @"' writeTo='debug' />
                            </rules>
                                  </nlog>");
                    }

                    ILogger logger = LogManager.GetLogger("A");

                    logger.Log(level, "message");
                    if (enabled == 1) AssertDebugLastMessage("debug", "message");

                    logger.Log(level, "message{0}", (ulong)1);
                    if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                    logger.Log(level, CultureInfo.InvariantCulture, "message{0}", (ulong)2);
                    if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                    logger.Log(level, "message{0}", (long)1);
                    if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                    logger.Log(level, CultureInfo.InvariantCulture, "message{0}", (long)2);
                    if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                    logger.Log(level, "message{0}", (uint)1);
                    if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                    logger.Log(level, CultureInfo.InvariantCulture, "message{0}", (uint)2);
                    if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                    logger.Log(level, "message{0}", (int)1);
                    if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                    logger.Log(level, CultureInfo.InvariantCulture, "message{0}", (int)2);
                    if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                    logger.Log(level, "message{0}", (ushort)1);
                    if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                    logger.Log(level, CultureInfo.InvariantCulture, "message{0}", (ushort)2);
                    if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                    logger.Log(level, "message{0}", (sbyte)1);
                    if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                    logger.Log(level, CultureInfo.InvariantCulture, "message{0}", (sbyte)2);
                    if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                    logger.Log(level, "message{0}", this);
                    if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                    logger.Log(level, CultureInfo.InvariantCulture, "message{0}", this);
                    if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                    logger.Log(level, "message{0}", (short)1);
                    if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                    logger.Log(level, CultureInfo.InvariantCulture, "message{0}", (short)2);
                    if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                    logger.Log(level, "message{0}", (byte)1);
                    if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                    logger.Log(level, CultureInfo.InvariantCulture, "message{0}", (byte)2);
                    if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                    logger.Log(level, "message{0}", 'c');
                    if (enabled == 1) AssertDebugLastMessage("debug", "messagec");

                    logger.Log(level, CultureInfo.InvariantCulture, "message{0}", 'd');
                    if (enabled == 1) AssertDebugLastMessage("debug", "messaged");

                    logger.Log(level, "message{0}", "ddd");
                    if (enabled == 1) AssertDebugLastMessage("debug", "messageddd");

                    logger.Log(level, CultureInfo.InvariantCulture, "message{0}", "eee");
                    if (enabled == 1) AssertDebugLastMessage("debug", "messageeee");

                    logger.Log(level, "message{0}{1}", "ddd", 1);
                    if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1");

                    logger.Log(level, CultureInfo.InvariantCulture, "message{0}{1}", "eee", 2);
                    if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2");

                    logger.Log(level, "message{0}{1}{2}", "ddd", 1, "eee");
                    if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1eee");

                    logger.Log(level, CultureInfo.InvariantCulture, "message{0}{1}{2}", "eee", 2, "fff");
                    if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fff");

                    logger.Log(level, "message{0}{1}{2}{3}", "eee", 2, "fff", "ggg");
                    if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fffggg");

                    logger.Log(level, "message{0}", true);
                    if (enabled == 1) AssertDebugLastMessage("debug", "messageTrue");

                    logger.Log(level, CultureInfo.InvariantCulture, "message{0}", false);
                    if (enabled == 1) AssertDebugLastMessage("debug", "messageFalse");

                    logger.Log(level, CultureInfo.InvariantCulture, "message{0}", (float)2.5);
                    if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                    logger.Log(level, CultureInfo.InvariantCulture, "message{0}", (double)2.5);
                    if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                    logger.Log(level, CultureInfo.InvariantCulture, "message{0}", (decimal)2.5);
                    if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                    logger.Log(level, new Exception("test"), "message");
                    if (enabled == 1) AssertDebugLastMessage("debug", "messagetest");

                    logger.Log(level, new Exception("test"), "message {0}", "from parameter");
                    if (enabled == 1) AssertDebugLastMessage("debug", "message from parametertest");

#pragma warning disable 0618
                    // Obsolete method requires testing until removed.
                    logger.LogException(level, "message", new Exception("test"));
                    if (enabled == 1) AssertDebugLastMessage("debug", "messagetest");
#pragma warning restore 0618

                    logger.Log(level, delegate { return "message from lambda"; });
                    if (enabled == 1) AssertDebugLastMessage("debug", "message from lambda");

                    if (enabled == 0)
                        AssertDebugCounter("debug", 0);
                }
            }
        }

        #region Conditional Logger
#if DEBUG

        [Fact]
        public void ConditionalTraceTest()
        {
            // test all possible overloads of the Trace() method

            for (int enabled = 0; enabled < 2; ++enabled)
            {
                if (enabled == 0)
                {
                    LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='' writeTo='debug' />
                    </rules>
                </nlog>");
                }
                else
                {
                    LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='Trace' writeTo='debug' />
                    </rules>
                </nlog>");
                }

                var logger = LogManager.GetLogger("A");

                //set current UI culture as invariant to receive exception messages in EN
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                var argException = new ArgumentException("arg1 is obvious wrong", "arg1");

                LogManager.Configuration.DefaultCultureInfo = CultureInfo.InvariantCulture;

                logger.ConditionalTrace("message");
                if (enabled == 1) AssertDebugLastMessage("debug", "message");

                logger.ConditionalTrace(404);
                if (enabled == 1) AssertDebugLastMessage("debug", "404");

                logger.ConditionalTrace(NLCulture, 404.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "404,5");

                logger.ConditionalTrace(NLCulture, "hello error {0} !", 404.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "hello error 404,5 !");

                logger.ConditionalTrace(NLCulture, "hello error {0} and {1} !", 404.5, 401);
                if (enabled == 1) AssertDebugLastMessage("debug", "hello error 404,5 and 401 !");

                logger.ConditionalTrace(NLCulture, "hello error {0}, {1} & {2} !", 404.5, 401, 500);
                if (enabled == 1) AssertDebugLastMessage("debug", "hello error 404,5, 401 & 500 !");

                logger.ConditionalTrace(NLCulture, "we've got error {0}, {1}, {2}, {3} ...", 500, 501, 502, 503);
                if (enabled == 1) AssertDebugLastMessage("debug", "we've got error 500, 501, 502, 503 ...");


                logger.ConditionalTrace(argException, NLCulture, "we've got error {0}, {1}, {2}, {3} ...", 500, 501, 502, 503.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "we\'ve got error 500, 501, 502, 503,5 ...arg1 is obvious wrong\r\nParameter name: arg1");

                logger.ConditionalTrace(argException, "we've got error {0}, {1}, {2}, {3} ...", 500, 501, 502, 503.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "we\'ve got error 500, 501, 502, 503.5 ...arg1 is obvious wrong\r\nParameter name: arg1");

                logger.ConditionalTrace("message{0}", (ulong)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.ConditionalTrace(CultureInfo.InvariantCulture, "message{0}", (ulong)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.ConditionalTrace("message{0}", (long)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.ConditionalTrace(CultureInfo.InvariantCulture, "message{0}", (long)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.ConditionalTrace("message{0}", (uint)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.ConditionalTrace(CultureInfo.InvariantCulture, "message{0}", (uint)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.ConditionalTrace("message{0}", 1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.ConditionalTrace(CultureInfo.InvariantCulture, "message{0}", 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.ConditionalTrace("message{0}", (ushort)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.ConditionalTrace(CultureInfo.InvariantCulture, "message{0}", (ushort)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.ConditionalTrace("message{0}", (sbyte)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.ConditionalTrace(CultureInfo.InvariantCulture, "message{0}", (sbyte)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.ConditionalTrace("message{0}", this);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                logger.ConditionalTrace(CultureInfo.InvariantCulture, "message{0}", this);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                logger.ConditionalTrace("message{0}", (short)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.ConditionalTrace(CultureInfo.InvariantCulture, "message{0}", (short)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.ConditionalTrace("message{0}", (byte)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.ConditionalTrace(CultureInfo.InvariantCulture, "message{0}", (byte)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.ConditionalTrace("message{0}", 'c');
                if (enabled == 1) AssertDebugLastMessage("debug", "messagec");

                logger.ConditionalTrace(CultureInfo.InvariantCulture, "message{0}", 'd');
                if (enabled == 1) AssertDebugLastMessage("debug", "messaged");

                logger.ConditionalTrace("message{0}", "ddd");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd");

                logger.ConditionalTrace(CultureInfo.InvariantCulture, "message{0}", "eee");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee");

                logger.ConditionalTrace("message{0}{1}", "ddd", 1);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1");

                logger.ConditionalTrace(CultureInfo.InvariantCulture, "message{0}{1}", "eee", 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2");

                logger.ConditionalTrace("message{0}{1}{2}", "ddd", 1, "eee");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1eee");

                logger.ConditionalTrace(CultureInfo.InvariantCulture, "message{0}{1}{2}", "eee", 2, "fff");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fff");

                logger.ConditionalTrace("message{0}{1}{2}{3}", "eee", 2, "fff", "ggg");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fffggg");

                logger.ConditionalTrace("message{0}", true);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageTrue");

                logger.ConditionalTrace(CultureInfo.InvariantCulture, "message{0}", false);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageFalse");

                logger.ConditionalTrace(CultureInfo.InvariantCulture, "message{0}", (float)2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                logger.ConditionalTrace(CultureInfo.InvariantCulture, "message{0}", 2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                logger.ConditionalTrace(CultureInfo.InvariantCulture, "message{0}", (decimal)2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                logger.ConditionalTrace(new Exception("test"), "message");
                if (enabled == 1) AssertDebugLastMessage("debug", "messagetest");

                logger.ConditionalTrace(new Exception("test"), "message {0}", "from parameter");
                if (enabled == 1) AssertDebugLastMessage("debug", "message from parametertest");

                logger.ConditionalTrace(delegate { return "message from lambda"; });
                if (enabled == 1) AssertDebugLastMessage("debug", "message from lambda");

                if (enabled == 0)
                    AssertDebugCounter("debug", 0);
            }
        }

        [Fact]
        public void ConditionalDebugTest()
        {
            // test all possible overloads of the Debug() method

            for (int enabled = 0; enabled < 2; ++enabled)
            {
                if (enabled == 0)
                {
                    LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='' writeTo='debug' />
                    </rules>
                </nlog>");
                }
                else
                {
                    LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='Debug' writeTo='debug' />
                    </rules>
                </nlog>");
                }

                var logger = LogManager.GetLogger("A");

                //set current UI culture as invariant to receive exception messages in EN
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                var argException = new ArgumentException("arg1 is obvious wrong", "arg1");
                LogManager.Configuration.DefaultCultureInfo = CultureInfo.InvariantCulture;

                logger.ConditionalDebug("message");
                if (enabled == 1) AssertDebugLastMessage("debug", "message");

                logger.ConditionalDebug(404);
                if (enabled == 1) AssertDebugLastMessage("debug", "404");

                logger.ConditionalDebug(NLCulture, 404.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "404,5");

                logger.ConditionalDebug(NLCulture, "hello error {0} !", 404.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "hello error 404,5 !");

                logger.ConditionalDebug(NLCulture, "hello error {0} and {1} !", 404.5, 401);
                if (enabled == 1) AssertDebugLastMessage("debug", "hello error 404,5 and 401 !");

                logger.ConditionalDebug(NLCulture, "hello error {0}, {1} & {2} !", 404.5, 401, 500);
                if (enabled == 1) AssertDebugLastMessage("debug", "hello error 404,5, 401 & 500 !");

                logger.ConditionalDebug(NLCulture, "we've got error {0}, {1}, {2}, {3} ...", 500, 501, 502, 503);
                if (enabled == 1) AssertDebugLastMessage("debug", "we've got error 500, 501, 502, 503 ...");


                logger.ConditionalDebug(argException, NLCulture, "we've got error {0}, {1}, {2}, {3} ...", 500, 501, 502, 503.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "we\'ve got error 500, 501, 502, 503,5 ...arg1 is obvious wrong\r\nParameter name: arg1");

                logger.ConditionalDebug(argException, "we've got error {0}, {1}, {2}, {3} ...", 500, 501, 502, 503.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "we\'ve got error 500, 501, 502, 503.5 ...arg1 is obvious wrong\r\nParameter name: arg1");

                logger.ConditionalDebug("message{0}", (ulong)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.ConditionalDebug(CultureInfo.InvariantCulture, "message{0}", (ulong)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.ConditionalDebug("message{0}", (long)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.ConditionalDebug(CultureInfo.InvariantCulture, "message{0}", (long)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.ConditionalDebug("message{0}", (uint)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.ConditionalDebug(CultureInfo.InvariantCulture, "message{0}", (uint)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.ConditionalDebug("message{0}", 1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.ConditionalDebug(CultureInfo.InvariantCulture, "message{0}", 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.ConditionalDebug("message{0}", (ushort)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.ConditionalDebug(CultureInfo.InvariantCulture, "message{0}", (ushort)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.ConditionalDebug("message{0}", (sbyte)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.ConditionalDebug(CultureInfo.InvariantCulture, "message{0}", (sbyte)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.ConditionalDebug("message{0}", this);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                logger.ConditionalDebug(CultureInfo.InvariantCulture, "message{0}", this);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                logger.ConditionalDebug("message{0}", (short)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.ConditionalDebug(CultureInfo.InvariantCulture, "message{0}", (short)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.ConditionalDebug("message{0}", (byte)1);
                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                logger.ConditionalDebug(CultureInfo.InvariantCulture, "message{0}", (byte)2);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                logger.ConditionalDebug("message{0}", 'c');
                if (enabled == 1) AssertDebugLastMessage("debug", "messagec");

                logger.ConditionalDebug(CultureInfo.InvariantCulture, "message{0}", 'd');
                if (enabled == 1) AssertDebugLastMessage("debug", "messaged");

                logger.ConditionalDebug("message{0}", "ddd");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd");

                logger.ConditionalDebug(CultureInfo.InvariantCulture, "message{0}", "eee");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee");

                logger.ConditionalDebug("message{0}{1}", "ddd", 1);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1");

                logger.ConditionalDebug(CultureInfo.InvariantCulture, "message{0}{1}", "eee", 2);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2");

                logger.ConditionalDebug("message{0}{1}{2}", "ddd", 1, "eee");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1eee");

                logger.ConditionalDebug(CultureInfo.InvariantCulture, "message{0}{1}{2}", "eee", 2, "fff");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fff");

                logger.ConditionalDebug("message{0}{1}{2}{3}", "eee", 2, "fff", "ggg");
                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fffggg");

                logger.ConditionalDebug("message{0}", true);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageTrue");

                logger.ConditionalDebug(CultureInfo.InvariantCulture, "message{0}", false);
                if (enabled == 1) AssertDebugLastMessage("debug", "messageFalse");

                logger.ConditionalDebug(CultureInfo.InvariantCulture, "message{0}", (float)2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                logger.ConditionalDebug(CultureInfo.InvariantCulture, "message{0}", 2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                logger.ConditionalDebug(CultureInfo.InvariantCulture, "message{0}", (decimal)2.5);
                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                logger.ConditionalDebug(new Exception("test"), "message");
                if (enabled == 1) AssertDebugLastMessage("debug", "messagetest");

                logger.ConditionalDebug(new Exception("test"), "message {0}", "from parameter");
                if (enabled == 1) AssertDebugLastMessage("debug", "message from parametertest");

                logger.ConditionalDebug(delegate { return "message from lambda"; });
                if (enabled == 1) AssertDebugLastMessage("debug", "message from lambda");

                if (enabled == 0)
                    AssertDebugCounter("debug", 0);
            }
        }

#endif
        #endregion

        [Fact]
        public void SwallowTest()
        {
            LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}' /></targets>
                    <rules>
                        <logger name='*' levels='Error' writeTo='debug' />
                    </rules>
                </nlog>");
            ILogger logger = LogManager.GetLogger("A");
            bool warningFix = true;

            bool executed = false;
            logger.Swallow(() => executed = true);
            Assert.True(executed);

            Assert.Equal(1, logger.Swallow(() => 1));
            Assert.Equal(1, logger.Swallow(() => 1, 2));

#if NET4_5
            logger.SwallowAsync(Task.WhenAll()).Wait();

            int executions = 0;
            logger.Swallow(Task.Run(() => ++executions));
            logger.SwallowAsync(async () => { await Task.Delay(20); ++executions; }).Wait();
            Assert.True(executions == 2);

            Assert.Equal(1, logger.SwallowAsync(async () => { await Task.Delay(20); return 1; }).Result);
            Assert.Equal(1, logger.SwallowAsync(async () => { await Task.Delay(20); return 1; }, 2).Result);
#endif

            AssertDebugCounter("debug", 0);

            logger.Swallow(() => { throw new InvalidOperationException("Test message 1"); });
            AssertDebugLastMessageContains("debug", "Test message 1");

            Assert.Equal(0, logger.Swallow(() => { if (warningFix) throw new InvalidOperationException("Test message 2"); return 1; }));
            AssertDebugLastMessageContains("debug", "Test message 2");

            Assert.Equal(2, logger.Swallow(() => { if (warningFix) throw new InvalidOperationException("Test message 3"); return 1; }, 2));
            AssertDebugLastMessageContains("debug", "Test message 3");

#if NET4_5
            var fireAndFogetCompletion = new TaskCompletionSource<bool>();
            fireAndFogetCompletion.SetException(new InvalidOperationException("Swallow fire and forget test message"));
            logger.Swallow(fireAndFogetCompletion.Task);
            while (!GetDebugLastMessage("debug").Contains("Swallow fire and forget test message"))
                Thread.Sleep(10); // Polls forever since there is nothing to wait on.

            var completion = new TaskCompletionSource<bool>();
            completion.SetException(new InvalidOperationException("Test message 4"));
            logger.SwallowAsync(completion.Task).Wait();
            AssertDebugLastMessageContains("debug", "Test message 4");

            logger.SwallowAsync(async () => { await Task.Delay(20); throw new InvalidOperationException("Test message 5"); }).Wait();
            AssertDebugLastMessageContains("debug", "Test message 5");

            Assert.Equal(0, logger.SwallowAsync(async () => { await Task.Delay(20); if (warningFix) throw new InvalidOperationException("Test message 6"); return 1; }).Result);
            AssertDebugLastMessageContains("debug", "Test message 6");

            Assert.Equal(2, logger.SwallowAsync(async () => { await Task.Delay(20); if (warningFix) throw new InvalidOperationException("Test message 7"); return 1; }, 2).Result);
            AssertDebugLastMessageContains("debug", "Test message 7");
#endif
        }

        [Fact]
        public void StringFormatWillNotCauseExceptions()
        {
            LogManager.Configuration = CreateConfigurationFromString(@"
                    <nlog throwExceptions='true'>
                        <targets><target name='debug' type='Debug' layout='${message}' /></targets>
                        <rules>
                            <logger name='*' minLevel='Info' writeTo='debug' />
                        </rules>
                    </nlog>");

            ILogger l = LogManager.GetLogger("StringFormatWillNotCauseExceptions");

            // invalid format string
            l.Info("aaaa {0");
            AssertDebugLastMessage("debug", "aaaa {0");
        }

        [Fact]
        public void MultipleLoggersWithSameNameShouldBothReceiveMessages()
        {
            LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets>
                        <target name='first' type='Debug' layout='${message}' />
                        <target name='second' type='Debug' layout='${message}' />
                    </targets>
                    <rules>
                        <logger name='*' minlevel='Debug' writeTo='first' />
                        <logger name='*' minlevel='Debug' writeTo='second' />
                    </rules>
                </nlog>");
            var logger = LogManager.GetLogger("A");

            const string logMessage = "Anything";
            logger.Debug(logMessage);
            AssertDebugLastMessage("first", logMessage);
            AssertDebugLastMessage("second", logMessage);
        }

        [Fact]
        public void When_Logging_LogEvent_Without_Level_Defined_No_Exception_Should_Be_Thrown()
        {
            var config = new LoggingConfiguration();
            var target = new MyTarget();
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, target));
            LogManager.Configuration = config;
            var logger = LogManager.GetLogger("A");

            Assert.Throws<InvalidOperationException>(() => logger.Log(new LogEventInfo()));
        }


        [Fact]
        public void SingleTargetMessageFormatOptimizationTest()
        {
            LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets>
                        <target name='target1' type='Debug' layout='${logger}|${message}' />
                        <target name='target2' type='Debug' layout='${logger}|${message}' />
                    </targets>
                    <rules>
                        <logger name='SingleTarget' writeTo='target1' />
                        <logger name='DualTarget' writeTo='target1,target2' />
                    </rules>
                </nlog>");

            var singleLogger = LogManager.GetLogger("SingleTarget");
            var dualLogger = LogManager.GetLogger("DualTarget");

            ConfigurationItemFactory.Default.ParseMessageTemplates = null;

            singleLogger.Debug("Hello");
            AssertDebugLastMessage("target1", "SingleTarget|Hello");
            singleLogger.Debug("Hello {0}", "World");
            AssertDebugLastMessage("target1", "SingleTarget|Hello World");

            dualLogger.Debug("Hello");
            AssertDebugLastMessage("target1", "DualTarget|Hello");
            AssertDebugLastMessage("target2", "DualTarget|Hello");
            dualLogger.Debug("Hello {0}", "World");
            AssertDebugLastMessage("target1", "DualTarget|Hello World");
            AssertDebugLastMessage("target2", "DualTarget|Hello World");

            ConfigurationItemFactory.Default.ParseMessageTemplates = true;

            singleLogger.Debug("Hello");
            AssertDebugLastMessage("target1", "SingleTarget|Hello");
            singleLogger.Debug("Hello {0}", "World");
            AssertDebugLastMessage("target1", "SingleTarget|Hello World");

            dualLogger.Debug("Hello");
            AssertDebugLastMessage("target1", "DualTarget|Hello");
            AssertDebugLastMessage("target2", "DualTarget|Hello");
            dualLogger.Debug("Hello {0}", "World");
            AssertDebugLastMessage("target1", "DualTarget|Hello World");
            AssertDebugLastMessage("target2", "DualTarget|Hello World");

            ConfigurationItemFactory.Default.ParseMessageTemplates = false;

            singleLogger.Debug("Hello");
            AssertDebugLastMessage("target1", "SingleTarget|Hello");
            singleLogger.Debug("Hello {0}", "World");
            AssertDebugLastMessage("target1", "SingleTarget|Hello World");

            dualLogger.Debug("Hello");
            AssertDebugLastMessage("target1", "DualTarget|Hello");
            AssertDebugLastMessage("target2", "DualTarget|Hello");
            dualLogger.Debug("Hello {0}", "World");
            AssertDebugLastMessage("target1", "DualTarget|Hello World");
            AssertDebugLastMessage("target2", "DualTarget|Hello World");
        }

        [Theory]
        [InlineData(null, "OrderId", "@Client")]
        [InlineData(true, "OrderId", "@Client")]
        [InlineData(null, "0", "@Client", Skip = "Not supported for performance reasons")]
        [InlineData(true, "0", "@Client")]
        [InlineData(null, "$0", "@Client")]
        [InlineData(true, "$0", "@Client")]
        [InlineData(null, "@0", "@Client")]
        [InlineData(true, "@0", "@Client")]
        [InlineData(null, "1", "@Client", Skip = "Not supported for performance reasons")]
        [InlineData(true, "1", "@Client")]
        [InlineData(null, "@Client", "1")]
        [InlineData(true, "@Client", "1")]
        [InlineData(true, "0", "1")]
        [InlineData(false, "0", "1")]
        [InlineData(true, "OrderId", "Client")] //succeeeds, but gives JSON like (no quoted key, missing quotes arround string, =, other spacing)
        public void MixedStructuredEventsConfigTest(bool? parseMessageTemplates, string param1, string param2)
        {
            LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog parseMessageTemplates='" + (parseMessageTemplates?.ToString() ?? string.Empty) + @"'>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' writeTo='debug' />
                    </rules>
                </nlog>");
            ILogger logger = LogManager.GetLogger("A");
            logger.Debug("Process order {" + param1 + "} for {" + param2 + "}", 13424, new { ClientId = 3001, ClientName = "John Doe" });



            string param1Value;

            if (param1.StartsWith("$"))
            {
                param1Value = "\"13424\"";
            }
            else
            {
                param1Value = "13424";
            }

            string param2Value;
            if (param2.StartsWith("@"))
            {
                param2Value = "{\"ClientId\":3001, \"ClientName\":\"John Doe\"}";
            }
            else
            {
                param2Value = "{ ClientId = 3001, ClientName = John Doe }";
            }

            AssertDebugLastMessage("debug", $"Process order {param1Value} for {param2Value}");
        }

        [Theory]
        [InlineData(true, null)]
        [InlineData(false, null)]
        [InlineData(null, true)]
        [InlineData(null, false)]
        [InlineData(null, null)]
        public void StructuredEventsConfigTest(bool? parseMessageTemplates, bool? overrideParseMessageTemplates)
        {
            LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog parseMessageTemplates='" + (parseMessageTemplates?.ToString() ?? string.Empty) + @"'>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' writeTo='debug' />
                    </rules>
                </nlog>");

            if (parseMessageTemplates.HasValue)
            {
                Assert.Equal(ConfigurationItemFactory.Default.ParseMessageTemplates, parseMessageTemplates.Value);
            }

            if (overrideParseMessageTemplates.HasValue)
            {
                ConfigurationItemFactory.Default.ParseMessageTemplates = overrideParseMessageTemplates.Value;
            }

            ILogger logger = LogManager.GetLogger("A");
            logger.Debug("Hello World {0}", new object[] { null });
            if (parseMessageTemplates == true || overrideParseMessageTemplates == true)
                AssertDebugLastMessage("debug", "Hello World NULL");
            else
                AssertDebugLastMessage("debug", "Hello World ");
        }

        [Fact]
        public void StructuredEventsTest1()
        {
            // test all possible overloads of the Error() method
            var James = new Person("James");
            var Mike = new Person("Mike");
            var Jane = new Person("Jane") { Childs = new List<Person> { James, Mike } };


            for (int enabled = 0; enabled < 2; ++enabled)
            {
                if (enabled == 0)
                {
                    LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='' writeTo='debug' />
                    </rules>
                </nlog>");
                }
                else
                {
                    LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog>
                    <targets><target name='debug' type='Debug' layout='${message}${exception}' /></targets>
                    <rules>
                        <logger name='*' levels='Error' writeTo='debug' />
                    </rules>
                </nlog>");
                }

                ILogger logger = LogManager.GetLogger("A");
                LogManager.Configuration.DefaultCultureInfo = CultureInfo.InvariantCulture;

                logger.Error("hello from {@Person}", Jane);
                if (enabled == 1) AssertDebugLastMessage("debug", "hello from {\"Name\":\"Jane\", \"Childs\":[{\"Name\":\"James\"},{\"Name\":\"Mike\"}]}");

                logger.Error("Test structured logging in {NLogVersion} for .NET {NETVersion}", "4.5-alpha01", new[] { 3.5, 4, 4.5 });
                if (enabled == 1) AssertDebugLastMessage("debug", "Test structured logging in \"4.5-alpha01\" for .NET 3.5, 4, 4.5");

                logger.Error("hello from {FamilyNames}", new Dictionary<int, string>() { { 1, "James" }, { 2, "Mike" }, { 3, "Jane" } });
                if (enabled == 1) AssertDebugLastMessage("debug", "hello from 1=\"James\", 2=\"Mike\", 3=\"Jane\"");

                logger.Error("message {a} {b}", 1, 2);
                if (enabled == 1)
                {
                    AssertDebugLastMessage("debug", "message 1 2");

                }

                logger.Error("message{a}{b}{c}", 1, 2, 3);
                if (enabled == 1)
                {
                    AssertDebugLastMessage("debug", "message123");
                }


                logger.Error("message {a} {b} {c}", "1", "2", "3");
                if (enabled == 1)
                {
                    //todo single quotes
                    AssertDebugLastMessage("debug", "message \"1\" \"2\" \"3\"");
                }


                logger.Error("message{a}{b}{c}", 1, 2, 3);
                if (enabled == 1) AssertDebugLastMessage("debug", "message123");


                //todo other tests

                //                logger.Error(NLCulture, "message{0}{1}{2}", 1.4, 2.5, 3.6);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message1,42,53,6");

                //                logger.Error("message{0}", (float)2.3);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                //                logger.Error("message{0}", (double)2.3);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                //                logger.Error("message{0}", (decimal)2.3);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                //                logger.Error("message{0}", (object)2.3);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message2.3");

                //                logger.Error(NLCulture, "message{0}", (object)2.3);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message2,3");

                //                logger.Error("message{0}", (ulong)1);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                //                logger.Error(CultureInfo.InvariantCulture, "message{0}", (ulong)2);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                //                logger.Error("message{0}", (long)1);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                //                logger.Error(CultureInfo.InvariantCulture, "message{0}", (long)2);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                //                logger.Error("message{0}", (uint)1);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                //                logger.Error(CultureInfo.InvariantCulture, "message{0}", (uint)2);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                //                logger.Error("message{0}", 1);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                //                logger.Error(CultureInfo.InvariantCulture, "message{0}", 2);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                //                logger.Error("message{0}", (ushort)1);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                //                logger.Error(CultureInfo.InvariantCulture, "message{0}", (ushort)2);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                //                logger.Error("message{0}", (sbyte)1);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                //                logger.Error(CultureInfo.InvariantCulture, "message{0}", (sbyte)2);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                //                logger.Error("message{0}", this);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                //                logger.Error(CultureInfo.InvariantCulture, "message{0}", this);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "messageobject-to-string");

                //                logger.Error("message{0}", (short)1);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                //                logger.Error(CultureInfo.InvariantCulture, "message{0}", (short)2);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                //                logger.Error("message{0}", (byte)1);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message1");

                //                logger.Error(CultureInfo.InvariantCulture, "message{0}", (byte)2);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message2");

                //                logger.Error("message{0}", 'c');
                //                if (enabled == 1) AssertDebugLastMessage("debug", "messagec");

                //                logger.Error(CultureInfo.InvariantCulture, "message{0}", 'd');
                //                if (enabled == 1) AssertDebugLastMessage("debug", "messaged");

                //                logger.Error("message{0}", "ddd");
                //                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd");

                //                logger.Error(CultureInfo.InvariantCulture, "message{0}", "eee");
                //                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee");

                //                logger.Error("message{0}{1}", "ddd", 1);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1");

                //                logger.Error(CultureInfo.InvariantCulture, "message{0}{1}", "eee", 2);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2");

                //                logger.Error("message{0}{1}{2}", "ddd", 1, "eee");
                //                if (enabled == 1) AssertDebugLastMessage("debug", "messageddd1eee");

                //                logger.Error(CultureInfo.InvariantCulture, "message{0}{1}{2}", "eee", 2, "fff");
                //                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fff");

                //                logger.Error("message{0}{1}{2}{3}", "eee", 2, "fff", "ggg");
                //                if (enabled == 1) AssertDebugLastMessage("debug", "messageeee2fffggg");

                //                logger.Error("message{0}", true);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "messageTrue");

                //                logger.Error(CultureInfo.InvariantCulture, "message{0}", false);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "messageFalse");

                //                logger.Error(CultureInfo.InvariantCulture, "message{0}", (float)2.5);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                //                logger.Error(CultureInfo.InvariantCulture, "message{0}", 2.5);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                //                logger.Error(CultureInfo.InvariantCulture, "message{0}", (decimal)2.5);
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message2.5");

                //#pragma warning disable 0618
                //                // Obsolete method requires testing until removed.
                //                logger.ErrorException("message", new Exception("test"));
                //                if (enabled == 1) AssertDebugLastMessage("debug", "messagetest");
                //#pragma warning restore 0618

                logger.Error(new Exception("test"), "message");
                if (enabled == 1) AssertDebugLastMessage("debug", "messagetest");

                logger.Error(new Exception("test"), "message {Exception}", "from parameter");
                if (enabled == 1) AssertDebugLastMessage("debug", "message \"from parameter\"test");




                //                logger.Error(delegate { return "message from lambda"; });
                //                if (enabled == 1) AssertDebugLastMessage("debug", "message from lambda");

                if (enabled == 0)
                    AssertDebugCounter("debug", 0);
            }
        }

        /// <summary>
        /// Only properties
        /// </summary>
        [Fact]
        public void TestStructuredProperties_json()
        {
            LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog throwExceptions='true'>
                    <targets>
                        <target name='debug' type='Debug'  >
                                <layout type='JsonLayout' IncludeAllProperties='true'>
                                    <attribute name='LogMessage' layout='${message:raw=true}' />
                                </layout>
                        </target>
                    </targets>
                    <rules>
                        <logger name='*' levels='Error' writeTo='debug' />
                    </rules>
                </nlog>");

            ILogger logger = LogManager.GetLogger("A");

            logger.Error("Login request from {Username} for {Application}", "John", "BestApplicationEver");

            AssertDebugLastMessage("debug", "{ \"LogMessage\": \"Login request from {Username} for {Application}\", \"Username\": \"John\", \"Application\": \"BestApplicationEver\" }");
        }

        /// <summary>
        /// Properties and message
        /// </summary>
        [Fact]
        public void TestStructuredProperties_json_compound()
        {
            LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog throwExceptions='true'>
                    <targets>
                        <target name='debug' type='Debug'  >
                              <layout type='CompoundLayout'>
                                <layout type='SimpleLayout' text='${message}' />
                                <layout type='JsonLayout' IncludeAllProperties='true' />
                              </layout>
                        </target>
                    </targets>
                    <rules>
                        <logger name='*' levels='Error' writeTo='debug' />
                    </rules>
                </nlog>");

            ILogger logger = LogManager.GetLogger("A");

            logger.Error("Login request from {Username} for {Application}", "John", "BestApplicationEver");

            AssertDebugLastMessage("debug", "Login request from \"John\" for \"BestApplicationEver\"{ \"Username\": \"John\", \"Application\": \"BestApplicationEver\" }");
        }

        [Fact]
        public void TestOptimizedBlackHoleLogger()
        {
            LogManager.ThrowExceptions = true;
            LogManager.Configuration = CreateConfigurationFromString(@"
                <nlog throwExceptions='true'>
                    <targets>
                        <target name='debug' type='Debug' layout='${message}' />
                    </targets>
                    <rules>
                        <logger name='Microsoft*' maxLevel='Info' writeTo='' final='true' />
                        <logger name='*' minlevel='Debug' writeTo='debug' />
                    </rules>
                </nlog>");

            ILogger loggerMicrosoft = LogManager.GetLogger("Microsoft.NoiseGenerator");
            ILogger loggerA = LogManager.GetLogger("A");

            loggerMicrosoft.Warn("Important Noise");
            AssertDebugLastMessage("debug", "Important Noise");
            loggerMicrosoft.Debug("White Noise");
            AssertDebugLastMessage("debug", "Important Noise");
            loggerA.Debug("Good Noise");
            AssertDebugLastMessage("debug", "Good Noise");
            loggerA.Error("Important Noise");
            AssertDebugLastMessage("debug", "Important Noise");
        }


        private class Person
        {
            public Person()
            {
            }

            public Person(string name)
            {
                Name = name;
            }

            public string Name { get; set; }

            public List<Person> Childs { get; set; }

        }

        public abstract class BaseWrapper
        {
            public void Log(string what)
            {
                InternalLog(what);
            }

            protected abstract void InternalLog(string what);
        }

        public class MyWrapper : BaseWrapper
        {
            private readonly ILogger wrapperLogger;

            public MyWrapper()
            {
                wrapperLogger = LogManager.GetLogger("WrappedLogger");
            }

            protected override void InternalLog(string what)
            {
                LogEventInfo info = new LogEventInfo(LogLevel.Warn, wrapperLogger.Name, what);

                // Provide BaseWrapper as wrapper type.
                // Expected: UserStackFrame should point to the method that calls a 
                // method of BaseWrapper.
                wrapperLogger.Log(typeof(BaseWrapper), info);
            }
        }

        public class MyTarget : TargetWithLayout
        {
            public MyTarget()
            {
                // enforce creation of stack trace
                Layout = "${stacktrace}";
            }

            public MyTarget(string name) : this()
            {
                Name = name;
            }

            public LogEventInfo LastEvent { get; private set; }

            protected override void Write(LogEventInfo logEvent)
            {
                LastEvent = logEvent;
                base.Write(logEvent);
            }
        }

        public override string ToString()
        {
            return "object-to-string";
        }


        [Fact]
        public void LogEventTemplateShouldHaveProperties()
        {
            var logEventInfo = new LogEventInfo(LogLevel.Debug, "logger1", null, "{A}", new object[] { "b" });
            var props = logEventInfo.Properties;
            Assert.Contains("A", props.Keys);
            Assert.Equal("b", props["A"]);
            Assert.Equal(1, props.Count);
        }

        [Fact]
        public void LogEventTemplateShouldHaveProperties_even_when_changed()
        {
            var logEventInfo = new LogEventInfo(LogLevel.Debug, "logger1", null, "{A}", new object[] { "b" });
            var props = logEventInfo.Properties;
            logEventInfo.Message = "{A}";
            Assert.Contains("A", props.Keys);
            Assert.Equal("b", props["A"]);
            Assert.Equal(1, props.Count);
        }
    }
}
