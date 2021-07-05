using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace WebApplication1.FeatureFilters
{
    [FilterAlias("DependsOn")]
    public class DependsOnFilter : IFeatureFilter
    {
        private readonly IDependsOnFilterPolicy _dependsOnFilterPolicy;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DependsOnFilter> _logger;

        public DependsOnFilter(ILogger<DependsOnFilter> logger, IHttpContextAccessor httpContextAccessor, IDependsOnFilterPolicy dependsOnFilterPolicy)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _dependsOnFilterPolicy = dependsOnFilterPolicy;
        }

        public async Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            var featureManagerSnapshot = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IFeatureManagerSnapshot>();

            var expectations = context.Parameters.GetSection("Expectations").Get<List<Expectation>>();

            if (!expectations.Any())
            {
                return true;
            }

            foreach (var expectation in expectations)
            {
                await _dependsOnFilterPolicy.GetPolicy(expectation.ShouldBeEnabled).ExecuteAsync(async () =>
                {
                    var isEnabled = await featureManagerSnapshot.IsEnabledAsync(expectation.FeatureName);
                    if (isEnabled != expectation.ShouldBeEnabled)
                    {
                        throw new DependsOnFilterException();
                    }

                    return true;
                });
            }

            return true;
        }

        public class Expectation
        {
            public string FeatureName { get; set; }

            public bool ShouldBeEnabled { get; set; }
        }
    }
}