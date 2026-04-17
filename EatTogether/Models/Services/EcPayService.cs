using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace EatTogether.Models.Services
{
    public class EcPaySettings
    {
        public string MerchantId  { get; set; } = "";
        public string HashKey     { get; set; } = "";
        public string HashIV      { get; set; } = "";
        public string PaymentUrl  { get; set; } = "";
        public string ReturnUrl   { get; set; } = "";
        public string ClientBackUrl { get; set; } = "";
    }

    public class EcPayService
    {
        private readonly EcPaySettings _cfg;

        public EcPayService(IConfiguration config)
        {
            _cfg = config.GetSection("EcPay").Get<EcPaySettings>() ?? new EcPaySettings();
        }

        public string PaymentUrl  => _cfg.PaymentUrl;
        public string ClientBackUrl => _cfg.ClientBackUrl;

        // ── 產生送到綠界的表單參數 ─────────────────────────────────────────
        // tradeNo：MerchantTradeNo（格式見 controller，≤20 碼英數字）
        public Dictionary<string, string> BuildParams(
            string tradeNo, int amount, string itemName, string tradeDesc,
            string? clientBackUrl = null)
        {
            var p = new Dictionary<string, string>
            {
                ["MerchantID"]        = _cfg.MerchantId,
                ["MerchantTradeNo"]   = tradeNo,
                ["MerchantTradeDate"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                ["PaymentType"]       = "aio",
                ["TotalAmount"]       = amount.ToString(),
                ["TradeDesc"]         = tradeDesc,
                ["ItemName"]          = itemName,
                ["ReturnURL"]         = _cfg.ReturnUrl,
                ["ClientBackURL"]     = clientBackUrl ?? _cfg.ClientBackUrl,
                ["ChoosePayment"]     = "Credit",
                ["EncryptType"]       = "1",
            };
            p["CheckMacValue"] = ComputeCheckMac(p);
            return p;
        }

        // ── 驗證綠界回傳的 CheckMacValue ──────────────────────────────────
        public bool VerifyCheckMac(IEnumerable<KeyValuePair<string, string>> form)
        {
            var dict = form.ToDictionary(kv => kv.Key, kv => kv.Value);
            if (!dict.TryGetValue("CheckMacValue", out var received)) return false;
            dict.Remove("CheckMacValue");
            return ComputeCheckMac(dict).Equals(received, StringComparison.OrdinalIgnoreCase);
        }

        // ── SHA-256 CheckMacValue ─────────────────────────────────────────
        private string ComputeCheckMac(Dictionary<string, string> p)
        {
            var sorted = p.OrderBy(kv => kv.Key, StringComparer.OrdinalIgnoreCase);
            var raw = "HashKey=" + _cfg.HashKey + "&"
                    + string.Join("&", sorted.Select(kv => $"{kv.Key}={kv.Value}"))
                    + "&HashIV=" + _cfg.HashIV;

            // 綠界規定：WebUtility.UrlEncode 後全轉小寫
            var encoded = WebUtility.UrlEncode(raw).ToLower();

            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(encoded));
            return Convert.ToHexString(bytes).ToUpper();
        }

        // ── MerchantTradeNo 編碼 ──────────────────────────────────────────
        // 格式：O{preOrderId:D7}{MMddHHmmss}  (外帶單筆)
        //       T{tableId:D7}{MMddHHmmss}      (內用整桌)
        //       S{tableId:D7}{MMddHHmmss}      (拆單)
        public static string MakeTradeNo(int id, bool isTable)
        {
            var ts = DateTime.Now.ToString("MMddHHmmss"); // 10 碼
            return (isTable ? "T" : "O") + id.ToString("D7") + ts; // 1+7+10 = 18 碼
        }

        public static string MakeSplitTradeNo(int tableId)
        {
            var ts = DateTime.Now.ToString("MMddHHmmss"); // 10 碼
            return "S" + tableId.ToString("D7") + ts; // 1+7+10 = 18 碼
        }

        // ── MerchantTradeNo 解碼 ──────────────────────────────────────────
        // 回傳 (prefix, id)；prefix: 'O' | 'T' | 'S'
        public static (char Prefix, int Id) ParseTradeNo(string tradeNo)
        {
            if (string.IsNullOrEmpty(tradeNo) || tradeNo.Length < 8)
                throw new FormatException("Invalid MerchantTradeNo");
            var prefix = tradeNo[0];
            var id     = int.Parse(tradeNo.Substring(1, 7));
            return (prefix, id);
        }
    }
}
