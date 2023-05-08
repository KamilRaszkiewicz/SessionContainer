namespace SessionContainer.Demo
{
    public class NumbersContainer : SessionContainer
    {
        public List<int> Numbers { get; set; }

        public NumbersContainer(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
    }
}
