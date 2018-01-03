using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Services;

namespace SMART_MiddleLayer
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string invokeOCR_Service(string proc_Ref)
        {
            string OCR_Result = "default";

            OCRInvoker_LocalIIS.WebService1 OCR_Obj = new OCRInvoker_LocalIIS.WebService1();
            OCR_Obj.Credentials = new NetworkCredential("Ap1", "Password1", "BPMSERVER");
            string OCR_Output = OCR_Obj.OCRInvoker_BPMSERVER();
            OCR_Output = OCR_Output.Trim();
            OCR_Output = OCR_Output.Replace("3_ac", "3_Ac");

            OCR_Output = "[[[_List of Process References:::" + proc_Ref + "]]]" + OCR_Output;
            if (OCR_Output.Contains("[[[3_"))
                OCR_Result = "Recognized";
            else OCR_Result = "Not Recognized";

            SubmissionProcess_EM.WS_OCR_File_Data_Sub sub_Proc_obj = new SubmissionProcess_EM.WS_OCR_File_Data_Sub();
            sub_Proc_obj.Credentials = new NetworkCredential("Ap1", "Password1", "BPMSERVER");
            int Ap_returnCode = sub_Proc_obj._WS_OCR_File_Data_Sub(OCR_Output);


            return OCR_Result;
        }

        [WebMethod]
        public string invokeRPA_Service(string rpaInput)
        {
            string rpaOutput = "";
            BP_AutomationService.InitBlueprismDataItemfromCService obj = new BP_AutomationService.InitBlueprismDataItemfromCService();
            obj.Credentials = new NetworkCredential("admin", "Password2");
            rpaOutput = obj.InitBlueprismDataItemfromC(rpaInput);
            return rpaOutput;
        }
    }
}
