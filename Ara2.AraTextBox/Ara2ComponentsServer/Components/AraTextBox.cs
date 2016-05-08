// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using Ara2;
using Ara2.Components.Textbox;
using Ara2.Dev;
using Ara2.Components.TextBox.KeyBoard;

namespace Ara2.Components
{
    [Serializable]
    [AraDevComponent(vConteiner:false,vResizable:false)]
    public class AraTextBox : AraComponentVisualAnchor, IAraDev,IDisposable
    {

        // Padão Ara 

        public AraTextBox(IAraContainerClient ConteinerFather,bool vPassword)
            : this(ConteinerFather)
        {
            this.Password = vPassword;
        }

        public AraTextBox(IAraContainerClient ConteinerFather)
				: base(AraObjectClienteServer.Create(ConteinerFather, "input", new Dictionary<string, string> {{"type","text"}}), ConteinerFather, "AraTextBox")
        {
            //,{"autofocus","autofocus"} 
            Construct();
        }

        public AraTextBox(string vNameObject, IAraObject vConteinerFather)
            : base(vNameObject, vConteinerFather, "AraTextBox")
        {
            Construct();
        }


        private void Construct()
        {
            Click = new AraComponentEvent<EventHandler>(this, "Click");
            Focus = new AraComponentEvent<EventHandler>(this, "Focus");
            LostFocus = new AraComponentEvent<EventHandler>(this, "LostFocus");
            Change = new AraComponentEvent<EventHandler>(this, "Change");

            KeyDown = new AraComponentEventKey<Key_delegate>(this, "KeyDown");
            KeyUp = new AraComponentEventKey<Key_delegate>(this, "KeyUp");
            KeyPress = new AraComponentEventKey<Key_delegate>(this, "KeyPress");

            AutoCompleteSearch = new AraComponentEvent<AutoCompleteSearch_delegate>(this, "AutoCompleteSearch", EAraComponentEventTypeThread.ThreadMulti);
            AutoCompleteSearch.ChangeEnabled += AutoCompleteSearch_ChangeEnabled;

            this.EventInternal += AraTextBox_EventInternal;
            this.SetProperty += AraTextBox_SetProperty;

            this.MinWidth = 10;
            this.MinHeight = 17;
            this.Width = 150;
            this.Height = 17;
        }

        private void AutoCompleteSearch_ChangeEnabled()
        {
            this.TickScriptCall();
            //if (AutoCompleteSearch.Enabled)
            //    Tick.GetTick().Script.Send(" vObj.SetAutoComplete_Create({\"AutoFoco\":" + (_AutoCompleteSearchAutoFoco?"true":"false") + "});\n");
            //else
            //    Tick.GetTick().Script.Send(" vObj.SetAutoComplete({disabled:false});\n");

            Tick.GetTick().Script.Send(" vObj.SetAutoComplete_disabled(" + (!AutoCompleteSearch.Enabled ? "true" : "false") + ");\n");   
            if (AutoCompleteSearch.Enabled)
                Tick.GetTick().Script.Send(" vObj.SetAutoComplete_Create();\n");
        }

        private bool _AutoCompleteSearchAutoFoco = false;
        public bool AutoCompleteSearchAutoFoco
        {
            get
            {
                return _AutoCompleteSearchAutoFoco;
            }
            set
            {
                if (_AutoCompleteSearchAutoFoco != value)
                {
                    _AutoCompleteSearchAutoFoco = value;

                    this.TickScriptCall();
                    Tick.GetTick().Script.Send(" vObj.SetAutoComplete_autoFocus(" + (_AutoCompleteSearchAutoFoco ? "true" : "false") + ");\n");                    
                }
            }
        }

        public static bool ArquivosHdCarregado = false;
        public override void LoadJS()
        {
            Tick vTick = Tick.GetTick();
            vTick.Session.AddJs("Ara2/Components/AraTextBox/AraTextBox.js");
        }
       

