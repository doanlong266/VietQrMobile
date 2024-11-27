using System.Text.Json;
using System.Text;
using VietQrMobile.Models;
using VietQrMobile.Services;
using Controls.UserDialogs.Maui;

namespace VietQrMobile.View
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _httpClient;
        private readonly DatabaseService _databaseService;
        public MainPage()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _databaseService = new DatabaseService(); 
        }
        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                string rawText = e.NewTextValue?.Replace(",", "").Trim() ?? "";

                if (long.TryParse(rawText, out long amount))
                {
                    entry.TextChanged -= Entry_TextChanged; 
                    entry.Text = string.Format("{0:N0}", amount);
                    entry.TextChanged += Entry_TextChanged;
                    entry.CursorPosition = entry.Text.Length;
                }
            }
        }

        private async void createQR(object sender, EventArgs e)
        {
            if (!Connectivity.NetworkAccess.HasFlag(NetworkAccess.Internet))
            {
                await DisplayAlert("Lỗi kết nối", "Không có kết nối Internet. Vui lòng kiểm tra và thử lại.", "OK");
                return;
            }
            var amountString = AmountEntry.Text;
           
            if (string.IsNullOrWhiteSpace(amountString))
            {
                await DisplayAlert("Error", "Vui lòng nhập số tiền!", "OK");
                return;
            }
            var amountText = amountString.Replace(",", "");
            var cleanAmountText = new string(amountText.Where(char.IsDigit).ToArray());
            if (!long.TryParse(cleanAmountText, out var amount) || amount <= 0)
            {
                await DisplayAlert("Error", "Số tiền không hợp lệ!", "OK");
                return;
            }

            var account = await _databaseService.GetAccountById(1);
            if (account == null)
            {
                await DisplayAlert("Error", "Không tìm thấy tài khoản trong cơ sở dữ liệu!", "OK");
                return;
            }

            var requestData = new
            {
                accountNo = account.AccountNo,
                accountName = account.AccountName,
                acqId = account.AcqId,
                amount = amount,
                addInfo = "",
                format = "text",
                template = "compact"
            };
            UserDialogs.Instance.ShowLoading("Đang tải mã QR...", MaskType.Black);
            try
            {
                var response = await _httpClient.PostAsync(
                    "https://api.vietqr.io/v2/generate",
                    new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json")
                );

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"Không thể tạo mã QR! Chi tiết lỗi: {errorMessage}", "OK");
                    return;
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true 
                };

                var responseData = JsonSerializer.Deserialize<VietQRResponse>(responseContent, options);

                if (responseData?.Code == "00")
                {
                    await Navigation.PushAsync(new QrView(responseData.Data.QrDataURL));
                }
                else
                {
                    await DisplayAlert("Error", responseData?.Desc ?? "Có lỗi xảy ra!", "OK");
                }
            }
            catch (JsonException jsonEx)
            {
                await DisplayAlert("Error", $"Lỗi khi phân tích JSON: {jsonEx.Message}", "OK");
            }
            catch (HttpRequestException httpEx)
            {
                await DisplayAlert("Error", $"Lỗi khi gửi yêu cầu HTTP: {httpEx.Message}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Có lỗi xảy ra: {ex.Message}", "OK");
            }
            finally
            {
                UserDialogs.Instance.HideHud();
            }

        }
    }
}
