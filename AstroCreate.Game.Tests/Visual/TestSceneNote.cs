using NUnit.Framework;
using osu.Framework.Graphics;
using osuTK;

namespace AstroCreate.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneNote : AstroCreateTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.

        public TestSceneNote()
        {
            Add(new Note(null)
            {
                Anchor = Anchor.Centre,
                Position = new Vector2(-250, 0)
            });

            Add(new Note(new NoteInfo
            {
                Type = NoteInfo.NoteType.EACH
            })
            {
                Anchor = Anchor.Centre
            });

            Add(new Note(new NoteInfo
            {
                Type = NoteInfo.NoteType.BREAK
            })
            {
                Anchor = Anchor.Centre,
                Position = new Vector2(250, 0)
            });
        }
    }
}