        private void AraTextBox_EventInternal(String vFunction)
        {
            Tick vTick = Tick.GetTick();
            int vKey;
            switch (vFunction.ToUpper())
            {
                case "FOCUS":
                    Focus.InvokeEvent(this, new EventArgs());
                break;
                case "LOSTFOCUS":
                    LostFocus.InvokeEvent(this, new EventArgs());
                break;
                case "KEYDOWN":
                vKey = Convert.ToInt16(vTick.Page.Request["KeyDown"]);
                    KeyDown.InvokeEvent(this, vKey);
                break;
                case "KEYUP":
                vKey = Convert.ToInt16(vTick.Page.Request["KeyUp"]);
                    KeyUp.InvokeEvent(this, vKey);
                break;
                case "KEYPRESS":
                vKey = Convert.ToInt16(vTick.Page.Request["KeyPress"]);
                    KeyPress.InvokeEvent(this, vKey);
                break;
                case "CHANGE":
                    Change.InvokeEvent(this, new EventArgs());
                break;
                case "CLICK":
                    Click.InvokeEvent(this, new EventArgs());
                break;
                case "AUTOCOMPLETESEARCH":
                {
                    AraTextBoxAutoCompleteResu TmpResu = AutoCompleteSearch.InvokeEvent(this, Convert.ToString(vTick.Page.Request["value"]));

                    string vReturn = "[";
                    if (TmpResu.Count > 0)
                    {
                        for (int n = 0; n < TmpResu.Count; n++)
                        {
                            vReturn += "{'id':'" + AraTools.StringToStringJS(TmpResu[n].Key) + "','label':'" + AraTools.StringToStringJS(TmpResu[n].Caption) + "','value':'" + AraTools.StringToStringJS(TmpResu[n].Key) + "'},";
                        }
                        vReturn = vReturn.Substring(0, vReturn.Length - 1);
                    }
                    vReturn += "]";

                    this.TickScriptCall();
                    vTick.Script.Send("vObj.SetAutoComplete_response(" + vReturn + ");");
                }
                break;
                case "AUTOCOMPLETESEARCH_VALIDATE":
                {
                    if (GetAutoCompleteSearch_Validate(Convert.ToString(vTick.Page.Request["value"])) == null)
                        this.Text = "";
                }
                break;
                    
            }
        }

        private string GetAutoCompleteSearch_Validate(string vValue)
        {
            AraTextBoxAutoCompleteResu TmpResu2 = AutoCompleteSearch.InvokeEvent(this, vValue );
            vValue = vValue.Trim().ToLower();
            if (TmpResu2.Count > 0)
            {
                for (int n = 0; n < TmpResu2.Count; n++)
                {
                    if (TmpResu2[n].Caption.Trim().ToLower() == vValue)
                        return TmpResu2[n].Key;
                }
            }
            return null;
        }

        private void AraTextBox_SetProperty(String vName, dynamic vValue)
        {
            switch (vName.ToUpper())
            {
                case "GETVALUE()":
                    _Text = Convert.ToString(vValue);
                    break;
                case "GETSELECTIONSTART()":
                    _SelectionStart = Convert.ToInt32(vValue);
                    break;
                case "GETSELECTIONEND()":
                    _SelectionEnd = Convert.ToInt32(vValue);
                    break;
            }


        }

        // Fim Padão 

        private string _Text = "";
        private string _TextAutoCompleteID = "";
        private string _Width = "";
        private bool _Visible = true;
        private string _Color_Focus = "";
        private string _Color_LostFocus = "";

        private string _ToolTip = "";
        private string _Placeholder = "";

        #region Events
    
        public delegate void Key_delegate(AraTextBox Object, int vKey);
        [AraDevEvent]
        public AraComponentEvent<EventHandler> Click { get; set; }
        [AraDevEvent]
        public AraComponentEvent<EventHandler> Focus { get; set; }
        [AraDevEvent]
        public AraComponentEvent<EventHandler> LostFocus { get; set; }
        [AraDevEvent]
        public AraComponentEvent<EventHandler> Change { get; set; }
        [AraDevEvent]
        public AraComponentEventKey<Key_delegate> KeyDown { get; set; }
        [AraDevEvent]
        public AraComponentEventKey<Key_delegate> KeyUp { get; set; }
        [AraDevEvent]
        public AraComponentEventKey<Key_delegate> KeyPress { get; set; }

