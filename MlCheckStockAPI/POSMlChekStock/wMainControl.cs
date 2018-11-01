using MlCheckStock.X_Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace POSMlChekStock
{
    public partial class wMainControl : Form
    {
        private cCallCheckByTdr oC_CallCheckByTdr = new cCallCheckByTdr();
        private int nC_SrcSeqNo = 0;
        private string tC_BBYQuotaType;
        public wMainControl()
        {
            InitializeComponent();
        }

        private void ocmQuota_Click(object sender, EventArgs e)
        {
            try
            {
                cCallCheckStock oCallCheckStock = new cCallCheckStock();

                var tResult = oCallCheckStock.GETxQuota(otbUrlCHK.Text , otbPlantCodeCHK.Text, otbBByProfIDCHK.Text, otbBByNoCHK.Text, otbSKUCodeCHK.Text , otbStartDateCHK.Text , otbEndDateCHK.Text);
                var aResult = tResult.Split('|');
                otbResult.Text = aResult[0];
                otbCode.Text = aResult[1];
                otbMsg.Text = aResult[2];
            }
            catch (Exception oEx)
            {
                MessageBox.Show("wMainControl :ocmQuota_Click ///"+oEx.Message);
            }
        }

        private void otoCheckQuota_Click(object sender, EventArgs e)
        {
            otaTabCtr.SelectedTab = otaCheckQuota;
        }

        private void otoQuotaByTender_Click(object sender, EventArgs e)
        {
            otaTabCtr.SelectedTab = otaQuotaByTender;
        }

        private void otoSetting_Click(object sender, EventArgs e)
        {
            otaTabCtr.SelectedTab = otaSetting;
        }

        private void ocmPromotion_Click(object sender, EventArgs e)
        {
            try
            {
                nC_SrcSeqNo = nC_SrcSeqNo + 1;
                otbSrcSeqNo.Text = nC_SrcSeqNo.ToString();

                otbPosNoUse.Text = otbPOSNo.Text;
                otbTransNoUse.Text = otbTransNo.Text;
                otbTransTypeUse.Text = otbTransType.Text;
                otbTransDateUse.Text = otbTransDate.Text;
                otbPlantCodeUse.Text = otbPlantCode.Text;
                otbBBYNoUse.Text = otbBBYNo.Text;
                otbAmtB4Use.Text = otbtAmtB4Disc.Text;

                var tResult = oC_CallCheckByTdr.GETtReserve(otbUrl.Text, otbStmCode.Text, otbPOSNo.Text, otbTransNo.Text, otbTransType.Text, otbTransDate.Text
                                                  , otbPlantCode.Text, otbMcardNo.Text, otbMcardType.Text, otbBBYProfID.Text
                                                  , otbBBYNo.Text, otbBBYStartDate.Text, otbBBYEndDate.Text, tC_BBYQuotaType, ocbBBYDayName.Text
                                                  , otbBBYQuota.Text, otbCrdQuota.Text, otbTdmCode.Text, otbTdmCardType.Text, otbtAmtB4Disc.Text
                                                  , otbUserName.Text, otbDateIns.Text, otbTimeIns.Text, otbSrcSeqNo.Text
                                              );
                C_GETxMessage(tResult);

                var aResult = tResult.Split('|');
                if (aResult[0].Equals("00"))
                {
                    if (MessageBox.Show("ต้องการใช้สิทธิ์โปรโมชั่น ?", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        tResult = oC_CallCheckByTdr.GETtUsed(otbUrl.Text, otbPosNoUse.Text, otbTransNoUse.Text, otbTransTypeUse.Text, otbTransDateUse.Text, otbPlantCodeUse.Text, otbBBYNoUse.Text, otbAmtB4Use.Text, otbDateInsUse.Text, otbTimeInsUse.Text, otbSrcSeqNo.Text);
                        C_GETxMessage(tResult);
                        C_ClearxTextBox();
                    }
                    else
                    {
                        tResult = oC_CallCheckByTdr.GETtCancel(otbUrl.Text, otbPosNoUse.Text, otbTransNoUse.Text, otbTransTypeUse.Text, otbTransDateUse.Text, otbPlantCodeUse.Text, otbBBYNoUse.Text, otbDateInsUse.Text, otbTimeInsUse.Text, otbSrcSeqNo.Text);
                        C_GETxMessage(tResult);
                        C_ClearxTextBox();
                    }
                }
            }
            catch (Exception oEx)
            {
                MessageBox.Show(oEx.Message);
            }
        }

        private void ocmByTdr_Click(object sender, EventArgs e)
        {
            try
            {
                string tResult;
                otbPosNoUse.Text = otbPOSNo.Text;
                otbTransNoUse.Text = otbTransNo.Text;
                otbTransTypeUse.Text = otbTransType.Text;
                otbTransDateUse.Text = otbTransDate.Text;
                otbPlantCodeUse.Text = otbPlantCode.Text;
                otbBBYNoUse.Text = otbBBYNo.Text;
                otbAmtB4Use.Text = otbtAmtB4Disc.Text;

                switch (ocbByTdr.SelectedItem.ToString())
                {
                    case "Reserv":

                        tResult = oC_CallCheckByTdr.GETtReserve(otbUrl.Text, otbStmCode.Text, otbPOSNo.Text, otbTransNo.Text, otbTransType.Text, otbTransDate.Text
                                                  , otbPlantCode.Text, otbMcardNo.Text, otbMcardType.Text, otbBBYProfID.Text
                                                  , otbBBYNo.Text, otbBBYStartDate.Text, otbBBYEndDate.Text, tC_BBYQuotaType, ocbBBYDayName.Text
                                                  , otbBBYQuota.Text, otbCrdQuota.Text, otbTdmCode.Text, otbTdmCardType.Text, otbtAmtB4Disc.Text
                                                  , otbUserName.Text, otbDateIns.Text, otbTimeIns.Text, otbSrcSeqNo.Text
                                              );
                        C_GETxMessage(tResult);
                        break;

                    case "Used":

                        tResult = oC_CallCheckByTdr.GETtUsed(otbUrl.Text, otbPosNoUse.Text, otbTransNoUse.Text, otbTransTypeUse.Text, otbTransDateUse.Text, otbPlantCodeUse.Text, otbBBYNoUse.Text, otbAmtB4Use.Text, otbDateInsUse.Text, otbTimeInsUse.Text, otbSrcSeqNo.Text);
                        C_GETxMessage(tResult);
                        C_ClearxTextBox();
                        break;

                    case "Cancel":

                        tResult = oC_CallCheckByTdr.GETtCancel(otbUrl.Text, otbPosNoUse.Text, otbTransNoUse.Text, otbTransTypeUse.Text, otbTransDateUse.Text, otbPlantCodeUse.Text, otbBBYNoUse.Text, otbDateInsUse.Text, otbTimeInsUse.Text, otbSrcSeqNo.Text);
                        C_GETxMessage(tResult);
                        C_ClearxTextBox();
                        break;
                }
            }
            catch (Exception oEx)
            {
                MessageBox.Show(oEx.Message);
            }
        }

        private void ocbBBYQuotaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (ocbBBYQuotaType.SelectedItem.ToString())
                {
                    case "Per BBY":
                        tC_BBYQuotaType = "1";
                        otbBBYQuota.Text = "100";
                        ocbBBYDayName.Enabled = false;
                        ocbBBYDayName.SelectedItem = null;
                        break;
                    case "Per Store":
                        tC_BBYQuotaType = "2";
                        otbBBYQuota.Text = "50";
                        ocbBBYDayName.Enabled = false;
                        ocbBBYDayName.SelectedItem = null;
                        break;
                    case "Per Plant":
                        tC_BBYQuotaType = "3";
                        otbBBYQuota.Text = "30";
                        ocbBBYDayName.Enabled = false;
                        ocbBBYDayName.SelectedItem = null;
                        break;
                    case "Per Day":
                        tC_BBYQuotaType = "4";
                        otbBBYQuota.Text = "10";
                        ocbBBYDayName.Enabled = true;
                        ocbBBYDayName.SelectedItem = null;
                        break;
                }
            }
            catch (Exception oEx)
            {
                MessageBox.Show(oEx.Message);
            }
        }

        private void C_GETxMessage(string ptResult)
        {
            string[] aResult;
            try
            {
                aResult = ptResult.Split('|');
                otbResCod.Text = aResult[0];
                otbMessage.Text = aResult[1];
                otbDateInsRes.Text = aResult[2];
                otbTimeInsRes.Text = aResult[3];
                if (aResult[0].Equals("00"))
                {
                    otbDateInsUse.Text = aResult[2];
                    otbTimeInsUse.Text = aResult[3];
                }
            }
            catch (Exception oEx)
            {
                MessageBox.Show(oEx.Message);
            }
        }

        private void C_ClearxTextBox()
        {
            try
            {
                otbPosNoUse.Text = "";
                otbTransNoUse.Text = "";
                otbTransTypeUse.Text = "";
                otbTransDateUse.Text = "";
                otbPlantCodeUse.Text = "";
                otbBBYNoUse.Text = "";
                otbAmtB4Use.Text = "";
                otbDateInsUse.Text = "";
                otbTimeInsUse.Text = "";
            }
            catch (Exception oEx)
            {
                MessageBox.Show(oEx.Message);
            }
        }

        private void oTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var oCultureEnInfo = new CultureInfo("en-US");
                var oDateEng = Convert.ToDateTime(DateTime.Now, oCultureEnInfo);
                otbDateIns.Text = oDateEng.ToString("yyyy-MM-dd", oCultureEnInfo);
                otbTransDate.Text = oDateEng.ToString("yyyy-MM-dd", oCultureEnInfo);
                otbTimeIns.Text = DateTime.Now.ToString("HH:mm:ss");
            }
            catch (Exception oEx)
            {
                MessageBox.Show(oEx.Message);
            }
        }
    }
}
