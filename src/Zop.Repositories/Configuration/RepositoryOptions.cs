
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Repositories.Configuration
{
    public class RepositoryOptions
    {

        /// <summary>
        /// Callback to configure the EF DbContext.
        /// </summary>
        /// <value>
        /// The configure database context.
        /// </value>
        public Action<DbContextOptionsBuilder> DbContextOptions { get; set; }

        /// <summary>
        /// 表名称配置
        /// </summary>
        public TableConfiguration TableConfig { get; set; } = new TableConfiguration();
    }
}