        #endregion

        [AraDevProperty("")]
        public string Text
        {
            set
            {
                _Text = value;
                if (Mask == AraTextBoxMaskTypes.Decimal || (AraTools.IsDecimal(_Text) && IsMaskDecimalGetNumberAfterComma(MaskText)>0))
                {
                    if (_Text != "")
                        _Text = string.Format("{0:N" + IsMaskDecimalGetNumberAfterComma(MaskText) + "}", Convert.ToDecimal(_Text));                    
                }else if (Mask == AraTextBoxMaskTypes.date)
                {
                    if (_Text != "")
                        _Text = string.Format("{0:dd/MM/yyyy}", DateTime.Parse(_Text));                    
                }
                else if (Mask == AraTextBoxMaskTypes.datetime)
                {
                    if (_Text != "")
                        _Text = string.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Parse(_Text));
                }
                Tick vTick = Tick.GetTick();
                this.TickScriptCall();
                vTick.Script.Send(" vObj.SetValue('" + AraTools.StringToStringJS(_Text) + "'); \n");
                //vTick.Script.Send(" vObj.ControlVar.SetValueUtm('GetValue()', vObj.GetValue() );");
            }
            get {
                if (Mask == AraTextBoxMaskTypes.Integer)
                {
                    if (_Text != "")
                        return _Text.Replace(".", "");
                    else
                        return _Text;
                }
                else if (Mask == AraTextBoxMaskTypes.Decimal)
                {
                    if (_Text != "")
                        return _Text.Replace(".", "");
                    else
                        return _Text;
                }
                else
                    return _Text; 
            }
        }

        private int IsMaskDecimalGetNumberAfterComma(string vMaskText)
        {
            int NAC = 0;
            foreach(var vChar in vMaskText.ToArray())
            {
                if (vChar == ',')
                    return NAC;
                else if (!(vChar == ',' || vChar == '9'))
                    return 0;
                else
                    NAC++;
            }

            return 0;
        }


        public string TextAutoCompleteID
        {
            get
            {
                return GetAutoCompleteSearch_Validate(this.Text);
            }
        }

        private bool _Password = false;
        [AraDevProperty(false)]
        public bool Password
        {
            set
            {
                _Password = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetPassword(" + (_Password == true ? "true" : "false") + "); \n");
            }
            get { return _Password; }
        }

        [AraDevProperty("")]
        public string Color_Focus
        {
            get { return _Color_Focus; }
            set { 
                _Color_Focus = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.Color_Focus = '" + AraTools.StringToStringJS(_Color_Focus) + "'; \n");
            }
        }

        [AraDevProperty("")]
        public string Color_LostFocus
        {
            get { return _Color_LostFocus; }
            set { 
                _Color_LostFocus = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.Color_LostFocus = '" + AraTools.StringToStringJS(_Color_LostFocus) + "'; \n");
            }
        }

        private bool _Enabled = true;
        [AraDevProperty(true)]
        public bool Enabled
        {
            get { return _Enabled; }
            set { 
                _Enabled = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetEnable(" + (_Enabled == true ? "true" : "false") + "); \n");
            }
        }

        private bool _Readonly = false;
        [AraDevProperty(false)]
        public bool Readonly
        {
            get { return _Readonly; }
            set
            {
                _Readonly = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetReadonly(" + (_Readonly == true ? "true" : "false") + "); \n");
            }
        }

        [AraDevProperty("")]
        public string ToolTip
        {
            get { return _ToolTip; }
            set { 
                _ToolTip = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetToolTip('" + AraTools.StringToStringJS(_ToolTip) + "'); \n");
            }
        }

        [AraDevProperty("")]
        public string Placeholder
        {
            get { return _Placeholder; }
            set
            {
                _Placeholder = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetPlaceholder('" + AraTools.StringToStringJS(_Placeholder) + "'); \n");
            }
        }

