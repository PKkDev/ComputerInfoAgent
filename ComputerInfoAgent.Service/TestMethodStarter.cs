using System;
using System.IO;
using System.Threading.Tasks;

using NickStrupat;

using System.Net.NetworkInformation;

using System.Diagnostics;

using Hardware.Info;

using System.ServiceProcess;

namespace ComputerInfoAgent.Service
{
    public class TestMethodStarter
    {
        private readonly string[] SizeSuffixes =
          { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public TestMethodStarter()
        {

        }

        public void GetComuterInfo()
        {
            Console.WriteLine();
            Console.WriteLine("computer info");

            try
            {
                ComputerInfo computerInfo = new ComputerInfo();
                Console.WriteLine($"Processor Count:{Environment.ProcessorCount}");
                Console.WriteLine();
                Console.WriteLine($"AvailablePhysicalMemory: {ByteConvert(computerInfo.AvailablePhysicalMemory)}");
                Console.WriteLine($"TotalPhysicalMemory: {ByteConvert(computerInfo.TotalPhysicalMemory)}");
                Console.WriteLine();
                Console.WriteLine($"AvailableVirtualMemory: {ByteConvert(computerInfo.AvailableVirtualMemory)}");
                Console.WriteLine($"TotalVirtualMemory: {ByteConvert(computerInfo.TotalVirtualMemory)}");
                Console.WriteLine();
                Console.WriteLine($"OSFullName: {computerInfo.OSFullName}");
                Console.WriteLine($"OSPlatform: {computerInfo.OSPlatform}");
                Console.WriteLine($"OSVersion: {computerInfo.OSVersion}");
                Console.WriteLine($"InstalledUICulture: {computerInfo.InstalledUICulture}");

                var disks = DriveInfo.GetDrives();
                foreach (var disk in disks)
                {
                    Console.WriteLine();
                    Console.WriteLine($"disk {disk}");
                    Console.WriteLine($"TotalSize: {ByteConvert(disk.TotalSize)}");
                    Console.WriteLine($"TotalFreeSpace: {ByteConvert(disk.TotalFreeSpace)}");
                    Console.WriteLine($"AvailableFreeSpace: {ByteConvert(disk.AvailableFreeSpace)}");
                    Console.WriteLine($"IsReady: { disk.IsReady}");
                    Console.WriteLine($"DriveFormat: { disk.DriveFormat}");
                    Console.WriteLine($"DriveType: { disk.DriveType}");
                    Console.WriteLine($"RootDirectory: { disk.RootDirectory}");
                    Console.WriteLine($"VolumeLabel: { disk.VolumeLabel}");
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void checksInternetCOnection()
        {
            Console.WriteLine();
            Console.WriteLine("check internet connect");

            try
            {
                var isConnect = NetworkInterface.GetIsNetworkAvailable();
                Console.WriteLine($"isConnect: {isConnect}");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void ShowNetworkInterfaces()
        {
            Console.WriteLine();
            Console.WriteLine("network interfaces");

            try
            {

                IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                Console.WriteLine($"Interface information for {computerProperties.HostName}.{computerProperties.DomainName}");

                if (nics == null || nics.Length < 1)
                    throw new Exception("No network interfaces found.");

                Console.WriteLine("Number of interfaces .................... : {0}", nics.Length);
                foreach (NetworkInterface adapter in nics)
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    Console.WriteLine();
                    Console.WriteLine(adapter.Description);
                    Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                    Console.WriteLine("Interface type .......................... : {0}", adapter.NetworkInterfaceType);
                    Console.WriteLine("Physical Address ........................ : {0}", adapter.GetPhysicalAddress().ToString());
                    Console.WriteLine("Operational status ...................... : {0}", adapter.OperationalStatus);

                    string versions = "";
                    // Create a display string for the supported IP versions.
                    if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                        versions = "IPv4";
                    if (adapter.Supports(NetworkInterfaceComponent.IPv6))
                    {
                        if (versions.Length > 0)
                            versions += " ";
                        versions += "IPv6";
                    }
                    Console.WriteLine("IP version .............................. : {0}", versions);
                    //ShowIPAddresses(properties);

                    // The following information is not useful for loopback adapters.
                    if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    {
                        continue;
                    }
                    Console.WriteLine("DNS suffix .............................. : {0}", properties.DnsSuffix);

                    // string label;
                    if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                    {
                        IPv4InterfaceProperties ipv4 = properties.GetIPv4Properties();
                        Console.WriteLine("MTU...................................... : {0}", ipv4.Mtu);
                        if (ipv4.UsesWins)
                        {
                            IPAddressCollection winsServers = properties.WinsServersAddresses;
                            if (winsServers.Count > 0)
                            {
                                // label = "  WINS Servers ............................ :";
                                // ShowIPAddresses(label, winsServers);
                            }
                        }
                    }

                    Console.WriteLine("DNS enabled ............................. : {0}", properties.IsDnsEnabled);
                    Console.WriteLine("Dynamically configured DNS .............. : {0}", properties.IsDynamicDnsEnabled);
                    Console.WriteLine("Receive Only ............................ : {0}", adapter.IsReceiveOnly);
                    Console.WriteLine("Multicast ............................... : {0}", adapter.SupportsMulticast);
                    //ShowInterfaceStatistics(adapter);

                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void HardVaeTest()
        {
            try
            {
                HardwareInfo hardwareInfo = new HardwareInfo();

                //hardwareInfo.RefreshMemoryStatus();
                //hardwareInfo.RefreshBatteryList();
                //hardwareInfo.RefreshBIOSList();
                //hardwareInfo.RefreshCPUList();
                //hardwareInfo.RefreshDriveList();
                //hardwareInfo.RefreshKeyboardList();
                //hardwareInfo.RefreshMemoryList();
                //hardwareInfo.RefreshMonitorList();
                //hardwareInfo.RefreshMotherboardList();
                //hardwareInfo.RefreshMouseList();
                //hardwareInfo.RefreshNetworkAdapterList();
                //hardwareInfo.RefreshPrinterList();
                //hardwareInfo.RefreshSoundDeviceList();
                //hardwareInfo.RefreshVideoControllerList();

                hardwareInfo.RefreshAll();

                Console.WriteLine(hardwareInfo.MemoryStatus);

                foreach (var hardware in hardwareInfo.BatteryList)
                    Console.WriteLine(hardware);

                foreach (var hardware in hardwareInfo.BiosList)
                    Console.WriteLine(hardware);

                foreach (var cpu in hardwareInfo.CpuList)
                {
                    Console.WriteLine(cpu);

                    foreach (var cpuCore in cpu.CpuCoreList)
                        Console.WriteLine(cpuCore);
                }

                foreach (var drive in hardwareInfo.DriveList)
                {
                    Console.WriteLine(drive);

                    foreach (var partition in drive.PartitionList)
                    {
                        Console.WriteLine(partition);

                        foreach (var volume in partition.VolumeList)
                            Console.WriteLine(volume);
                    }
                }

                Console.ReadLine();

                foreach (var hardware in hardwareInfo.KeyboardList)
                    Console.WriteLine(hardware);

                foreach (var hardware in hardwareInfo.MemoryList)
                    Console.WriteLine(hardware);

                foreach (var hardware in hardwareInfo.MonitorList)
                    Console.WriteLine(hardware);

                foreach (var hardware in hardwareInfo.MotherboardList)
                    Console.WriteLine(hardware);

                foreach (var hardware in hardwareInfo.MouseList)
                    Console.WriteLine(hardware);

                foreach (var hardware in hardwareInfo.NetworkAdapterList)
                    Console.WriteLine(hardware);

                foreach (var hardware in hardwareInfo.PrinterList)
                    Console.WriteLine(hardware);

                foreach (var hardware in hardwareInfo.SoundDeviceList)
                    Console.WriteLine(hardware);

                foreach (var hardware in hardwareInfo.VideoControllerList)
                    Console.WriteLine(hardware);

                Console.ReadLine();

                foreach (var address in HardwareInfo.GetLocalIPv4Addresses(NetworkInterfaceType.Ethernet, OperationalStatus.Up))
                    Console.WriteLine(address);

                Console.WriteLine();

                foreach (var address in HardwareInfo.GetLocalIPv4Addresses(NetworkInterfaceType.Wireless80211))
                    Console.WriteLine(address);

                Console.WriteLine();

                foreach (var address in HardwareInfo.GetLocalIPv4Addresses(OperationalStatus.Up))
                    Console.WriteLine(address);

                Console.WriteLine();

                foreach (var address in HardwareInfo.GetLocalIPv4Addresses())
                    Console.WriteLine(address);

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task CheckCpuUsageForThisProcess()
        {

            try
            {
                var startTime = DateTime.UtcNow;
                var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
                await Task.Delay(500);

                var endTime = DateTime.UtcNow;
                var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
                var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
                var totalMsPassed = (endTime - startTime).TotalMilliseconds;
                var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
                var cpu = cpuUsageTotal * 100;

                Console.WriteLine();
                Console.WriteLine("check cpu this process");
                Console.WriteLine($"cpu: {cpu}");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void GetServiseHealthAsync()
        {
            try
            {
                ServiceController sc = new ServiceController();

                //if (sc.ServiceName.ToLower().Contains("nssm.exe");
                //    throw new Exception("обнаружен запуск через nssm.exe");

                //result.Running = sc.Status == ServiceControllerStatus.Running;
                //ManagementObject service = new ManagementObject(@"Win32_service.Name='" + sc.ServiceName + "'");
                //object processIdObject = service.GetPropertyValue("ProcessId");
                //int processId = (int)((UInt32)processIdObject);
                //using Process process = Process.GetProcessById(processId);

                //using PerformanceCounter theCPUCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
                //var cpuValue = theCPUCounter.NextValue();

                //using PerformanceCounter workungSetPrivate = new PerformanceCounter("Process", "Working Set - Private", process.ProcessName);
                //var workungSetPrivateValue = workungSetPrivate.NextValue();

                //result.MemoryUsed = ByteConvert(workungSetPrivateValue)
                //result.CpuUsed = $"{cpuValue}%";
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// byte parser to str
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        private string ByteConvert(double value, int decimalPlaces = 1)
        {
            if (value < 0) { return "-" + ByteConvert(-value); }

            int i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }
    }
}
