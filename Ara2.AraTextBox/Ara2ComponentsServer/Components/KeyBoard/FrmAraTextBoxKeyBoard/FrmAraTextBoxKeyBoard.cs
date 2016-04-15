// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ara2.Components;
using Ara2;
using System.Collections;
using AraDesign;

namespace Ara2.Components.TextBox.KeyBoard
{
    public class FrmAraTextBoxKeyBoard : AraDesign.FrmAraTextBoxKeyBoardAraDesign
    {
        
        private AraObjectInstance<AraTextBox> _TextBox = new AraObjectInstance<AraTextBox>();
        public AraTextBox TextBox 
        {
            get{return _TextBox.Object;}
            set {_TextBox.Object = value;}
        }

        public bool AguardandoSegundoFoco = false;

        #region Times 
        private AraObjectInstance<AraTimer> _Timer = new AraObjectInstance<AraTimer>();
        private AraTimer TimerClose
        {
            get { return _Timer.Object; }
            set { _Timer.Object = value; }
        }

        private AraObjectInstance<AraTimer> _TimerAntLost = new AraObjectInstance<AraTimer>();
        private AraTimer TimerAntLost
        {
            get { return _TimerAntLost.Object; }
            set { _TimerAntLost.Object = value; }
        }
        #endregion

        public FrmAraTextBoxKeyBoard(IAraContainerClient ConteinerFather, AraTextBox vTextBox, KeyBoardModel vModelBase)
            : base(ConteinerFather)
        {
            this.ButtonMinimize = false;
            this.ButtonMaximize = false;
            this.ButtonClose = false;

            TextBox = vTextBox;
            TimerClose = new AraTimer(this);
            TimerClose.Interval = 500;
            TimerClose.tick += TimerClose_Tick;
            TimerClose.Enabled = false;

            TimerAntLost = new AraTimer(this);
            TimerAntLost.Interval = 700;
            TimerAntLost.tick += TimerAntLost_tick;
            TimerAntLost.Enabled = true;

            this.Anchor.Bottom = 10;
            this.Anchor.CenterH = true;
            ModelBase = vModelBase;
        }

        private KeyBoardModel _ModelBase;
        public KeyBoardModel ModelBase
        {
            get { return _ModelBase; }
            set
            {
                _ModelBase = value;
                Model = _ModelBase;
            }
        }

        private KeyBoardModel _Model;
        public KeyBoardModel Model
        {
            get { return _Model; }
            set
            {
                _Model = value;

                _Botao.ForEach((AraObjectInstance<Ara2.Components.AraButton> b) =>
                    {
                        b.Object.Dispose();
                    });
                _Botao = new List<AraObjectInstance<AraButton>>();
                
                int vTmpZIndex = 1;
                int Top = 5;
                int Left = 5;
                int vHeight = 45;
                int WidthMaxForm = 0;
                foreach (var vLine in Model.Line)
                {
                    foreach (KeyBoardKey Key in vLine)
                    {
                        if (Key.Empty == false)
                        {
                            Ara2.Components.AraButton vTmpBotao = new Ara2.Components.AraButton(this);

                            vTmpBotao.Text =  (Key.Caption != null ? Key.Caption : @"<font size=""5"">"+ Key.Key.ToString() + "</font>")  ;
                            vTmpBotao.Left = Left;
                            vTmpBotao.Top = Top;
                            vTmpBotao.Width = Key.Width;
                            vTmpBotao.Height = vHeight;
                            vTmpBotao.ZIndex = vTmpZIndex;
                            vTmpBotao.Tag = Key;
                            vTmpBotao.Click += Boato_Click;
                            vTmpZIndex++;

                            AraObjectInstance<AraButton> vTmpObjIntance = new AraObjectInstance<AraButton>();
                            vTmpObjIntance.Object = vTmpBotao;

                            _Botao.Add(vTmpObjIntance);
                            if (WidthMaxForm < vTmpBotao.Left.Value + Key.Width + 5)
                                WidthMaxForm = Convert.ToInt32( vTmpBotao.Left.Value) + Key.Width + 5;
                        }

                        Left += Key.Width + 5;
                    }

                    Top += vHeight + 5;
                    Left = 5;
                }

                this.Width = WidthMaxForm;
                this.Height = Top;
                this.Anchor.CenterH = false;
                this.Anchor.CenterH = true;
            }
        }