        private AraTextBoxMaskTypes _Mask = AraTextBoxMaskTypes.customize;
        [AraDevProperty(AraTextBoxMaskTypes.customize)]
        public AraTextBoxMaskTypes Mask
        {
            get { return _Mask; }
            set
            {
                switch (value)
                {
                    case AraTextBoxMaskTypes.phone:
                        MaskText = "(99) 9999-9999";
                        MaskType = "";
                        MaskDefaultValue = "";
                        break;
                    case AraTextBoxMaskTypes.phone_us:
                        MaskText = "(999) 9999-9999";
                        MaskType = "";
                        MaskDefaultValue = "";
                        break;
                    case AraTextBoxMaskTypes.phone_sp:
                        MaskText = "9999-9999";
                        MaskType = "";
                        MaskDefaultValue = "";
                        break;
                    case AraTextBoxMaskTypes.phone_sp_ddd:
                        MaskText = "(99)9999-9999";
                        MaskType = "";
                        MaskDefaultValue = "";
                        break;
                    case AraTextBoxMaskTypes.phone_sp_cel:
                        MaskText = "99999-9999";
                        MaskType = "";
                        MaskDefaultValue = "";
                        break;
                    case AraTextBoxMaskTypes.phone_sp_ddd_cel:
                        MaskText = "(99)99999-9999";
                        MaskType = "";
                        MaskDefaultValue = "";
                        break;
                    case AraTextBoxMaskTypes.cpf:
                        MaskText = "999.999.999-99";
                        MaskType = "";
                        MaskDefaultValue = "";
                        break;
                    case AraTextBoxMaskTypes.cnpj:
                        MaskText = "99.999.999/9999-99";
                        MaskType = "";
                        MaskDefaultValue = "";
                        break;
                    case AraTextBoxMaskTypes.date:
                        MaskText = "39/19/9999";
                        MaskType = "";
                        MaskDefaultValue = "";
                        break;
                    case AraTextBoxMaskTypes.date_us:
                        MaskText = "19/39/9999";
                        MaskType = "";
                        MaskDefaultValue = "";
                        break;
                    case AraTextBoxMaskTypes.cep:
                        MaskText = "99999-999";
                        MaskType = "";
                        MaskDefaultValue = "";
                        break;
                    case AraTextBoxMaskTypes.time:
                        MaskText = "29:69";
                        MaskType = "";
                        MaskDefaultValue = "";
                        break;
                    case AraTextBoxMaskTypes.datetime:
                        MaskText = "39/19/9999 29:69";
                        MaskType = "";
                        MaskDefaultValue = "";
                        break;
                    case AraTextBoxMaskTypes.cc:
                        MaskText = "9999 9999 9999 9999";
                        MaskType = "";
                        MaskDefaultValue = "";
                        break;
                    case AraTextBoxMaskTypes.Integer:
                        MaskText = "999.999.999.999";
                        MaskType = "reverse";
                        MaskDefaultValue = "0";
                        break;
                    case AraTextBoxMaskTypes.Decimal:
                        MaskText = "99,999.999.999.999";
                        MaskType = "reverse";
                        MaskDefaultValue = "000";
                        break;
                    case AraTextBoxMaskTypes.decimal_us:
                        MaskText = "99.999,999,999,999";
                        MaskType = "reverse";
                        MaskDefaultValue = "000";
                        break;
                    case AraTextBoxMaskTypes.signed_decimal:
                        MaskText = "99,999.999.999.999";
                        MaskType = "reverse";
                        MaskDefaultValue = "+000";
                        break;
                    case AraTextBoxMaskTypes.signed_decimal_us:
                        MaskText = "99,999.999.999.999";
                        MaskType = "reverse";
                        MaskDefaultValue = "+000";
                        break;
                }

                _Mask = value;
                CommitMask();
            }
        }

