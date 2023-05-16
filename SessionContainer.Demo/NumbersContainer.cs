using System.ComponentModel;

namespace SessionContainer.Demo
{
    public class NumbersContainer : SessionContainer
    {
        public List<int> Numbers { get; set; } = new List<int>() { };

        public NumbersContainer(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
    }
}
