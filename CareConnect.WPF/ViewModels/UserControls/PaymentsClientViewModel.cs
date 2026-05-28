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
    public class PaymentsClientViewModel : NotifyPropertyService
    {
        private readonly ModelContext _modelContext;
        private readonly ActiveUserService _activeUserService;
        private readonly PaymentsClientService _paymentsClientService;

        private string _selectedPaymentMethod;
        private bool _cashMethodError = false;
        private bool _invalidAmmountError = false;
        private string? _ammountValue;

        public ICommand PayCommand { get; }

        public ObservableCollection<Payment> PaymentsHistoryList { get; set; }
        public ObservableCollection<Booking> PendingPaymentsList { get; set; }
        public Booking? SelectedPendingPayment { get; set; }

        public PaymentsClientViewModel(ModelContext modelContext, ActiveUserService activeUserService, PaymentsClientService paymentsClientService) 
        {
            _modelContext = modelContext;
            _activeUserService = activeUserService;
            _paymentsClientService = paymentsClientService;

            _selectedPaymentMethod = "Card";
            PaymentsHistoryList = new ObservableCollection<Payment>(_paymentsClientService.GetPaymentsHistory(_activeUserService.ActiveUser!.Id));
            PendingPaymentsList = new ObservableCollection<Booking>(_paymentsClientService.GetPendingPayments(_activeUserService.ActiveUser!.Id));
            PayCommand = new RelayCommand(Pay, CanPay);
        }

        public bool CashMethodError
        {
            get => _cashMethodError;
            set
            {
                _cashMethodError = value;

                OnPropertyChanged(nameof(CashMethodError));
                ((RelayCommand)PayCommand).Refresh();
            }
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

                if (_selectedPaymentMethod == "Cash")
                    CashMethodError = true;
                else
                    CashMethodError = false;

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

                if(decimal.TryParse(_ammountValue, out ammount) && SelectedPendingPayment != null)
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
            _paymentsClientService.GeneratePayment(SelectedPendingPayment!.Id, decimal.Parse(AmmountValue!), SelectedPaymentMethod);

            PaymentsHistoryList = new ObservableCollection<Payment>(_paymentsClientService.GetPaymentsHistory(_activeUserService.ActiveUser!.Id));
            PendingPaymentsList = new ObservableCollection<Booking>(_paymentsClientService.GetPendingPayments(_activeUserService.ActiveUser!.Id));

            AmmountValue = string.Empty;
            OnPropertyChanged(nameof(AmmountValue));

            OnPropertyChanged(nameof(PaymentsHistoryList));
            OnPropertyChanged(nameof(PendingPaymentsList));

            MessageBox.Show("Payment has been made!");
        }

        private bool CanPay(object? parameter)
        {
            if (CashMethodError || InvalidAmmountError || string.IsNullOrWhiteSpace(AmmountValue))
                return false;
            return true;
        }
    }
}
