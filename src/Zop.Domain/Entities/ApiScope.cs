using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Zop.Domain.Entities
{
    /// <summary>
    /// Api 范围
    /// </summary>
    [Serializable]
    public class ApiScope : Entity<int>
    {
        #region 构造函数
        /// <summary>
        /// 初始化<see cref ="ApiScope"/>类的新实例
        /// </summary>
        public ApiScope()
        {
        }
        /// <summary>
        /// 初始化<see cref ="ApiScope"/>类的新实例
        /// </summary>
        /// <param name="name">范围名称.</param>
        public ApiScope(string name)
            : this(name, name, null)
        {

        }
        /// <summary>
        /// 初始化<see cref ="ApiScope"/>类的新实例
        /// </summary>
        /// <param name="name">范围名称.</param>
        /// <param name="displayName">显示名称.</param>
        public ApiScope(string name, string displayName)
              : this(name, displayName, null)
        {


        }
        /// <summary>
        /// 初始化<see cref ="ApiScope"/>类的新实例
        /// </summary>
        /// <param name="name">范围名称.</param>
        /// <param name="displayName">显示名称.</param>
        /// <param name="userClaims">用户声明</param>
        [JsonConstructor]
        private ApiScope(string name, string displayName, string userClaims)
        {
            if (name.IsNull()) throw new ArgumentNullException(nameof(name));
            this.Name = name;
            this.DisplayName = displayName;
            this.UserClaims = userClaims;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 范围名称. 客户端用来请求范围.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; private set; }
        /// <summary>
        ///显示名称. 用于显示在同意页面上
        /// </summary>
        [MaxLength(200)]
        public string DisplayName { get; set; }
        /// <summary>
        /// 描述. 用于显示在同意页面上
        /// </summary>
        [MaxLength(1000)]
        public string Description { get; set; }
        /// <summary>
        /// 是否可以在同意页面上取消选择范围。 默认为false。
        /// </summary>
        public bool Required { get; set; } = false;
        /// <summary>
        /// 是否在同意页面强调此范围。 默认为false。
        /// </summary>
        public bool Emphasize { get; set; } = false;
        /// <summary>
        /// 指定此范围是否显示在发现文档中。 默认为true。
        /// </summary>
        public bool ShowInDiscoveryDocument { get; set; } = true;
        /// <summary>
        /// 访问令牌中应包含的用户声明类型列表
        /// </summary>
        [MaxLength(2000)]
        public string UserClaims { get; private set; }
        #endregion

        /// <summary>
        /// 设置令牌中应包含的用户声明类型列表
        /// </summary>
        /// <param name="callback"></param>
        public void SetUserClaims(Func<string,string> callback)
        {
            this.UserClaims = callback?.Invoke(this.UserClaims);
        }
        /// <summary>
        /// 获取访问令牌中应包含的用户声明类型列表
        /// </summary>
        public List<string> GetUserClaims()
        {
            return this.UserClaims.GetList();
        }
    }

}
