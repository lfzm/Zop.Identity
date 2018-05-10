using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
namespace Zop.Domain.Entities
{
    /// <summary>
    /// Api 资源
    /// </summary>
    [Serializable]
    public class ApiResource : AggregateConcurrencySafe<int>
    {
        #region 构造函数
        public ApiResource()
        {

        }
        /// <summary>
        /// 初始化<see cref="ApiResource"/>
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="claimTypes">声明类型</param>
        public ApiResource(string name, IList<string> claimTypes)
            : this(name, name, claimTypes)
        {
        }
        /// <summary>
        /// 初始化<see cref="ApiResource"/>
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="claimTypes">声明类型</param>
        public ApiResource(string name, string displayName, IList<string> claimTypes)
        {
            if (name.IsNull()) throw new ArgumentNullException(nameof(name));

            Name = name;
            DisplayName = displayName;

            Scopes.Add(new ApiScope(name, displayName));

            if (claimTypes != null && !claimTypes.Any())
            {
                foreach (var type in claimTypes)
                {
                    this.SetUserClaims(s => s.SetValue(type));
                }
            }
        }
        [JsonConstructor]
        private ApiResource(string userClaims, IList<Secret> secrets, IList<ApiScope> scopes)
        {
            this.UserClaims = userClaims;
            if (secrets != null) this.Secrets = secrets;
            if (scopes != null) this.Scopes = scopes;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enabled { get; set; } = true;
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
        /// 访问令牌中应包含的用户声明类型列表
        /// </summary>
        [MaxLength(2000)]
        public string UserClaims { get; private set; }
        /// <summary>
        /// 密钥
        /// </summary>
        [CollectionCount(minCount:1)]
        public IList<Secret> Secrets { get; private set; } = new List<Secret>();
        /// <summary>
        /// 包含范围
        /// </summary>
        [CollectionCount(minCount: 1)]
        public IList<ApiScope> Scopes { get; private set; } = new List<ApiScope>();
        #endregion

        /// <summary>
        /// 获取访问令牌中应包含的用户声明类型列表
        /// </summary>
        public List<string> GetUserClaims()
        {
            return this.UserClaims.GetList();
        }
        /// <summary>
        /// 设置令牌中应包含的用户声明类型列表
        /// </summary>
        /// <param name="callback"></param>
        public void SetUserClaims(Func<string, string> callback)
        {
            this.UserClaims = callback?.Invoke(this.UserClaims);
        }


    }
}
