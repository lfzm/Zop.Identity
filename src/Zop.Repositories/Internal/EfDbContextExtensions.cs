using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zop.Domain.Entities;
using Zop.Repositories.ChangeDetector;

namespace Zop.Repositories
{
    public static class EFDbContextExtensions
    {
        /// <summary>
        /// 删除实体
        /// </summary>
        public static void Delete(this DbContext context, object entity)
        {
            if (entity == null)
                throw new RepositoryDataException("Delete entity Can not be empty");

            var Properties = entity.GetType().GetProperties()
               .Where(f => typeof(IEntity).IsAssignableFrom(f.PropertyType)).ToList();
            if (Properties.IsNullOrAuy())
            {
                foreach (var item in Properties)
                {
                    var value = item.GetValue(entity);
                    Delete(context, value);
                }
            }

            //获取所有的继承IList的属性
            Properties = entity.GetType().GetProperties()
                  .Where(f => f.PropertyType.FullName.Contains("System.Collections.Generic.IList`1")
                  && typeof(IEntity).IsAssignableFrom(f.PropertyType.GenericTypeArguments[0])).ToList();

            if (Properties.IsNullOrAuy())
            {
                foreach (var item in Properties)
                {
                    var values = item.GetValue(entity);
                    if (values == null)
                        continue;
                    foreach (var value in ((IList)values))
                    {
                        Delete(context, value);
                    }
                }
            }
            //标记删除状态
            context.Remove(entity);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        public static void Update(this DbContext context, IChangeManager changeManager, object entry)
        {
            if (entry == null)
                throw new RepositoryDataException("Update newestEntity and originalEntity Can not be empty");

            //清除跟踪器
            context.ClearChangeTracker();
            //删除
            var removes = changeManager.GetChangers(ChangeEntryType.Remove);
            if (removes.IsNullOrAuy())
            {
                foreach (var remove in removes)
                {
                    context.Delete(remove.OriginalEntry);
                }
            }
            //修改和添加
            context.ChangeTracker.TrackGraph(entry, e =>
            {
                e.Entry.State = EntityState.Unchanged;
                var targetType = e.Entry.Entity.GetType();
                bool isTransient = (bool)targetType.GetProperties().First(f => f.Name == "IsTransient")?.GetValue(e.Entry.Entity);
                if (isTransient)
                {
                    e.Entry.State = EntityState.Added;
                }
                else
                {
                    //获取ID（唯一标示）
                    var id = targetType.GetProperties().Where(f => f.Name == "Id" && f.PropertyType.IsValueType).FirstOrDefault()?.GetValue(e.Entry.Entity);
                    ChangeEntry change = changeManager.GetChanger(targetType, id);
                    if (change == null)
                        return;

                    foreach (var item in change.ChangePropertys)
                    {
                        e.Entry.Member(item.Name).IsModified = true;
                    }
                }

            });

        }


        private static void ClearChangeTracker(this DbContext context)
        {
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            //清除所有的快照
            foreach (var item in context.ChangeTracker.Entries().ToList())
            {
                item.State = EntityState.Detached;
            }

        }
    }
}
