using Syncfusion.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Syncfusion.Addin.Utility
{
    /// <summary>
    /// This class contains various useful features for detecting network state
    /// </summary>
    public static class NetworkHelper
    {
        /// <summary>
        /// Occurs when the IP Address of a network interface changes
        /// </summary>
        /// <remarks>
        /// This event is provided for consistency only.
        /// It simply passes through to NetworkChange.NetworkAddressChanged
        /// </remarks>
        public static event NetworkAddressChangedEventHandler NetworkAddressChanged
        {
            add
            {
                NetworkChange.NetworkAddressChanged += value;
            }
            remove
            {
                NetworkChange.NetworkAddressChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the availability of a network changes
        /// </summary>
        /// <remarks>
        /// This event is provided for consistency only.
        /// It simply passes through to NetworkChange.NetworkAvailabilityChanged
        /// </remarks>
        public static event NetworkAvailabilityChangedEventHandler NetworkAvailabilityChanged
        {
            add
            {
                NetworkChange.NetworkAvailabilityChanged += value;
            }
            remove
            {
                NetworkChange.NetworkAvailabilityChanged -= value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a valid network conneciton is available
        /// </summary>
        /// <value><c>true</c> if [network available]; otherwise, <c>false</c>.</value>
        /// <example>
        /// if (NetworkHelper.NetworkAvailable)
        /// {
        ///     // ... do something
        /// }
        /// </example>
        public static bool NetworkAvailable
        {
            get
            {
                return NetworkInterface.GetIsNetworkAvailable();
            }
        }

        /// <summary>
        /// Gets a value indicating whether a connection to the Internet is available
        /// </summary>
        /// <value>
        /// 	<c>true</c> if an Internet connection is available..
        /// </value>
        /// <example>
        /// if (NetworkHelper.InternetConnectionAvailable)
        /// {
        ///     // ... do something
        /// }
        /// </example>
        public static bool InternetConnectionAvailable
        {
            get
            {
                return NetworkHelper.NetworkAvailable && NetworkHelper.CanConnect("www.msn.com");
            }
        }

        /// <summary>
        /// Gets the speed of the fastest available connection in bits per second.
        /// </summary>
        /// <value>Connection speed.</value>
        /// <remarks>
        /// Tunnel and Loopback adapters are ignored.
        /// </remarks>
        /// <example>
        /// if (NetworkHelper.FastestConnectionSpeed &gt;= 54000000)
        /// {
        ///     // ... download something large
        /// }
        /// else
        /// {
        ///     MessageBox.Show("You need at least an 802.11a connection (or better) for this feature.");
        /// }
        /// </example>
        public static long FastestConnectionSpeed
        {
            get
            {
                long result;
                if (NetworkHelper.NetworkAvailable)
                {
                    long maxSpeed = 0L;
                    NetworkInterface[] networks = NetworkInterface.GetAllNetworkInterfaces();
                    NetworkInterface[] array = networks;
                    for (int i = 0; i < array.Length; i++)
                    {
                        NetworkInterface network = array[i];
                        if (network.OperationalStatus == OperationalStatus.Up && network.NetworkInterfaceType != NetworkInterfaceType.Loopback && network.NetworkInterfaceType != NetworkInterfaceType.Tunnel && network.Speed > maxSpeed)
                        {
                            maxSpeed = network.Speed;
                        }
                    }
                    result = maxSpeed;
                }
                else
                {
                    result = -1L;
                }
                return result;
            }
        }

        /// <summary>
        /// Gets all the IP addresses (IPv4) utilized by the current system.
        /// </summary>
        /// <value>All IP addresses.</value>
        /// <remarks>
        /// Only IPv4 addresses are included.
        /// Tunnel and Loopback adapters are ignored.
        /// Only adapters that are up are considered.
        /// </remarks>
        /// <example>
        /// StringBuilder sb = new StringBuilder();
        /// sb.AppendLine("IP Addresses used by this system:");
        /// foreach (IPAddressInformation address in NetworkHelper.AllIPAddresses)
        /// {
        ///     sb.AppendLine(address.Address.ToString());
        /// }
        /// MessageBox.Show(sb.ToString());
        /// </example>
        public static Collection<IPAddressInformation> AllIPAddresses
        {
            get
            {
                Collection<IPAddressInformation> addresses = new Collection<IPAddressInformation>();
                NetworkInterface[] networks = NetworkInterface.GetAllNetworkInterfaces();
                NetworkInterface[] array = networks;
                for (int i = 0; i < array.Length; i++)
                {
                    NetworkInterface network = array[i];
                    if (network.OperationalStatus == OperationalStatus.Up && network.NetworkInterfaceType != NetworkInterfaceType.Loopback && network.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                    {
                        IPInterfaceProperties ipProperties = network.GetIPProperties();
                        foreach (UnicastIPAddressInformation address in ipProperties.UnicastAddresses)
                        {
                            if (address.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                addresses.Add(address);
                            }
                        }
                    }
                }
                return addresses;
            }
        }

        /// <summary>
        /// Gets all the IP addresses (IPv4) utilized by the current system in clear text representation.
        /// </summary>
        /// <value>All IP addresses.</value>
        /// <remarks>
        /// Only IPv4 addresses are included.
        /// Tunnel and Loopback adapters are ignored.
        /// Only adapters that are up are considered.
        /// </remarks>
        /// <example>
        /// StringBuilder sb = new StringBuilder();
        /// sb.AppendLine("IP Addresses used by this system:");
        /// foreach (string address in NetworkHelper.AllIPAddressesClearText)
        /// {
        ///     sb.AppendLine(address);
        /// }
        /// MessageBox.Show(sb.ToString());
        /// </example>
        public static Collection<string> AllIPAddressesClearText
        {
            get
            {
                Collection<IPAddressInformation> addresses = NetworkHelper.AllIPAddresses;
                Collection<string> addressesClearText = new Collection<string>();
                foreach (IPAddressInformation address in addresses)
                {
                    addressesClearText.Add(address.Address.ToString());
                }
                return addressesClearText;
            }
        }

        /// <summary>
        /// Gets the system's current IP address (IPv4) in clear text.
        /// </summary>
        /// <value>The current ip address.</value>
        /// <remarks>
        /// If the system currently uses multiple IP addresses, the first unicast address
        /// on the fastest adapter will be returned.
        /// </remarks>
        /// <example>
        /// MessageBox.Show("Current IP Address: " + NetworkHelper.CurrentIPAddressClearText);
        /// </example>
        public static string CurrentIPAddressClearText
        {
            get
            {
                IPAddressInformation address = NetworkHelper.CurrentIPAddress;
                string result;
                if (address != null)
                {
                    result = address.Address.ToString();
                }
                else
                {
                    result = string.Empty;
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the system's current IP address (IPv4).
        /// </summary>
        /// <value>The current ip address.</value>
        /// <remarks>
        /// If the system currently uses multiple IP addresses, the first unicast address
        /// on the fastest adapter will be returned.
        /// </remarks>
        /// <example>
        /// MessageBox.Show("Current IP Address: " + NetworkHelper.CurrentIPAddress.Address.ToString());
        /// </example>
        public static IPAddressInformation CurrentIPAddress
        {
            get
            {
                NetworkInterface[] networks = NetworkInterface.GetAllNetworkInterfaces();
                NetworkInterface fastestInterface = null;
                NetworkInterface[] array = networks;
                for (int i = 0; i < array.Length; i++)
                {
                    NetworkInterface network = array[i];
                    if (network.OperationalStatus == OperationalStatus.Up && network.NetworkInterfaceType != NetworkInterfaceType.Loopback && network.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                    {
                        if (fastestInterface == null)
                        {
                            fastestInterface = network;
                        }
                        else if (network.Speed > fastestInterface.Speed)
                        {
                            fastestInterface = network;
                        }
                    }
                }
                IPAddressInformation result;
                if (fastestInterface != null)
                {
                    IPInterfaceProperties ipProperties = fastestInterface.GetIPProperties();
                    foreach (UnicastIPAddressInformation address in ipProperties.UnicastAddresses)
                    {
                        if (address.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            result = address;
                            return result;
                        }
                    }
                    result = null;
                }
                else
                {
                    result = null;
                }
                return result;
            }
        }

        /// <summary>
        /// Gets a collection of all current domains we are connected to.
        /// </summary>
        /// <value>All current domains.</value>
        /// <example>
        /// StringBuilder sb = new StringBuilder();
        /// sb.AppendLine("Current domains:");
        /// foreach (string domain in NetworkHelper.AllCurrentDomains)
        /// {
        ///     sb.AppendLine(domain);
        /// }
        /// MessageBox.Show(sb.ToString());
        /// </example>
        public static Collection<string> AllCurrentDomains
        {
            get
            {
                Collection<string> domains = new Collection<string>();
                NetworkInterface[] networks = NetworkInterface.GetAllNetworkInterfaces();
                NetworkInterface[] array = networks;
                for (int i = 0; i < array.Length; i++)
                {
                    NetworkInterface network = array[i];
                    if (network.OperationalStatus == OperationalStatus.Up && network.NetworkInterfaceType != NetworkInterfaceType.Loopback && network.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                    {
                        string domain = network.GetIPProperties().DnsSuffix;
                        if (!string.IsNullOrEmpty(domain))
                        {
                            domains.Add(domain);
                        }
                    }
                }
                return domains;
            }
        }

        /// <summary>
        /// Returns true, if the system is currently connected to the specified domain.
        /// </summary>
        /// <param name="domain">The domain (case insensitive).</param>
        /// <returns>True if connected to the domain.</returns>
        /// <example>
        /// if (NetworkHelper.CurrentlyConnectedToDomain("mydomain"))
        /// {
        ///     // ... do something with it
        /// }
        /// </example>
        public static bool CurrentlyConnectedToDomain(string domain)
        {
            Collection<string> domains = NetworkHelper.AllCurrentDomains;
            bool result;
            foreach (string foundDomain in domains)
            {
                if (StringHelper.Compare(domain, foundDomain, true))
                {
                    result = true;
                    return result;
                }
            }
            result = false;
            return result;
        }

        /// <summary>
        /// Determines whether it is possible to connect to the specified host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can connect the specified host; otherwise, <c>false</c>.
        /// </returns>
        /// <example>
        /// if (NetworkHelper.CanConnect("www.Microsoft.com"))
        /// {
        /// // ... do something with the connection
        /// }
        /// </example>
        /// <remarks>Port 80 is used to make the connection attempt.</remarks>
        public static bool CanConnect(string host)
        {
            return NetworkHelper.CanConnect(host, 80);
        }

        /// <summary>
        /// Determines whether it is possible to connect to the specified host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can connect the specified host; otherwise, <c>false</c>.
        /// </returns>
        /// <example>
        /// if (NetworkHelper.CanConnect("www.Microsoft.com", 80))
        /// {
        ///     // ... do something with the connection
        /// }
        /// </example>
        public static bool CanConnect(string host, int port)
        {
            bool result;
            try
            {
                string localHostname = Dns.GetHostName();
                IPHostEntry localHostEntry = Dns.GetHostEntry(localHostname);
                IPHostEntry remoteHostEntry = Dns.GetHostEntry(host);
                NetworkInterface[] networks = NetworkInterface.GetAllNetworkInterfaces();
                NetworkInterface[] array = networks;
                for (int i = 0; i < array.Length; i++)
                {
                    NetworkInterface network = array[i];
                    if (network.OperationalStatus == OperationalStatus.Up && network.NetworkInterfaceType != NetworkInterfaceType.Loopback && network.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                    {
                        IPInterfaceProperties interfaceProperties = network.GetIPProperties();
                        List<IPAddressInformation> adapterAddressList = NetworkHelper.GetAllAddressesForAdapter(interfaceProperties);
                        IPAddress[] addressList = localHostEntry.AddressList;
                        for (int j = 0; j < addressList.Length; j++)
                        {
                            IPAddress localHostAddress = addressList[j];
                            if (NetworkHelper.CanConnectFromAddress(localHostAddress, adapterAddressList, remoteHostEntry, port))
                            {
                                result = true;
                                return result;
                            }
                        }
                    }
                }
                result = false;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Gets all addresses for an adapter.
        /// </summary>
        /// <param name="interfaceProperties">The interface properties.</param>
        /// <returns>List of IP Addresses</returns>
        /// <remarks>For internal use only</remarks>
        private static List<IPAddressInformation> GetAllAddressesForAdapter(IPInterfaceProperties interfaceProperties)
        {
            List<IPAddressInformation> adapterAddressList = interfaceProperties.AnycastAddresses.ToList<IPAddressInformation>();
            adapterAddressList.AddRange(interfaceProperties.MulticastAddresses);
            adapterAddressList.AddRange(interfaceProperties.UnicastAddresses);
            return adapterAddressList;
        }

        /// <summary>
        /// Determines whether it is possible to connect to the specified local host address.
        /// </summary>
        /// <param name="localHostAddress">The local host address.</param>
        /// <param name="adapterAddressList">The adapter address list.</param>
        /// <param name="remoteHostEntry">The remote host entry.</param>
        /// <param name="port">The port.</param>
        /// <returns>true if this instance can connect from the specified local host addr; otherwise false.</returns>
        /// <remarks>For internal use only</remarks>
        private static bool CanConnectFromAddress(IPAddress localHostAddress, IEnumerable<IPAddressInformation> adapterAddressList, IPHostEntry remoteHostEntry, int port)
        {
            bool result;
            foreach (IPAddressInformation adapterAddress in adapterAddressList)
            {
                if (localHostAddress.Equals(adapterAddress.Address))
                {
                    IPEndPoint localEndpoint = new IPEndPoint(localHostAddress, 8081);
                    if (NetworkHelper.CanConnectFromEndPoint(localEndpoint, remoteHostEntry, port))
                    {
                        result = true;
                        return result;
                    }
                }
            }
            result = false;
            return result;
        }

        /// <summary>
        /// Determines whether it is possible to connect to the remote host from the specified endpoint.
        /// </summary>
        /// <param name="localEndPoint">The local end point.</param>
        /// <param name="remoteHostEntry">The remote host entry.</param>
        /// <param name="port">The port.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can connect from end point] the specified local EP; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>For internal use only</remarks>
        private static bool CanConnectFromEndPoint(EndPoint localEndPoint, IPHostEntry remoteHostEntry, int port)
        {
            IPAddress[] addressList = remoteHostEntry.AddressList;
            bool result;
            for (int i = 0; i < addressList.Length; i++)
            {
                IPAddress remoteAddress = addressList[i];
                try
                {
                    IPEndPoint remoteEndpoint = new IPEndPoint(remoteAddress, port);
                    Socket sock = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint ipLocal = localEndPoint as IPEndPoint;
                    sock.Bind((ipLocal != null) ? new IPEndPoint(ipLocal.Address, ipLocal.Port) : localEndPoint);
                    bool connected;
                    try
                    {
                        sock.Connect(remoteEndpoint);
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        connected = sock.Connected;
                        if (connected)
                        {
                            sock.Disconnect(false);
                        }
                        sock.Close();
                    }
                    if (connected)
                    {
                        result = true;
                        return result;
                    }
                }
                catch (Exception)
                {
                    result = false;
                    return result;
                }
            }
            result = false;
            return result;
        }
    }
}