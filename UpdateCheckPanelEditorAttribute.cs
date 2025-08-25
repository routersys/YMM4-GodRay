using System.Windows;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Plugin.Community.Effect.Video.GodRay.Controls;

namespace YukkuriMovieMaker.Plugin.Community.Effect.Video.GodRay
{
    internal class UpdateCheckPanelEditorAttribute : PropertyEditorAttribute2
    {
        public override FrameworkElement Create()
        {
            return new UpdateCheckPanel();
        }

        public override void SetBindings(FrameworkElement control, ItemProperty[] itemProperties)
        {
        }

        public override void ClearBindings(FrameworkElement control)
        {
        }
    }
}