using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Helpers
{    
    public class ErrorLogger
    {
        private const bool ThrowIfDebug = true;

        public static void LogException(Exception error)
        {
#if DEBUG
            if (ThrowIfDebug)
            {
                throw error;
            }
#endif

            // TODO: implement exception logging
        }
    }
}
