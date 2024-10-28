using ProjectCRUD.Data;
using System.Security.Permissions;

namespace Pokemon_Review_API.Authorization
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =false)]
    public class CheckPermissionAttribute:Attribute
    {
        public CheckPermissionAttribute(premission premission )
        {
            this.premission = premission;
        }
        public premission premission { get;  }
    }
}
