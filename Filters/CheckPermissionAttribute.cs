using Api.Model;

namespace Api.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CheckPermissionAttribute : Attribute
    {
        public Permision _permision { get; }

        public CheckPermissionAttribute(Permision permision)
        {
            _permision = permision;
        }
    }
}
