using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Common.Interfaces
{
    public interface ITools
    {
        string ResourcesPath { get; }
        string ResourcesPathUri { get; }
        string GetTemplateText<T>(string html, T model);
        string GetHtmlTemplate<T>(string template, T model);
        string GetFilePath(params string[] paths);
        string GetFileText(string filename);
        string ShearFileInDirectory(string FinFile);
        //FileContentResult DownloadFile(string path, string mimetype = default);
        //FileContentResult TextToFileResult(string text, string mimetype = default);
        bool VerificaCedula(string validarCedula);
        bool VerificaIdentificacion(string identificacion, bool cedula = false);
        bool VerificaPersonaJuridica(string validarCedula);
        bool VerificaSectorPublico(string validarCedula);
        Task<byte[]> DigitalSignPDFAsync(string filename);

        //Task<IOperationResult<byte[]>> GetPDFAsync(ReportBuilder report, string path = null);
        //Task<IOperationResult<byte[]>> GeneratePDFAsync(ReportBuilder report, string path = null);
        //Task<IOperationResult> SendMailAsync<T>(T model, string email, string subject, string template, params Attachment[] attachments);
        //Task<IOperationResult> SendMailNominaAsync<T>(T model, string email, string subject, string template, params MimePart[] attachments);
        //Task<IOperationResult> SendMailAsync(string recipients, string subject, string html, params Attachment[] attachments);
        //Task<IOperationResult> SendMailNominaAsync(string recipients, string subject, string html, params MimePart[] attachments);
        //Task<IOperationResult> SendMailActividades(string recipients, string subject, string html);
        public bool IsValidEmail(string email);
    }
}
