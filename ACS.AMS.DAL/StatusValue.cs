using System;

namespace ACS.AMS.DAL
{
    public enum StatusValue
    {
        Draft = 10,

        Active = 1,

        Inactive = 100,

        WaitingForApproval = 150,

        Rejected = 200,

        Disposed = 250,

        Deleted = 500,

        DeletedOLD = 3,
        ReApproval=550,

    }

    public enum TransactionTypeValue
    {
        AssetTransfer = 5,
        AssetRetirement = 10,
        InternlaAssetTransfer=11,
        AssetMaintenance = 15,
        AssetMaintenanceRequest=16,
        AMCSchedule=21
    }
}