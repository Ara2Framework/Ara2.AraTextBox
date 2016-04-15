// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ara2.Components.TextBox.KeyBoard
{
    [Serializable]
    public  class KeyBoardKey
    {
        public char Key;
        public string Caption = null;
        public KeyBoardModel SubKeyBoardModel = null;
        public int Width=55;
        public bool Bsp = false;
        public bool Empty = false;
        public bool IgnoreAddText = false;
        public bool Close = false;

        public KeyBoardKey()
        {
        }
        public KeyBoardKey(char vKey, string vCaption)
        {
            Key=vKey;
            Caption = vCaption;
        }
        public KeyBoardKey(char vKey)
        {
            Key = vKey;
        }
        public KeyBoardKey(char vKey, string vCaption, int vWidth)
        {
            Key = vKey;
            Caption = vCaption;
            Width = vWidth;
        }
        public KeyBoardKey(char vKey, string vCaption, KeyBoardModel vSubKeyBoardModel)
        {
            Key = vKey;
            Caption = vCaption;
            SubKeyBoardModel = vSubKeyBoardModel;
        }
        public KeyBoardKey(char vKey, string vCaption, KeyBoardModel vSubKeyBoardModel, int vWidth)
        {
            Key = vKey;
            Caption = vCaption;
            SubKeyBoardModel = vSubKeyBoardModel;
            Width = vWidth;
        }
    }
}
