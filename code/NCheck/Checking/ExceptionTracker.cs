using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NCheck.Checking
{
    internal class ExceptionTracker
    {
        private IList<PropertyCheckException> exceptions;
        private StringBuilder builder;

        public ExceptionTracker(string baseName = null)
        {
            exceptions = new List<PropertyCheckException>();
            builder = new StringBuilder();

            if (!string.IsNullOrEmpty(baseName))
            {
                builder.AppendLine(baseName);
            }
        }

        public void Add(string propertyName, object expected, object actual)
        {
            var p = new PropertyCheckException(propertyName, expected, actual);
            Add(p);
        }

        public void Add(PropertyCheckException p)
        {
            builder.AppendLine(p.Message);
            exceptions.Add(p);
        }

        public PropertyCheckException Report(string objectName)
        {
            if (exceptions.Count == 0)
            {
                return null;
            }

            // Get the message and ignore the last crlf
            var message = builder.ToString().TrimEnd('\r', '\n');
            var p = new PropertyCheckException(objectName, message);
            foreach (var pex in exceptions)
            {
                p.Exceptions.Add(pex);
            }

            return p;
        }
    }
}