using osu.Framework.Testing;

namespace AstroCreate.Game.Tests.Visual
{
    public partial class AstroCreateTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new AstroCreateTestSceneTestRunner();

        private partial class AstroCreateTestSceneTestRunner : AstroCreateGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }
    }
}
