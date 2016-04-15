// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2.Json;


namespace Ara2.Components.Textbox
{
    [Serializable]
    public class AraTextDatePicker
    {
        public  AraTextDatePicker()
        {

        }

        public AraTextDatePicker(EAraTextDatePicker vType)
        {
            Type = vType;
        }

        public enum EAraTextDatePicker
        {
            Data=1,
            DataHora=2,
            Hora=3
        }

        public EAraTextDatePicker Type = EAraTextDatePicker.Data;
        public SOptions Options = new SOptions(SOptions.EPais.PTBR);

        public string GetScript()
        {
            var vObj = new
            {
                type = (int)Type,
                scriptcuston = Options,
            };

            return DynamicJson.Serialize(vObj);
        }
    
    }

    [Serializable]
    public class SOptions
    {
        public SOptions()
        {
            dateFormat = null;
            altTimeFormat = null;
            timeFormat = null;
            currentText = null;
            closeText = null;
        }

        public bool gotoCurrent { get; set; }
        
        #region Data
        public string dateFormat { get; set; }
        public string altFormat { get; set; }
        #endregion

        #region Data Hora DataHora
        public string altTimeFormat {get;set;}
        public string[] dayNames {get;set;}
        public string[] dayNamesMin { get; set; }
        public string[] dayNamesShort { get; set; }
        public string[] monthNames { get; set; }
        public string[] monthNamesShort { get; set; }
        public string nextText { get; set; }
        public string prevText { get; set; }
        #endregion

        #region Timer
        public string timeOnlyTitle { get; set; }
        
        public string timeText { get; set; }
        public string hourText { get; set; }
        public string minuteText { get; set; }
        
        public string timeFormat { get; set; }
        public bool? showSecond { get; set; }
        public int? stepHour { get; set; }
        public int? stepMinute { get; set; }
        public int? stepSecond { get; set; }

        public int? hourMin { get; set; }
        public int? hourMax { get; set; }

        public DateTime? minDate { get; set; }
        public DateTime? maxDate { get; set; }

        public string currentText { get; set; }
        public string closeText { get; set; }
        #endregion

        public enum EPais
        {
            PTBR = 1
        }

        public SOptions(EPais Pais)
        {

            if (Pais == EPais.PTBR)
            {
                gotoCurrent = true;
                     
                #region Data
                dateFormat = "dd/mm/yy";
                altFormat = "dd/mm/yy";
                #endregion

                #region Data Hora DataHora
                altTimeFormat = "dd/mm/yyyy HH:mm";
                dayNames = new string[] { "Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado", "Domingo" };
                dayNamesMin = new string[] { "D", "S", "T", "Q", "Q", "S", "S", "D" };
                dayNamesShort = new string[] { "Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb", "Dom" };
                monthNames = new string[] { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };
                monthNamesShort = new string[] { "Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez" };
                nextText = "Próximo";
                prevText = "Anterior";

                currentText = "Data Atual";
                closeText = "Fechar";
                #endregion

                #region Timer
                timeOnlyTitle = "Escolha o Tempo";
                timeText = "Tempo";
                hourText = "Hora";
                minuteText = "Minuto";

                timeFormat = "HH:mm";
                showSecond = false;
                stepHour = 1;
                stepMinute = 1;
                stepSecond = 1;

                hourMin = 0;
                hourMax = 23;

                minDate = null;
                maxDate = null;
                #endregion
            }
            else
                throw new Exception("Pais não encontrado.");

        }
    }
}
