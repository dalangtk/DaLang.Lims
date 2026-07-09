using DaLang.Lims.Web.Common.Helpers;

namespace DaLang.Lims.GetMachineCode;

internal class Program
{
    static void Main(string[] args)
    {
        var machineCode = MachineHelper.GetMachineCode();

        Console.WriteLine(machineCode);

        Console.ReadKey();
    }
}
