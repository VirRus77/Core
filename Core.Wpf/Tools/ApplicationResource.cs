using System;
using System.IO;
using System.Management.Instrumentation;
using System.Windows;

namespace Core.Tools
{
    public class ApplicationResource
    {
        public static Stream GetStream(Uri resourceUri)
        {
            var streamResourceInfo = Application.GetResourceStream(resourceUri);
            if (streamResourceInfo == null)
            {
                throw new InstanceNotFoundException("streamResourceInfo");
            }
            return streamResourceInfo.Stream;
        }
    }
}
