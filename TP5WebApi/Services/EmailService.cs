using MailKit;
using MailKit.Net.Smtp;
using MimeKit;

namespace TP5WebApi.Services;

public class EmailService
{
    private readonly IConfiguration _config;
    public EmailService(IConfiguration config) => _config = config;

    public async Task EnviarPdf(string destinatario, string asunto, string cuerpo, byte[] pdf, string nombreArchivo)
    {
        var smtp = _config.GetSection("Smtp");

        string host = smtp["Host"] ?? throw new InvalidOperationException("Falta Smtp:Host en la configuración");
        int puerto = int.Parse(smtp["Port"] ?? "2525");
        string usuario = smtp["User"] ?? throw new InvalidOperationException("Falta Smtp:User en la configuración");
        string clave = smtp["Pass"] ?? throw new InvalidOperationException("Falta Smtp:Pass en la configuración");
        string remitente = smtp["From"] ?? usuario;

        var mensaje = new MimeMessage();
        mensaje.From.Add(MailboxAddress.Parse(remitente));
        mensaje.To.Add(MailboxAddress.Parse(destinatario));
        mensaje.Subject = asunto;

        var builder = new BodyBuilder { TextBody = cuerpo };
        builder.Attachments.Add(nombreArchivo, pdf, new MimeKit.ContentType("application", "pdf"));
        mensaje.Body = builder.ToMessageBody();

        using var cliente = new SmtpClient();
        await cliente.ConnectAsync(host, puerto, MailKit.Security.SecureSocketOptions.StartTls);
        await cliente.AuthenticateAsync(usuario, clave);
        await cliente.SendAsync(mensaje);
        await cliente.DisconnectAsync(true);
    }
}