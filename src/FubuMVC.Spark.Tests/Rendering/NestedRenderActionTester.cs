﻿using System.IO;
using FubuMVC.Spark.Rendering;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Spark.Tests.Rendering
{
    [TestFixture]
    public class NestedRenderActionTester : InteractionContext<NestedRenderAction>
    {
        private IFubuSparkView _view;

        protected override void beforeEach()
        {
            var writer = new StringWriter();

            var nestedOutput = new NestedOutput();
            nestedOutput.SetWriter(() => writer);

            _view = MockFor<IFubuSparkView>();

            var viewFactory = MockFor<IViewFactory>();
            viewFactory.Stub(x => x.GetView()).Return(_view);

            _view.Expect(x => x.RenderView(nestedOutput.Writer));

            Services.Inject(nestedOutput);
            Services.Inject(viewFactory);

        }
        [Test]
        public void renders_the_view_from_the_factory_using_the_nested_output_writer()
        {
            ClassUnderTest.Render();
            _view.VerifyAllExpectations();
        }
    }
}