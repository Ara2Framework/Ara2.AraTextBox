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
    public class KeyBoardModel
    {
        public string Caption;
        public List<List<KeyBoardKey>> Line = new List<List<KeyBoardKey>>();

        public enum EKeyBoardModel
        {
            Custom,
            Numeric,
            //Internacional,
            PtBr
        }

        public static KeyBoardModel GetKeyBoardModelPtBr()
        {
            KeyBoardModel KeyBoardModelPtBrCaixaBaixa = new KeyBoardModel()
            {
                Caption = "PtBrCaixaBaixa",
                Line = new List<List<KeyBoardKey>>()
                    {
                        new List<KeyBoardKey>()
                        {
                            new KeyBoardKey() {Empty=true,Width = 80},
                            new KeyBoardKey() {Key = 'q'},
                            new KeyBoardKey() {Key = 'w'},
                            new KeyBoardKey() {Key = 'e'},
                            new KeyBoardKey() {Key = 'r'},
                            new KeyBoardKey() {Key = 't'},
                            new KeyBoardKey() {Key = 'y'},
                            new KeyBoardKey() {Key = 'u'},
                            new KeyBoardKey() {Key = 'i'},
                            new KeyBoardKey() {Key = 'o'},
                            new KeyBoardKey() {Key = 'p'},
                            new KeyBoardKey() {Caption=@"<font size=""5""><</font>", Bsp = true},
                            new KeyBoardKey() {Empty=true,Width = 10},
                            new KeyBoardKey() {Key = '7'},
                            new KeyBoardKey() {Key = '8'},
                            new KeyBoardKey() {Key = '9'},
                            new KeyBoardKey() {Key = '+'}
                        },
                        new List<KeyBoardKey>()
                        {
                            new KeyBoardKey() {Caption=@"<font size=""2"">Caps Lock</font>" ,Width =110},
                            new KeyBoardKey() {Key = 'a'},
                            new KeyBoardKey() {Key = 's'},
                            new KeyBoardKey() {Key = 'd'},
                            new KeyBoardKey() {Key = 'f'},
                            new KeyBoardKey() {Key = 'g'},
                            new KeyBoardKey() {Key = 'h'},
                            new KeyBoardKey() {Key = 'j'},
                            new KeyBoardKey() {Key = 'k'},
                            new KeyBoardKey() {Key = 'l'},
                            new KeyBoardKey() {Key = 'ç'},
                            new KeyBoardKey() {Empty=true,Width = 40},
                            new KeyBoardKey() {Key = '4'},
                            new KeyBoardKey() {Key = '5'},
                            new KeyBoardKey() {Key = '6'},
                            new KeyBoardKey() {Key = '-'}
                        },
                        new List<KeyBoardKey>()
                        {
                            new KeyBoardKey() {Empty=true,Width = 130},
                            new KeyBoardKey() {Key = 'z'},
                            new KeyBoardKey() {Key = 'x'},
                            new KeyBoardKey() {Key = 'c'},
                            new KeyBoardKey() {Key = 'v'},
                            new KeyBoardKey() {Key = 'b'},
                            new KeyBoardKey() {Key = 'n'},
                            new KeyBoardKey() {Key = 'm'},
                            new KeyBoardKey() {Caption=@"<font size=""5"">OK</font>",IgnoreAddText=true,Close=true,Width = 55*3},
                            new KeyBoardKey() {Empty=true,Width = 30},
                            new KeyBoardKey() {Key = '1'},
                            new KeyBoardKey() {Key = '2'},
                            new KeyBoardKey() {Key = '3'},
                            new KeyBoardKey() {Key = '.'},
                            
                        },
                        new List<KeyBoardKey>()
                        {
                            new KeyBoardKey() {Empty=true,Width=140 },
                            new KeyBoardKey() {Key = ' ',Width = 500},
                            new KeyBoardKey() {Empty=true,Width=105 },
                            new KeyBoardKey() {Key = '0',Width = (55*3)+10},
                            new KeyBoardKey() {Key = ','},
                        }
                    }
            };

            KeyBoardModel KeyBoardModelPtBrCaixaAlta = new KeyBoardModel()
            {
                Caption = "PtBrCaixaAlta",
                Line = new List<List<KeyBoardKey>>()
                    {
                        new List<KeyBoardKey>()
                        {
                            new KeyBoardKey() {Empty=true,Width = 80},
                            new KeyBoardKey() {Key = 'Q'},
                            new KeyBoardKey() {Key = 'W'},
                            new KeyBoardKey() {Key = 'E'},
                            new KeyBoardKey() {Key = 'R'},
                            new KeyBoardKey() {Key = 'T'},
                            new KeyBoardKey() {Key = 'Y'},
                            new KeyBoardKey() {Key = 'U'},
                            new KeyBoardKey() {Key = 'I'},
                            new KeyBoardKey() {Key = 'O'},
                            new KeyBoardKey() {Key = 'P'},
                            new KeyBoardKey() {Caption=@"<font size=""5""><</font>", Bsp = true},
                            new KeyBoardKey() {Empty=true,Width = 10},
                            new KeyBoardKey() {Key = '7'},
                            new KeyBoardKey() {Key = '8'},
                            new KeyBoardKey() {Key = '9'},
                            new KeyBoardKey() {Key = '+'}
                        },
                        new List<KeyBoardKey>()
                        {
                            new KeyBoardKey() {Caption=@"<font size=""2"">Caps Lock</font>" ,Width =110},
                            new KeyBoardKey() {Key = 'A'},
                            new KeyBoardKey() {Key = 'S'},
                            new KeyBoardKey() {Key = 'D'},
                            new KeyBoardKey() {Key = 'F'},
                            new KeyBoardKey() {Key = 'G'},
                            new KeyBoardKey() {Key = 'H'},
                            new KeyBoardKey() {Key = 'J'},
                            new KeyBoardKey() {Key = 'K'},
                            new KeyBoardKey() {Key = 'L'},
                            new KeyBoardKey() {Key = 'Ç'},
                            new KeyBoardKey() {Empty=true,Width = 40},
                            new KeyBoardKey() {Key = '4'},
                            new KeyBoardKey() {Key = '5'},
                            new KeyBoardKey() {Key = '6'},
                            new KeyBoardKey() {Key = '-'}
                        },
                        new List<KeyBoardKey>()
                        {
                            new KeyBoardKey() {Empty=true,Width = 130},
                            new KeyBoardKey() {Key = 'Z'},
                            new KeyBoardKey() {Key = 'X'},
                            new KeyBoardKey() {Key = 'C'},
                            new KeyBoardKey() {Key = 'V'},
                            new KeyBoardKey() {Key = 'B'},
                            new KeyBoardKey() {Key = 'N'},
                            new KeyBoardKey() {Key = 'M'},
                            new KeyBoardKey() {Caption=@"<font size=""5"">OK</font>",IgnoreAddText=true,Close=true,Width = 55*3},
                            new KeyBoardKey() {Empty=true,Width = 30},
                            new KeyBoardKey() {Key = '1'},
                            new KeyBoardKey() {Key = '2'},
                            new KeyBoardKey() {Key = '3'},
                            new KeyBoardKey() {Key = '.'},
                            
                        },
                        new List<KeyBoardKey>()
                        {
                            new KeyBoardKey() {Empty=true,Width=140 },
                            new KeyBoardKey() {Key = ' ',Width = 500},
                            new KeyBoardKey() {Empty=true,Width=105 },
                            new KeyBoardKey() {Key = '0',Width = (55*3)+10},
                            new KeyBoardKey() {Key = ','},
                        }
                    }
            };

            KeyBoardModelPtBrCaixaBaixa.Line[1][0].SubKeyBoardModel = KeyBoardModelPtBrCaixaAlta;
            KeyBoardModelPtBrCaixaAlta.Line[1][0].SubKeyBoardModel = KeyBoardModelPtBrCaixaBaixa;

            return KeyBoardModelPtBrCaixaBaixa;
        }

        public static Dictionary<EKeyBoardModel, KeyBoardModel> Models = new Dictionary<EKeyBoardModel, KeyBoardModel>()
        {
            {
                EKeyBoardModel.Numeric ,
                new KeyBoardModel(){ Caption="Numeric", 
                    Line = new List<List<KeyBoardKey>>()
                    {
                        new List<KeyBoardKey>()
                        {
                            new KeyBoardKey() {Key = '7'},
                            new KeyBoardKey() {Key = '8'},
                            new KeyBoardKey() {Key = '9'},
                            new KeyBoardKey() {Caption="<", Bsp = true}
                        },
                        new List<KeyBoardKey>()
                        {
                            new KeyBoardKey() {Key = '4'},
                            new KeyBoardKey() {Key = '5'},
                            new KeyBoardKey() {Key = '6'},
                            new KeyBoardKey() {Key = ','}
                        },
                        new List<KeyBoardKey>()
                        {
                            new KeyBoardKey() {Key = '1'},
                            new KeyBoardKey() {Key = '2'},
                            new KeyBoardKey() {Key = '3'},
                            new KeyBoardKey() {Key = '.'}
                        },
                        new List<KeyBoardKey>()
                        {
                            new KeyBoardKey() {Key = '0',Width = 55*2},
                            new KeyBoardKey() {Caption=@"<font size=""5"">OK</font>",IgnoreAddText=true,Close=true,Width = 55*2}
                        }
                    }
                }
            },
            {
                EKeyBoardModel.PtBr ,
                GetKeyBoardModelPtBr()
            }
        };
    }
}
