using CareConnect.Model.Models;
using CareConnect.Model.Services;
using CareConnect.ViewModel.Commands;
using CareConnect.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CareConnect.WPF.ViewModels.UserControls
{
    public class ServicesStaffViewModel : NotifyPropertyService
    {
        private readonly ServiceStaffService _serviceStaffService;
        private readonly ActiveUserService _activeUserService;

        private string? _serviceName;
        private DateTime? _openingTime;
        private DateTime? _closingTime;
        private string? _minutesAppointment;
        private string? _servicePrice;
        private Service? _selectedService;


        public ObservableCollection<Service> StaffServicesList { get; set; }
        public ICommand DeleteServiceCommand { get; }
        public ICommand CreateServiceCommand { get; }

        public ServicesStaffViewModel(ServiceStaffService serviceStaffService, ActiveUserService activeUserService)
        {
            _serviceStaffService = serviceStaffService;
            _activeUserService = activeUserService;

            DeleteServiceCommand = new RelayCommand(DeleteService, CanDeleteService);
            CreateServiceCommand = new RelayCommand(CreateService, CanCreateService);

            StaffServicesList = new ObservableCollection<Service>(_serviceStaffService.GetStaffServiceList(_activeUserService.ActiveUser!.Id));
            InvalidMinutesError = false;
            InvalidMinutesError = false;
        }

        public bool InvalidMinutesError { get; set; }
        public bool InvalidPriceError { get; set; }

        public string? ServiceName
        {
            get => _serviceName;
            set
            {
                _serviceName = value;

                OnPropertyChanged();
                ((RelayCommand)CreateServiceCommand).Refresh();
            }
        }

        public DateTime? OpeningTime
        {
            get => _openingTime;
            set
            {
                _openingTime = value;

                OnPropertyChanged();
                ((RelayCommand)CreateServiceCommand).Refresh();
            }
        }

        public DateTime? ClosingTime
        {
            get => _closingTime;
            set
            {
                _closingTime = value;

                OnPropertyChanged();
                ((RelayCommand)CreateServiceCommand).Refresh();
            }
        }

        public string? MinutesAppointment
        {
            get => _minutesAppointment;
            set
            {
                _minutesAppointment = value;

                int minutes;

                if (string.IsNullOrWhiteSpace(MinutesAppointment))
                    InvalidMinutesError = false;

                else if (int.TryParse(MinutesAppointment, out minutes))
                {
                    if (minutes <= 0)
                        InvalidMinutesError = true;
                    else
                        InvalidMinutesError = false;
                }

                else
                    InvalidMinutesError = true;

                OnPropertyChanged(nameof(InvalidMinutesError));
                OnPropertyChanged();
                ((RelayCommand)CreateServiceCommand).Refresh();
            }
        }

        public string? ServicePrice
        {
            get => _servicePrice;
            set
            {
                _servicePrice = value;

                decimal price;

                if (string.IsNullOrWhiteSpace(ServicePrice))
                    InvalidPriceError = false;

                else if (decimal.TryParse(MinutesAppointment, out price))
                {
                    if (price <= 0)
                        InvalidPriceError = true;
                    else
                        InvalidPriceError = false;
                }

                else
                    InvalidPriceError = true;

                OnPropertyChanged(nameof(InvalidPriceError));
                OnPropertyChanged();
                ((RelayCommand)CreateServiceCommand).Refresh();
            }
        }

        public Service? SelectedService 
        {   get => _selectedService;
            set
            {
                _selectedService = value;

                OnPropertyChanged();
                ((RelayCommand)DeleteServiceCommand).Refresh();
            } 
        }

        private void DeleteService(object? parameter)
        {
            _serviceStaffService.DeleteService(SelectedService!);

            StaffServicesList = new ObservableCollection<Service>(_serviceStaffService.GetStaffServiceList(_activeUserService.ActiveUser!.Id));
            OnPropertyChanged(nameof(StaffServicesList));

            MessageBox.Show("Service deleted!");
        }

        private bool CanDeleteService(object? parameter)
        {
            if (SelectedService == null)
                return false;
            return true;
        }

        public void CreateService(object? parameter)
        {
            _serviceStaffService.CreateService(_activeUserService.ActiveUser!.Id, ServiceName!, MinutesAppointment!, OpeningTime!.Value, ClosingTime!.Value, ServicePrice!);

            StaffServicesList = new ObservableCollection<Service>(_serviceStaffService.GetStaffServiceList(_activeUserService.ActiveUser!.Id));
            OnPropertyChanged(nameof(StaffServicesList));

            MessageBox.Show("Service created!");
        }

        public bool CanCreateService(object? parameter)
        {
            if(OpeningTime >= ClosingTime || InvalidMinutesError || string.IsNullOrWhiteSpace(ServiceName) || InvalidPriceError 
                || string.IsNullOrWhiteSpace(ServicePrice) || string.IsNullOrWhiteSpace(MinutesAppointment) 
                || OpeningTime == null || ClosingTime == null)
                return false;

            return true;
        }
    }
}
