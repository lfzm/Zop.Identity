using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Zop.Domain.Entities
{
    /// <summary>
    /// 身份认证资源
    /// </summary>
    [Serializable]
    public class IdentityResource : AggregateConcurrencySafe<int>
    {
        #region 构造函数
        /// <summary>
        /// 初始化 <see cref="IdentityResource"/> 
        /// </summary>
        public IdentityResource()
        {
        }

        /// <summary>
        /// 初始化 <see cref="IdentityResource"/> 
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="claimTypes">The claim types.</param>
        public IdentityResource(string name)
            : this(name, name, null)
        {
        }

        /// <summary>
        /// 初始化 <see cref="IdentityResource"/> 
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="claimTypes">The claim types.</param>
        /// <exception cref="System.ArgumentNullException">name</exception>
        [JsonConstructor]
        private IdentityResource(string name, string displayName, string claimTypes)
        {
            if (name.IsNull()) throw new ArgumentNullException(nameof(name));

            this.Name = name;
            this.DisplayName = displayName;
            this.UserClaims = claimTypes;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        [MaxLength(200)]
        public string DisplayName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(1000)]
        public string Description { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enabled { get; set; } = true;
        /// <summary>
        /// 访问令牌中应包含的用户声明类型列表
        /// </summary>
        [MaxLength(2000)]
        public string UserClaims { get; private set; }
        /// <summary>
        /// 是否可以在同意页面上取消选择范围。 默认为false。
        /// </summary>
        public bool Required { get; set; } = false;
        /// <summary>
        /// 是否在同意页面强调此范围。 默认为false。
        /// </summary>
        public bool Emphasize { get; set; } = false;
        /// <summary>
        /// 是否显示在发现文档中
        /// </summary>
        public bool ShowInDiscoveryDocument { get; set; } = true;
        #endregion

        /// <summary>
        /// 获取访问令牌中应包含的用户声明类型列表
        /// </summary>
        public List<string> GetUserClaims()
        {
            return this.UserClaims.GetList();
        }
        /// <summary>
        /// 设置访问令牌中应包含的用户声明类型列表
        /// </summary>
        /// <param name="callback"></param>
        public void SetUserClaims(Func<string, string> callback)
        {
            this.UserClaims = callback?.Invoke(this.UserClaims);
        }
    }
}
