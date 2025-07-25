using CommunityToolkit.WinUI.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Services;

namespace WpfApp1.Utility
{
    public class DailyScheduler
    {
        private System.Threading.Timer _timer;

        public void Schedule(int hour, int minute)
        {
            var now = DateTime.Now;
            var targetTime = DateTime.Today.AddHours(hour).AddMinutes(minute);
            if (now > targetTime)
                targetTime = targetTime.AddDays(1); // next day

            var delay = targetTime - now;

            _timer = new System.Threading.Timer(_ =>
            {
                PerformScheduledTask();

                // Reschedule
                Schedule(hour, minute);

            }, null, delay, Timeout.InfiniteTimeSpan);
        }

        public void PerformScheduledTask()
        {
            // Step 1: Your logic (e.g., check condition)
            IObligationService obligationService = new ObligationService();
            obligationService.filterOldObligations();
            int countTomorrowObligations = obligationService.findNextObligationsCount();         
            if (countTomorrowObligations>0)
            {
                AppSettings.getSettings().LastNotificationTime = DateTime.Now;
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string JSONPath = Path.Combine(basePath, "appsettings.json");
                string updatedJson = JsonConvert.SerializeObject(AppSettings.getSettings(), Formatting.Indented);
                File.WriteAllText(JSONPath, updatedJson);
                ShowToast(countTomorrowObligations);
            }
        }
        public void ShowToast(int obligationCount)
        {
             new ToastContentBuilder()
            .AddText("تنبيه إلتزامات")
            .AddText($"لديك عدد ( {obligationCount} ) إلتزام غداً")
            .Show();
        }

    }
}
