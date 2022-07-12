namespace WTLib.Utils
{
    using WTLib.Logger;
    using System;
    using System.IO;
    using System.Xml;

    public class ParseAddressPort
    {
        private const string ElementName = "endpoint";
        private const string ElementAttribute = "address";
        private const string NetTcp = @"net.tcp://";
        private const string Pattern = @"net\.tcp\:\/\/(\S*):([0-9]+)\/";

        private XmlDocument _xml = null;
        private string _path = null;
        private readonly string _identification = null;

        public string Address { get; private set; }
        public string Port { get; private set; }

        public ParseAddressPort(string identification)
        {
            _identification = String.Concat(@"/", identification, @"/");
            _xml = new XmlDocument();
        }

        /// <summary>
        /// Load address and port with the service.
        /// </summary>
        /// <param name="configFileName">It`s app.config file name. e.g: QCTrader.exe.config</param>
        public void Load()
        {
            this.Address = null;
            this.Port = null;
            _path = string.Format("{0}.config", System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            if (!File.Exists(_path))
            {
                Log.Trace.Error("The '{0}' does not exist.", _path);
                return;
            }
            try
            {
                _xml.Load(_path);
                var nodes = _xml.GetElementsByTagName(ElementName);
                if (nodes == null || nodes.Count > 1)
                {
                    Log.Trace.Error("The '{0}' does not exist in {1}.", ElementName, _path);
                    return;
                }
                var address = nodes[0].Attributes[ElementAttribute];
                var matchs = System.Text.RegularExpressions.Regex.Matches(address.Value, Pattern);
                if (matchs.Count <= 0)
                {
                    Log.Trace.Error("The '{0}' does not exist in {1}.", ElementAttribute, ElementName);
                    return;
                }
                this.Address = matchs[0].Groups[1].ToString();
                this.Port = matchs[0].Groups[2].ToString();
            }
            catch (Exception e)
            {
                Log.Trace.Error("*** Load {4} Exception *** {0}Message: {1}{0}Source: {2}{0}StackTrace: {3}{0}",
                    Environment.NewLine,
                    e.Message,
                    e.Source,
                    e.StackTrace,
                    _path);
            }
        }

        /// <summary>
        /// Save address and port with the service. 
        /// </summary>
        /// <param name="address">service address</param>
        /// <param name="port">service port</param>
        public bool Save(string address, string port)
        {
            if (string.IsNullOrEmpty(address))
            {
                Log.Trace.Error("The address is null.");
                return false;
            }
            if (string.IsNullOrEmpty(port))
            {
                Log.Trace.Error("The port is null.");
                return false;
            }
            if (!File.Exists(_path))
            {
                Log.Trace.Error("The '{0}' does not exist.", _path);
                return false;
            }
            try
            {
                var nodes = _xml.GetElementsByTagName(ElementName);
                if (nodes == null || nodes.Count > 1)
                {
                    Log.Trace.Error("The '{0}' does not exist in {1}.", ElementName, _path);
                    return false;
                }
                var property = nodes[0].Attributes[ElementAttribute];
                property.Value = string.Concat(NetTcp, address, ":", port, _identification);
                _xml.Save(_path);
                this.Address = address;
                this.Port = port;
                return true;
            }
            catch (Exception e)
            {
                Log.Trace.Error("*** Save {4} Exception *** {0}Message: {1}{0}Source: {2}{0}StackTrace: {3}{0}",
                    Environment.NewLine,
                    e.Message,
                    e.Source,
                    e.StackTrace,
                    _path);
                return false;
            }
        }

        public static bool VerifyServiceAddress(string configFileName)
        {
            if (string.IsNullOrEmpty(configFileName))
            {
                Log.Trace.Error("The configFileName is null.");
                return false;
            }
            var path = Path.Combine(Environment.CurrentDirectory, configFileName);
            if (!File.Exists(path))
            {
                Log.Trace.Error("The '{0}' does not exist.", path);
                return false;
            }
            try
            {
                var xml = new XmlDocument();
                xml.Load(path);
                var nodes = xml.GetElementsByTagName(ElementName);
                if (nodes == null || nodes.Count > 1)
                {
                    Log.Trace.Error("The '{0}' does not exist in {1}.", ElementName, path);
                    return false;
                }
                var address = nodes[0].Attributes[ElementAttribute];
                var matchs = System.Text.RegularExpressions.Regex.Matches(address.Value, Pattern);
                if (matchs.Count <= 0)
                {
                    Log.Trace.Error("The '{0}' does not exist in {1}.", ElementAttribute, ElementName);
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                Log.Trace.Error("*** Load {4} Exception *** {0}Message: {1}{0}Source: {2}{0}StackTrace: {3}{0}",
                    Environment.NewLine,
                    e.Message,
                    e.Source,
                    e.StackTrace,
                    configFileName);
                return false;
            }
        }
    }
}