        private int? _MaxLength = null;
        [AraDevProperty(null)]
        public int? MaxLength
        {
            get { return _MaxLength; }
            set
            {
                if (_MaxLength != value)
                {
                    _MaxLength = value;
                    this.TickScriptCall();
                    Tick.GetTick().Script.Send(" vObj.SetMaxLength(" + (_MaxLength == null ? "null" : AraTools.StringToStringJS(_MaxLength.ToString())) + "); \n");
                    CommitMask();
                }
            }
        }

        private string _MaskText = "";
        [AraDevProperty("")]
        public string MaskText
        {
            get { return _MaskText; }
            set
            {
                if (_MaskText != value)
                {
                    _MaskText = value;
                    CommitMask();
                    Mask = AraTextBoxMaskTypes.customize;
                }
            }
        }

        private string _MaskType = "";
        [AraDevProperty("")]
        public string MaskType
        {
            get { return _MaskType; }
            set
            {
                if (_MaskType != value)
                {
                    _MaskType = value;
                    CommitMask();
                    Mask = AraTextBoxMaskTypes.customize;
                }
            }
        }

        private string _MaskDefaultValue = "";
        [AraDevProperty("")]
        public string MaskDefaultValue
        {
            get { return _MaskDefaultValue; }
            set
            {
                if (_MaskDefaultValue != value)
                {
                    _MaskDefaultValue = value;
                    CommitMask();
                    Mask = AraTextBoxMaskTypes.customize;
                }
            }
        }

        private string CommitMaskOld = null;
        private void CommitMask()
        {
            string CommitMaskOldTmp = _MaskText + ";" + _MaskType + ";" + _MaskDefaultValue + ";" + MaxLength + ";";

            if (CommitMaskOldTmp != CommitMaskOld && _MaskText != "")
            {
                string vTmpScript = "{";
                vTmpScript += "mask : '" + MaskText + "',";
                vTmpScript += "type : '" + MaskType + "',";
                vTmpScript += "defaultValue : '" + MaskDefaultValue + "',";
                if (MaxLength != null && MaxLength>0)
                    vTmpScript += "maxLength : " + MaxLength + ",";
                vTmpScript = vTmpScript.Substring(0, vTmpScript.Length - 1);
                vTmpScript += "}";

                Tick vTick = Tick.GetTick();
                vTick.Session.AddJs("Ara2/Components/AraTextBox/files/jquery_meio_mask.js");

                this.TickScriptCall();
                vTick.Script.Send(" vObj.SetMask(" + vTmpScript + "); \n");
                CommitMaskOld = CommitMaskOldTmp;
            }

            if (Mask == AraTextBoxMaskTypes.date)
            {
                if (DatePicker == null)
                    DatePicker = new AraTextDatePicker(AraTextDatePicker.EAraTextDatePicker.Data);
            }
            else if (Mask == AraTextBoxMaskTypes.datetime)
            {
                if (DatePicker == null)
                    DatePicker = new AraTextDatePicker(AraTextDatePicker.EAraTextDatePicker.DataHora);
            }
            else if (Mask == AraTextBoxMaskTypes.time)
            {
                if (DatePicker == null)
                    DatePicker = new AraTextDatePicker(AraTextDatePicker.EAraTextDatePicker.Hora);
            }
            else
            {
                if (DatePicker != null)
                {
                    if (DatePicker.Type == AraTextDatePicker.EAraTextDatePicker.Data ||
                        DatePicker.Type == AraTextDatePicker.EAraTextDatePicker.DataHora ||
                        DatePicker.Type == AraTextDatePicker.EAraTextDatePicker.Hora
                        )
                        DatePicker = null;
                }
            }
        }

        private AraTextDatePicker _DatePicker=null;
        public AraTextDatePicker DatePicker
        {
            get { return _DatePicker; }
            set
            {
                _DatePicker = value;

                Tick vTick = Tick.GetTick();

                vTick.Session.AddCss("Ara2/Components/AraTextBox/files/jquery-ui-timepicker-addon.css");
                vTick.Session.AddJs("Ara2/Components/AraTextBox/files/jquery-ui-timepicker-addon.js");

                this.TickScriptCall();
                if (_DatePicker != null)
                    vTick.Script.Send(" vObj.SetDatePicker(" + _DatePicker.GetScript() + "); \n");
                else
                    vTick.Script.Send(" vObj.SetDatePicker(null); \n");

            }
        }

