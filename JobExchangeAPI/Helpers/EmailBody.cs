namespace JobExchangeAPI.Helpers
{
    public class EmailBody
    {
        private readonly IConfiguration _config;
        private readonly string? _webUrl = "";
        public EmailBody(IConfiguration config)
        {
            _config = config;
            _webUrl = _config["JobExchangeWebUrl"]?.ToString() ?? string.Empty;
        }

        public string EmailStringBody(string email, string emailToken)
        {
            return $@"
            <html>
                <head>
                </head>
                <body style=""margin:0;padding:0;font-famaly:Arial,Helvetica,sans-serif;"">
                    <div style=""height:auto;background:linear-gradient(to top, #c9c9ff 50%, #6e6ef6 90%) no-repeat;width:400px;padding:30px"">
                        <div>
                            <h1>Reset your Password</h1>
                            <hr>
                            <p>You're receiving this e-mail because you a password reset for your Let's program account.</p>
                            <p>Please tap the button below to choose a new password.</p>
                            <a href=""{_webUrl}/reset?email={email}&code={emailToken}"" target=""_blank"" style=""background:#0d6efd;padding:10px;border:none;color:white;border-radius:4px;display:block;margin:0 auto;width:50%;text-align:center;text-decoration:none;"">
                                Reset Password
                            </a>
                            <br>
                            <p>Kind Regards, <br><br> Lets Program</p>
                        </div>
                    </div>
                </body>
            </html>
            ";
        }
    }
}
