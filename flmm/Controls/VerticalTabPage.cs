﻿using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Fomm.Controls
{
  /// <summary>
  ///   A page in a <see cref="VertecalTabControl" />.
  /// </summary>
  public class VerticalTabPage : Panel, IStatusProviderAware
  {
    #region Properties

    /// <summary>
    ///   Gets the buttons associated with this page.
    /// </summary>
    /// <remarks>
    ///   This is the button used to select this page in the <see cref="VertecalTabControl" />.
    /// </remarks>
    /// <value>The buttons associated with this page.</value>
    [Browsable(false)]
    public VerticalTabButton TabButton { get; private set; }

    /// <summary>
    ///   Gets or sets the text that appears in this page's tab.
    /// </summary>
    /// <value>The text that appears in this page's tab.</value>
    [Browsable(true), Category("Appearance")]
    public override string Text
    {
      get
      {
        return TabButton.Text;
      }
      set
      {
        TabButton.Text = value;
      }
    }

    /// <summary>
    ///   Gets or sets the index of this page in the <see cref="VertecalTabControl" />.
    /// </summary>
    /// <value>The index of this page in the <see cref="VertecalTabControl" />.</value>
    [Category("Behavior")]
    public Int32 PageIndex
    {
      get
      {
        return TabButton.Index;
      }
      set
      {
        TabButton.Index = value;
      }
    }

    #endregion

    #region Constructors

    /// <summary>
    ///   The default constructor.
    /// </summary>
    public VerticalTabPage()
    {
      TabButton = new VerticalTabButton(this);
    }

    #endregion

    /// <summary>
    ///   Disposes of the control.
    /// </summary>
    /// <remarks>
    ///   This ensures that to page's tab button is also disposed of.
    /// </remarks>
    /// <param name="disposing"></param>
    protected override void Dispose(bool disposing)
    {
      TabButton.Button.Dispose();
      base.Dispose(disposing);
    }

    #region IErrorProviderAware Members

    /// <summary>
    ///   Gets the button upon which to display status message from <see cref="SiteStatusProvider" />s.
    /// </summary>
    /// <value>The button upon which to display status message from <see cref="SiteStatusProvider" />s.</value>
    public Control StatusProviderSite
    {
      get
      {
        return TabButton.Button;
      }
    }

    #endregion
  }
}