        public delegate AraTextBoxAutoCompleteResu AutoCompleteSearch_delegate(AraTextBox Object, string vSearch);
        [AraDevEvent]
        public AraComponentEvent<AutoCompleteSearch_delegate> AutoCompleteSearch { get; set; }


        private bool _AutoCompleteSearch_Validate = false;
        [AraDevProperty(false)]
        public bool AutoCompleteSearch_Validate
        {
            get { return _AutoCompleteSearch_Validate; }
            set
            {
                _AutoCompleteSearch_Validate = value;

                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.AutoCompleteSearch_Validate = " + (_AutoCompleteSearch_Validate ? "true" : "false") + "; \n");
            }
        }


        public void AutoCompleteShow()
        {
            this.TickScriptCall();
            Tick.GetTick().Script.Send(" vObj.SetAutoComplete_Show(); \n");
        }

        private int _AutoCompleteSearch_minLength = 2;
        [AraDevProperty(2)]
        public int AutoCompleteSearch_minLength
        {
            get { return _AutoCompleteSearch_minLength; }
            set
            {
                _AutoCompleteSearch_minLength = value;

                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetAutoComplete_minLength(" + _AutoCompleteSearch_minLength + "); \n");
            }
        }

        public void SetFocus()
        {
            this.TickScriptCall();
            Tick.GetTick().Script.Send("vObj.SetFocus();");

        }

        private static string[] ETypeString= new string[]{
            "color",
            "date",
            "datetime",
            "datetime-local",
            "email",
            "month",
            "number",
            "range",
            "search",
            "tel",
            "time",
            "url",
            "week",
            "text"
        };

        public enum EType
        {
            color=0,
            date=1,
            datetime=2,
            datetime_local=3,
            email=4,
            month=5,
            number=6,
            range=7,
            search=8,
            tel=9,
            time=10,
            url=11,
            week=12,
            text=13
        }

        EType _Type = EType.text;

