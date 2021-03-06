using FubuMVC.Core;
using FubuMVC.IntegrationTesting.Conneg;
using FubuMVC.TestingHarness;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.IntegrationTesting.ViewEngines.Spark.NestedPartials
{
    [TestFixture]
    public class NestedPartialsIntegrationTester : FubuRegistryHarness
    {
        protected override void configure(FubuRegistry registry)
        {
            registry.Actions.IncludeType<FamilyController>();
        }

        [Test]
        public void partials_nest_deeply_1()
        {
            endpoints.Get<FamilyController>(x => x.Marcus(null)).ReadAsText().RemoveNewlines()
                .ShouldContain("<div>Marcus-><div>Silvia</div></div>");
        }

        [Test]
        public void partials_nest_deeply_2()
        {
            endpoints.Get<FamilyController>(x => x.Jack(null)).ReadAsText().RemoveNewlines()
                .ShouldContain("<div>Jack-><div>Marcus-><div>Silvia</div></div></div>");
        }

        [Test]
        public void partials_nest_deeply_3()
        {
            endpoints.Get<FamilyController>(x => x.George(null)).ReadAsText().RemoveNewlines()
                .ShouldContain("<div>George-><div>Jack-><div>Marcus-><div>Silvia</div></div></div></div>");
        }
    }
}