using VietQrMobile.Models;
using VietQrMobile.Services;

namespace VietQrMobile.View
{
    public partial class SettingPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        private readonly BankService _bankService;
        private List<Bank> _originalBankList; 
        private List<Bank> _filteredBankList;

        public SettingPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            _bankService = new BankService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var bankService = new BankService();
            _originalBankList = await bankService.GetBanksAsync();
            _filteredBankList = new List<Bank>(_originalBankList);

            BankList.ItemsSource = _filteredBankList;
            var account = await _databaseService.GetAccountById(1); 
            if (account != null)
            {
                AccountNoEntry.Text = account.AccountNo;
                AccountNameEntry.Text = account.AccountName;
                BankList.Text = account.AcqId.ToString();
            }
            else
            {
                await DisplayAlert("Thông báo", "Bạn chưa có tài khoản hãy setup.", "OK");
            }
        }

        private async void updateAccount(object sender, EventArgs e)
        {
            var accountNo = AccountNoEntry.Text;
            var accountName = AccountNameEntry.Text;
            var acqIdText = BankList.Text;

            if (string.IsNullOrWhiteSpace(accountNo) ||
                string.IsNullOrWhiteSpace(accountName) ||
                string.IsNullOrWhiteSpace(acqIdText) ||
                !int.TryParse(acqIdText, out int acqId) || acqIdText.Length != 6)
            {
                await DisplayAlert("Error", "Vui lòng nhập thông tin hợp lệ!", "OK");
                return;
            }

            var account = new Account
            {
                Id = 1, 
                AccountNo = accountNo,
                AccountName = accountName,
                AcqId = acqId
            };
            await _databaseService.AddOrUpdateAccount(account);

            await DisplayAlert("Success", "Cập nhật tài khoản thành công!", "OK");
        }
        private void bank_SuggestionChosen(object sender, zoft.MauiExtensions.Controls.AutoCompleteEntrySuggestionChosenEventArgs e)
        {
            if (sender is zoft.MauiExtensions.Controls.AutoCompleteEntry autoCompleteEntry)
            {
                var selectedWard = e.SelectedItem as Bank;
                if (selectedWard != null)
                {

                    autoCompleteEntry.Text = selectedWard.Bin;
                }
            }
        }
        private void bankFilter_TextChanged(object sender, zoft.MauiExtensions.Controls.AutoCompleteEntryTextChangedEventArgs e)
        {
            if (e.Reason == zoft.MauiExtensions.Controls.AutoCompleteEntryTextChangeReason.UserInput)
            {
                var filterText = (sender as zoft.MauiExtensions.Controls.AutoCompleteEntry)?.Text?.ToLower() ?? string.Empty;

                _filteredBankList = _originalBankList
                    .Where(bank => bank.Name.ToLower().Contains(filterText) ||
                                   bank.ShortName.ToLower().Contains(filterText))
                    .ToList();

                BankList.ItemsSource = _filteredBankList;
            }
        }

    }

}
