using Prometheus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Services
{
    public class MetricsService
    {
        private static readonly Gauge CpuUsageGauge = Metrics.CreateGauge("cpu_usage", "CPU Usage");
        private static readonly Gauge MemoryUsageGauge = Metrics.CreateGauge("memory_usage", "Memory Usage");

        public MetricsService()
        {
            // Configurar uma tarefa periódica para coletar métricas
            var timer = new Timer(CollectMetrics, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        private void CollectMetrics(object state)
        {
            // Coletar uso de CPU
            var cpuUsage = GetCpuUsage();
            CpuUsageGauge.Set(cpuUsage);

            // Coletar uso de memória
            var memoryUsage = GetMemoryUsage();
            MemoryUsageGauge.Set(memoryUsage);
        }

        private double GetCpuUsage()
        {
            // Implementar a lógica para coletar uso de CPU
            return Process.GetCurrentProcess().TotalProcessorTime.TotalMilliseconds / Environment.ProcessorCount;
        }

        private double GetMemoryUsage()
        {
            // Implementar a lógica para coletar uso de memória
            return Process.GetCurrentProcess().WorkingSet64 / (1024 * 1024);
        }
    }
}
