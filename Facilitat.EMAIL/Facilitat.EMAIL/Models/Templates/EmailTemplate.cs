namespace Facilitat.EMAIL.Models.Templates
{
    public static class EmailTemplates
    {
        public static string ScheduleEmailTemplate => @"
                <html>
                <body>
                    <p>Dear {{UserName}},</p>
                    <p>Your visit to Tower {{Tower}} at Apartment {{Apartment}} is scheduled for {{Date}}.</p>
                    <!-- more of your email HTML -->
                </body>
                </html>";
    }
}
