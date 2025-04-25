using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System.Reflection;
using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ACS.AMS.DAL.DBContext
{
    public partial class AMSContext
    {
        public static string ConnectionString { get; set; }
        public static int CurrentPageID { get; set; } = 100;
        public IConfiguration Configuration { get; }
        
        public static AMSContext CreateNewContext()
        {
            var contextOptions = new DbContextOptionsBuilder<AMSContext>().UseSqlServer(ConnectionString).Options;
            var db = new AMSContext(contextOptions);
            
            db.AssignCreatedModifiedDetails = true;
            db.CurrentUserID = 1;
           
            if ((SessionUserHelper.IsAuthenticated))
            {
                if (SessionUserHelper.UserID > 0)
                {
                    db.CurrentUserID = SessionUserHelper.UserID;
                }
                else
                {
                    db.CurrentUserID = 1;
                }
            }
            else
            {
                db.CurrentUserID = 1;
            }

            return db;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();

            optionsBuilder.LogTo(m => LogData(m));//,
                //(eventId, logLevel) => logLevel >= LogLevel.Information
                //                   || eventId == RelationalEventId.ConnectionOpened
                //                   || eventId == RelationalEventId.ConnectionClosed
                //                   || eventId == RelationalEventId.CommandExecuted
                //                   );

            base.OnConfiguring(optionsBuilder);
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
        public class BlankTriggerAddingConvention : IModelFinalizingConvention
        {
            public virtual void ProcessModelFinalizing(
                IConventionModelBuilder modelBuilder,
                IConventionContext<IConventionModelBuilder> context)
            {
                foreach (var entityType in modelBuilder.Metadata.GetEntityTypes())
                {
                    var table = StoreObjectIdentifier.Create(entityType, StoreObjectType.Table);
                    if (table != null
                        && entityType.GetDeclaredTriggers().All(t => t.GetDatabaseName(table.Value) == null))
                    {
                        entityType.Builder.HasTrigger(table.Value.Name + "_Trigger");
                    }

                    foreach (var fragment in entityType.GetMappingFragments(StoreObjectType.Table))
                    {
                        if (entityType.GetDeclaredTriggers().All(t => t.GetDatabaseName(fragment.StoreObject) == null))
                        {
                            entityType.Builder.HasTrigger(fragment.StoreObject.Name + "_Trigger");
                        }
                    }
                }
            }
        }
        public static bool EnableDBAuditLog { get; set; }
        public static bool EnableQueryLog { get; set; } = false;
        public static string QueryLogLocation { get; set; } = "";

        public bool EnableInstanceQueryLog { get; set; } = false;

        private void LogData(string value)
        {
            if ((EnableQueryLog) || (EnableInstanceQueryLog))
            {
                var dir = QueryLogLocation;
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                File.AppendAllText(Path.Combine(QueryLogLocation, $"qry_{DateTime.Now.Hour}_{DateTime.Now.Minute}.txt"), value);
            }
        }

        public bool AssignCreatedModifiedDetails { get; set; }

        public int CurrentUserID { get; set; }

        internal bool AuditLogIsInProcess { get; set; }
        protected virtual int IsObjectDeleted { get; set; }

        public override int SaveChanges()
        {
            EnableDBAuditLog = AppConfigurationManager.GetValue<bool>(AppConfigurationManager.EnableAuditLog);
            AuditLogTransactionTable trans = new AuditLogTransactionTable();

            string currentSessionID = "";
            string URL = "";
            currentSessionID = "xxx"; //HttpContext.Session.id;
            ////if ((System.Web.HttpContext.Current != null) && (System.Web.HttpContext.Current.Session != null))
            ////    currentSessionID = System.Web.HttpContext.Current.Session.SessionID;

            trans.AuditLogTransactionDateTime = DateTime.Now;
            trans.SessionID = currentSessionID;
            trans.URL = URL;
            trans.UserID = 1;

            ////if ((System.Web.HttpContext.Current != null) && (System.Web.HttpContext.Current.Request != null) && (System.Web.HttpContext.Current.Request.Url != null))
            ////    trans.URL = HttpContext.Current.Request.Url.ToString();
            ////else
            ////    trans.Url = URL;

            if (SessionUserHelper.IsAuthenticated)
            {
                if (SessionUserHelper.UserID <= 0)
                    trans.UserID = 0;//HHTUserModel.UserID;
                else
                    trans.UserID = SessionUserHelper.UserID;
            }
            else
            {
                
                trans.UserID = 1;
            }

            //var checkedItems = ChangeTracker.Entries().Where(p => p.State == System.Data.Entity.EntityState.Added).ToList();

            var items = ChangeTracker.Entries<BaseEntityObject>().Where(p => p.State == EntityState.Added).ToList();
            foreach (var itm in items.Select(x => x.Entity).ToList())
            {
                itm.BeforeSave(this);
            }
            ValidateEventArgs<AMSContext> args = new ValidateEventArgs<AMSContext>(this, CreateNewContext(), EntityObjectState.New);
            ValidateObjects(items.Select(x => x.Entity).ToList(), args);

            var xx = this.ChangeTracker.Entries().Where(p => p.State == EntityState.Modified).ToList();
            var modifieditms = this.ChangeTracker.Entries<BaseEntityObject>().Where(p => p.State == EntityState.Modified).ToList();
            foreach (var itm in modifieditms.Select(x => x.Entity).ToList())
            {
                itm.BeforeSave(this);
            }
            args = new ValidateEventArgs<AMSContext>(this, CreateNewContext(), EntityObjectState.Edit);
            ValidateObjects(modifieditms.Select(x => x.Entity).ToList(), args);
            foreach (var modifiedData in modifieditms)
            {
                if (BaseEntityObject.auditLogNotRequiredTables.Contains(modifiedData.Entity.GetType().Name))
                    continue;
                var primaryKey = modifiedData.Metadata.FindPrimaryKey();
                var primaykeyID = string.Join(',', primaryKey.Properties.Select(prop => prop.PropertyInfo.GetValue(modifiedData.Entity)));

                foreach (var prop in modifiedData.Properties)
                {
                    if (BaseEntityObject.auditLogNotRequiredProperies.Contains(prop.Metadata.GetColumnName()))
                        continue;
                    var originalValue = (modifiedData.OriginalValues == null) ? null : JsonConvert.SerializeObject(prop.OriginalValue);
                    var currentValue = (modifiedData.CurrentValues == null) ? null : JsonConvert.SerializeObject(prop.CurrentValue);

                    if (originalValue != currentValue) //Only create a log if the value changes
                    {
                        modifiedData.Entity.AuditChange(prop.Metadata.GetColumnName(), originalValue, currentValue, primaykeyID);
                    };
                }

            }

            if (EnableDBAuditLog)// if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.EnableDBAuditLog))
            {
                PrepareAuditLog(modifieditms, 2, trans);
            }
            var deleteditms = this.ChangeTracker.Entries<BaseEntityObject>().Where(p => p.State == EntityState.Deleted).ToList();
            args = new ValidateEventArgs<AMSContext>(this, CreateNewContext(), EntityObjectState.Delete);
            ValidateObjects(deleteditms.Select(x => x.Entity).ToList(), args);
            bool transactionTableAdded = false;

            if (EnableDBAuditLog)// if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.EnableAuditLog))
            {
                PrepareAuditLog(deleteditms, 3, trans);
                if ((!transactionTableAdded) && (trans.AuditLogTable.Count > 0))
                {
                    this.AuditLogTransactionTable.Add(trans);
                    transactionTableAdded = true;
                }
            }

            base.SaveChanges();
            // save the inserted transaction IDs
            if (EnableDBAuditLog) //(AppConfigurationManager.GetValue<bool>(AppConfigurationManager.EnableAuditLog))
            {
                PrepareAuditLog(items, 1, trans);
                if ((!transactionTableAdded) && (trans.AuditLogTable.Count > 0))
                {
                    this.AuditLogTransactionTable.Add(trans);
                }
            }

            //foreach (var entity in this.ChangeTracker.Entries())
            //{
            //    if (entity.State == EntityState.Detached || entity.State == EntityState.Unchanged)
            //    {
            //        continue;
            //    }

            //    var audits = new List<Audit>();

            //    //the typeId is a string representing the primary keys of this entity.
            //    //this will not be available for ADDED entities with generated primary keys, so we need to update those later
            //    string typeId;
            //    string primaryname;
            //    if (entity.State == EntityState.Added && entity.Properties.Any(prop => prop.Metadata.IsPrimaryKey() && prop.IsTemporary))
            //    {
            //        typeId = null;
            //    }
            //    else
            //    {
            //        var primaryKey = entity.Metadata.FindPrimaryKey();
            //        typeId = string.Join(',', primaryKey.Properties.Select(prop => prop.PropertyInfo.GetValue(entity.Entity)));

            //    }

            //    //record an audit for each property of each entity that has been changed
            //    foreach (var prop in entity.Properties)
            //    {
            //        ////don't audit anything about primary keys (those can't change, and are already in the typeId)
            //        if (prop.Metadata.IsPrimaryKey() && entity.Properties.Any(p => !p.Metadata.IsPrimaryKey()))
            //        {
            //            continue;
            //        }

            //        ////ignore values that won't actually be written
            //        //if (entity.State != EntityState.Deleted && entity.State != EntityState.Added && prop.Metadata.AfterSaveBehavior != PropertySaveBehavior.Save)
            //        //{
            //        //    continue;
            //        //}

            //        ////ignore values that won't actually be written
            //        //if (entity.State == EntityState.Added && prop.Metadata.BeforeSaveBehavior != PropertySaveBehavior.Save)
            //        //{
            //        //    continue;
            //        //}

            //        //ignore properties that didn't change
            //        if (entity.State == EntityState.Modified && !prop.IsModified)
            //        {
            //            continue;
            //        }

            //        var audit = new Audit
            //        {
            //            Action = (int)entity.State,
            //            TypeId = typeId,
            //            ColumnName = prop.Metadata.GetColumnName(), //prop.Metadata.SqlServer().ColumnName,
            //            OldValue = (entity.State == EntityState.Added || entity.OriginalValues == null) ? null : JsonConvert.SerializeObject(prop.OriginalValue),
            //            NewValue = entity.State == EntityState.Deleted ? null : JsonConvert.SerializeObject(prop.CurrentValue)
            //        };
            //    }

            //    //Do something with audits
            //}

            return base.SaveChanges();
            //foreach (var itm in modifiedEntities) itm.BeforeSave(this);
            //ValidateEventArgs<FASOFTModel> args = new ValidateEventArgs<FASOFTModel>(this, CreateNewContext(), EntityObjectState.New);
            //ValidateObjects(modifiedEntities, args);
        }


        private void HandleChangeTracking()
        {
            foreach (var entity in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                //UpdateTrackedEntity(entity);
            }
        }

        private void ValidateObjects(IList<BaseEntityObject> items, ValidateEventArgs<AMSContext> args)
        {
            foreach (var itm in items)
            {
                if (args.State == EntityObjectState.New)
                {
                    if (!itm.ValidateNewObject(args))
                    {
                        //
                        if (string.IsNullOrEmpty(args.ErrorMessage))
                            args.ErrorMessage = "Unknown error occurred in " + itm.ToString();

                        throw new ValidationException(args.ErrorMessage, itm, args.FieldName);
                    }
                }

                if (args.State == EntityObjectState.Edit)
                {
                    int statusID = (int)StatusValue.Active;

                    var Status = GetProperty(itm, "StatusID");
                    if (Status != null)
                        statusID = (int)Status.GetValue(itm);

                    if (statusID == (int)StatusValue.Deleted)
                    {
                        if (!itm.ValidateDeleteObject(args))
                        {
                            //
                            if (string.IsNullOrEmpty(args.ErrorMessage))
                                args.ErrorMessage = "Unknown error occurred in " + itm.ToString();

                            throw new ValidationException(args.ErrorMessage, itm, args.FieldName);
                        }
                    }
                    else
                    {
                        if (!itm.ValidateUpdateObject(args))
                        {
                            //
                            if (string.IsNullOrEmpty(args.ErrorMessage))
                                args.ErrorMessage = "Unknown error occurred in " + itm.ToString();

                            throw new ValidationException(args.ErrorMessage, itm, args.FieldName);
                        }
                    }
                }
            }

            foreach (var itm in items)
            {
                if (args.NewDB.AssignCreatedModifiedDetails)  //if (AssignCreatedModifiedDetails)
                {
                    if (args.State == EntityObjectState.New)
                    {
                        //check the created by & created date properties
                        var properties = itm.GetType().GetProperties();
                        var user = GetProperty(itm, "CreatedBy");
                        if (user != null)
                        {
                            var createdBy = itm.GetFieldValue("CreatedBy");
                            if (string.IsNullOrEmpty(createdBy + "") || createdBy.ToString() == "0")
                                user.SetValue(itm, args.NewDB.CurrentUserID, null);
                        }

                        var date = GetProperty(itm, "CreatedDatetime");
                        if (date != null)
                        {
                            date.SetValue(itm, DateTime.Now, null);
                        }

                        var Status = GetProperty(itm, "StatusID");
                        if (Status != null)
                        {
                            var StatusID = itm.GetFieldValue("StatusID");
                            if (string.IsNullOrEmpty(StatusID + "") || StatusID.ToString() == "0")
                                Status.SetValue(itm, (int)StatusValue.Active, null);
                        }
                    }

                    if (args.State == EntityObjectState.Edit)
                    {
                        //check the created by & created date properties
                        var properties = itm.GetType().GetProperties();
                        var user = GetProperty(itm, "LastModifyby");
                        if (user != null)
                        {
                            var LastModifiedBy = itm.GetFieldValue("LastModifyby");
                            if (string.IsNullOrEmpty(LastModifiedBy + "") || LastModifiedBy.ToString() == "0")
                                user.SetValue(itm, args.NewDB.CurrentUserID, null);
                        }
                        var user1 = GetProperty(itm, "LastModifiedBy");
                        if (user1 != null)
                        {
                            var LastModifiedBy = itm.GetFieldValue("LastModifiedBy");
                            if (string.IsNullOrEmpty(LastModifiedBy + "") || LastModifiedBy.ToString() == "0")
                                user1.SetValue(itm, args.NewDB.CurrentUserID, null);
                        }

                        //user.SetValue(itm, CurrentUserID, null);

                        var date = GetProperty(itm, "LastModifieddate");
                        if (date != null)
                            date.SetValue(itm, DateTime.Now, null);


                        var date1 = GetProperty(itm, "LastModifiedDateTime");
                        if (date1 != null)
                            date1.SetValue(itm, DateTime.Now, null);
                    }

                    if (args.State == EntityObjectState.Delete)
                    {
                        if (!itm.ValidateDeleteObject(args))
                        {
                            if (string.IsNullOrEmpty(args.ErrorMessage))
                                args.ErrorMessage = "Unknown error occurred in " + itm.ToString();

                            throw new ValidationException(args.ErrorMessage, itm, args.FieldName);
                        }
                    }

                }
            }
        }

        private PropertyInfo GetProperty(object obj, string propertyName)
        {
            var properties = obj.GetType().GetProperties();
            var prop = (from b in properties
                        where string.Compare(b.Name, propertyName, true) == 0 && b.CanWrite == true
                        select b).FirstOrDefault();

            return prop;
        }

        private void PrepareAuditLog(IList<EntityEntry<BaseEntityObject>> items, short actionType, AuditLogTransactionTable trans)
        {
            try
            {
                AuditLogIsInProcess = true;
                foreach (var obj in items)
                {
                    if (obj != null)
                    {
                        // object Value = obj.GetFieldValue("IsDeleted");
                        var Value = obj.Entity.GetFieldValue("StatusID");
                        if (Value != null)
                            IsObjectDeleted = (int)Value;

                        if (IsObjectDeleted == (int)StatusValue.Deleted)
                        {
                            obj.Entity.CreateAuditLogs(this, 3, trans, obj);
                        }
                        else
                        {
                            obj.Entity.CreateAuditLogs(this, actionType, trans, obj);
                        }
                    }
                }
            }
            finally
            {
                AuditLogIsInProcess = false;
            }
        }
    }
}

