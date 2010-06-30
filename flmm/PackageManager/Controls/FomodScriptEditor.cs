﻿using System;
using System.Windows.Forms;
using System.IO;
using System.Xml.Schema;
using Fomm.PackageManager.XmlConfiguredInstall;
using System.Text.RegularExpressions;

namespace Fomm.PackageManager.Controls
{
	public partial class FomodScriptEditor : UserControl
	{
		public static string DEFAULT_CSHARP_SCRIPT = @"using System;
using fomm.Scripting;

class Script : BaseScript {
	public static bool OnActivate() {
        //Install all files from the fomod and activate any esps
        PerformBasicInstall();
		return true;
	}
}
";
		#region Properties

		/// <summary>
		/// Gets or sets the <see cref="FomodScript"/> being edited.
		/// </summary>
		/// <value>The <see cref="FomodScript"/> being edited.</value>
		public FomodScript Script
		{
			get
			{
				FomodScript fscScript = new FomodScript(FomodScriptType.CSharp, null);
				if (ddtScript.SelectedTabPage == dtpCSharp)
				{
					fscScript.Type = FomodScriptType.CSharp;
					fscScript.Text = sedScript.Text;
				}
				else
				{
					if (!String.IsNullOrEmpty(xedScript.Text))
					{
						fscScript.Type = FomodScriptType.XMLConfig;
						string strHeader = "<?xml version=\"1.0\" encoding=\"UTF-16\" ?>" + Environment.NewLine +
											"<config xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://qconsulting.ca/fo3/ModConfig{0}.xsd\">";
						strHeader = String.Format(strHeader, cbxVersion.SelectedItem.ToString());
						fscScript.Text = xedScript.Text.Replace("<config>", strHeader);
					}
				}
				return String.IsNullOrEmpty(fscScript.Text) ? null : fscScript;
			}
			set
			{
				if (value == null)
				{
					ddtScript.SelectedTabPage = dtpCSharp;
					sedScript.Text = null;
				}
				else
				{
					switch (value.Type)
					{
						case FomodScriptType.XMLConfig:
							switch (Parser.GetConfigVersion(value.Text))
							{
								case "1.0":
									cbxVersion.SelectedIndex = 0;
									break;
								case "2.0":
									cbxVersion.SelectedIndex = 1;
									break;
								case "3.0":
									cbxVersion.SelectedIndex = 2;
									break;
								default:
									ddtScript.SelectedTabPage = dtpCSharp;
									sedScript.Text = null;
									return;
							}
							Regex rgxXMLConfigCleanup = new Regex(@"<\?xml[^>]+\?>.*?<config[^>]*>", RegexOptions.Singleline);
							ddtScript.SelectedTabPage = dtpXML;
							xedScript.Text = rgxXMLConfigCleanup.Replace(value.Text, "<config>");
							break;
						case FomodScriptType.CSharp:
							ddtScript.SelectedTabPage = dtpCSharp;
							sedScript.Text = value.Text;
							break;
						default:
							throw new Exception("Unrecognized value for FomodScriptType enum.");
					}
				}
			}
		}

		/// <summary>
		/// Gets whether or not the script being edited is valid.
		/// </summary>
		/// <value>Whether or not the script being edited is valid.</value>
		public bool IsValid
		{
			get
			{
				if (((ddtScript.SelectedTabPage == dtpCSharp) && !sedScript.ValidateSyntax()) ||
					((ddtScript.SelectedTabPage == dtpXML) && !xedScript.ValidateXml()))
					return false;
				return true;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// The default constructor.
		/// </summary>
		public FomodScriptEditor()
		{
			InitializeComponent();

			cbxVersion.Items.Add("1.0");
			cbxVersion.Items.Add("2.0");
			cbxVersion.Items.Add("3.0");
			cbxVersion.SelectedIndex = 2;
			LoadConfigSchema();
		}

		#endregion

		/// <summary>
		/// Handles the <see cref="ComboBox.SelectedIndexChanged"/> event of the XML config version drop down list.
		/// </summary>
		/// <param name="sender">The object the raised the event.</param>
		/// <param name="e">An <see cref="EventArgs"/> describing the event arguments.</param>
		private void cbxVersion_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadConfigSchema();
		}

		/// <summary>
		/// This loads the selected version of the XML configuration script's schema into the XML editor.
		/// </summary>
		/// <remarks>
		/// Loading the schema into the editor allows for validation and auto-completion.
		/// </remarks>
		protected void LoadConfigSchema()
		{
			string strSchemaPath = Path.Combine(Program.exeDir, String.Format(@"fomm\ModConfig{0}.xsd", cbxVersion.SelectedItem.ToString()));
			if (File.Exists(strSchemaPath))
				using (FileStream fsmSchema = new FileStream(strSchemaPath, FileMode.Open))
				{
					xedScript.Schema = XmlSchema.Read(fsmSchema, null);
					fsmSchema.Close();
				}
		}
	}
}