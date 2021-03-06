﻿using System;
using System.Windows.Forms;

namespace Fomm.Controls
{
  /// <summary>
  ///   The object that can be added to <see cref="PanelToolStrip" />s.
  /// </summary>
  /// <remarks>
  ///   This class is a wrapper for the actual controls that get added to the <see cref="PanelToolStrip" />.
  ///   The purpose of this wrapper is to provide a single event that gets raised in a consistent manner
  ///   to which commands can be attached. This class also serves to track various metadata about the
  ///   controls.
  /// </remarks>
  public class PanelToolStripItem
  {
    public event EventHandler IndexChanged;

    private Int32 m_intIndex;
    private ToolTip m_ttpToolTip = new ToolTip();
    private ToolStripItemDisplayStyle m_tdsDisplayStyle = ToolStripItemDisplayStyle.Image;

    #region Properties

    /// <summary>
    ///   Gets or sets the Display style of the PanelToolStripItem.
    /// </summary>
    /// <value>The Display style of the PanelToolStripItem.</value>
    public ToolStripItemDisplayStyle Display
    {
      get
      {
        return m_tdsDisplayStyle;
      }
      set
      {
        m_tdsDisplayStyle = value;
      }
    }

    /// <summary>
    ///   Gets or sets the index of the PanelToolStripItem.
    /// </summary>
    /// <value>The index of the PanelToolStripItem.</value>
    public Int32 Index
    {
      get
      {
        return m_intIndex;
      }
      set
      {
        if (value != m_intIndex)
        {
          m_intIndex = value;
          if (IndexChanged != null)
          {
            IndexChanged(this, new EventArgs());
          }
        }
      }
    }

    /// <summary>
    ///   Gets or sets the Enabled of the button.
    /// </summary>
    /// <value>The Enabled of the button.</value>
    public bool Enabled
    {
      get
      {
        return Button.Enabled;
      }
      set
      {
        Button.Enabled = value;
      }
    }

    /// <summary>
    ///   Gets or sets the Visible of the button.
    /// </summary>
    /// <value>The Visible of the button.</value>
    public bool Visible
    {
      get
      {
        return Button.Visible;
      }
      set
      {
        Button.Visible = value;
      }
    }

    /// <summary>
    ///   Gets the actual control that is to be added to the <see cref="PanelToolStrip" />.
    /// </summary>
    /// <value>The actual control that is to be added to the <see cref="PanelToolStrip" />.</value>
    public Control Button { get; private set; }

    #endregion

    #region Constructors

    /// <summary>
    ///   A simple constructor.
    /// </summary>
    /// <remarks>
    ///   This constructor wires the appropriate event <paramref name="p_ctlButton" /> to the
    ///   <see cref="Select" /> event.
    /// </remarks>
    /// <param name="p_ctlButton">The actual control that is to be added to the <see cref="PanelToolStrip" />.</param>
    /// <param name="p_strEvent">
    ///   The name of the event on the button control to which to bind the <see cref="Selected" />
    ///   event.
    /// </param>
    /// <param name="p_intIndex">The index of this item in the panel.</param>
    /// <param name="p_tdsDisplayStyle">
    ///   The <see cref="ToolStripItemDisplayStyle" /> indicating how text and
    ///   images are displayed on this item.
    /// </param>
    public PanelToolStripItem(Control p_ctlButton, string p_strEvent, Int32 p_intIndex,
                              ToolStripItemDisplayStyle p_tdsDisplayStyle)
    {
      Button = p_ctlButton;
      m_tdsDisplayStyle = p_tdsDisplayStyle;

      var tpeButtonType = Button.GetType();

      Button.Tag = this;
      m_intIndex = p_intIndex;
      m_ttpToolTip.SetToolTip(Button, Button.Text);

      tpeButtonType.GetEvent(p_strEvent)
                   .AddEventHandler(p_ctlButton, Delegate.CreateDelegate(typeof (EventHandler), this, "OnSelected"));
    }

    #endregion

    /// <summary>
    ///   The homogeneous event to which commands can be attached to the <see cref="PanelToolStrip" />'s buttons.
    /// </summary>
    public event EventHandler<EventArgs> Selected;

    /// <summary>
    ///   Raises the <see cref="Select" /> event.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">An <see cref="EventArgs" /> desribing the event arguments.</param>
    protected virtual void OnSelected(object sender, EventArgs e)
    {
      if (Selected != null)
      {
        Selected(this, e);
      }
    }

    /// <summary>
    ///   Sets the item as not selected.
    /// </summary>
    public virtual void SetUnselected() {}

    /// <summary>
    ///   Sets the item as selected.
    /// </summary>
    public virtual void SetSelected()
    {
      OnSelected(this, new EventArgs());
    }
  }
}