        [AraDevProperty(EType.text)]
        public EType Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetType('" + ETypeString[(int)_Type] + "'); \n");
            }
        }



        #region Ara2Dev
        private string _Name = "";
        [AraDevProperty("")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private AraEvent<DStartEditPropertys> _StartEditPropertys = null;
        public AraEvent<DStartEditPropertys> StartEditPropertys
        {
            get
            {
                if (_StartEditPropertys == null)
                {
                    _StartEditPropertys = new AraEvent<DStartEditPropertys>();
                    this.Click += this_ClickEdit;
                }

                return _StartEditPropertys;
            }
            set
            {
                _StartEditPropertys = value;
            }
        }
        private void this_ClickEdit(object sender, EventArgs e)
        {
            if (_StartEditPropertys.InvokeEvent != null)
                _StartEditPropertys.InvokeEvent(this);
        }

        private AraEvent<DStartEditPropertys> _ChangeProperty = new AraEvent<DStartEditPropertys>();
        public AraEvent<DStartEditPropertys> ChangeProperty
        {
            get
            {
                return _ChangeProperty;
            }
            set
            {
                _ChangeProperty = value;
            }
        }
        #endregion



        public enum AraTextBoxMaskTypes
        {
            phone,
            phone_us,
            phone_sp,
            phone_sp_ddd,
            phone_sp_cel,
            phone_sp_ddd_cel,
            cpf,
            cnpj,
            date,
            date_us,
            cep,
            time,
            cc,
            Integer,
            Decimal,
            decimal_us,
            signed_decimal,
            signed_decimal_us,
            customize,
            datetime
        }

        private string _Step = "";
        [AraDevProperty("")]
        public string Step
        {
            get { return _Step; }
            set
            {
                _Step = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetStep('" + AraTools.StringToStringJS(_Step) + "'); \n");
            }
        }

        private string _Max = "";
        [AraDevProperty("")]
        public string Max
        {
            get { return _Max; }
            set
            {
                _Max = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetMax('" + AraTools.StringToStringJS(_Max) + "'); \n");
            }
        }

        private string _Min = "";
        [AraDevProperty("")]
        public string Min
        {
            get { return _Min; }
            set
            {
                _Min = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetMin('" + AraTools.StringToStringJS(_Min) + "'); \n");
            }
        }

        private string _Pattern = "";
        [AraDevProperty("")]
        public string Pattern
        {
            get { return _Pattern; }
            set
            {
                _Pattern = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetPattern('" + AraTools.StringToStringJS(_Pattern) + "'); \n");
            }
        }

        #region KeyBoard
        private static Dictionary<string, AraObjectInstance<FrmAraTextBoxKeyBoard>> _FrmAraTextBoxKeyBoard = new Dictionary<string, AraObjectInstance<FrmAraTextBoxKeyBoard>>();
        public FrmAraTextBoxKeyBoard FrmAraTextBoxKeyBoard
        {
            get {
                lock (_FrmAraTextBoxKeyBoard)
                {
                    if (!_FrmAraTextBoxKeyBoard.ContainsKey(Tick.GetTick().Session.Id))
                        _FrmAraTextBoxKeyBoard.Add(Tick.GetTick().Session.Id, new AraObjectInstance<FrmAraTextBoxKeyBoard>());

                    return _FrmAraTextBoxKeyBoard[Tick.GetTick().Session.Id].Object;
                }
            }
            set {
                lock (_FrmAraTextBoxKeyBoard)
                {
                    if (!_FrmAraTextBoxKeyBoard.ContainsKey(Tick.GetTick().Session.Id))
                        _FrmAraTextBoxKeyBoard.Add(Tick.GetTick().Session.Id, new AraObjectInstance<FrmAraTextBoxKeyBoard>());

                    if (value != null)
                        _FrmAraTextBoxKeyBoard[Tick.GetTick().Session.Id].Object = value;
                    else
                    {
                        _FrmAraTextBoxKeyBoard[Tick.GetTick().Session.Id] = new AraObjectInstance<TextBox.KeyBoard.FrmAraTextBoxKeyBoard>();
                        _FrmAraTextBoxKeyBoard.Remove(Tick.GetTick().Session.Id);
                    }
                }
            }
        }

        

        private KeyBoardModel.EKeyBoardModel _KeyboardModeType = KeyBoardModel.EKeyBoardModel.Custom;
        [AraDevProperty(KeyBoardModel.EKeyBoardModel.Custom)]
        public KeyBoardModel.EKeyBoardModel KeyboardModeType
        {
            get { return _KeyboardModeType; }
            set
            {
                _KeyboardModeType = value;
                if (_KeyboardModeType != KeyBoardModel.EKeyBoardModel.Custom)
                    KeyboardMode = KeyBoardModel.Models[_KeyboardModeType];
                else
                    KeyboardMode = null;
            }
        }

        private KeyBoardModel _KeyboardMode = null;
        public KeyBoardModel KeyboardMode
        {
            get { return _KeyboardMode; }
            set
            {
                _KeyboardMode = value;

                if (_KeyboardMode!=null)
                {
                    this.Focus += this_FocusKeybord;
                    this.LostFocus += this_LostFocusKeybord;
                }
                else
                {
                    if (FrmAraTextBoxKeyBoard != null)
                    {
                        FrmAraTextBoxKeyBoard.Close();
                        FrmAraTextBoxKeyBoard = null;
                    }

                    this.Focus -= this_FocusKeybord;
                    this.LostFocus -= this_LostFocusKeybord;
                }
            }
        }

        private void this_FocusKeybord(object sender, EventArgs e)
        {
            if (_KeyboardMode != null)
            {

                if (FrmAraTextBoxKeyBoard == null || FrmAraTextBoxKeyBoard.TextBox.InstanceID != this.InstanceID || FrmAraTextBoxKeyBoard.ModelBase != KeyboardMode)
                {
                    while (FrmAraTextBoxKeyBoard != null && FrmAraTextBoxKeyBoard.AguardandoSegundoFoco && (FrmAraTextBoxKeyBoard.TextBox.InstanceID != this.InstanceID || FrmAraTextBoxKeyBoard.ModelBase != KeyboardMode))
                    { System.Threading.Thread.Sleep(100); }

                    lock (_FrmAraTextBoxKeyBoard)
                    {
                        IAraComponentVisual vPai = (IAraComponentVisual)this;

                        while (!(((IAraComponentVisual)vPai) is WindowMain))
                        {
                            vPai = (IAraComponentVisual)vPai.ConteinerFather;
                        }

                        if (FrmAraTextBoxKeyBoard == null)
                        {
                            FrmAraTextBoxKeyBoard = new FrmAraTextBoxKeyBoard((IAraContainerClient)vPai, this, KeyboardMode);
                            FrmAraTextBoxKeyBoard.Show(false, FrmAraTextBoxKeyBoard_Unload);

                            FrmAraTextBoxKeyBoard.AguardandoSegundoFoco = true;
                            this.SetFocus();
                        }
                        else
                        {
                            if (FrmAraTextBoxKeyBoard.ModelBase != KeyboardMode)
                                FrmAraTextBoxKeyBoard.ModelBase = KeyboardMode;
                            if (FrmAraTextBoxKeyBoard.TextBox.InstanceID != this.InstanceID)
                                FrmAraTextBoxKeyBoard.TextBox = this;
                        }
                    }
                }
                else
                    FrmAraTextBoxKeyBoard.AguardandoSegundoFoco = false;

                FrmAraTextBoxKeyBoard.CancelLostFocusTextbox();

            }
        }

        private void FrmAraTextBoxKeyBoard_Unload(object vObj)
        {
            FrmAraTextBoxKeyBoard = null;
        }

        private void this_LostFocusKeybord(object sender, EventArgs e)
        {
            if (FrmAraTextBoxKeyBoard != null)
                FrmAraTextBoxKeyBoard.LostFocusTextbox();
        }
        #endregion

        public void Dispose()
        {
            if (_KeyboardMode != null && FrmAraTextBoxKeyBoard != null)
            {
                FrmAraTextBoxKeyBoard.LostFocusTextbox();
                FrmAraTextBoxKeyBoard.Close();
                FrmAraTextBoxKeyBoard = null;
            }

            base.Dispose();
        }

        private int _SelectionStart=0;
        [AraDevProperty(0)]
        public int SelectionStart
        {
            get { return _SelectionStart; }
            set { 
                _SelectionStart = value;

                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetSelectionStart(" + _SelectionStart + "); \n");
            }
        }

        private int _SelectionEnd = 0;
        [AraDevProperty(0)]
        public int SelectionEnd
        {
            get { return _SelectionEnd; }
            set
            {
                _SelectionEnd = value;

                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetSelectionEnd(" + _SelectionEnd + "); \n");
            }
        }

    }

    public class AraTextBoxAutoCompleteResu
    {
        List<AraTextBoxAutoCompleteResuIten> Itens = new List<AraTextBoxAutoCompleteResuIten>();

        public void Add(string vCaption)
        {
            this.Add(vCaption, vCaption);
        }
        public void Add(string vCaption, string vkey)
        {
            Itens.Add(new AraTextBoxAutoCompleteResuIten(vCaption, vkey));
        }

        public int Count
        {
            get
            {
                return Itens.Count;
            }
        }

        public AraTextBoxAutoCompleteResuIten this[int vIndex]
        {
            get{
                return Itens[vIndex];
            }
        }

        
    }

    public class AraTextBoxAutoCompleteResuIten
    {
        public string Caption="";
        public string Key="";

        public AraTextBoxAutoCompleteResuIten(string vCaption, string vKey)
        {
            Caption = vCaption;
            Key = vKey;
        }
    }

}
