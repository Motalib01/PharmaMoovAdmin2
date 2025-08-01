using System;

namespace PharmaMoov.PushNotification
{
    public class PushNotificationData
    {
        public Guid UserId { get; set; }
        public string Header { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public int? Platform { get; set; } // 1-ios, 2-android
        public string FCMDeviceToken { get; set; }
        public int orderId { get; set; }
        public bool? IsPaymentSuccessful { get; set; }
    }

    public class NonSilentNotification
    {
        public string sound { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public string body { get; set; }
        public string icon { get; set; }
        public string android_channel_id { get; set; }
        public int badge { get; set; }
    }

    public class PushNotificationPayload
    {
        public bool content_available { get; set; }
        public string priority { get; set; }
        public NonSilentNotification notification { get; set; }
        public PushNotificationData data { get; set; }
        public string sound { get; set; }
        public string to { get; set; }
    }

    public class FCMReturnData
    {
        public Int64? multicast_id { get; set; }
        public int? success { get; set; }
        public int? failure { get; set; }
        public int? canonical_ids { get; set; }
        public FCMReturnDataResult[] results { get; set; }
    }

    public class FCMReturnDataResult
    {
        public string error { get; set; }
        public string info { get; set; }
    }
}
