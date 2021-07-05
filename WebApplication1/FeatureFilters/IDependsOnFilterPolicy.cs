using Polly;

namespace WebApplication1.FeatureFilters
{
    public interface IDependsOnFilterPolicy
    {
        public IAsyncPolicy<bool> GetPolicy(bool shouldBeEnabled);
    }
}