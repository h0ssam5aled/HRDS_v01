namespace HRDS.Web.Areas.Finance.ViewModels
{
    public class CostCenterViewModel
    {
        public int CostCenterId { get; set; }
        public int? ParentCostCenterId { get; set; }
        public string? ParentCostCenterName { get; set; }
        public string CostCenterCode { get; set; } = null!;
        public string CostCenterNameAr { get; set; } = null!;
        public string? CostCenterNameEn { get; set; }
        public string DisplayCostCenterName { get; set; } = null!;
        public int? CompanyId { get; set; }
        public int? CompanyBranchId { get; set; }
        public byte CostCenterLevel { get; set; }
        public bool IsLeaf { get; set; }
        public string? HierarchyPath { get; set; }
        public bool IsActive { get; set; }
    }
}