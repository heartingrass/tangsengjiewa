﻿using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Win32;
using System.ComponentModel;

namespace 唐僧解瓦.通用
{
    /// <summary>
    /// 升级文件
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    class Cmd_UpdateFiles : IExternalCommand
    {
        private string folderpath = default(string);
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var dbapp = uiapp.Application;
            var uidoc = commandData.Application.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;
            var acview = doc.ActiveView;

            var filepath = default(string);

            OpenFileDialog opdg = new OpenFileDialog();
            //opdg.Filter = "(rvtfiles)|*.rvt()|*.rfa";
            opdg.Filter = "(*.rfa)|*.rfa|(*.rvt)|*.rvt";

            opdg.Multiselect = true;
            opdg.FileOk += OnfileOk;
            var dialogresult = opdg.ShowDialog();

            var count = opdg.FileNames.Length;
            string[] files = new string[count];

            if (dialogresult == true)
            {
                files = opdg.FileNames;
            }
            foreach (var file in files)
            {
                var temdoc = dbapp.OpenDocumentFile(file);
                temdoc.Save();
                temdoc.Close();
            }
            return Result.Succeeded;
        }
        private void OnfileOk(object sender, CancelEventArgs e)
        {
            (sender as OpenFileDialog).RestoreDirectory = true;
        }
    }
}
