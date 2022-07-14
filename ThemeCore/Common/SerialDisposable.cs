using System;

namespace ThemeCore.Common
{
    public sealed class SerialDisposable : IDisposable
    {
        IDisposable _content;

        public IDisposable Content
        {
            get { return _content; }
            set
            {
                if (_content != null)
                {
                    _content.Dispose();
                }

                _content = value;
            }
        }

        public void Dispose()
        {
            Content = null;
        }
    }
}
