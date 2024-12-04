// Copyright (c) Paul Harrington.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.

using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Media;
using static System.Globalization.CultureInfo;

namespace EditorGuidelines
{
    #region Adornment Factory
    /// <summary>
    /// Establishes an <see cref="IAdornmentLayer"/> to place the adornment on and exports the <see cref="IWpfTextViewCreationListener"/>
    /// that instantiates the adornment on the event of a <see cref="IWpfTextView"/>'s creation
    /// </summary>
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class ColumnGuideAdornmentFactory : IWpfTextViewCreationListener, IPartImportsSatisfiedNotification
    {
        public const string AdornmentLayerName = "ColumnGuide";

        /// <summary>
        /// Defines the adornment layer for the adornment. This layer is ordered 
        /// below the text in the Z-order
        /// </summary>
        [Export(typeof(AdornmentLayerDefinition))]
        [Name(AdornmentLayerName)]
        [Order(Before = PredefinedAdornmentLayers.Text)]
        [TextViewRole(PredefinedTextViewRoles.Document)]
        public AdornmentLayerDefinition editorAdornmentLayer = null;

        /// <summary>
        /// Instantiates a ColumnGuide manager when a textView is created.
        /// </summary>
        /// <param name="textView">The <see cref="IWpfTextView"/> upon which the adornment should be placed</param>
        public void TextViewCreated(IWpfTextView textView)
        {
            // Always create the adornment, even if there are no guidelines, since we
            // respond to dynamic changes.
            var _ = new ColumnGuideAdornment(textView, TextEditorGuidesSettings, GuidelineBrush, CodingConventions);
        }

        public void OnImportsSatisfied()
        {
        }

        [Import]
        private ITextEditorGuidesSettings TextEditorGuidesSettings { get; set; }

        [Import]
        private GuidelineBrush GuidelineBrush { get; set; }

        [Import]
        private CodingConventions CodingConventions { get; set; }

        [Import]
        private HostServices HostServices { get; set; }
    }
#endregion //Adornment Factory
}
