using Polly;

namespace WebApplication1.FeatureFilters
{
    public class DependsOnFilterPolicy : IDependsOnFilterPolicy
    {
        public IAsyncPolicy<bool> GetPolicy(bool shouldBeEnabled)
        {
            //return Policy<bool>.Handle<DependsOnFilterException>().FallbackAsync(shouldBeEnabled);
            return Policy.NoOpAsync<bool>();
        }
    }
}