using CareConnect.Model.Models;
using CareConnect.WPF.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CareConnect.WPF.ViewModels.UserControls
{
    public class ServiceLegendItem
    {
        public string Label { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }

    public class StatsAdminViewModel : NotifyPropertyService
    {
        private readonly ModelContext _context;

        // Revenue
        public ISeries[]? RevenueSeries { get; set; }
        public Axis[]? RevenueXAxes { get; set; }

        // Appointments
        public ISeries[]? AppointmentsSeries { get; set; }
        public Axis[]? AppointmentsXAxes { get; set; }

        // Services
        public ISeries[]? ServicesSeries { get; set; }
        public ObservableCollection<ServiceLegendItem>? ServicesLegend { get; set; }

        public StatsAdminViewModel(ModelContext context)
        {
            _context = context;

            LoadRevenueChart();
            LoadAppointmentsChart();
            LoadServicesChart();
        }

        private void LoadRevenueChart()
        {
            var last6Months = Enumerable.Range(0, 6)
                .Select(i => DateTime.Now.AddMonths(-3 + i))
                .ToList();

            var revenues = last6Months.Select(month =>
                _context.Payments
                    .Where(p => p.PaymentDate.Month == month.Month && p.PaymentDate.Year == month.Year)
                    .Sum(p => (double?)p.PaymentValue) ?? 0
            ).ToArray();

            RevenueSeries = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = revenues,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.DeepSkyBlue, 3),
                    GeometryStroke = new SolidColorPaint(SKColors.DeepSkyBlue, 3),
                    GeometryFill = new SolidColorPaint(SKColors.White),
                    Name = "Revenue (lei)"
                }
            };

            RevenueXAxes = new Axis[]
            {
                new Axis
                {
                    Labels = last6Months.Select(m => m.ToString("MMM yyyy")).ToArray()
                }
            };
        }

        private void LoadAppointmentsChart()
        {
            var months = Enumerable.Range(0, 7)
                .Select(i => DateTime.Now.AddMonths(-3 + i))
                .ToList();

            var appointments = months.Select(month =>
            {
                var m = month.Month;
                var y = month.Year;
                var count = (double)_context.Bookings
                    .Count(b => b.BookingDate.Month == m && b.BookingDate.Year == y);

                Debug.WriteLine($"{month:MMM yyyy}: {count}");

                return count;
            }).ToArray();

            AppointmentsSeries = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = appointments,
                    Fill = new SolidColorPaint(SKColors.MediumSeaGreen),
                    Name = "Appointments"
                }
            };

            AppointmentsXAxes = new Axis[]
            {
                new Axis
                {
                    Labels = months.Select(m => m.ToString("MMM yyyy")).ToArray()
                }
            };
        }

        private void LoadServicesChart()
        {
            var colors = new[] { "#4FC3F7", "#81C784", "#FFB74D", "#E57373", "#BA68C8" };

            var serviceGroups = _context.Bookings
                .GroupBy(b => b.IdServiceNavigation!.Name)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToList();

            ServicesSeries = serviceGroups.Select((s, i) => (ISeries)new PieSeries<double>
            {
                Values = new double[] { s.Count },
                Name = s.Name,
                Fill = new SolidColorPaint(SKColor.Parse(colors[i % colors.Length]))
            }).ToArray();

            ServicesLegend = new ObservableCollection<ServiceLegendItem>(
                serviceGroups.Select((s, i) => new ServiceLegendItem
                {
                    Label = $"{s.Name} ({s.Count})",
                    Color = colors[i % colors.Length]
                })
            );
        }
    }
}