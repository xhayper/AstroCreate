using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace AstroCreate.Game
{
    public partial class Note : CompositeDrawable
    {
        private Container note;
        public NoteInfo noteInfo;

        public Note(NoteInfo? info)
        {
            noteInfo = info ?? new NoteInfo();

            AutoSizeAxes = Axes.Both;
            Origin = Anchor.Centre;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            InternalChild = note = new Container
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new Drawable[]
                {
                    new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Texture = textures.Get(GetTexture(noteInfo.Type))
                    },
                }
            };
        }

        private string GetTexture(NoteInfo.NoteType type)
        {
            switch (type)
            {
                case NoteInfo.NoteType.EACH:
                    return "AstroDX/IMG_GAME_EACH_TAP_1";

                case NoteInfo.NoteType.BREAK:
                    return "AstroDX/IMG_GAME_BREAK_TAP_1";

                case NoteInfo.NoteType.SINGLE:
                default:
                    return "AstroDX/IMG_GAME_SINGLE_TAP_1";
            }
        }
    }
}
