using CareConnect.Model.Models;
using CareConnect.Model.Services;
using CareConnect.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CareConnect.WPF.ViewModels.UserControls
{
    public class AppointmentsStaffViewModel : NotifyPropertyService
    {
        private readonly AppointmentsStaffService _appointmentsStaffService;
        private readonly ActiveUserService _activeUserService;

        private string? _selectedPeriodAppointment;

        public ObservableCollection<Booking> AppointmentsList { get; set; }

        public AppointmentsStaffViewModel(AppointmentsStaffService appointmentsStaffService, ActiveUserService activeUserService)
        {
            _appointmentsStaffService = appointmentsStaffService;
            _activeUserService = activeUserService;

            AppointmentsList = new ObservableCollection<Booking>(_appointmentsStaffService.GetBookings(_activeUserService.ActiveUser!.Id));
            SelectedPeriodAppointment = "All appointments";
        }

        public string? SelectedPeriodAppointment
        {
            get => _selectedPeriodAppointment;
            set
            {
                _selectedPeriodAppointment = value;

                AppointmentsList = new ObservableCollection<Booking>(_appointmentsStaffService.GetBookings(_activeUserService.ActiveUser!.Id, SelectedPeriodAppointment));
                OnPropertyChanged(nameof(AppointmentsList));
            }
        }
    }
}
