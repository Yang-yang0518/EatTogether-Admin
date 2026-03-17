using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace EatTogether.Models.Services
{
	public interface IPasswordResetEmailService
	{
		Task SendPasswordResetEmailAsync(string toEmail, string resetLink);
	}

	public class PasswordResetEmailService : IPasswordResetEmailService
	{
		private readonly IConfiguration _config;

		public PasswordResetEmailService(IConfiguration config)
		{
			_config = config;
		}

		public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
		{
			var smtp = _config.GetSection("Smtp");

			var message = new MimeMessage();
			message.From.Add(new MailboxAddress(smtp["FromName"], smtp["FromAddress"]));
			message.To.Add(MailboxAddress.Parse(toEmail));
			message.Subject = "【義起吃後台系統】密碼重設通知";
			message.Body = new BodyBuilder
			{
				HtmlBody = BuildHtml(resetLink)
			}.ToMessageBody();

			using var client = new SmtpClient();
			await client.ConnectAsync(smtp["Host"], int.Parse(smtp["Port"]!), SecureSocketOptions.StartTls);
			await client.AuthenticateAsync(smtp["UserName"], smtp["Password"]);
			await client.SendAsync(message);
			await client.DisconnectAsync(true);

		}

		private static string BuildHtml(string resetLink)
		{
			return $@"<!DOCTYPE html>
<html lang='zh-TW'>
<head>
  <meta charset='UTF-8' />
  <style>
    body {{ font-family: Arial, sans-serif; background: #F2EDE4; padding: 32px; }}
    .card {{ background: #fff; border-radius: 8px; padding: 32px; max-width: 480px; margin: auto; }}
    .btn {{
      display: inline-block; padding: 12px 28px;
      background: #1A0D08; color: #F5D87A !important;
      border-radius: 6px; text-decoration: none;
      font-weight: bold; margin: 24px 0;
    }}
    .note {{ color: #888; font-size: 13px; margin-top: 16px; }}
  </style>
</head>
<body>
  <div class='card'>
    <h2 style='color:#1A0D08;'>義起吃 — 密碼重設</h2>
    <p>您好，我們收到您的密碼重設請求。</p>
    <p>請點擊下方按鈕完成密碼重設：</p>
    <a href='{resetLink}' class='btn'>重設我的密碼</a>
    <p class='note'>⚠️ 此連結將於 <strong>60 分鐘後</strong>失效，且只能使用一次。</p>
    <p class='note'>若您未提出此申請，請忽略這封信，您的帳號不會受到任何影響。</p>
    <hr style='border:none;border-top:1px solid #eee;margin:24px 0;'/>
    <p class='note'>義起吃後台系統 — 自動發送，請勿回覆</p>
  </div>
</body>
</html>";
		}
	}

	// 只把連結印到 Console，不實際寄信(未註冊到DI)
	//public class FakePasswordResetEmailService : IPasswordResetEmailService
	//{
	//	private readonly ILogger<FakePasswordResetEmailService> _logger;

	//	public FakePasswordResetEmailService(ILogger<FakePasswordResetEmailService> logger)
	//	{
	//		_logger = logger;
	//	}
	//	public Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
	//	{
	//		_logger.LogWarning(
	//			"[FakePasswordResetEmail] To:{Email} | ResetLink: {Link}",
	//			toEmail, resetLink
	//		);
	//		return Task.CompletedTask;
	//	}
	//}
}
