﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HyoutaTools.Other.PicrossDS {
	class SaveEditor {
		public static int Execute( List<string> args ) {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			var form = new PuzzleEditorForm();
			if ( !form.IsDisposed ) {
				Application.Run( form );
			}
			return 0;
		}
	}
}
