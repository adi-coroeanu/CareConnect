using CareConnect.Model.Models;
using CareConnect.Model.Services;
using CareConnect.ViewModel.Commands;
using CareConnect.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CareConnect.WPF.ViewModels.UserControls
{
    public class HomeClientViewModel : NotifyPropertyService
    {
        private readonly HomeClientService _homeClientService;
        private readonly ActiveUserService _activeUserService;

        private Service? _selectedService;
        private DateTime? _selectedDate;
        private string? _selectedPeriod;
        private string? _selectedPaymentType;

        public ICommand MakeAppointmentCommand { get; }
        public ObservableCollection<Service> ServicesList { get; set; }
        public ObservableCollection<string>? FreePeriodsList { get; set; } = null;
        public HomeClientViewModel(HomeClientService homeClientService, ActiveUserService activeUserService) 
        {
            _homeClientService = homeClientService;
            _activeUserService = activeUserService;

            MakeAppointmentCommand = new RelayCommand(MakeAppointment, CanMakeAppointment);
            ServicesList = new ObservableCollection<Service>(_homeClientService.GetServices());
        }

        public Service? SelectedService
        {
            get => _selectedService;
            set
            {
                _selectedService = value;

                FreePeriodsList = new ObservableCollection<string>(_homeClientService.GetFreePeriods(SelectedDate, SelectedService));
                OnPropertyChanged(nameof(FreePeriodsList));
            }
        }

        public DateTime? SelectedDate 
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;

                OnPropertyChanged(nameof(SelectedDate));

                FreePeriodsList = new ObservableCollection<string>(_homeClientService.GetFreePeriods(SelectedDate, SelectedService));
                OnPropertyChanged(nameof(FreePeriodsList));
            }
        }

        public string? SelectedPeriod
        {
            get => _selectedPeriod;
            set
            {
                _selectedPeriod = value;

                ((RelayCommand)MakeAppointmentCommand).Refresh();
            }
        }

        public string? SelectedPaymentType
        {
            get => _selectedPaymentType;
            set
            {
                _selectedPaymentType = value;

                ((RelayCommand)MakeAppointmentCommand).Refresh();
            }
        }

        private bool CanMakeAppointment(object? parameter)
        {
            if (SelectedPeriod == null)
                return false;
            return true;
        }

        private void MakeAppointment(object? parameter)
        {
            _homeClientService.MakeNewAppointment(SelectedService!.Id, _activeUserService.ActiveUser!.Id, SelectedDate!.Value, SelectedPeriod!);

            MessageBox.Show("The appointment have been created!");

            SelectedService = null;
            SelectedDate = null;
            SelectedPaymentType = null;

            OnPropertyChanged(nameof(SelectedService));
            OnPropertyChanged(nameof(SelectedDate));
            OnPropertyChanged(nameof(SelectedPaymentType));
        }

    }
}