        private List<AraObjectInstance<Ara2.Components.AraButton>> _Botao = new List<AraObjectInstance<AraButton>>();
        //public Ara2.Components.AraButton Botao(char vButton)
        //{
        //    return _Botao.First(a => ((KeyBoardKey)a.Object.Tag).Key == vButton).Object;
        //}

        public void AddTexto(KeyBoardKey vKey)
        {
            if (vKey.Close==false)
                TextBox.SetFocus();

            if (TextBox.KeyDown.InvokeEvent != null)
                TextBox.KeyDown.InvokeEvent(TextBox, (int)vKey.Key);

            if (vKey.IgnoreAddText == false)
            {
                if (TextBox.Mask == AraTextBox.AraTextBoxMaskTypes.Decimal)
                {
                    int vVligula = TextBox.Text.IndexOf(",");
                    string vTmpTexto = TextBox.Text.Replace(",", "");
                    TextBox.Text = vTmpTexto.Substring(0, vVligula + 1) + "," + vTmpTexto.Substring(vVligula + 1, vTmpTexto.Length - (vVligula + 1)) + vKey.Key.ToString();
                }
                else
                    TextBox.Text = TextBox.Text + vKey.Key.ToString();
            }

            if (TextBox.KeyPress.InvokeEvent != null)
                TextBox.KeyPress.InvokeEvent(TextBox, (int)vKey.Key);
            if (TextBox.KeyUp.InvokeEvent != null)
                TextBox.KeyUp.InvokeEvent(TextBox, (int)vKey.Key);
        }

        public void Apagar()
        {
            TextBox.SetFocus();
            if (TextBox.Mask == AraTextBox.AraTextBoxMaskTypes.Decimal)
            {
                int vVligula = TextBox.Text.IndexOf(",");
                string vTmpTexto = TextBox.Text.Replace(",", "");
                string vTmpTextoAntes = vTmpTexto.Substring(0, vVligula - 1);
                if (vTmpTextoAntes == "") vTmpTextoAntes = "0";
                string vTmpTextoDepois = vTmpTexto.Substring(vVligula - 1, vTmpTexto.Length  - vVligula);
                if (vTmpTextoAntes == "") vTmpTextoAntes = "0";

                TextBox.Text = vTmpTextoAntes + "," + vTmpTextoDepois;
            }
            else
            {
                if (TextBox.Text != "")
                    TextBox.Text = TextBox.Text.Substring(0, TextBox.Text.Length - 1);
            }
        }

        public void Boato_Click(object sender, EventArgs e)
        {
            KeyBoardKey vKey = ((KeyBoardKey)((AraButton)sender).Tag);
            if (vKey.SubKeyBoardModel != null)
            {
                Model = vKey.SubKeyBoardModel;
                TextBox.SetFocus();
            }
            else if (vKey.Bsp)
                Apagar();
            else 
                AddTexto(vKey);

            if (vKey.Close)
                this.Close();
        }
      

        #region Close
        private void TimerClose_Tick()
        {
            TimerClose.Enabled = false;
            this.Close();
        }

        public void LostFocusTextbox()
        {
            TimerClose.Enabled = true;
        }

        public void CancelLostFocusTextbox()
        {
            TimerClose.Enabled = false;
        }

        private void TimerAntLost_tick()
        {
            if (TextBox == null)
                this.Close();
            else
            {
                if (TextBox.FrmAraTextBoxKeyBoard ==null || TextBox.FrmAraTextBoxKeyBoard.InstanceID != this.InstanceID)
                    this.Close();
            }
        }
        #endregion
    }
}