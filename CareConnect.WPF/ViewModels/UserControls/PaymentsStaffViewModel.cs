using CareConnect.Model.Models;
using CareConnect.Model.Services;
using CareConnect.ViewModel.Commands;
using CareConnect.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CareConnect.WPF.ViewModels.UserControls
{
    public class PaymentsStaffViewModel : NotifyPropertyService
    {
        private readonly ModelContext _modelContext;
        private readonly ActiveUserService _activeUserService;
        private readonly PaymentsStaffService _paymentsStaffService;

        private string _selectedPaymentMethod;
        private bool _invalidAmmountError = false;
        private string? _ammountValue;

        public ICommand PayCommand { get; }

        public ObservableCollection<Booking> PendingPaymentsList { get; set; }
        public Booking? SelectedPendingPayment { get; set; }

        public PaymentsStaffViewModel(ModelContext modelContext, ActiveUserService activeUserService, PaymentsStaffService paymentsStaffService)
        {
            _modelContext = modelContext;
            _activeUserService = activeUserService;
            _paymentsStaffService = paymentsStaffService;

            _selectedPaymentMethod = "Card";
            PendingPaymentsList = new ObservableCollection<Booking>(_paymentsStaffService.GetPendingPayments(_activeUserService.ActiveUser!.Id));
            PayCommand = new RelayCommand(Pay, CanPay);
        }

        public bool InvalidAmmountError
        {
            get => _invalidAmmountError;
            set
            {
                _invalidAmmountError = value;

                OnPropertyChanged(nameof(InvalidAmmountError));
                ((RelayCommand)PayCommand).Refresh();
            }
        }

        public string SelectedPaymentMethod
        {
            get => _selectedPaymentMethod;
            set
            {
                _selectedPaymentMethod = value;

                OnPropertyChanged();
            }
        }

        public string? AmmountValue
        {
            get => _ammountValue;

            set
            {
                _ammountValue = value;

                if (string.IsNullOrWhiteSpace(_ammountValue))
                {
                    InvalidAmmountError = false;
                    return;
                }

                decimal ammount;

                if (decimal.TryParse(_ammountValue, out ammount) && SelectedPendingPayment != null)
                {
                    if (ammount > SelectedPendingPayment.TotalAmmount)
                        InvalidAmmountError = true;
                    else
                        InvalidAmmountError = false;
                    return;
                }

                InvalidAmmountError = true;

                OnPropertyChanged();
            }
        }

        private void Pay(object? parameter)
        {
            _paymentsStaffService.GeneratePayment(SelectedPendingPayment!.Id, decimal.Parse(AmmountValue!), SelectedPaymentMethod, _activeUserService.ActiveUser.Id);

            PendingPaymentsList = new ObservableCollection<Booking>(_paymentsStaffService.GetPendingPayments(_activeUserService.ActiveUser!.Id));

            AmmountValue = string.Empty;
            OnPropertyChanged(nameof(AmmountValue));

            OnPropertyChanged(nameof(PendingPaymentsList));

            MessageBox.Show("Payment has been made!");
        }

        private bool CanPay(object? parameter)
        {
            if (InvalidAmmountError || string.IsNullOrWhiteSpace(AmmountValue))
                return false;
            return true;
        }
    }
}
