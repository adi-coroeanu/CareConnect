using CareConnect.Model.Models;
using CareConnect.Model.Services;
using CareConnect.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CareConnect.WPF.ViewModels.UserControls
{
    public class AppointmentsClientViewModel : NotifyPropertyService
    {
        private readonly AppointmentsClientService _appointmentsClientService;
        private readonly ActiveUserService _activeUserService;

        private string? _selectedPeriodAppointment;

        public ObservableCollection<Booking> AppointmentsList { get; set; }

        public AppointmentsClientViewModel(AppointmentsClientService appointmentsClientService, ActiveUserService activeUserService)
        {
            _appointmentsClientService = appointmentsClientService;
            _activeUserService = activeUserService;

            AppointmentsList = new ObservableCollection<Booking>(_appointmentsClientService.GetBookings(_activeUserService.ActiveUser!.Id, SelectedPeriodAppointment));
            SelectedPeriodAppointment = "All appointments";
        }

        public string? SelectedPeriodAppointment
        {
            get => _selectedPeriodAppointment;
            set
            {
                _selectedPeriodAppointment = value;

                AppointmentsList = new ObservableCollection<Booking>(_appointmentsClientService.GetBookings(_activeUserService.ActiveUser!.Id, SelectedPeriodAppointment));
                OnPropertyChanged(nameof(AppointmentsList));
            }
        }

    }
}
