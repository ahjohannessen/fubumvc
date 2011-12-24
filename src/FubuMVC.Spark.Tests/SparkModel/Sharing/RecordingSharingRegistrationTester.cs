using System.Linq;
using Bottles.Diagnostics;
using FubuMVC.Spark.SparkModel.Sharing;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Spark.Tests.SparkModel.Sharing
{
    [TestFixture]
    public class RecordingSharingRegistrationTester : InteractionContext<RecordingSharingRegistration>
    {
        private string[] _sharings;
        protected override void beforeEach()
        {
            var graph = new SharingGraph();
            ClassUnderTest.Global("Global1");
            ClassUnderTest.Global("Global2");
            ClassUnderTest.Dependency("Pak1", "Dependency1");
            ClassUnderTest.Dependency("Pak1", "Dependency2");
            ClassUnderTest.Apply(new PackageLog(), graph);
            graph.CompileDependencies("Pak1");
            _sharings = graph.SharingsFor("Pak1").ToArray();
        }

        [Test]
        public void sharings_are_applied_against_the_graph()
        {
            _sharings.ShouldHaveCount(4);
        }

        [Test]
        public void globals_are_applied_against_the_graph()
        {
            _sharings.ShouldContain("Global1");
            _sharings.ShouldContain("Global2");
        }

        [Test]
        public void dependencies_are_applied_against_the_graph()
        {
            _sharings.ShouldContain("Dependency1");
            _sharings.ShouldContain("Dependency2");
        }
    }
}