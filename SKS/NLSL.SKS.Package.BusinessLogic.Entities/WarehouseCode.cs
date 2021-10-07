using System.Diagnostics.CodeAnalysis;

namespace NLSL.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class WarehouseCode
    {
        public string Code
        {
            get;
            init;
        }

        public WarehouseCode(string code)
        {
            Code = code;
        }
    }
}