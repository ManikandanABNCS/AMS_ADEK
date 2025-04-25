using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ASelectionControlQueryTable
{
    public static ASelectionControlQueryTable GetItem(AMSContext _db, string controlName)
    {
        return (from b in _db.ASelectionControlQueryTable
                where b.ControlName == controlName
                select b).FirstOrDefault();
    }
}