﻿using Controls.UserDialogs.Maui;
using System.Text.Json;
using VietQrMobile.Models;

namespace VietQrMobile.Services
{
    public class BankService
    {
        private readonly HttpClient _httpClient;

        public BankService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Bank>> GetBanksAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                UserDialogs.Instance.Alert("Lỗi kết nối", "Không có kết nối Internet. Vui lòng kiểm tra và thử lại.", "OK");
                return new List<Bank>();
            }

            var url = "https://api.vietqr.io/v2/banks";
            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new List<Bank>();
                }

                var json = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var root = JsonSerializer.Deserialize<BankApiResponse>(json, options);

                return root?.Data ?? new List<Bank>();
            }
            catch (HttpRequestException httpEx)
            {
                UserDialogs.Instance.Alert("Lỗi", $"Không thể kết nối đến máy chủ: {httpEx.Message}", "OK");
                return new List<Bank>();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Lỗi", $"Có lỗi xảy ra: {ex.Message}", "OK");
                return new List<Bank>();
            }
        }



        private class BankApiResponse
        {
            public string Code { get; set; }
            public string Desc { get; set; }
            public List<Bank> Data { get; set; }
        }
    }
}
